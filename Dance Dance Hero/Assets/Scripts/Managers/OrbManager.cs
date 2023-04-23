using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public GameObject orb;
    public float radius { get { return GameObject.Find("EarthRendering").GetComponent<SphereCollider>().radius * 0.6f; } }
    public int scoreForOnBeatHit = 2;
    public int scoreForOffBeatHit = 1;
    public List<Orb> orbs = new List<Orb>();
    public Material orbCol;
    public AudioClip spawnAudio;
    public int stage = 0;

    void Start()
    {
        //UnityEngine.XR.XRInputSubsystem

        // heights vary from abt 1.3-1.8 spawn orbs around 2
        // plane spawns about 2 in right and left direction and about 1 in front and back
        // spawn earth at feet at origin with radius between 0.5-0.75

        // Zone 1: 247 - 220 0
        // Zone 2: 220 - 182 1
        // Zone 3: 182 - 163 2
        // Zone 5: 163 - 127 1
        // Zone 6: 127 - 107 2
        // Zone 7: 107 - 90 0
        // Zone 8: 90 - 52 1
        // Zone 9: 52 - 15 2
        // Zone 10 : 15 - 0 0
    }

    public void SpawnOrb()
    {
        float rand = Random.value;
        if (stage == 0)
        {
            if (rand < 0.5)
            {
                orbs.Add(Instantiate(orb).GetComponent<Orb>());
                //AudioSource audioSource = new AudioSource();
                //audioSource.PlayOneShot(spawnAudio);
            }
        } else if (stage == 1)
        {
            if (rand < 0.9)
            {
                orbs.Add(Instantiate(orb).GetComponent<Orb>());
                //AudioSource audioSource = new AudioSource();
                //audioSource.PlayOneShot(spawnAudio);
            }

        } else
        {
            orbs.Add(Instantiate(orb).GetComponent<Orb>());
        }
    }

    public void onBeatUpdate()
    {
        Pulse();
    }

    public void HandlePunch()
    {
        int score;
        ItemManager manager = GameObject.Find("GlobalObject").GetComponent<ItemManager>();
        bool onBeat = GameObject.Find("GlobalObject").GetComponent<SongController>().onBeat;
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
    public void Pulse()
    {
        orbCol.SetColor("_Color", Color.red);
        orbCol.SetColor("_EmissionColor", new Color(0.368f, 0.075f, 0.087f, 1.0f));

        Invoke(nameof(ColorReturn), 0.2f);
    }

    private void ColorReturn()
    {
        orbCol.SetColor("_Color", Color.magenta);
        orbCol.SetColor("_EmissionColor", new Color(0.086f, 0.075f, 0.368f, 1.0f));
    }

}
