using UnityEngine;

public class SunKryptoManager : MonoBehaviour
{
    public GameObject sun, kryptonite;
    public float recoverTime = 3.0f;
    public bool punishOffBeat { get; private set; }
    public bool punishOnBeat { get; private set; }

    void Start()
    {
        punishOffBeat = true;
        punishOnBeat = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnSun()
    {

    }

    void SpawnKryptonite()
    {

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
