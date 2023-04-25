using System.Collections.Generic;
using System.Linq;
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

        GameObject.Find("ExternalAudio").GetComponent<AudioSource>().PlayOneShot(spawnAudio);
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
        ItemManager manager = GameObject.Find("GlobalObject").GetComponent<ItemManager>();

        // grabbed Kryptonite
        if (manager.punishOnBeat)
        {
            ColorReturn();
            return;
        }

        orbCol.SetColor("_Color", Color.red);
        orbCol.SetColor("_EmissionColor", new Color(0.368f, 0.075f, 0.087f, 1.0f));

        // haven't grabbed Sun
        if (manager.punishOffBeat)
        {
            Invoke(nameof(ColorReturn), 0.2f);
            return;
        }
        return;
    }

    private void ColorReturn()
    {
        orbCol.SetColor("_Color", Color.magenta);
        orbCol.SetColor("_EmissionColor", new Color(0.086f, 0.075f, 0.368f, 1.0f));
    }

}
