using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Got here 1");

        if (other.CompareTag("GameController"))
        {
            Debug.Log("Got here 2");
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
