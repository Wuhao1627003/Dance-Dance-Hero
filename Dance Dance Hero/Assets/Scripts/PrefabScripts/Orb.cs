using UnityEngine;

public class Orb : Item
{
    // called on beat
    // public void Update()
    // {
    //     transform.position += velocity;
    //     if ((transform.position - GameObject.Find("Main Camera").transform.position).magnitude > 20) {
    //         Destroy(gameObject);
    //     }
    // }

    public override void HandleGrab()
    {
        GameObject.Find("GlobalObject").GetComponent<OrbManager>().HandlePunch();
    }

    public override void HandleEarthCollision()
    {
    }

}
