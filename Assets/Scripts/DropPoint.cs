using UnityEngine;
using UnityEngine.UI;

public class DropPoint : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("How close the clothing needs to be to snap (in UI units/pixels)")]
    public float snapRadius = 100f;

    [Header("Visual Feedback (Optional)")]
    [Tooltip("Optional Image to show when this drop point is being hovered")]
    public Image highlightImage;
    public Color highlightColor = new Color(1f, 1f, 0f, 0.5f);

    public bool IsOccupied { get; private set; }
    public DraggableClothing CurrentClothing { get; private set; }

    private Color originalHighlightColor;

    private void Awake()
    {
        if (highlightImage != null)
        {
            originalHighlightColor = highlightImage.color;
            highlightImage.enabled = false;
        }
    }

    /// <summary>
    /// Called when a valid clothing item starts hovering over this drop point
    /// </summary>
    public void OnHoverEnter()
    {
        if (highlightImage != null && !IsOccupied)
        {
            highlightImage.enabled = true;
            highlightImage.color = highlightColor;
        }
    }

    /// <summary>
    /// Called when a clothing item stops hovering over this drop point
    /// </summary>
    public void OnHoverExit()
    {
        if (highlightImage != null)
        {
            highlightImage.enabled = false;
            highlightImage.color = originalHighlightColor;
        }
    }

    /// <summary>
    /// Called when a clothing item is dropped on this point
    /// </summary>
    public void Occupy(DraggableClothing clothing)
    {
        IsOccupied = true;
        CurrentClothing = clothing;
        OnHoverExit();
    }

    /// <summary>
    /// Called when a clothing item is removed from this point
    /// </summary>
    public void Release()
    {
        IsOccupied = false;
        CurrentClothing = null;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw snap radius in editor for easy visualization
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, snapRadius);
    }
}
