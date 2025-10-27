using UnityEngine;

[System.Serializable]
public class Cell 
{
    public Vector2 position;
    public GameObject building;
    
    public Vector2 GetPosition()
    {
        return position;
    }
    
    public GameObject GetValue()
    {
        return building;
    }
    
    public void SetValue(GameObject newBuilding)
    {
        building = newBuilding;
    }
}
