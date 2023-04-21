using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AudioProcessor;

public class BeatProvider : MonoBehaviour, AudioCallbacks
{
    public int maxBeatsPerSecond = 2;
    private GameObject orb;
    private long startTime;
    private long lastTime;
    private int beatsThisSecond = 0;
    public bool onBeat {get; private set;}

    void Start()
    {
        //Select the instance of AudioProcessor and pass a reference
        //to this object
        AudioProcessor processor = GameObject.Find("Main Camera").GetComponent<AudioProcessor>();
        processor.addAudioCallback(this);
        startTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
        lastTime = startTime;
        onBeat = false;
        orb = GameObject.Find("Orb");
    }

    private long getDuration()
    {
        long currentTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerMillisecond;
        long duration = currentTime - startTime;
        if (currentTime - lastTime < 1000)
        {
            beatsThisSecond++;
            if (beatsThisSecond > maxBeatsPerSecond)
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

    //this event will be called every time a beat is detected.
    //Change the threshold parameter in the inspector
    //to adjust the sensitivity
    void AudioCallbacks.onOnbeatDetected()
    {
        long duration = getDuration();
        if (duration != -1) {
            Debug.Log("Duration: " + duration.ToString() + " Beats this second: " + beatsThisSecond.ToString());
            
            onBeat = true;
            Invoke(nameof(RecoverOnBeat), 0.2f);

            OrbManager orbManager = GameObject.Find("GlobalObject").GetComponent<OrbManager>();
            orbManager.SpawnOrb();
            orbManager.onBeatUpdate();

            ItemManager itemManager = GameObject.Find("GlobalObject").GetComponent<ItemManager>();
            itemManager.SpawnSomething();
            itemManager.onBeatUpdate();
        }
    }

    //This event will be called every frame while music is playing
    void AudioCallbacks.onSpectrum(float[] spectrum)
    {
        //The spectrum is logarithmically averaged
        //to 12 bands

        for (int i = 0; i < spectrum.Length; ++i)
        {
            Vector3 start = new Vector3(i, 0, 0);
            Vector3 end = new Vector3(i, spectrum[i], 0);
            Debug.DrawLine(start, end);
        }
    }

    void RecoverOnBeat()
    {
        onBeat = false;
    }
}
