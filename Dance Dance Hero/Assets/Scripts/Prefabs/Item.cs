using UnityEngine;

public abstract class Item : MonoBehaviour
{

    public bool randomizeDirection = false;
    public float speed = 0.2f;
    public Vector3 velocity = Vector3.down;
    public AudioClip spawnAudio;

    void Start()
    {
        float radius = GameObject.Find("GlobalObject").GetComponent<OrbManager>().radius;
        Vector2 randomPointOnCircle = Random.insideUnitCircle * radius * 1.2f;
        transform.position = new(randomPointOnCircle.x, GameObject.Find("GlobalObject").GetComponent<ItemManager>().initialCameraPosition.y + 1.5f, randomPointOnCircle.y);
        if (spawnAudio != null)
        {
            AudioSource.PlayClipAtPoint(spawnAudio, transform.position);
        }
        if (randomizeDirection)
        {
            Vector3 randomPointOnEarth = Random.insideUnitSphere * radius;
            velocity = (randomPointOnEarth - transform.position).normalized;
        }

        velocity *= speed;
    }

    // public virtual void onBeatUpdate()
    // {
    //     transform.position += velocity;
    // }
    public void Update()
    {
        transform.position += velocity * Time.deltaTime;
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