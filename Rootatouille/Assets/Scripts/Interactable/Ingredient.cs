using UnityEngine;

public class Ingredient : Dragable
{
    public IngredientType IngredientType { get => ingredientType; }
    [SerializeField] private IngredientType ingredientType;
}