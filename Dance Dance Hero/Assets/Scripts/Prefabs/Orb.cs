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

    MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void changeColor()
    {
        Color randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        meshRenderer.material.SetColor("_Color", randomColor);
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
