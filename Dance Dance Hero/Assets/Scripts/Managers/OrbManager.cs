using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    public GameObject orb;
    public float radius { get { return GameObject.Find("EarthRendering").GetComponent<SphereCollider>().radius * 0.6f; } }
    public int scoreForOnBeatHit = 2;
    public int scoreForOffBeatHit = 1;
    public List<Orb> orbs = new List<Orb>();

    public void SpawnOrb()
    {
        float rand = Random.value;
        if (rand < 0.05)
        {
            orbs.Add(Instantiate(orb).GetComponent<Orb>());
        }
    }

    public void onBeatUpdate()
    {
        foreach (Orb orb in orbs)
        {
            orb.onBeatUpdate();
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
