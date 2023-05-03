using UnityEngine;
using UnityEngine.XR;

public abstract class Item : MonoBehaviour
{
    public bool shootAtPlayer = false;
    public float speed = 0.2f;
    public Vector3 velocity = Vector3.down;
    private Vector3 camPos;
    public AudioClip punchAudio;

    private GameObject globalObj;

    void Awake()
    {
        globalObj = GameObject.Find("GlobalObject");
        float radius = 1.5f;
        Vector2 randomPointOnCircle = Random.insideUnitCircle.normalized * radius;
        camPos = globalObj.GetComponent<ItemManager>().initialCameraPosition;
        transform.position = new(randomPointOnCircle.x, camPos.y + 3.0f, -1 * Mathf.Abs(randomPointOnCircle.y));

        if (shootAtPlayer)
        {
            Vector3 playerCenter = new Vector3(camPos.x, camPos.y / 2, camPos.z);
            velocity = (playerCenter - transform.position).normalized;
        }

        velocity *= speed;
    }

    public virtual void FixedUpdate()
    {
        transform.position += velocity * Time.fixedDeltaTime;
    }

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
}