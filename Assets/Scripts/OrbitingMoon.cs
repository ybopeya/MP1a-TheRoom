using UnityEngine;


public class OrbitingMoon : MonoBehaviour
{
    [Tooltip("Degrees per second to rotate around the Y axis.")]
    public float rotationSpeed = 30f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
