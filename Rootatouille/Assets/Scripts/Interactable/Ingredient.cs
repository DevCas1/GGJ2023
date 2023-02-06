using UnityEngine;

public class Ingredient : Dragable
{
    public bool HasColor;
    public Color IngredientColor;
    public IngredientType IngredientType { get => ingredientType; }
    [SerializeField] private IngredientType ingredientType;
}