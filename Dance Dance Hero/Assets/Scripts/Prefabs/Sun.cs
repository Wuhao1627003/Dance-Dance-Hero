using UnityEngine;

public class Sun : Item
{
    public override void HandleItem()
    {
        GameObject.Find("GlobalObject").GetComponent<SunKryptoManager>().HandleGrabSun();
    }
}
