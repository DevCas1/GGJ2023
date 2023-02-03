using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    private enum PointerClickStatus { None, Clicked, Released }

    public LayerMask InteractableLayer;

    [SerializeField] private InputHandler inputHandler;

    private Camera camera;

    private Vector2 pointerPos;
    private Vector2 pointerDelta;
    private PointerClickStatus pointerStatus;

    private bool isDragging;

    private bool dragableSelected;
    private Dragable selectedDragable;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        PlayerInput input = inputHandler.Input;
        input.Mouse.Position.performed += context => OnPointerMove(true, context.ReadValue<Vector2>());
        input.Mouse.Position.canceled += context => OnPointerMove(false, context.ReadValue<Vector2>());
        input.Mouse.Click.performed += context => OnPointerClicked();
        input.Mouse.Click.canceled += context => OnPointerReleased();
    }

    private void Start()
    {

    }

    private void Update()
    {
        CheckForObjects();

        if (pointerStatus == PointerClickStatus.Clicked)
        {
            if (dragableSelected)
            {
                selectedDragable.Drag();
            }
        }
    }

    private void CheckForObjects()
    {
        Ray ray = camera.ScreenPointToRay(pointerPos);
        // RaycastHit2D hit;
        // if (Physics2D.Raycast(ray, ))
        // {

        // }
    }

    private void OnPointerMove(bool performed, Vector2 pointerPosition)
    {
        if (performed)
        {
            pointerDelta = pointerPosition - pointerPos;
            pointerPos = pointerPosition;
        }
        else
            pointerDelta = Vector2.zero;
    }

    private void OnPointerClicked() => pointerStatus = PointerClickStatus.Clicked;

    private void OnPointerReleased() => pointerStatus = PointerClickStatus.Released;
}