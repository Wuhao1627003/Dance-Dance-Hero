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
    protected Vector3 velocity;
    protected Vector3 acceleration;
    protected Vector3 camPos;
    public AudioClip punchAudio;
    protected SpawnMethod spawnMethod;

    protected GameObject globalObj;

    public virtual void FixedUpdate()
    {
        UpdateVA();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            AudioSource.PlayClipAtPoint(punchAudio, camPos + (transform.position - camPos).normalized * 7, .55f);

            InputDevice device = InputDevices.GetDeviceAtXRNode(other.gameObject.name.Contains("Left") ? XRNode.LeftHand : XRNode.RightHand);
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    float amplitude = 1.0f;
                    float duration = 0.2f;
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

    protected void UpdateVA()
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
        else if (spawnMethod == SpawnMethod.Comet)
        {
            if (transform.position.x > 5f || transform.position.x < -5f)
            {
                Destroy(gameObject);
            }
        }
    }

    protected void SpawnDirect()
    {
        spawnMethod = SpawnMethod.Direct;

        globalObj = GameObject.Find("GlobalObject");
        float radius = 1.5f;
        float theta = Random.Range(0.0f, 2.0f / 3 * Mathf.PI);
        camPos = globalObj.GetComponent<ItemManager>().initialCameraPosition;
        transform.position = new(camPos.x + radius * Mathf.Cos(theta), camPos.y + 1.0f, camPos.z - radius * Mathf.Sin(theta));

        Vector3 playerCenter = new Vector3(camPos.x, camPos.y * 3.0f / 4, camPos.z);
        velocity = (playerCenter - transform.position).normalized * speed;

        acceleration = new Vector3(0, -0.1f, 0);
    }

    protected void SpawnZigZag()
    {
        spawnMethod = SpawnMethod.ZigZag;

        globalObj = GameObject.Find("GlobalObject");
        float radius = 1.5f;
        camPos = globalObj.GetComponent<ItemManager>().initialCameraPosition;
        transform.position = new(0, camPos.y + 1.0f, -radius);

        bool toLeft = Random.value < 0.5;
        velocity = new Vector3(toLeft ? -2 : 2, -1, 1).normalized * speed;

        acceleration = new Vector3(0, -0.1f, 0);
    }

    protected void SpawnComet()
    {
        spawnMethod = SpawnMethod.Comet;

        bool fromLeft = Random.value < 0.5;
        transform.position = new Vector3(fromLeft ? -5 : 5, camPos.y + .5f, -0.5f);

        velocity = new Vector3(fromLeft ? 1 : -1, 0, 0).normalized * speed * 3;

        acceleration = Vector3.zero;
    }
}