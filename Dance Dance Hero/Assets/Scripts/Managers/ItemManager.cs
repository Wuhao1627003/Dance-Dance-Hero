using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject sun, kryptonite;
    public float recoverTime = 3.0f;
    public Vector3 initialCameraPosition { get; private set; }
    public bool punishOffBeat { get; private set; }
    public bool punishOnBeat { get; private set; }

    void Start()
    {
        punishOffBeat = true;
        punishOnBeat = false;
        initialCameraPosition = GameObject.Find("Main Camera").transform.position;
    }

    public void SpawnSomething()
    {
        float rand = Random.value;
        if (rand < 0.1)
        {
            SpawnSun();
        }
        else if (rand > 0.9)
        {
            SpawnKryptonite();
        }
    }

    void SpawnSun()
    {
        Instantiate(sun);
    }

    void SpawnKryptonite()
    {
        Instantiate(kryptonite);
    }

    public void HandleGrabSun()
    {
        punishOffBeat = false;
        punishOnBeat = false;
        Invoke(nameof(RecoverPunishOffBeat), recoverTime);
    }

    public void HandleGrabKryptonite()
    {
        punishOffBeat = true;
        punishOnBeat = true;
        Invoke(nameof(RecoverPunishOnBeat), recoverTime);
    }

    private void RecoverPunishOffBeat()
    {
        punishOffBeat = true;
    }

    private void RecoverPunishOnBeat()
    {
        punishOnBeat = false;
    }
}
