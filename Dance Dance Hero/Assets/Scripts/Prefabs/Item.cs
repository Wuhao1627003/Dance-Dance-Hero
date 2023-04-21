using UnityEngine;

public abstract class Item : MonoBehaviour
{

    public bool randomizeDirection = false;
    public float speed = 0.1f;
    public Vector3 velocity = Vector3.down;
    public AudioClip spawnAudio;

    void Start()
    {
        float radius = GameObject.Find("GlobalObject").GetComponent<OrbManager>().radius;
        Vector2 randomPointOnCircle = Random.insideUnitCircle * radius * 1.2f;
        transform.position = new(randomPointOnCircle.x, GameObject.Find("GlobalObject").GetComponent<ItemManager>().initialCameraPosition.y + 1.5f, randomPointOnCircle.y);
        AudioSource.PlayClipAtPoint(spawnAudio, transform.position);
        if (randomizeDirection)
        {
            Vector3 randomPointOnEarth = Random.insideUnitSphere * radius;
            velocity = (randomPointOnEarth - transform.position).normalized;
        }

        velocity *= speed;
    }

    public virtual void onBeatUpdate()
    {
        transform.position += velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
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
        if (gameObject.GetComponent<Orb>())
        {
            GameObject.Find("GlobalObject").GetComponent<OrbManager>().orbs.Remove(gameObject.GetComponent<Orb>());
        }
        else
        {
            GameObject.Find("GlobalObject").GetComponent<ItemManager>().items.Remove(gameObject.GetComponent<Item>());
        }
        Destroy(gameObject);
    }

    public abstract void HandleGrab();
    public virtual void HandleEarthCollision() {}
}