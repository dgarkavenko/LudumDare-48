using UnityEngine;
using UnityEngine.EventSystems;

public class UIInteractableElement : UIBehaviour
{
    [SerializeField] private bool _isInteractable = true;
    
    public bool IsInteractable
    {
        get => _isInteractable;
        set
        {
            if (_isInteractable == value)
                return;
            
            _isInteractable = value;   
        }
    }

    public float MinX { get; private set; }
    public float MinY { get; private set; }
    public float MaxX { get; private set; }
    public float MaxY { get; private set; }

    private bool _onMouseOver;

    public bool OnMouseOver
    {
        get => _onMouseOver;
        private set
        {
            if (_onMouseOver == value)
                return;
            
            
            _onMouseOver = value;
        }
    }

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
        
        OnInteractableChanged();
    }

    public void CheckMouseOver(Vector3 position)
    {
        OnMouseOver = position.x > MinX && position.x < MaxX
                && position.y > MinY && position.y < MaxY;
    }

    public virtual void OnUIMouseEnter()
    {
        OnMouseOver = true;
    }

    public virtual void OnUIMouseExit()
    {
        OnMouseOver = false;
    }

    public virtual void OnUIMouseClicked()
    {
    }

    protected virtual void OnInteractableChanged()
    {
        
    }
}
