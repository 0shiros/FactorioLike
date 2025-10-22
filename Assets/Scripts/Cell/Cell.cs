using UnityEngine;

[System.Serializable]
public class Cell
{
    public Vector2 position;
    public int value;
    
    public void InitializeCell(Vector2 position, int value)
    {
        this.position = position;
        this.value = value;
    }
    
    public Vector2 GetPosition()
    {
        return position;
    }
    
    public int GetValue()
    {
        return value;
    }
    
    public void SetValue(int newValue)
    {
        value = newValue;
    }
    
    
}
