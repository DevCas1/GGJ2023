using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Interactable : MonoBehaviour
{
    public virtual bool IsSelectable { get => collider.enabled; }
    protected new Collider2D collider;

    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public virtual void Select()
    {
        Debug.Log($"Interactable {transform.name} selected");
        // Trigger outline effect
    }

    public virtual void Deselect()
    {
        Debug.Log($"Interactable {transform.name} Deselected");
        // Disable outline effect
    }
}