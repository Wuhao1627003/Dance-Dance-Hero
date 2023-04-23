using System;
using System.Threading;
using UnityEngine;

using System.Numerics;
using DSPLib;


public class SongController : MonoBehaviour {
	int numChannels;
	int numTotalSamples;
	int sampleRate;
	float clipLength;
	float[] multiChannelSamples;
	SpectralFluxAnalyzer preProcessedSpectralFluxAnalyzer;

	AudioSource audioSource;
	public bool preProcessSamples = false;

    public int maxBeatsPerSecond = 2;
    private GameObject orb;
    private float lastTime;
    private float lastPreTime;
    private float lastUpdateTime;
    private int beatsThisSecond = 0;
    private int beatsPreThisSecond = 0;
    public bool onBeat { get; private set; }

    void Start() {
		audioSource = GameObject.Find("GlobalObject").GetComponent<AudioSource>();
		
        onBeat = false;
        orb = GameObject.Find("Orb");

		// Preprocess entire audio file upfront
		if (preProcessSamples) {
			preProcessedSpectralFluxAnalyzer = new SpectralFluxAnalyzer ();

			// Need all audio samples.  If in stereo, samples will return with left and right channels interweaved
			// [L,R,L,R,L,R]
			multiChannelSamples = new float[audioSource.clip.samples * audioSource.clip.channels];
			numChannels = audioSource.clip.channels;
			numTotalSamples = audioSource.clip.samples;
			clipLength = audioSource.clip.length;
            lastTime = 0;

            // We are not evaluating the audio as it is being played by Unity, so we need the clip's sampling rate
            this.sampleRate = audioSource.clip.frequency;

			audioSource.clip.GetData(multiChannelSamples, 0);
			getFullSpectrumThreaded();
			audioSource.Play();
		}
	}

    private float getDuration(float currentTime)
    {
        float duration = clipLength - currentTime;
        if (currentTime - lastTime < 1.0f)
        {
            beatsThisSecond++;
            if (beatsThisSecond >= maxBeatsPerSecond)
            {
                return -1;
            }
        }
        else
        {
            lastTime = currentTime;
            beatsThisSecond = 0;
        }
        return duration;
    }

    private float getPreDuration(float currentTime)
    {
        float duration = clipLength - currentTime;
        if (currentTime - lastPreTime < 1.0f)
        {
            beatsPreThisSecond++;
            if (beatsPreThisSecond >= maxBeatsPerSecond)
            {
                return -1;
            }
        }
        else
        {
            lastPreTime = currentTime;
            beatsPreThisSecond = 0;
        }
        return duration;
    }

    void Update() {
		// Preprocessed
		if (preProcessSamples && audioSource.time != lastUpdateTime) {
            lastUpdateTime = audioSource.time;
            OrbManager orbManager = GameObject.Find("GlobalObject").GetComponent<OrbManager>();
            ItemManager itemManager = GameObject.Find("GlobalObject").GetComponent<ItemManager>();
            int startBeat = getIndexFromTime(audioSource.time + .5f) / 1024;
			if (startBeat >= 0 && startBeat < preProcessedSpectralFluxAnalyzer.spectralFluxSamples.Count && preProcessedSpectralFluxAnalyzer.spectralFluxSamples[startBeat].isPeak)
			{
                float duration = getPreDuration(audioSource.time + 0.5f);
				if (duration >= 0.0f)
				{
					onBeat = true;
					orbManager.onBeatUpdate();
					Invoke(nameof(RecoverOnBeat), 1.0f);
				}
			}

            int indexToPlot = getIndexFromTime(audioSource.time) / 1024;
            if (indexToPlot >= 0 && indexToPlot < preProcessedSpectralFluxAnalyzer.spectralFluxSamples.Count && preProcessedSpectralFluxAnalyzer.spectralFluxSamples[indexToPlot].isPeak)
			{
                float duration = getDuration(audioSource.time);
                if (duration >= 0.0f)
                {
                    orbManager.SpawnOrb();
                    itemManager.SpawnSomething();
					// Zone 1: 247 - 220 0
					// Zone 2: 220 - 182 1
					// Zone 3: 182 - 163 2
					// Zone 5: 163 - 127 1
					// Zone 6: 127 - 107 2
					// Zone 7: 107 - 90 0
					// Zone 8: 90 - 52 1
					// Zone 9: 52 - 15 2
					// Zone 10 : 15 - 0 0
					Debug.Log(duration);
					Debug.Log(orbManager.stage);
					if (duration < 224)
					{
						orbManager.stage = 1;
					}
					if (duration < 186)
                    {
						orbManager.stage = 2;
					}
					if (duration < 167)
                    {
						orbManager.stage = 1;
					}
					if (duration < 131)
					{
						orbManager.stage = 2;
					}
					if (duration < 111)
					{
						orbManager.stage = 0;
					}
					if (duration < 94)
					{
						orbManager.stage = 1;
					}
					if (duration < 56)
					{
						orbManager.stage = 2;
					}
					if (duration < 15)
					{
						orbManager.stage = 0;
					}
				}
			}
		}
	}

