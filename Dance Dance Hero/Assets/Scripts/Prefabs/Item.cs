using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public bool randomizeDirection = false;
    public float speed = 0.1f;
    private Vector3 velocity = Vector3.down;

    void Start()
    {
        if (randomizeDirection)
        {
            Vector3 startPosition = transform.position;
            float radius = GameObject.Find("Earth").GetComponent<SphereCollider>().radius * 0.6f;
            Vector3 randomPointOnEarth = Random.insideUnitSphere * radius;
            velocity = (randomPointOnEarth - startPosition).normalized;
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
