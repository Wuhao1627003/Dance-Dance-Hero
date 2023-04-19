using UnityEngine;

public class Sun : Item
{
    public override void HandleItem()
    {
        GameObject.Find("GlobalObject").GetComponent<ItemManager>().HandleGrabSun();
        Debug.Log("Grabbed Sun");
    }
}
