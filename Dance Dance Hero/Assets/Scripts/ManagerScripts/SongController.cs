﻿using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum Performance
{
    Poor,
    Good,
    Perfect
}

public class SongController : MonoBehaviour {
    #region CONST

    // BPM search range
    private const int MIN_BPM = 60;
    private const int MAX_BPM = 400;
    // Base frequency (44.1kbps)
    private const int BASE_FREQUENCY = 44100;
    // Base channels (2ch)
    private const int BASE_CHANNELS = 2;
    // Base split size of sample data (case of 44.1kbps & 2ch)
    private const int BASE_SPLIT_SAMPLE_SIZE = 2205;

    #endregion

    AudioSource audioSource;

    private GameObject orb;
    private float secondsPerBeat, clipLength, timeGood, timePerfect;
    private float currentTime = 0.3f;
    private OrbManager orbManager;
    private ItemManager itemManager;

    public AudioSource beatPlayer;
    public AudioClip one, two, three, poor, good, perfect;

    private GameObject globalObj;
    public AudioClip[] songs;

    public Performance performance { get; private set; }

    void Start() {
        globalObj = GameObject.Find("GlobalObject");

        audioSource = globalObj.GetComponent<AudioSource>();

        audioSource.clip = songs[SongSelectionController.songSelected - 1];

        performance = Performance.Poor;
        orb = GameObject.Find("Orb");
        clipLength = audioSource.clip.length;

        orbManager = globalObj.GetComponent<OrbManager>();
        itemManager = globalObj.GetComponent<ItemManager>();

        // Preprocess entire audio file upfront
        float avgBpm = AnalyzeBpm(audioSource.clip);
        Debug.Log(avgBpm);
        secondsPerBeat = 60.0f / avgBpm;
        timeGood = 0.5f * secondsPerBeat;
        timePerfect = 0.1f * secondsPerBeat;
        audioSource.Play();
    }

    int stageSJLT(float remainingTime)
    {
        // Zone 1: 247 - 220 0
        // Zone 2: 220 - 182 1
        // Zone 3: 182 - 163 2
        // Zone 5: 163 - 127 1
        // Zone 6: 127 - 107 2
        // Zone 7: 107 - 90 0
        // Zone 8: 90 - 52 1
        // Zone 9: 52 - 15 2
        // Zone 10 : 15 - 0 0
        if (remainingTime < 15)
        {
            return 0;
        }
        if (remainingTime < 56)
        {
            return 2;
        }
        if (remainingTime < 94)
        {
            return 1;
        }
        if (remainingTime < 111)
        {
            return 0;
        }
        if (remainingTime < 131)
        {
            return 2;
        }
        if (remainingTime < 167)
        {
            return 1;
        }
        if (remainingTime < 186)
        {
            return 2;
        }
        if (remainingTime < 224)
        {
            return 1;
        }
        return 0;
    }

    int stageUptownFunk(float remainingTime)
    {

        // Zone 1: 270 - 210 0
        // Zone 2: 210 - 180 1
        // Zone 3: 180 - 135 0
        // Zone 5: 135 - 105 1
        // Zone 6: 105 - 65 0
        // Zone 7: 65 - 10 1
        // Zone 10 : 10 - 0 0

        if (remainingTime < 10)
        {
            return 0;
        }
        if (remainingTime < 65)
        {
            return 1;
        }
        if (remainingTime < 105)
        {
            return 0;
        }
        if (remainingTime < 135)
        {
            return 1;
        }
        if (remainingTime < 180)
        {
            return 0;
        }
        if (remainingTime < 210)
        {
            return 1;
        }
        return 0;
    }

    int stageCosmicGirl(float remainingTime)
    {
        // TODO
        return 0;
    }

    int stageHero(float remainingTime)
    {
        // Zone 1: 196 - 166 2
        // Zone 2: 166 - 126 0
        // Zone 3: 126 - 82 1
        // Zone 5: 82 - 56 0
        // Zone 6: 56 - 36 2
        // Zone 7: 36 - 21 0
        // Zone 8: 21 - 5 2
        // Zone 9: 5 - 0 0

        if (remainingTime < 5)
        {
            return 0;
        }
        if (remainingTime < 21)
        {
            return 2;
        }
        if (remainingTime < 36)
        {
            return 0;
        }
        if (remainingTime < 56)
        {
            return 2;
        }
        if (remainingTime < 82)
        {
            return 0;
        }
        if (remainingTime < 126)
        {
            return 1;
        }
        if (remainingTime < 166)
        {
            return 0;
        }
        return 2;
    }

    int stageCloser(float remainingTime)
    {
        // Zone 1: 245 - 215 0
        // Zone 2: 215 - 194 1
        // Zone 3: 194 - 155 2
        // Zone 5: 155 - 135 0
        // Zone 6: 135 - 115 1
        // Zone 7: 115 - 75 2
        // Zone 8: 75 - 40 1
        // Zone 9: 40 - 15 2
        // Zone 10 : 15 - 0 0
        if (remainingTime < 15)
        {
            return 0;
        }
        if (remainingTime < 40)
        {
            return 2;
        }
        if (remainingTime < 75)
        {
            return 1;
        }
        if (remainingTime < 115)
        {
            return 2;
        }
        if (remainingTime < 135)
        {
            return 1;
        }
        if (remainingTime < 155)
        {
            return 0;
        }
        if (remainingTime < 194)
        {
            return 2;
        }
        if (remainingTime < 215)
        {
            return 1;
        }
        return 0;
    }

