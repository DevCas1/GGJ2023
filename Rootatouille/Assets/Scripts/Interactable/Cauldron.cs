using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Ingredients = Brew.Ingredients;

public class Cauldron : Interactable
{
    public UnityEvent OnBrewAdd;
    public UnityEvent OnEmptyCauldron;

    public new InteractableType InteractableType => InteractableType.Cauldron;
    public Brew CurrentBrew { get => currentBrew; }

    [SerializeField] private SpriteRenderer cauldronContent;
    [SerializeField] private Color failedBrewColor;
    [SerializeField] private Color neutralBrewColor;
    [SerializeField] private float colorChangeTime = 1;

    private Brew currentBrew;
    private bool containsMold;
    private bool containsBeetroot;
    private bool containsSunflower;
    private int amountOfHerbs = 0;

    private void Start()
    {
        currentBrew = new Brew(new Ingredients(containsBeetroot, containsMold, containsSunflower, amountOfHerbs), neutralBrewColor);
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

        if (containsBeetroot && containsMold && containsSunflower)
        {
            FailBrew();
            return;
        }

        if (ingredient.HasColor)
            ChangeContentColor(ingredient);

        currentBrew.SetIngredients(new Ingredients(containsBeetroot, containsMold, containsSunflower, amountOfHerbs), cauldronContent.color, false);
    }

    private void FailBrew()
    {
        currentBrew.SetIngredients(new Ingredients(containsBeetroot, containsMold, containsSunflower, amountOfHerbs), failedBrewColor, true);
        ChangeContentColor();

        Debug.Log("Brew failed!");
    }

    private void ChangeContentColor(Ingredient ingredient = null) //TODO: Lerp color over time
    {
        if (currentBrew.FailedBrew)
            cauldronContent.DOColor(failedBrewColor, colorChangeTime);
        else
        {
            bool mixColor = cauldronContent.color != neutralBrewColor;
            // cauldronContent.color = mixColor ? Color.Lerp(cauldronContent.color, ingredient.IngredientColor, 0.5f) : ingredient.IngredientColor;
            cauldronContent.DOColor((mixColor ? Color.Lerp(cauldronContent.color, ingredient.IngredientColor, 0.5f) : ingredient.IngredientColor), colorChangeTime);
        }
    }

    public Brew GetBrew() => CurrentBrew;

    public void EmptyCauldron()
    {
        containsBeetroot = false;
        containsMold = false;
        containsSunflower = false;
        amountOfHerbs = 0;
        currentBrew.SetIngredients(new Ingredients(containsBeetroot, containsMold, containsSunflower, amountOfHerbs), neutralBrewColor, false);

        // Reset cauldron FX
        cauldronContent.DOColor(neutralBrewColor, colorChangeTime);
        if (OnEmptyCauldron != null)
            OnEmptyCauldron.Invoke();
    }
}