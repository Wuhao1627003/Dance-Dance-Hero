using UnityEngine;

public class Orb : MonoBehaviour
{
    public bool randomizeDirection = false;
    public float speed = 0.1f;
    private Vector3 velocity = Vector3.down;
    private bool punched = false;

    // Start is called before the first frame update
    void Start()
    {
        if (randomizeDirection)
        {
            Vector3 startPosition = transform.position;
            float radius = 2;
            Vector3 randomPointOnEarth = Random.insideUnitSphere * radius;
            velocity = (randomPointOnEarth - startPosition).normalized;
        }

        velocity *= speed;
    }

    // called on beat
    public void onBeatUpdate()
    {
        if (!punched)
        {
            transform.position += velocity;
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.LogError(other.tag + other.name);
        if (other.CompareTag("GameController"))
        {
            Vector3 normal = other.transform.position.normalized;
            velocity = Vector3.Reflect(velocity.normalized, normal) * speed;
            GetComponent<Rigidbody>().AddForce(velocity * 30);
            punched = true;
        }
    }
}
