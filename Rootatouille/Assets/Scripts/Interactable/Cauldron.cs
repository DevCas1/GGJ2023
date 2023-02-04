using UnityEngine;

public class Cauldron : Interactable
{
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
                amountOfHerbs = 0;
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

        string message = (correctRecipe ? "Current brew: " + brewnRecipe.name : "Current brew isn't known") + ". Remaining possibilities: " + remainingRecipies;
        Debug.Log(message);

        // Change cauldron FX
    }

    private void FailBrew()
    {
        correctRecipe = false;
        brewnRecipe = null;
        remainingRecipies = 0;

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