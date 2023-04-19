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
            float radius = GameObject.Find("GlobalObject").GetComponent<OrbManager>().radius;
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
        if ((transform.position - GameObject.Find("Main Camera").transform.position).magnitude > 20) {
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            Vector3 normal = other.transform.position.normalized;
            velocity = Vector3.Reflect(velocity.normalized, normal) * speed;
            GetComponent<Rigidbody>().AddForce(velocity * 300);
            punched = true;
            GameObject.Find("GlobalObject").GetComponent<OrbManager>().HandlePunch();
        }
        else if (other.CompareTag("Earth"))
        {
            Debug.LogError("Hit Earth!");
            Destroy(gameObject);
        }
    }
}
