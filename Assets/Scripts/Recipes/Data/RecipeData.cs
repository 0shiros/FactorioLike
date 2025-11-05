using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName => name;
    public ResourceAndAmount[] ingredients;
    public ExtractOrTranformResource result;
}

[System.Serializable]
public class ResourceAndAmount
{
    public ExtractOrTranformResource resource;
    
    [SerializeField, Range(0, 2000000000)]
    private int _quantity;

    public int quantity
    {
        get => _quantity;
        set => _quantity = Mathf.Clamp(value, 0, 2000000000);
    }
    
    public ResourceAndAmount(ExtractOrTranformResource resource, int quantity)
    {
        this.resource = resource;
        this.quantity = quantity;
    }
}
