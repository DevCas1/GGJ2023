using System.Collections.Generic;
using UnityEngine;

public class Cauldron : Interactable
{
    [SerializeField] private SpriteRenderer Center;
    private List<IngredientType> currentIngredients = new();

    public void AddIngredient(Ingredient ingredient)
    {
        currentIngredients.Add(ingredient.IngredientType);
        // Change cauldron FX
    }

    public void GetFromCauldron()
    {
        // Return current brew
    }

    public void EmptyCauldron()
    {
        currentIngredients = new();
        // Change cauldron FX
    }
}