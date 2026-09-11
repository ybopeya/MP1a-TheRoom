using UnityEngine;


public class OrbitingComet : MonoBehaviour
{
    [Tooltip("The object the comet falls towards, e.g. the planet's Transform. Leave empty to use world origin (0,0,0).")]
    public Transform attractor;

    [Tooltip("Strength of the simulated gravity. Increase for a faster/stronger pull.")]
    public double gravity = 0.2;

    // The comet's current velocity, accumulated frame by frame.
    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        Vector3 attractorPos = (attractor != null) ? attractor.position : Vector3.zero;

        // Offset vector from the attractor to the comet.
        Vector3 offset = transform.position - attractorPos;
        double distance = System.Math.Sqrt(offset.x * offset.x + offset.y * offset.y + offset.z * offset.z);

        // Avoid dividing by zero if the comet is exactly on top of the attractor.
        if (distance < 0.001) return;

        // Acceleration pulling the comet towards the attractor (Newtonian-style gravity,
        // divided by distance^3 because we're using the un-normalized offset vector
        // instead of normalizing it and dividing by distance^2 separately).
        double ax = -gravity * offset.x / System.Math.Pow(distance, 3);
        double ay = -gravity * offset.y / System.Math.Pow(distance, 3);
        double az = -gravity * offset.z / System.Math.Pow(distance, 3);

        velocity.x += (float)(ax * Time.deltaTime);
        velocity.y += (float)(ay * Time.deltaTime);
        velocity.z += (float)(az * Time.deltaTime);

        transform.position += velocity * Time.deltaTime;
    }
}
