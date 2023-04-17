using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.XRInputSubsystem;

public class OrbManager : MonoBehaviour
{
    public GameObject orb;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        //UnityEngine.XR.XRInputSubsystem

        // heights vary from abt 1.3-1.8 spawn orbs around 2
        // plane spawns about 2 in right and left direction and about 1 in front and back
        // spawn earth at feet at origin with radius between 0.5-0.75
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnOrbs()
    {
        //cam.transform.position.y
    }
}
