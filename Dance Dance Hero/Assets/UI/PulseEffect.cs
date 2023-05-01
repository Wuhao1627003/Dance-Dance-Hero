using System.Collections;
using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [Tooltip("Size of the heartbeat")]
    public float Strength;

    [Tooltip("Delay between pulses")]
    public float PulseDelay;

    [Tooltip("How fast the heart size returns to normal")]
    public float ReturnToNormalSpeed;

    [Tooltip("Delay between heartbeats")]
    public float BeatDelay;

    void Start()
    {
        StartCoroutine(Pulse());
    }

    IEnumerator Pulse()
    {
        // Loops forever
        while (true)
        {
float timer = 0f;
            float originalSize = transform.localScale.x;

            // Heart beat twice
            for (int i = 0; i < 1; i++)
            {
                // Beat 1
                while (timer < PulseDelay)
                {
                    yield return new WaitForEndOfFrame();
                    timer += Time.deltaTime;

                    transform.localScale = new Vector3
(
                        transform.localScale.x + (Time.deltaTime * Strength * 2), transform.localScale.y + (Time.deltaTime * Strength * 2)
                    );
                }

                timer = 0f;

                // Beat 2
                while (timer < PulseDelay)
                {
                    yield return new WaitForEndOfFrame();
                    timer += Time.deltaTime;

                    transform.localScale = new Vector3
(
                        transform.localScale.x - (Time.deltaTime * Strength),
                        transform.localScale.y - (Time.deltaTime * Strength)
);
}

                timer = 0f;
            }

            // Return to normal
            while (transform.localScale.x > originalSize)
            {
                yield return new WaitForEndOfFrame();

                transform.localScale = new Vector3
                (
                    transform.localScale.x - Time.deltaTime * Strength * ReturnToNormalSpeed,
                    transform.localScale.y - Time.deltaTime * Strength * ReturnToNormalSpeed
);
}

            transform.localScale = new Vector3(originalSize, originalSize);

            yield return new WaitForSeconds(BeatDelay);
}
    }
}