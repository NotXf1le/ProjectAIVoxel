using UnityEngine;
[System.Serializable]
public class Voxel
{
    public int X { get; }
    public int Y { get; }
    public int Z { get; }

    public float R;
    public float G;
    public float B;

    public bool IsActive { get; set; }

    public Voxel(Vector3 position, Color color, bool isActive)
    {
        X = (int)position.x;
        Y = (int)position.y;
        Z = (int)position.z;

        R = color.r;
        G = color.g;
        B = color.b;
        IsActive = isActive;

    }
    public void Reset()
    {
        R = default;
        G = default;
        B = default;
        IsActive = false;
    }
    
}
