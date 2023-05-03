using UnityEngine;

public class Orb : Item
{
    public AudioClip hitEarthAudio;
    public int damage = 5;

    private GameObject globalObj;

    private void Start()
    {
        globalObj = GameObject.Find("GlobalObject");
    }

    public override void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        transform.localScale = globalObj.GetComponent<OrbManager>().scale * Vector3.one;
    }

    public override void HandleGrab()
    {
        globalObj.GetComponent<OrbManager>().HandlePunch();
    }

    public override void HandleEarthCollision()
    {
        AudioSource.PlayClipAtPoint(hitEarthAudio, transform.position);
        GameObject.Find("Health").GetComponent<Health>().DecreaseHealth(damage);
    }
}
