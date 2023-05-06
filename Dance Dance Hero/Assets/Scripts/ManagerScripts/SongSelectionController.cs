using Unity.VisualScripting;
using UnityEngine;

public class SongSelectionController : MonoBehaviour
{
    public static SongSelectionController singleton = null;

    public static int songSelected;

    private void Awake()
    {
        InitSingleton();
    }

    void InitSingleton()
    {
        if(singleton == null)
        {
            singleton = this;
            DontDestroyOnLoad(this);
        }
        else if(this != singleton)
        {
            Destroy(this.gameObject);
        }
    }
}
