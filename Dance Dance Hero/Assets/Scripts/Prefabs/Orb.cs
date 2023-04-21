using UnityEngine;

public class Orb : Item
{
    // called on beat
    public override void onBeatUpdate()
    {
        transform.position += velocity;
        if ((transform.position - GameObject.Find("Main Camera").transform.position).magnitude > 20) {
            Destroy(gameObject);
        }
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
