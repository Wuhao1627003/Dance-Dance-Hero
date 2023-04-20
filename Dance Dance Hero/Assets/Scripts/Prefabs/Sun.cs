using UnityEngine;

public class Sun : Item
{
    public override void HandleGrab()
    {
        GameObject.Find("GlobalObject").GetComponent<ItemManager>().HandleGrabSun();
        Debug.Log("Grabbed Sun");
    }
}
