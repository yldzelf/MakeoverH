using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(Image))]
public class DraggableClothing : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Drop Settings")]
    [Tooltip("List of valid drop points for this clothing item")]
    public List<DropPoint> acceptableDropPoints;

    [Header("Visual Feedback")]
    [Tooltip("Color when hovering over a valid drop point")]
    public Color validDropColor = new Color(0.5f, 1f, 0.5f, 1f);
    [Tooltip("Color when hovering over an invalid area")]
    public Color invalidDropColor = new Color(1f, 0.5f, 0.5f, 0.7f);

    private Vector3 startPosition;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Image image;
    private Color originalColor;
    private DropPoint currentHoveredDropPoint;
    private DropPoint equippedDropPoint;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (image != null)
        {
            originalColor = image.color;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;

        // Bring to front while dragging
        transform.SetAsLastSibling();

        // Allow raycasts to pass through while dragging (for drop detection)
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }

        // If currently equipped, release from drop point
        if (equippedDropPoint != null)
        {
            equippedDropPoint.Release();
            equippedDropPoint = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the UI element with the mouse
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        // Check for valid drop points
        DropPoint nearestValid = GetNearestValidDropPoint();

        if (nearestValid != currentHoveredDropPoint)
        {
            if (currentHoveredDropPoint != null)
            {
                currentHoveredDropPoint.OnHoverExit();
            }

            currentHoveredDropPoint = nearestValid;

            if (currentHoveredDropPoint != null)
            {
                currentHoveredDropPoint.OnHoverEnter();
            }
        }

        UpdateVisualFeedback(nearestValid != null);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Re-enable raycasts
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        // Reset color
        if (image != null)
        {
            image.color = originalColor;
        }

        DropPoint validDropPoint = GetNearestValidDropPoint();

        if (validDropPoint != null && !validDropPoint.IsOccupied)
        {
            // Snap to drop point
            rectTransform.anchoredPosition = validDropPoint.GetComponent<RectTransform>().anchoredPosition;
            validDropPoint.Occupy(this);
            equippedDropPoint = validDropPoint;
        }
        else
        {
            // Return to start position
            rectTransform.anchoredPosition = startPosition;
        }

        // Clear hover state
        if (currentHoveredDropPoint != null)
        {
            currentHoveredDropPoint.OnHoverExit();
            currentHoveredDropPoint = null;
        }
    }

    private DropPoint GetNearestValidDropPoint()
    {
        DropPoint nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (DropPoint dropPoint in acceptableDropPoints)
        {
            if (dropPoint == null) continue;

            RectTransform dropRectTransform = dropPoint.GetComponent<RectTransform>();
            if (dropRectTransform == null) continue;

            float distance = Vector2.Distance(rectTransform.anchoredPosition, dropRectTransform.anchoredPosition);

            if (distance <= dropPoint.snapRadius && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = dropPoint;
            }
        }

        return nearest;
    }

    private void UpdateVisualFeedback(bool isValidDrop)
    {
        if (image == null) return;

        image.color = isValidDrop ? validDropColor : invalidDropColor;
    }

    /// <summary>
    /// Resets the clothing to its original position
    /// </summary>
    public void ResetToStart()
    {
        if (equippedDropPoint != null)
        {
            equippedDropPoint.Release();
            equippedDropPoint = null;
        }

        rectTransform.anchoredPosition = startPosition;

        if (image != null)
        {
            image.color = originalColor;
        }
    }
}
