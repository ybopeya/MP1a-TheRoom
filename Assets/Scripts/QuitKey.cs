using UnityEngine;
using UnityEngine.InputSystem;


public class QuitKey : MonoBehaviour
{
    [Tooltip("The controller button that triggers quitting. Assign via the circle icon in the Inspector.")]
    public InputActionReference quitAction;

    private void OnEnable()
    {
        if (quitAction != null)
        {
            quitAction.action.Enable();
            quitAction.action.performed += OnQuitPerformed;
        }
    }

    private void OnDisable()
    {
        if (quitAction != null)
        {
            quitAction.action.performed -= OnQuitPerformed;
            quitAction.action.Disable();
        }
    }

    private void OnQuitPerformed(InputAction.CallbackContext context)
    {
        Quit();
    }

    private void Quit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
