using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
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

            SceneManager.LoadScene("Menu");
        }
    }

    public void ResetScore()
    {
        currHealth = 100;
    }
}
