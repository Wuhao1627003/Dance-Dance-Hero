using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int currScore { get; private set; }
    private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GamePlay")
        {
            scoreText.text = "Score: " + currScore.ToString();
        }
    }

    public void IncreaseScore(int score)
    {
        currScore += score;
    }

    public void ResetScore()
    {
        currScore = 0;
    }
}
