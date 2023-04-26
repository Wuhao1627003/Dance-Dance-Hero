using UnityEngine;
using UnityEngine.XR;

public abstract class Item : MonoBehaviour
{
    public bool shootAtPlayer = false;
    public float speed = 0.2f;
    public Vector3 velocity = Vector3.down;

    void Awake()
    {
        float radius = GameObject.Find("GlobalObject").GetComponent<OrbManager>().radius;
        Vector2 randomPointOnCircle = Random.insideUnitCircle * radius * 1.2f;
        Vector3 camPos = GameObject.Find("GlobalObject").GetComponent<ItemManager>().initialCameraPosition;
        transform.position = new(randomPointOnCircle.x, camPos.y + 1.0f, randomPointOnCircle.y);

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
            InputDevice device = InputDevices.GetDeviceAtXRNode(other.gameObject.name.Contains("Left") ? XRNode.LeftHand : XRNode.RightHand);
            HapticCapabilities capabilities;
            if (device.TryGetHapticCapabilities(out capabilities))
            {
                if (capabilities.supportsImpulse)
                {
                    uint channel = 0;
                    float amplitude = 0.7f;
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
        var go = GameObject.Find("GlobalObject");

        if (go == null)
        {
            return;
        }

        if (gameObject.GetComponent<Orb>())
        {
            go.GetComponent<OrbManager>().orbs.Remove(gameObject.GetComponent<Orb>());
        }
        else
        {
            go.GetComponent<ItemManager>().items.Remove(gameObject.GetComponent<Item>());
        }
    }

    public abstract void HandleGrab();
    public virtual void HandleEarthCollision() {}
}