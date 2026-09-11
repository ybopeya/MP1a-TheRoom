using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

// User Feedback: Particle Bursts (2pts, requires Object Spawning)
// User Feedback: Spatial Sound (2pts, requires Object Spawning)
//
// This does NOT do the spawning itself — it hooks into the "objectSpawned" event
// already provided by the Starter Assets' ObjectSpawner component, and adds
// particle + sound feedback co-located with whatever it just spawned.
//
// Setup:
//  1. Attach this script to the SAME GameObject that already has the
//     Starter Assets "Object Spawner" component on it.
//  2. Assign a Particle System prefab to "Particle Prefab" (GameObject -> Effects ->
//     Particle System in the scene, tweak it, drag into your Project window to make
//     it a prefab, then delete the scene copy).
//  3. Assign an AudioClip to "Spawn Sound" (drag any short audio file from your Project).
[RequireComponent(typeof(ObjectSpawner))]
public class SpawnFeedback : MonoBehaviour
{
    [Header("Particle Feedback")]
    public ParticleSystem particlePrefab;

    [Header("Spatial Sound Feedback")]
    public AudioClip spawnSound;
    [Range(0f, 1f)] public float spatialBlend = 1f; // 1 = fully 3D/spatial

    private ObjectSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<ObjectSpawner>();
    }

    private void OnEnable()
    {
        if (spawner != null)
            spawner.objectSpawned += OnObjectSpawned;
    }

    private void OnDisable()
    {
        if (spawner != null)
            spawner.objectSpawned -= OnObjectSpawned;
    }

    // Called automatically by the Starter Assets ObjectSpawner right after it
    // spawns something, passing in the object that was just created.
    private void OnObjectSpawned(GameObject spawnedObject)
    {
        Vector3 pos = spawnedObject.transform.position;
        Quaternion rot = spawnedObject.transform.rotation;

        // --- Particle Bursts: co-located with the spawned object ---
        if (particlePrefab != null)
        {
            ParticleSystem ps = Instantiate(particlePrefab, pos, rot);
            ps.Play();
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }

        // --- Spatial Sound: co-located with the spawned object ---
        if (spawnSound != null)
        {
            GameObject audioObj = new GameObject("SpawnSound");
            audioObj.transform.position = pos;
            AudioSource source = audioObj.AddComponent<AudioSource>();
            source.clip = spawnSound;
            source.spatialBlend = spatialBlend;
            source.Play();
            Destroy(audioObj, spawnSound.length);
        }
    }
}
