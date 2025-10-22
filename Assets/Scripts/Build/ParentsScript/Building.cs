using System.Collections.Generic;
using UnityEngine;

public abstract class Building
{
    public string name { get; protected set; }
    public Vector2Int size { get; protected set; }
    public Sprite sprite{ get; protected set; }
    public string description { get; protected set; }
    public bool needsAlimentation { get; protected set; }
    public Dictionary<string, int> resourceCosts { get; protected set; }
    
    protected Building(string name, Vector2Int size, Sprite sprite, string description, bool needsAlimentation, Dictionary<string, int> resourceCosts)
    {
        this.name = name;
        this.size = size;
        this.sprite = sprite;
        this.description = description;
        this.needsAlimentation = needsAlimentation;
        this.resourceCosts = resourceCosts;
    }
    
    public bool CanConstruct(Dictionary<string, int> availableResources)
    {
        foreach (var ressource in resourceCosts)
        {
            if (!availableResources.ContainsKey(ressource.Key) || availableResources[ressource.Key] < ressource.Value)
            {
                return false;
            }
        }
        return true;
    }
    
    public void Construct(Dictionary<string, int> availableResources)
    {
        if (!CanConstruct(availableResources))
        {
            Debug.LogError("Not enough resources to construct " + name);
            return;
        }
        
        foreach (var ressource in resourceCosts)
        {
            availableResources[ressource.Key] -= ressource.Value;
        }
        
        Debug.Log(name + " constructed successfully!");
    }
}
