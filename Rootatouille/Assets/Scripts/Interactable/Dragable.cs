public class Dragable : Interactable
{
    public override bool IsSelectable { get => !isBeingDragged && collider.enabled; }
    public DragableType dragableType;
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
        isBeingDragged = true;
        collider.enabled = false;
    }

    public void Drop()
    {
        isBeingDragged = false;
        collider.enabled = true;
    }

    protected virtual void OnEnable()
    {

    }

    protected virtual void OnDisable()
    {

    }
}