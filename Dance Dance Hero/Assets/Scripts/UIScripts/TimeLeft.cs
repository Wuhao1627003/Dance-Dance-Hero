using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimeLeft : MonoBehaviour
{
    public static long secondsLeft { get; private set; }
    private long lastTime;
    private Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource audio = GameObject.Find("GlobalObject").GetComponent<AudioSource>();
        secondsLeft = (long)audio.clip.length;
        timeText = GetComponent<Text>();
        long minutes = secondsLeft / 60;
        long seconds = secondsLeft % 60;
        timeText.text = "Time left: " + minutes.ToString() + "m " + seconds.ToString() + "s";
        lastTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        long currentTime = System.DateTime.Now.Ticks / System.TimeSpan.TicksPerSecond;
        long elapsedTime = currentTime - lastTime;
        secondsLeft -= elapsedTime;
        lastTime = currentTime;
        long minutes = secondsLeft / 60;
        long seconds = secondsLeft % 60;
        if (secondsLeft <= 0)
        {
            // TODO: End game and show score
            GameObject.Find("FinalInfo").GetComponent<FinalInfo>().PrintFinalInfo(true);
            Invoke(nameof(ReloadScene), 3.0f);
        }
        timeText.text = "Time left: " + minutes.ToString() + "m " + seconds.ToString() + "s";
    }

    void ReloadScene()
    {
        SceneManager.LoadScene("Menu");
    }
}