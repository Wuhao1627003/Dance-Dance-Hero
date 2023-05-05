using UnityEngine;

public class Sun : Item
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
            SpawnComet();
        }
    }

    public override void HandleGrab()
    {
        GameObject.Find("GlobalObject").GetComponent<ItemManager>().HandleGrabSun();
    }
}
