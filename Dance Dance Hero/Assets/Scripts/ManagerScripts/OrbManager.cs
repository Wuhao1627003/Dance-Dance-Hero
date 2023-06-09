using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public GameObject orb;
    public float radius { get { return GameObject.Find("EarthRendering").GetComponent<SphereCollider>().radius * 0.6f; } }
    public int scorePerfect = 10, scoreGood = 5, scorePoor = 1;
    public List<Orb> orbs = new List<Orb>();
    public Material orbCol;
    public int stage = 0;
    public float scale = 0.5f;

    public GameObject[] hitEffects;

    private GameObject globalObj;

    public Color orbDefault = new Color(1.0f, 0.8165964f, 0.0f, 1.0f);
    public Color orbBeat = new Color(0.9712478f, 1.0f, 0.0f, 1.0f);

    private void Start()
    {
        orbCol.SetColor("_MainColor", orbDefault);
        globalObj = GameObject.Find("GlobalObject");
    }

    public void SpawnOrb()
    {
        float rand = Random.value;
        if (stage == 0 && rand < 0.5) CreateOrb();
        else if (stage == 1 && rand < 0.65) CreateOrb();
        else if (stage == 2)
        {
            if (rand < 0.7)
            {
                if (rand < 0.25)
                {
                    CreateOrb();
                    CreateOrb();
                }
                else
                {
                    CreateOrb();
                }
            }

        }
    }

    private void CreateOrb()
    {
        GameObject newOrb = Instantiate(orb);
        newOrb.transform.localScale = scale * Vector3.one;
        orbs.Add(newOrb.GetComponent<Orb>());
    }

    public void onBeatUpdate()
    {
        Pulse();
    }

    public void HandlePunch(Vector3 orbPos)
    {
        int score = scorePoor;
        ItemManager manager = globalObj.GetComponent<ItemManager>();
        Performance performance = globalObj.GetComponent<SongController>().performance;

        if (manager.punishOnBeat)
        {
            performance = Performance.Poor;
        }
        if (!manager.punishOffBeat)
        {
            performance = Performance.Perfect;
        }

        GameObject hitEffect = hitEffects[0];
        switch (performance)
        {
            case Performance.Poor:
                score = scorePoor;
                hitEffect = hitEffects[0];
                break;
            case Performance.Good:
                score = scoreGood;
                hitEffect = hitEffects[1];
                break;
            case Performance.Perfect:
                score = scorePerfect;
                hitEffect = hitEffects[2];
                break;
        }

        GameObject effect = Instantiate(hitEffect, orbPos, Quaternion.identity);
        Destroy(effect, 0.75f);

        GameObject.Find("Score").GetComponent<Score>().IncreaseScore(score);
    }
    public void Pulse()
    {
        ItemManager manager = globalObj.GetComponent<ItemManager>();

        // grabbed Kryptonite
        if (manager.punishOnBeat)
        {
            scale = 0.5f;
            ReturnOffBeat();
            return;
        }

        scale = 0.7f;
        // haven't grabbed Sun
        if (manager.punishOffBeat)
        {
            orbCol.SetColor("_MainColor", orbBeat);
            Invoke(nameof(ReturnOffBeat), 0.1f);
            return;
        }

        orbCol.SetColor("_MainColor", orbBeat);
    }

    private void ReturnOffBeat()
    {
        scale = 0.5f;
        orbCol.SetColor("_MainColor", orbDefault);
    }

}
