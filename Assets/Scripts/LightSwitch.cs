using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Light))]
public class LightSwitch : MonoBehaviour
{
    [Tooltip("The controller button that changes the light color.")]
    public InputActionReference toggleAction;

    [Tooltip("Colors to cycle through each time the button is pressed.")]
    public Color[] colors = { Color.white, Color.red, Color.green, Color.blue, Color.yellow };

    private Light pointLight;
    private int colorIndex = 0;

    private void Start()
    {
        pointLight = GetComponent<Light>();
    }

    private void OnEnable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.Enable();
            toggleAction.action.performed += OnTogglePerformed;
        }
    }

    private void OnDisable()
    {
        if (toggleAction != null)
        {
            toggleAction.action.performed -= OnTogglePerformed;
            toggleAction.action.Disable();
        }
    }

    private void OnTogglePerformed(InputAction.CallbackContext context)
    {
        colorIndex = (colorIndex + 1) % colors.Length;
        pointLight.color = colors[colorIndex];
    }
}
