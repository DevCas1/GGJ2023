using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Recipe", fileName = "New Recipe")]
public class Recipe : ScriptableObject
{
    public const int MAX_AMOUNT_OF_HERBS = 3;
    [Serializable]
    public struct IngredientList
    {
        public bool Beet;
        public bool Mold;
        public bool Sunflower;
        [Range(1, MAX_AMOUNT_OF_HERBS)] public int Herbs;
    }

    [SerializeField] public IngredientList Content;
    public Color RecipeColor = Color.white;
}