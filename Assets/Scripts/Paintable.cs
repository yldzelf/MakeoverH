using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Paintable : MonoBehaviour, IPointerClickHandler
{
    [Header("Target Settings")]
    [Tooltip("The Image to apply color to. If not set, uses this object's Image.")]
    public Image targetImage;

    [Header("Shader Property (Optional)")]
    [Tooltip("If using a custom shader, specify the color property name")]
    public string colorPropertyName = "_Color";
    public bool useShaderProperty = false;

    private Material materialInstance;

    private void Awake()
    {
        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        // Create material instance if using shader property
        if (useShaderProperty && targetImage != null)
        {
            materialInstance = Instantiate(targetImage.material);
            targetImage.material = materialInstance;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ToolManager.Instance == null) return;
        if (ToolManager.Instance.CurrentTool != ToolType.PaintBrush) return;

        ApplyColor(ToolManager.Instance.CurrentPaintColor);
    }

    public void ApplyColor(Color color)
    {
        if (useShaderProperty && materialInstance != null)
        {
            materialInstance.SetColor(colorPropertyName, color);
        }
        else if (targetImage != null)
        {
            targetImage.color = color;
        }
    }

    public Color GetCurrentColor()
    {
        if (useShaderProperty && materialInstance != null)
        {
            return materialInstance.GetColor(colorPropertyName);
        }
        else if (targetImage != null)
        {
            return targetImage.color;
        }
        return Color.white;
    }

    private void OnDestroy()
    {
        if (materialInstance != null)
        {
            Destroy(materialInstance);
        }
    }
}
