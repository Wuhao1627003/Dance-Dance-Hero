using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ItemManager : MonoBehaviour
{
    public GameObject sun, kryptonite;
    public float recoverTime = 3.0f;
    
    public Vector3 initialCameraPosition { get; private set; }
    public bool punishOffBeat { get; private set; }
    public bool punishOnBeat { get; private set; }
    public List<Item> items = new List<Item>();

    [SerializeField]
    private PostProcessVolume postfx;
    private ColorGrading cg;

    void Start()
    {
        punishOffBeat = true;
        punishOnBeat = false;
        initialCameraPosition = GameObject.Find("Main Camera").transform.position;
        postfx = GameObject.Find("PostFX").GetComponent<PostProcessVolume>();
        postfx.profile.TryGetSettings(out cg);
    }

    public void SpawnSomething()
    {
        float rand = Random.value;
        if (rand < 0.1)
        {
            SpawnSun();
        }
        else if (rand > 0.9)
        {
            SpawnKryptonite();
        }
    }

    void SpawnSun()
    {
        items.Add(Instantiate(sun).GetComponent<Sun>());
    }

    void SpawnKryptonite()
    {
        items.Add(Instantiate(kryptonite).GetComponent<Kryptonite>());
    }

    public void HandleGrabSun()
    {
        cg.colorFilter.value = Color.red;
        punishOffBeat = false;
        punishOnBeat = false;
        GameObject.Find("Score").GetComponent<Score>().IncreaseScore(1);
        Invoke(nameof(RecoverPunishOffBeat), recoverTime);
    }

    public void HandleGrabKryptonite()
    {
        cg.colorFilter.value = Color.green;
        punishOffBeat = true;
        punishOnBeat = true;
        Invoke(nameof(RecoverPunishOnBeat), recoverTime);
    }

    private void RecoverPunishOffBeat()
    {
        cg.colorFilter.value = Color.white;
        punishOffBeat = true;
    }

    private void RecoverPunishOnBeat()
    {
        cg.colorFilter.value = Color.white;
        punishOnBeat = false;
    }
}
