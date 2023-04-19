using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatProvider : MonoBehaviour
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
        AudioProcessor processor = GameObject.Find("GlobalObject").GetComponent<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);
        processor.onSpectrum.AddListener(onSpectrum);
        startTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
        lastTime = startTime;
        onBeat = false;
        orb = GameObject.Find("Orb");
    }

    private long getDuration()
    {
        long currentTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
        long duration = currentTime - startTime;
        if (currentTime == lastTime)
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
    void onOnbeatDetected()
    {
        long duration = getDuration();
        if (duration != -1) {
            Debug.Log("Duration: " + duration.ToString() + " Beats this second: " + beatsThisSecond.ToString());
            //sphere.transform.localScale *= 2;
            orb.GetComponent<Orb>().onBeatUpdate();
            onBeat = true;
            Invoke(nameof(RecoverOnBeat), 0.2f);
        }
    }

    //This event will be called every frame while music is playing
    void onSpectrum(float[] spectrum)
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
