using UnityEngine;
using UnityEngine.Tilemaps;

public class TransformBuilding : Building
{
    public RecipeData recipe;

    protected override void Update()
    {
        base.Update();
        TransformResources();
    }
    
    public override void InitializeBuilding(BuildingData data, int order, Tilemap tileMapResources)
    {
        base.InitializeBuilding(data, order, tileMapResources);
        recipe = data.recipe;
        timeElapsed = 0f;
        resourcesStored.Add(new ResourceAndAmount(recipe.result, 0));
        foreach (ResourceAndAmount ingredient in recipe.ingredients)
        {
            resourcesStored.Add(new ResourceAndAmount(ingredient.resource, 0));
        }
    }
    
    private bool CanTransformResources()
    {
        foreach (var ingredient in recipe.ingredients)
        {
            var resource = resourcesStored.Find(r => r.resource == ingredient.resource);
            if (resource == null || resource.quantity < ingredient.quantity)
            {
                return false;
            }
        }
        return true;
    }

    private void TransformResources()
    {
        if(!CanTransformResources()) return;
        
        timeElapsed += Time.deltaTime;
        float timeToGenerate = cycleTime / quantityResourcesPerCycle;

        if (!(timeElapsed >= timeToGenerate)) return;
        
        resourcesStored[0].quantity++;
        timeElapsed -= timeToGenerate;
    }
}
