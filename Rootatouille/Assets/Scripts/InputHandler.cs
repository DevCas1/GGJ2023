using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AMPR/Input Manager", fileName = "Input Handler")]
public class InputHandler : ScriptableObject
{
    public PlayerInput Input { get => input; }
    private PlayerInput input;

    private void Awake() // TODO:Integrate Cursor Lockstate with Menu UI
    {
        if (input == null)
            input = new PlayerInput();

        // Input.Mouse.Cancel.performed += context => RemoveCursorLockState();
        // Cursor.lockState = CursorLockMode.Confined;
    }

    private static void RemoveCursorLockState() => Cursor.lockState = CursorLockMode.None;
}