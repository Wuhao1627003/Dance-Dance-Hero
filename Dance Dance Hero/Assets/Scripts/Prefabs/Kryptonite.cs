using UnityEngine;

public class Kryptonite : Item
{
    public override void HandleItem()
    {
        GameObject.Find("GlobalObject").GetComponent<SunKryptoManager>().HandleGrabKryptonite();
    }
}
