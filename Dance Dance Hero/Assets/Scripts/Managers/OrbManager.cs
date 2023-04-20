using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.XRInputSubsystem;

public class OrbManager : MonoBehaviour
{
    public GameObject orb;
    public float radius { get { return GameObject.Find("EarthRendering").GetComponent<SphereCollider>().radius * 0.6f; } }
    private Camera cam;
    public int scoreForOnBeatHit = 2;
    public int scoreForOffBeatHit = 1;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //UnityEngine.XR.XRInputSubsystem

        // heights vary from abt 1.3-1.8 spawn orbs around 2
        // plane spawns about 2 in right and left direction and about 1 in front and back
        // spawn earth at feet at origin with radius between 0.5-0.75

        // Zone 1: until 3min 40 seconds
        // Zone 2: until 3 min 1 second
        // Zone 3: until 2 min 43
        // Zone 4: until 2 min 25
        // Zone 5: until 2 min 6
        // Zone 6: until 1 min 46
        // Zone 7: until 1 min 10
        // Zone 8: until 50s
        // 5 seconds of silence at beginning
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnOrb()
    {
        float rand = Random.value;
        if (rand < 0.5)
        {
            Instantiate(orb);
        }
    }

    public void HandlePunch()
    {
        int score;
        ItemManager manager = GameObject.Find("GlobalObject").GetComponent<ItemManager>();
        bool onBeat = GameObject.Find("GlobalObject").GetComponent<BeatProvider>().onBeat;
        if (onBeat)
        {
            score = manager.punishOnBeat ? scoreForOffBeatHit : scoreForOnBeatHit;
        }
        else
        {
            score = manager.punishOffBeat ? scoreForOffBeatHit : scoreForOnBeatHit;
        }
        GameObject.Find("Score").GetComponent<Score>().IncreaseScore(score);
    }
}
