using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public GameObject orb;
    public float radius { get { return GameObject.Find("EarthRendering").GetComponent<SphereCollider>().radius * 0.6f; } }
    public int scorePerfect = 10, scoreGood = 5, scorePoor = 1;
    public List<Orb> orbs = new List<Orb>();
    public Material orbCol;
    public AudioClip spawnAudio;
    public int stage = 0;
    public float scale = 1;

    public void SpawnOrb()
    {
        float rand = Random.value;
        if (stage == 0)
        {
            if (rand < 0.5)
            {
                orbs.Add(Instantiate(orb).GetComponent<Orb>());
            }
        }
        else if (stage == 1)
        {
            if (rand < 0.9)
            {
                orbs.Add(Instantiate(orb).GetComponent<Orb>());
            }

        }
        else
        {
            orbs.Add(Instantiate(orb).GetComponent<Orb>());

            if (rand < 0.1)
            {
                orbs.Add(Instantiate(orb).GetComponent<Orb>());
            }
        }
    }

    public void onBeatUpdate()
    {
        Pulse();
    }

    public void HandlePunch()
    {
        int score = scorePoor;
        ItemManager manager = GameObject.Find("GlobalObject").GetComponent<ItemManager>();
        Performance performance = GameObject.Find("GlobalObject").GetComponent<SongController>().performance;

        if (manager.punishOnBeat)
        {
            performance = Performance.Poor;
        }
        if (!manager.punishOffBeat)
        {
            performance = Performance.Perfect;
        }

        switch (performance)
        {
            case Performance.Poor:
                score = scorePoor;
                break;
            case Performance.Good:
                score = scoreGood;
                break;
            case Performance.Perfect:
                score = scorePerfect;
                break;
        }
        Debug.Log(score);
        GameObject.Find("Score").GetComponent<Score>().IncreaseScore(score);
    }
    public void Pulse()
    {
        ItemManager manager = GameObject.Find("GlobalObject").GetComponent<ItemManager>();

        // grabbed Kryptonite
        if (manager.punishOnBeat)
        {
            scale = 1;
            ReturnOffBeat();
            return;
        }

        scale = 1.3f;
        // haven't grabbed Sun
        if (manager.punishOffBeat)
        {
            orbCol.SetColor("_Color", Color.red);
            orbCol.SetColor("_EmissionColor", new Color(0.368f, 0.075f, 0.087f, 1.0f));
            Invoke(nameof(ReturnOffBeat), 0.2f);
            return;
        }

        orbCol.SetColor("_Color", Color.red);
        orbCol.SetColor("_EmissionColor", new Color(0.368f, 0.075f, 0.087f, 1.0f));
    }

    private void ReturnOffBeat()
    {
        scale = 1;
        orbCol.SetColor("_Color", Color.magenta);
        orbCol.SetColor("_EmissionColor", new Color(0.086f, 0.075f, 0.368f, 1.0f));
    }

}
