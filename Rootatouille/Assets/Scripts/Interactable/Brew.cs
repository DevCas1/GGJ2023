using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brew : Dragable
{
    public new InteractableType InteractableType => InteractableType.Brew;
    public Recipe ContainedRecipe { get => containedRecipe; }

    private Recipe containedRecipe;

    public void SetRecipe(Recipe recipe) => containedRecipe = recipe;
}