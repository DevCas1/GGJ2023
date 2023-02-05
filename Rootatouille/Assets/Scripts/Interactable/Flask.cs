using UnityEngine;
using UnityEngine.Events;

public class Flask : Dragable
{
    public UnityEvent OnFillFlask;
    public UnityEvent OnEmptyFlask;
    public new InteractableType InteractableType => InteractableType.Flask;
    public Brew ContainedBrew { get => containedBrew; }
    // public Brew Brew { get => currentBrew; }
    public bool HasBrew { get => hasBrew; }

    [SerializeField] private SpriteRenderer flaskContent;

    private Brew containedBrew;
    // private Brew currentBrew;
    private bool hasBrew;

    public void EmptyFlask()
    {
        flaskContent.enabled = false;
    }

    // public void FillWithBrew(Brew brew)
    public void FillWithBrew(Brew brew)
    {
        hasBrew = true;
        containedBrew = brew;
        SetBrewColor();
    }

    private void SetBrewColor()
    {
        // Lerp color
        flaskContent.color = containedBrew.Color;
        flaskContent.enabled = true;
    }
}