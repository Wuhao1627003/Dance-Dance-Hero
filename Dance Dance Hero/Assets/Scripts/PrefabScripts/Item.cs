using UnityEngine;
using UnityEngine.XR;

public enum SpawnMethod
{
    Comet,
    Direct,
    ZigZag
}

public abstract class Item : MonoBehaviour
{
    public float speed = 0.2f;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Vector3 camPos;
    public AudioClip punchAudio;
    public SpawnMethod spawnMethod;

    private GameObject globalObj;

    void Awake()
    {
        float rand = Random.value;
        if (rand < 0.6)
        {
            SpawnDirect();
        }
        else if (rand < 0.8)
        {
            SpawnZigZag();
        }
        else
        {
            SpawnComet();
        }
    }

    public void FixedUpdate()
    {
        velocity += acceleration * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;

        if (spawnMethod == SpawnMethod.ZigZag)
        {
            if (transform.position.x > 1.5f || transform.position.x < -1.5f)
            {
                velocity.x = -velocity.x;
                acceleration.x = -acceleration.x;
            }
        }
    }

    public

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            AudioSource.PlayClipAtPoint(punchAudio, camPos + (transform.position - camPos).normalized * 7, 1.0f);

            InputDevice device = InputDevices.GetDeviceAtXRNode(other.gameObject.name.Contains("Left") ? XRNode.LeftHand : XRNode.RightHand);
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    float amplitude = 1.0f;
                    float duration = 1.0f;
                    device.SendHapticImpulse(channel, amplitude, duration);
                }
            }

            HandleGrab();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Earth"))
        {
            HandleEarthCollision();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (globalObj == null)
        {
            return;
        }

        if (gameObject.GetComponent<Orb>())
        {
            globalObj.GetComponent<OrbManager>().orbs.Remove(gameObject.GetComponent<Orb>());
        }
        else
        {
            globalObj.GetComponent<ItemManager>().items.Remove(gameObject.GetComponent<Item>());
        }
    }

    public abstract void HandleGrab();
    public virtual void HandleEarthCollision() {}

    private void SpawnDirect()
    {
        spawnMethod = SpawnMethod.Direct;
        globalObj = GameObject.Find("GlobalObject");
        float radius = 1.5f;
        float theta = Random.Range(0.0f, 2 / 3 * Mathf.PI);
        camPos = globalObj.GetComponent<ItemManager>().initialCameraPosition;
        transform.position = new(radius * Mathf.Cos(theta), camPos.y + 3.0f, -radius * Mathf.Sin(theta));

        Vector3 playerCenter = new Vector3(camPos.x, camPos.y / 2, camPos.z);
        velocity = (playerCenter - transform.position).normalized * speed;

        acceleration = new Vector3(0, -1, 0);
    }

    private void SpawnZigZag()
    {
        spawnMethod = SpawnMethod.ZigZag;

        globalObj = GameObject.Find("GlobalObject");
        float radius = 1.5f;
        camPos = globalObj.GetComponent<ItemManager>().initialCameraPosition;
        transform.position = new(0, camPos.y + 3.0f, -radius);

        bool toLeft = Random.value < 0.5;
        velocity = (new Vector3(toLeft ? -1 : 1, -1, 1)).normalized * speed;

        acceleration = new Vector3(0, -1, 0);
    }

    private void SpawnComet()
    {
        spawnMethod = SpawnMethod.Comet;

        bool fromLeft = Random.value < 0.5;
        transform.position = new Vector3(fromLeft ? -10 : 10, 0, -3);

        velocity = new Vector3(fromLeft ? 1 : -1, 1, 1).normalized * speed;

        acceleration = new Vector3(0, 2, 0);
    }
}