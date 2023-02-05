using UnityEngine;
using UnityEngine.Events;

public class Dragable : Interactable
{
    public UnityEvent OnDrag;
    public UnityEvent OnDrop;
    public override bool IsSelectable { get => !isBeingDragged && collider.enabled; }
    public DragableType dragableType;
    protected Vector3 originalPosition;
    protected bool isBeingDragged;

    // public override void Select()
    // {
    //     // Trigger outline effect
    // }

    // public override void Deselect()
    // {
    //     // Trigger outline effect
    // }

    public void Drag()
    {
        originalPosition = transform.position;
        isBeingDragged = true;
        collider.enabled = false;

        if (OnDrag != null)
            OnDrag.Invoke();
    }

    public void Drop()
    {
        isBeingDragged = false;
        collider.enabled = true;
        transform.position = originalPosition;
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
}