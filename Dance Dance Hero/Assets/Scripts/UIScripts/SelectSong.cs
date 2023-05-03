using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectSong : MonoBehaviour
{
    public int songSelected;

    void OnTriggerEnter(Collider other)
    {
        SongSelectionController.songSelected = songSelected;
        SceneManager.LoadScene("GamePlay");
    }
}
