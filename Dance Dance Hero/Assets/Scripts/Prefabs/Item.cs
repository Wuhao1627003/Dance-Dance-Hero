using UnityEngine;

public abstract class Item : MonoBehaviour
{

    public bool randomizeDirection = false;
    public float speed = 0.1f;
    public Vector3 velocity = Vector3.down;

    void Start()
    {
        float radius = GameObject.Find("GlobalObject").GetComponent<OrbManager>().radius;
        Vector2 randomPointOnCircle = Random.insideUnitCircle * radius * 1.2f;
        transform.position = new(randomPointOnCircle.x, GameObject.Find("GlobalObject").GetComponent<ItemManager>().initialCameraPosition.y + 5.0f, randomPointOnCircle.y);

        if (randomizeDirection)
        {
            Vector3 randomPointOnEarth = Random.insideUnitSphere * radius;
            velocity = (randomPointOnEarth - transform.position).normalized;
        }

        velocity *= speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            HandleItem();
            Destroy(gameObject);
        }
        else if (other.CompareTag("Earth"))
        {
            Destroy(gameObject);
        }
    }

    public abstract void HandleItem();
}