using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    public InteractableType InteractableType;
    public UnityEvent OnSelect;
    public UnityEvent OnDeselect;
    public virtual bool IsSelectable { get => collider.enabled; }
    protected new Collider2D collider;

    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public virtual void Select()
    {
        // Debug.Log($"Interactable {transform.name} selected");
        // Trigger outline effect
        if (OnSelect != null)
            OnSelect.Invoke();
    }

    public virtual void Deselect()
    {
        // Debug.Log($"Interactable {transform.name} Deselected");
        // Disable outline effect
        if (OnDeselect != null)
            OnDeselect.Invoke();
    }
}