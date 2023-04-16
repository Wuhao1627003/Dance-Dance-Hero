using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatProvider : MonoBehaviour
{
    public int maxBeatsPerSecond = 2;
    private GameObject sphere;
    private long startTime;
    private long lastTime;
    private int beatsThisSecond = 0;

    void Start()
    {
        //Select the instance of AudioProcessor and pass a reference
        //to this object
        AudioProcessor processor = GameObject.Find("GlobalObject").GetComponent<AudioProcessor>();
        processor.onBeat.AddListener(onOnbeatDetected);
        processor.onSpectrum.AddListener(onSpectrum);
        startTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
        lastTime = startTime;
        sphere = GameObject.Find("Sphere");
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
            sphere.transform.position += Vector3.down;
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
}
