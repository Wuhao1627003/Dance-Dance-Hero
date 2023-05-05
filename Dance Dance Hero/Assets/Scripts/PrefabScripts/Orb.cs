using UnityEngine;

public class Orb : Item
{
    public AudioClip hitEarthAudio;
    public int damage = 5;

    private void Awake()
    {
        globalObj = GameObject.Find("GlobalObject");
        float rand = Random.value;
        if (rand < 0.75)
        {
            SpawnDirect();
        }
        else if (rand < 0.95)
        {
            SpawnZigZag();
        }
        else
        {
            SpawnComet();
        }
    }

    public override void FixedUpdate()
    {
        UpdateVA();
        gameObject.transform.localScale = globalObj.GetComponent<OrbManager>().scale * Vector3.one;
    }

    public override void HandleGrab()
    {
        globalObj.GetComponent<OrbManager>().HandlePunch(gameObject.transform.position);
    }

    public override void HandleEarthCollision()
    {
        AudioSource.PlayClipAtPoint(hitEarthAudio, gameObject.transform.position);
        GameObject.Find("Health").GetComponent<Health>().DecreaseHealth(damage);
    }
}
