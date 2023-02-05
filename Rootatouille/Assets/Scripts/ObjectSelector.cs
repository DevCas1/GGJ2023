using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class ObjectSelector : MonoBehaviour
{
    private enum PointerClickStatus { None, Click, Hold, Release }
    public UnityEvent OnSelectInteractable;
    public UnityEvent OnDeselectInteractable;
    // public UnityEvent 

    public LayerMask InteractableFilter;

    [SerializeField] private InputHandler InputHandler;
    [SerializeField] private Cauldron Cauldron;
    [SerializeField] private Flask Flask;

    private new Camera camera;

    private Vector2 pointerPos;
    private Vector3 pointerWorldPos;
    private PointerClickStatus pointerStatus;
    private RaycastHit2D[] raycastResults;
    private bool hasHit;
    private Transform hit;

    private bool hoversOverCauldron;
    private bool hoversOverPatient;

    private Dragable selectedDragable = null;
    private bool dragableSelected = false;
    private bool isDragging = false;
    private Dragable draggingDragable = null;
    private DragableType draggingDragableType;

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
            hoversOverCauldron = hasHit && hit.name == Cauldron.transform.name;
            hoversOverPatient = hasHit && (hit.GetComponent<Interactable>()?.InteractableType == InteractableType.Patient);

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
        if (OnSelectInteractable != null)
            OnSelectInteractable.Invoke();
    }

    private void DeselectDragable()
    {
        selectedDragable.Deselect();
        dragableSelected = false;
        selectedDragable = null;
        if (OnDeselectInteractable != null)
            OnDeselectInteractable.Invoke();
    }

    private void StartDragging()
    {
        draggingDragable = selectedDragable;

        DeselectDragable();
        draggingDragable.Drag();

        pointerStatus = PointerClickStatus.Hold;
        isDragging = true;
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
        draggingDragable.transform.position = new(pointerWorldPos.x, pointerWorldPos.y, 0);
    }

    private void StopDragging()
    {
        if (hoversOverCauldron)
        {
            if (draggingDragableType == DragableType.Ingredient)
            {
                Cauldron.AddIngredient(draggingDragable.GetComponent<Ingredient>());
            }
            else if (draggingDragableType == DragableType.Brew)
            {
                // Flask.FillWithBrew(Cauldron.GetBrew());
                Debug.Log("Fill flask with Brew");
            }
        }

        draggingDragable.Drop();
        isDragging = false;
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