    void FixedUpdate() {
        currentTime += Time.fixedDeltaTime;
        if ((currentTime + timePerfect) % secondsPerBeat < Time.fixedDeltaTime)
        {
            performance = Performance.Perfect;
            Invoke(nameof(Good), timePerfect * 2);
            Invoke(nameof(Poor), timePerfect + timeGood);
            beatPlayer.Play();
            orbManager.onBeatUpdate();
        }

        if (currentTime % secondsPerBeat < Time.fixedDeltaTime)
        {
            orbManager.SpawnOrb();
            itemManager.SpawnSomething();
            
            float remainingTime = clipLength - audioSource.time;
            switch (SongSelectionController.songSelected)
            {
                case 0:
                    orbManager.stage = stageSJLT(remainingTime);
                    break;
                case 1:
                    orbManager.stage = stageUptownFunk(remainingTime);
                    break;
                default:
                    orbManager.stage = stageCosmicGirl(remainingTime);
                    break;
            }
        }
    }

    void Poor()
    {
        performance = Performance.Poor;
    }

    void Good()
    {
        performance = Performance.Good;
    }

    #region BPM
    public struct BpmMatchData
    {
        public int bpm;
        public float match;
    }

    private static BpmMatchData[] bpmMatchDatas = new BpmMatchData[MAX_BPM - MIN_BPM + 1];

    /// <summary>
    /// Analyze BPM from an audio clip
    /// </summary>
    /// <param name="clip">target audio clip</param>
    /// <returns>bpm</returns>
    public static int AnalyzeBpm(AudioClip clip)
    {
        for (int i = 0; i < bpmMatchDatas.Length; i++)
        {
            bpmMatchDatas[i].match = 0f;
        }
        if (clip == null)
        {
            return -1;
        }

        int frequency = clip.frequency;

        int channels = clip.channels;

        int splitFrameSize = Mathf.FloorToInt(((float)frequency / (float)BASE_FREQUENCY) * ((float)channels / (float)BASE_CHANNELS) * (float)BASE_SPLIT_SAMPLE_SIZE);

        // Get all sample data from audioclip
        var allSamples = new float[clip.samples * channels];
        clip.GetData(allSamples, 0);

        // Create volume array from all sample data
        var volumeArr = CreateVolumeArray(allSamples, frequency, channels, splitFrameSize);

        // Search bpm from volume array
        int bpm = SearchBpm(volumeArr, frequency, splitFrameSize);

        return bpm;
    }

    /// <summary>
    /// Create volume array from all sample data
    /// </summary>
    private static float[] CreateVolumeArray(float[] allSamples, int frequency, int channels, int splitFrameSize)
    {
        // Initialize volume array
        var volumeArr = new float[Mathf.CeilToInt((float)allSamples.Length / (float)splitFrameSize)];
        int powerIndex = 0;

        // Sample data analysis start
        for (int sampleIndex = 0; sampleIndex < allSamples.Length; sampleIndex += splitFrameSize)
        {
            float sum = 0f;
            for (int frameIndex = sampleIndex; frameIndex < sampleIndex + splitFrameSize; frameIndex++)
            {
                if (allSamples.Length <= frameIndex)
                {
                    break;
                }
                // Use the absolute value, because left and right value is -1 to 1
                float absValue = Mathf.Abs(allSamples[frameIndex]);
                if (absValue > 1f)
                {
                    continue;
                }

                // Calculate the amplitude square sum
                sum += (absValue * absValue);
            }

            // Set volume value
            volumeArr[powerIndex] = Mathf.Sqrt(sum / splitFrameSize);
            powerIndex++;
        }

        // Representing a volume value from 0 to 1
        float maxVolume = volumeArr.Max();
        for (int i = 0; i < volumeArr.Length; i++)
        {
            volumeArr[i] = volumeArr[i] / maxVolume;
        }

        return volumeArr;
    }

    /// <summary>
    /// Search bpm from volume array
    /// </summary>
    private static int SearchBpm(float[] volumeArr, int frequency, int splitFrameSize)
    {
        // Create volume diff list
        var diffList = new List<float>();
        for (int i = 1; i < volumeArr.Length; i++)
        {
            diffList.Add(Mathf.Max(volumeArr[i] - volumeArr[i - 1], 0f));
        }

        // Calculate the degree of coincidence in each BPM
        int index = 0;
        float splitFrequency = (float)frequency / (float)splitFrameSize;
        for (int bpm = MIN_BPM; bpm <= MAX_BPM; bpm++)
        {
            float sinMatch = 0f;
            float cosMatch = 0f;
            float bps = (float)bpm / 60f;

            if (diffList.Count() > 0)
            {
                for (int i = 0; i < diffList.Count; i++)
                {
                    sinMatch += (diffList[i] * Mathf.Cos(i * 2f * Mathf.PI * bps / splitFrequency));
                    cosMatch += (diffList[i] * Mathf.Sin(i * 2f * Mathf.PI * bps / splitFrequency));
                }

                sinMatch *= (1f / (float)diffList.Count);
                cosMatch *= (1f / (float)diffList.Count);
            }

            float match = Mathf.Sqrt((sinMatch * sinMatch) + (cosMatch * cosMatch));

            bpmMatchDatas[index].bpm = bpm;
            bpmMatchDatas[index].match = match;
            index++;
        }

        // Returns a high degree of coincidence BPM
        int matchIndex = Array.FindIndex(bpmMatchDatas, x => x.match == bpmMatchDatas.Max(y => y.match));

        return bpmMatchDatas[matchIndex].bpm;
    }
    #endregion
}