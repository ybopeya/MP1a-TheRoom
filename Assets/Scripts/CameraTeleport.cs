using UnityEngine;
using UnityEngine.InputSystem;


public class CameraTeleport : MonoBehaviour
{
    [Tooltip("The XR Origin / player rig to move. Defaults to this object if left empty.")]
    public Transform playerRig;

    [Tooltip("Empty GameObject marking the starting position inside the room.")]
    public Transform roomPosition;

    [Tooltip("Empty GameObject marking the external viewing position.")]
    public Transform externalPosition;

    [Tooltip("The controller button that triggers the teleport.")]
    public InputActionReference teleportAction;

    // Tracks which of the two locations the player is currently at.
    private bool isOutside = false;

    private void Start()
    {
        if (playerRig == null) playerRig = transform;
    }

    private void OnEnable()
    {
        if (teleportAction != null)
        {
            teleportAction.action.Enable();
            teleportAction.action.performed += OnTeleportPerformed;
        }
    }

    private void OnDisable()
    {
        if (teleportAction != null)
        {
            teleportAction.action.performed -= OnTeleportPerformed;
            teleportAction.action.Disable();
        }
    }

    private void OnTeleportPerformed(InputAction.CallbackContext context)
    {
        Teleport();
    }

    private void Teleport()
    {
        Transform target = isOutside ? roomPosition : externalPosition;

        if (target == null)
        {
            Debug.LogWarning("CameraTeleport: target position not assigned in the Inspector.");
            return;
        }

        playerRig.position = target.position;
        playerRig.rotation = target.rotation;
        isOutside = !isOutside;
    }
}
