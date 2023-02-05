using UnityEngine;

public class Brew
{
    public struct Ingredients
    {
        public bool Beetroot { get => beetroot; }
        public bool Mold { get => mold; }
        public bool Sunflower { get => sunflower; }
        public int Herbs { get => herbs; }

        private bool beetroot;
        private bool mold;
        private bool sunflower;
        private int herbs;

        public Ingredients(bool beetroot, bool mold, bool sunflower, int herbs)
        {
            this.beetroot = beetroot;
            this.mold = mold;
            this.sunflower = sunflower;
            this.herbs = herbs;
        }
    }

    public Brew(Ingredients ingredients, Color brewColor, bool failedBrew = false)
    {
        SetContent(ingredients, brewColor, failedBrew);
    }

    public Ingredients BrewIngredients { get => ingredients; }
    public bool FailedBrew { get => failedBrew; }
    public Color Color { get => brewColor; }

    private Ingredients ingredients;
    private bool failedBrew;
    private Color brewColor;

    public void SetContent(Ingredients ingredients, Color brewColor, bool failedBrew = false)
    {
        this.ingredients = ingredients;
        this.brewColor = brewColor;
        this.failedBrew = failedBrew || (ingredients.Beetroot && ingredients.Mold && ingredients.Sunflower);
    }

    public void SetContent(Brew brew)
    {
        ingredients = brew.ingredients;
        failedBrew = brew.FailedBrew;
        brewColor = brew.Color;
    }
}