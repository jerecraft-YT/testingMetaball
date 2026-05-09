using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    public followObject moveObject;

    public float velocidadMouse = 0.01f;


    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        gameObject.transform.position = mousePosition;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            moveObject.addPoint(mousePosition);
        }
    }
}
