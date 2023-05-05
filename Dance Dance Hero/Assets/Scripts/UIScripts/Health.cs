using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Canvas GUICanvas;
    public static int currHealth { get; private set; }
    private Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        healthText = GetComponent<Text>();
        currHealth = 100;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + currHealth.ToString();
    }

    public void DecreaseHealth(int damage)
    {
        currHealth -= damage;

        if (currHealth <= 0)
        {
            currHealth = 0;
            // TODO: Game over
            GUICanvas.gameObject.SetActive(true);
            SceneManager.LoadScene("Menu");
        }
    }

    public void ResetScore()
    {
        currHealth = 100;
    }

    private void OnGUI()
    {
        string displayText = "You lost :( \n Final score: " + Score.currScore.ToString();
        GUI.Box(new Rect(0, 0, 10, 10), displayText);
    }
}
