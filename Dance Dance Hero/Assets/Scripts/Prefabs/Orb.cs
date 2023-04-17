using UnityEngine;

public class Orb : MonoBehaviour
{
    public bool randomizeDirection = false;
    public float speed = 0.1f;
    private Vector3 velocity = Vector3.down;

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
        transform.position += velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GameController"))
        {
            Vector3 normal = collision.gameObject.transform.position.normalized;
            velocity = Vector3.Reflect(velocity.normalized, normal) * speed;
            GetComponent<Rigidbody>().AddForce(velocity * 3);
        }
    }
}
