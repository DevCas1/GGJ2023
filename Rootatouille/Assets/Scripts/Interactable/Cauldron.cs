using UnityEngine;

public class Cauldron : Interactable
{
    public Color FailedBrewColor;
    public new InteractableType InteractableType => InteractableType.Cauldron;

    [SerializeField] private SpriteRenderer cauldronContent;
    [SerializeField] private RecipeBook knownRecipies;

    private bool containsMold;
    private bool containsBeetroot;
    private bool containsSunflower;
    private int amountOfHerbs = 0;

    private int remainingRecipies;
    private Recipe brewnRecipe;
    private bool correctRecipe;

    private void Start()
    {
        remainingRecipies = knownRecipies.Recipes.Length;
    }

    public void AddIngredient(Ingredient ingredient)
    {
        switch (ingredient.IngredientType)
        {
            case IngredientType.Carrot:
                if (amountOfHerbs > 0)
                    amountOfHerbs--;
                break;

            case IngredientType.Herb:
                amountOfHerbs++;
                if (amountOfHerbs > Recipe.MAX_AMOUNT_OF_HERBS)
                {
                    FailBrew();
                    return;
                }
                break;

            case IngredientType.Mold:
                if (containsMold)
                {
                    FailBrew();
                    return;
                }
                else
                    containsMold = true;
                break;

            case IngredientType.Beetroot:
                if (containsBeetroot)
                {
                    FailBrew();
                    return;
                }
                else
                    containsBeetroot = true;
                break;

            case IngredientType.Sunflower:
                if (containsSunflower)
                {
                    FailBrew();
                    return;
                }
                else
                    containsSunflower = true;
                break;
        }

        int possibleCount = 0;
        foreach (Recipe recipe in knownRecipies.Recipes)
        {
            if (CheckRecipe(recipe, out bool possible))
            {
                correctRecipe = true;
                brewnRecipe = recipe;

            }

            if (possible)
                possibleCount++;
        }

        remainingRecipies = possibleCount;
        if (remainingRecipies == 0)
        {
            FailBrew();
            return;
        }

        string message = (correctRecipe ? "Current brew: " + brewnRecipe.name : "Current brew isn't known") + ". Remaining possibilities: " + remainingRecipies;
        Debug.Log(message);

        if (correctRecipe)
            ChangeContentColor(false, brewnRecipe.RecipeColor);
        else if (ingredient.HasColor)
            ChangeContentColor(containsBeetroot != false || containsMold != false || containsSunflower != false || ingredient.IngredientType == IngredientType.Carrot, ingredient.IngredientColor);
    }

    private void FailBrew()
    {
        correctRecipe = false;
        brewnRecipe = null;
        remainingRecipies = 0;
        ChangeContentColor(false, FailedBrewColor);

        Debug.Log("Brew failed!");
    }

    private bool CheckRecipe(Recipe recipe, out bool possible)
    {
        Recipe.IngredientList ingredientList = recipe.Content;
        int requiredHerbs = ingredientList.Herbs;
        bool requiresBeet = ingredientList.Beet;
        bool requiresMold = ingredientList.Mold;
        bool requiresSunflower = ingredientList.Sunflower;

        possible = ((amountOfHerbs == requiredHerbs) || (amountOfHerbs < requiredHerbs)) && ((containsBeetroot == requiresBeet) || (containsBeetroot == false && requiresBeet == true)) && ((containsMold == requiresMold) || (containsMold == false && requiresMold == true)) && ((containsSunflower == requiresSunflower) || (containsSunflower == false && requiresSunflower == true));
        return (amountOfHerbs == requiredHerbs) && (containsBeetroot == requiresBeet) && (containsMold == requiresMold) && (containsSunflower == requiresSunflower);
    }

    private void ChangeContentColor(bool mixColor, Color newColor) //TODO: Lerp color over time
    {
        cauldronContent.color = mixColor ? Color.Lerp(cauldronContent.color, newColor, 0.5f) : newColor;
    }

    public bool GetFromCauldron(out Recipe recipe)
    {
        recipe = brewnRecipe;
        return correctRecipe;
    }

    public void EmptyCauldron()
    {
        containsMold = false;
        containsBeetroot = false;
        containsSunflower = false;
        amountOfHerbs = 0;
        remainingRecipies = knownRecipies.Recipes.Length;
        brewnRecipe = null;
        correctRecipe = false;

        // Reset cauldron FX
    }
}