using UnityEngine;

public class Orb : Item
{
    public override void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
        transform.localScale = GameObject.Find("GlobalObject").GetComponent<OrbManager>().scale * Vector3.one;
    }

    public override void HandleGrab()
    {
        GameObject.Find("GlobalObject").GetComponent<OrbManager>().HandlePunch();
        //Debug.Log("Hit Orb");
    }

    public override void HandleEarthCollision()
    {
        //Debug.Log("Hit Earth!");
    }

}
