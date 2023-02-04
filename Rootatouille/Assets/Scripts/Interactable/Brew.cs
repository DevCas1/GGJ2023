using UnityEngine;

public class Brew : Dragable
{
    public new InteractableType InteractableType => InteractableType.Brew;
    public Recipe ContainedRecipe { get => containedRecipe; }
    public bool CorrectRecipe { get => correctRecipe; }
    public Color BrewColor { get => brewColor; }

    private SpriteRenderer flaskContent;

    private Recipe containedRecipe;
    private bool correctRecipe;
    private Color brewColor;

    public void EmptyFlask()
    {
        flaskContent.enabled = false;
    }

    public void SetRecipe(bool correctRecipe, Color brewColor, Recipe recipe = null)
    {
        this.correctRecipe = correctRecipe;
        containedRecipe = recipe;
        SetBrewColor(brewColor);
    }

    private void SetBrewColor(Color color)
    {
        // Lerp color
        brewColor = color;
        flaskContent.color = color;
    }
}