using UnityEngine;

public class Kryptonite : Item
{
    public override void HandleGrab()
    {
        GameObject.Find("GlobalObject").GetComponent<ItemManager>().HandleGrabKryptonite();
    }
}
