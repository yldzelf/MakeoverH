using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum ToolType
{
    None,
    Shaver,
    PaintBrush
}

public class ToolManager : MonoBehaviour
{
    public static ToolManager Instance { get; private set; }

    [Header("UI Cursor Images")]
    [Tooltip("UI Image for the default hand cursor")]
    public Image handCursorImage;
    [Tooltip("UI Image for the shaver cursor")]
    public Image shaverCursorImage;
    [Tooltip("UI Image for the paint brush cursor")]
    public Image paintBrushCursorImage;

    [Header("Cursor Settings")]
    public Vector2 handCursorOffset = Vector2.zero;
    public Vector2 shaverCursorOffset = Vector2.zero;
    public Vector2 paintCursorOffset = Vector2.zero;

    [Header("Current State")]
    [SerializeField] private ToolType currentTool = ToolType.None;
    [SerializeField] private Color currentPaintColor = Color.white;

    public ToolType CurrentTool => currentTool;
    public Color CurrentPaintColor => currentPaintColor;

    public event System.Action<ToolType> OnToolChanged;
    public event System.Action<Color> OnPaintColorChanged;

    private RectTransform handCursorRect;
    private RectTransform shaverCursorRect;
    private RectTransform paintCursorRect;
    private Image activeCursorImage;
    private Vector2 activeCursorOffset;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Cache RectTransforms and hide all cursors initially
        if (handCursorImage != null)
        {
            handCursorRect = handCursorImage.GetComponent<RectTransform>();
            handCursorImage.gameObject.SetActive(false);
        }
        if (shaverCursorImage != null)
        {
            shaverCursorRect = shaverCursorImage.GetComponent<RectTransform>();
            shaverCursorImage.gameObject.SetActive(false);
        }
        if (paintBrushCursorImage != null)
        {
            paintCursorRect = paintBrushCursorImage.GetComponent<RectTransform>();
            paintBrushCursorImage.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        // Activate default hand cursor on start
        UpdateCursor();
    }

    private void Update()
    {
        // Update active cursor position to follow mouse
        if (activeCursorImage != null && Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            activeCursorImage.rectTransform.position = mousePos + activeCursorOffset;
        }
    }

    public void SetTool(ToolType tool)
    {
        if (currentTool == tool) return;

        currentTool = tool;
        UpdateCursor();
        OnToolChanged?.Invoke(currentTool);
    }

    public void SetPaintBrush(Color color)
    {
        currentPaintColor = color;

        // Update the UI cursor color
        if (paintBrushCursorImage != null)
        {
            paintBrushCursorImage.color = color;
        }

        // Force tool change even if already paint brush (to update color)
        if (currentTool == ToolType.PaintBrush)
        {
            OnPaintColorChanged?.Invoke(color);
        }
        else
        {
            SetTool(ToolType.PaintBrush);
            OnPaintColorChanged?.Invoke(color);
        }
    }

    public void SetShaver()
    {
        SetTool(ToolType.Shaver);
    }

    public void ClearTool()
    {
        SetTool(ToolType.None);
    }

    private void UpdateCursor()
    {
        // Hide hardware cursor
        Cursor.visible = false;

        // Deactivate all UI cursors
        if (handCursorImage != null) handCursorImage.gameObject.SetActive(false);
        if (shaverCursorImage != null) shaverCursorImage.gameObject.SetActive(false);
        if (paintBrushCursorImage != null) paintBrushCursorImage.gameObject.SetActive(false);

        // Activate the appropriate cursor
        switch (currentTool)
        {
            case ToolType.Shaver:
                if (shaverCursorImage != null)
                {
                    shaverCursorImage.gameObject.SetActive(true);
                    activeCursorImage = shaverCursorImage;
                    activeCursorOffset = shaverCursorOffset;
                }
                break;
            case ToolType.PaintBrush:
                if (paintBrushCursorImage != null)
                {
                    paintBrushCursorImage.gameObject.SetActive(true);
                    activeCursorImage = paintBrushCursorImage;
                    activeCursorOffset = paintCursorOffset;
                }
                break;
            default: // None - show hand cursor
                if (handCursorImage != null)
                {
                    handCursorImage.gameObject.SetActive(true);
                    activeCursorImage = handCursorImage;
                    activeCursorOffset = handCursorOffset;
                }
                break;
        }
    }

    private void OnDisable()
    {
        // Reset cursor when disabled
        Cursor.visible = true;

        if (handCursorImage != null) handCursorImage.gameObject.SetActive(false);
        if (shaverCursorImage != null) shaverCursorImage.gameObject.SetActive(false);
        if (paintBrushCursorImage != null) paintBrushCursorImage.gameObject.SetActive(false);

        activeCursorImage = null;
    }
}
