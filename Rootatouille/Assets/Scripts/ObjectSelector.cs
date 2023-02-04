using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ObjectSelector : MonoBehaviour
{
    private enum PointerClickStatus { None, Click, Hold, Release }

    public LayerMask InteractableFilter;

    [SerializeField] private InputHandler InputHandler;
    [SerializeField] private Cauldron Cauldron;

    private new Camera camera;

    private Vector2 pointerPos;
    private Vector3 pointerWorldPos;
    private PointerClickStatus pointerStatus;
    private RaycastHit2D[] raycastResults;
    private bool hasHit;
    private Transform hit;

    private bool hoversOverCauldron;

    private Dragable selectedDragable = null;
    private bool dragableSelected = false;
    private bool isDragging = false;
    private Dragable draggingDragable = null;
    private DragableType draggingDragableType;
    private Vector3 dragableStartPos = Vector3.zero;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        Init();
    }

    private void Start()
    {
        // hit.transform.position = new(0, 0, 9000);
    }

    private void Update()
    {
        CheckForObjects();

        if (!dragableSelected && raycastResults.Length > 0)
        {
            Dragable tempDragable = hit.GetComponent<Dragable>();

            // for (int index = 0; index < raycastResults.Length; index++)
            // {
            //     Dragable tempDragable = raycastResults[index].transform.GetComponent<Dragable>();

            //     if (tempDragable == null || !tempDragable.IsSelectable)
            //         continue;

            //     if (nearestDragable == null || tempDragable.transform.position.z < nearestDragable.transform.position.z)
            //         nearestDragable = tempDragable;
            // }

            if (tempDragable == null)
            {
                if (dragableSelected)
                    DeselectDragable();
            }
            else
                SelectDragable(tempDragable);
        }

        if (dragableSelected)
        {
            if (!isDragging && pointerStatus == PointerClickStatus.Click)
                StartDragging();

            if (raycastResults.Length == 0)
            {
                DeselectDragable();
                return;
            }
        }

        if (isDragging)
        {
            if (hasHit)
                hoversOverCauldron = hit.name == Cauldron.transform.name;
            else
                hoversOverCauldron = false;

            switch (pointerStatus)
            {
                case PointerClickStatus.Hold:
                    Drag();
                    break;
                case PointerClickStatus.Release:
                    StopDragging();
                    pointerStatus = PointerClickStatus.None;
                    break;
            }
        }
    }

    private void Init()
    {
        PlayerInput input = InputHandler.Input;

        input.Mouse.Position.performed += context => OnPointerMove(context.ReadValue<Vector2>());
        input.Mouse.Click.performed += context => OnPointerClicked();
        input.Mouse.Click.canceled += context => OnPointerReleased();
        InputHandler.Input.Mouse.Enable();
    }

    private void SelectDragable(Dragable newDragable)
    {
        selectedDragable = newDragable;
        dragableSelected = true;
        draggingDragableType = selectedDragable.dragableType;
        selectedDragable.Select();
    }

    private void DeselectDragable()
    {
        selectedDragable.Deselect();
        dragableSelected = false;
        selectedDragable = null;
    }

    private void StartDragging()
    {
        draggingDragable = selectedDragable;

        DeselectDragable();
        draggingDragable.Drag();

        pointerStatus = PointerClickStatus.Hold;
        isDragging = true;
        dragableStartPos = draggingDragable.transform.position;
    }

    private void CheckForObjects()
    {
        raycastResults = Physics2D.RaycastAll(pointerWorldPos, transform.forward, camera.farClipPlane, InteractableFilter);
        hasHit = raycastResults.Length > 0;

        if (raycastResults.Length == 0)
            return;

        for (int index = 0; index < raycastResults.Length; index++)
        {
            RaycastHit2D tempHit = raycastResults[index];
            if (tempHit.transform != null || tempHit.transform.position.z < hit.transform.position.z)
                hit = tempHit.transform;
        }
    }

    private void Drag()
    {
        draggingDragable.transform.position = new(pointerWorldPos.x, pointerWorldPos.y, dragableStartPos.z);
    }

    private void StopDragging()
    {
        isDragging = false;
        draggingDragable.transform.position = dragableStartPos;
        dragableStartPos = Vector2.zero;

        if (draggingDragableType == DragableType.Ingredient && hoversOverCauldron)
        {
            Debug.Log($"Dropped {draggingDragable.dragableType} {draggingDragable.transform.name} into the cauldron.");
            Cauldron.AddIngredient(draggingDragable.GetComponent<Ingredient>());
        }

        draggingDragable.Drop();
        draggingDragable = null;
    }

    private void OnPointerMove(Vector2 pointerPosition)
    {
        pointerPos = pointerPosition;
        pointerWorldPos = camera.ScreenToWorldPoint(new(pointerPos.x, pointerPos.y, transform.position.z));
    }

    private void OnPointerClicked()
    {
        pointerStatus = PointerClickStatus.Click;
    }

    private void OnPointerReleased()
    {
        pointerStatus = PointerClickStatus.Release;
    }

    private void OnEnable()
    {
        InputHandler.Input.Mouse.Enable();
    }

    private void OnDisable()
    {
        PlayerInput input = InputHandler.Input;

        input.Mouse.Position.performed -= context => OnPointerMove(context.ReadValue<Vector2>());
        input.Mouse.Click.performed -= context => OnPointerClicked();
        input.Mouse.Click.canceled -= context => OnPointerReleased();
        InputHandler.Input.Mouse.Disable();
    }
}