using UnityEngine;

public class Orb : Item
{
    public AudioClip hitEarthAudio;

    public override void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        transform.localScale = GameObject.Find("GlobalObject").GetComponent<OrbManager>().scale * Vector3.one;
    }

    public override void HandleGrab()
    {
        GameObject.Find("GlobalObject").GetComponent<OrbManager>().HandlePunch();
    }

    public override void HandleEarthCollision()
    {
        AudioSource.PlayClipAtPoint(hitEarthAudio, transform.position);
    }

}
