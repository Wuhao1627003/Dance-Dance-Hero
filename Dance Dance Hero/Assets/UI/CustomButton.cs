using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomButton : MonoBehaviour
{

    public string sceneToLoad;

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
        if (other.CompareTag("GameController"))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
