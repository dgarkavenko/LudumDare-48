using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractableElement : UIBehaviour
{
    public float MinX { get; private set; }
    public float MinY { get; private set; }
    public float MaxX { get; private set; }
    public float MaxY { get; private set; }
    
    protected override void Start()
    {
        var rectTransform = (RectTransform) transform;

        var wc = new Vector3[4];
        rectTransform.GetWorldCorners(wc);

        MinX = float.MaxValue;
        MinY = float.MaxValue;
        MaxX = float.MinValue;
        MaxY = float.MinValue;

        foreach (var corner in wc)
        {
            if (corner.x < MinX)
                MinX = corner.x;
            if (corner.x > MaxX)
                MaxX = corner.x;
            if (corner.y < MinY)
                MinY = corner.y;
            if (corner.y > MaxY)
                MaxY = corner.y;
        }
    }

    public bool IsCollides(Vector3 position)
    {
        return position.x > MinX && position.x < MaxX
            && position.y > MinY && position.y < MaxY;
    }
}
