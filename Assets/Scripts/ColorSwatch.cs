using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColorSwatch : MonoBehaviour, IPointerClickHandler
{
    [Header("Color Settings")]
    [Tooltip("The color this swatch represents. If not set, uses the Image color.")]
    public Color swatchColor = Color.white;

    [Header("Optional")]
    [Tooltip("If true, automatically uses the Image component's color")]
    public bool useImageColor = true;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();

        if (useImageColor && image != null)
        {
            swatchColor = image.color;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ToolManager.Instance == null)
        {
            Debug.LogWarning("ToolManager not found in scene!");
            return;
        }

        // This will cancel any active tool (like shaver) and activate paint brush
        ToolManager.Instance.SetPaintBrush(swatchColor);
    }
}
