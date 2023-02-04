using UnityEngine;

[CreateAssetMenu(menuName = "Input Manager", fileName = "Input Handler")]
public class InputHandler : ScriptableObject
{
    public PlayerInput Input { get => input ??= Init(); }
    private PlayerInput input;

    private PlayerInput Init() => input = new PlayerInput();

    private static void RemoveCursorLockState() => Cursor.lockState = CursorLockMode.None;
}