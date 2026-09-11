using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

// Execution: Object Spawning (1pt)
//
// The Starter Assets ObjectSpawner doesn't have its own "Spawn Point" field —
// it expects something else to tell it where to spawn each time. This script
// does that: on a controller button press, it tells the ObjectSpawner to spawn
// at wherever "Spawn Point" (below) currently is.
//
// Setup:
//  1. Attach this to the SAME GameObject that has the Starter Assets
//     "Object Spawner" component (and your SpawnFeedback script) on it.
//  2. Spawn Point: drag in your right controller's Transform, so objects
//     appear at the controller's position.
//  3. Spawn Action: assign a controller button.
[RequireComponent(typeof(ObjectSpawner))]
public class SpawnTrigger : MonoBehaviour
{
    [Tooltip("Where spawned objects should appear — drag in your controller's Transform.")]
    public Transform spawnPoint;

    [Tooltip("The controller button that triggers spawning.")]
    public InputActionReference spawnAction;

    private ObjectSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<ObjectSpawner>();
    }

    private void OnEnable()
    {
        if (spawnAction != null)
        {
            spawnAction.action.Enable();
            spawnAction.action.performed += OnSpawnPerformed;
        }
    }

    private void OnDisable()
    {
        if (spawnAction != null)
        {
            spawnAction.action.performed -= OnSpawnPerformed;
            spawnAction.action.Disable();
        }
    }

    private void OnSpawnPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("SpawnTrigger: button press detected!");

        if (spawner == null || spawnPoint == null)
        {
            Debug.LogWarning("SpawnTrigger: spawner or spawnPoint is null, cannot spawn.");
            return;
        }

        // "up" is used as a stand-in surface normal here since we're not spawning
        // on top of a detected surface, just at a fixed point in space.
        bool success = spawner.TrySpawnObject(spawnPoint.position, Vector3.up);
        Debug.Log("SpawnTrigger: TrySpawnObject returned " + success);
    }
}
