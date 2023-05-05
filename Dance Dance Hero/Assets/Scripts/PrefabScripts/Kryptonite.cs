using UnityEngine;

public class Kryptonite : Item
{
    private void Awake()
    {
        globalObj = GameObject.Find("GlobalObject");
        float rand = Random.value;
        if (rand < 0.4)
        {
            SpawnDirect();
        }
        else
        {
            SpawnZigZag();
        }
    }

    public override void HandleGrab()
    {
        GameObject.Find("GlobalObject").GetComponent<ItemManager>().HandleGrabKryptonite();
    }
}