    void RecoverOnBeat()
    {
        onBeat = false;
    }

    public int getIndexFromTime(float curTime) {
		float lengthPerSample = this.clipLength / (float)this.numTotalSamples;

		return Mathf.FloorToInt (curTime / lengthPerSample);
	}

	public float getTimeFromIndex(int index) {
		return ((1f / (float)this.sampleRate) * index);
	}

	public void getFullSpectrumThreaded() {
		try {
			// We only need to retain the samples for combined channels over the time domain
			float[] preProcessedSamples = new float[this.numTotalSamples];

			int numProcessed = 0;
			float combinedChannelAverage = 0f;
			for (int i = 0; i < multiChannelSamples.Length; i++) {
				combinedChannelAverage += multiChannelSamples [i];

				// Each time we have processed all channels samples for a point in time, we will store the average of the channels combined
				if ((i + 1) % this.numChannels == 0) {
					preProcessedSamples[numProcessed] = combinedChannelAverage / this.numChannels;
					numProcessed++;
					combinedChannelAverage = 0f;
				}
			}

			// Once we have our audio sample data prepared, we can execute an FFT to return the spectrum data over the time domain
			int spectrumSampleSize = 1024;
			int iterations = preProcessedSamples.Length / spectrumSampleSize;

			FFT fft = new FFT ();
			fft.Initialize ((UInt32)spectrumSampleSize);

			Debug.Log (string.Format("Processing {0} time domain samples for FFT", iterations));
			double[] sampleChunk = new double[spectrumSampleSize];
			for (int i = 0; i < iterations; i++) {
				// Grab the current 1024 chunk of audio sample data
				Array.Copy (preProcessedSamples, i * spectrumSampleSize, sampleChunk, 0, spectrumSampleSize);

				// Apply our chosen FFT Window
				double[] windowCoefs = DSP.Window.Coefficients (DSP.Window.Type.Hanning, (uint)spectrumSampleSize);
				double[] scaledSpectrumChunk = DSP.Math.Multiply (sampleChunk, windowCoefs);
				double scaleFactor = DSP.Window.ScaleFactor.Signal (windowCoefs);

				// Perform the FFT and convert output (complex numbers) to Magnitude
				Complex[] fftSpectrum = fft.Execute (scaledSpectrumChunk);
				double[] scaledFFTSpectrum = DSPLib.DSP.ConvertComplex.ToMagnitude (fftSpectrum);
				scaledFFTSpectrum = DSP.Math.Multiply (scaledFFTSpectrum, scaleFactor);

				// These 1024 magnitude values correspond (roughly) to a single point in the audio timeline
				float curSongTime = getTimeFromIndex(i) * spectrumSampleSize;

				// Send our magnitude data off to our Spectral Flux Analyzer to be analyzed for peaks
				preProcessedSpectralFluxAnalyzer.analyzeSpectrum (Array.ConvertAll (scaledFFTSpectrum, x => (float)x), curSongTime);
			}
		} catch (Exception e) {
			// Catch exceptions here since the background thread won't always surface the exception to the main thread
			Debug.Log (e.ToString ());
		}
	}
}