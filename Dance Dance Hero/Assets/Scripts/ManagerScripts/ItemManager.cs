using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ItemManager : MonoBehaviour
{
    public GameObject sun, kryptonite;
    public float recoverTime = 5.0f;
    
    public Vector3 initialCameraPosition { get; private set; }
    public bool punishOffBeat { get; private set; }
    public bool punishOnBeat { get; private set; }
    public List<Item> items = new List<Item>();

    [SerializeField]
    private PostProcessVolume postfx;
    private ColorGrading cg;
    private Bloom b;

    private OrbManager orbManager;

    void Start()
    {
        punishOffBeat = true;
        punishOnBeat = false;
        initialCameraPosition = GameObject.Find("Main Camera").transform.position;
        postfx = GameObject.Find("PostFX").GetComponent<PostProcessVolume>();
        postfx.profile.TryGetSettings(out cg);
        postfx.profile.TryGetSettings(out b);
        orbManager = GameObject.Find("GlobalObject").GetComponent<OrbManager>();
    }

    public void SpawnSomething()
    {
        float rand = Random.value;
        if (orbManager.stage == 0)
        {
            return;
        } else if (orbManager.stage == 1) 
        {
            if (rand < 0.05)
            {
                if (rand < 0.025)
                {
                    SpawnSun();
                } else
                {
                    SpawnKryptonite();
                }
            }
            return;
        } else
        {
            if (rand < 0.05)
            {
                SpawnSun();
            }
            else if (rand > 0.95)
            {
                SpawnKryptonite();
            }
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
        cg.tint.value = 100;
        b.intensity.value = 20;
        punishOffBeat = false;
        punishOnBeat = false;
        GameObject.Find("Score").GetComponent<Score>().IncreaseScore(1);
        Invoke(nameof(RecoverPunishOffBeat), recoverTime);
    }

    public void HandleGrabKryptonite()
    {
        cg.colorFilter.value = Color.red;
        punishOffBeat = true;
        punishOnBeat = true;
        Invoke(nameof(RecoverPunishOnBeat), recoverTime);
    }

    private void RecoverPunishOffBeat()
    {
        cg.tint.value = 0;
        b.intensity.value = 0;
        punishOffBeat = true;
    }

    private void RecoverPunishOnBeat()
    {
        cg.colorFilter.value = Color.white;
        punishOnBeat = false;
    }
}
