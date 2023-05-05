using UnityEngine;
using UnityEngine.UI;

public class FinalInfo : MonoBehaviour
{
    public void PrintFinalInfo(bool win)
    {
        GameObject.Find("Score").GetComponent<Text>().enabled = false;
        GameObject.Find("TimeLeft").GetComponent<Text>().enabled = false;
        GameObject.Find("Health").GetComponent<Text>().enabled = false;
        Text infoText = GetComponent<Text>();
        infoText.text = win ? "You WON!!!" : "You FAILED :(";
        infoText.text += "\n Your final score is: " + Score.currScore.ToString();
    }
}