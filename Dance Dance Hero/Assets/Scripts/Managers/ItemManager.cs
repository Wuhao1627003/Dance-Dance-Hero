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
        if (rand < 0.05)
        {
            SpawnSun();
        }
        else if (rand > 0.95)
        {
            SpawnKryptonite();
        }
    }

    public void onBeatUpdate()
    {
        // foreach (Item item in items)
        // {
        //     item.onBeatUpdate();
        // }
    }

    void SpawnSun()
    {
        items.Add(Instantiate(sun).GetComponent<Item>());
    }

    void SpawnKryptonite()
    {
        items.Add(Instantiate(kryptonite).GetComponent<Item>());
    }

    public void HandleGrabSun()
    {
        cg.colorFilter.value = Color.magenta;
        punishOffBeat = false;
        punishOnBeat = false;
        GameObject.Find("Score").GetComponent<Score>().IncreaseScore(1);
        Invoke(nameof(RecoverPunishOffBeat), recoverTime);
        Invoke(nameof(RecoverScreenColor), recoverTime);
    }

    public void HandleGrabKryptonite()
    {
        cg.colorFilter.value = Color.red;
        punishOffBeat = true;
        punishOnBeat = true;
        Invoke(nameof(RecoverPunishOnBeat), recoverTime);
        Invoke(nameof(RecoverScreenColor), recoverTime);
    }

    private void RecoverPunishOffBeat()
    {
        punishOffBeat = true;
    }

    private void RecoverPunishOnBeat()
    {
        punishOnBeat = false;
    }
    
    private void RecoverScreenColor()
    {
        cg.colorFilter.value = Color.white;
    }
}
