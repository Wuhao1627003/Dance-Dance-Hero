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
            velocity = (startPosition - randomPointOnEarth).normalized;
        }

        velocity *= speed;
    }

    // called on beat
    public void onBeatUpdate()
    {
        transform.position += velocity;
    }
}
