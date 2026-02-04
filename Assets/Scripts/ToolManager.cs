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

    [Header("Hardware Cursor Settings")]
    public Texture2D defaultCursor;
    public Texture2D shaverCursor;
    public Vector2 cursorHotspot = Vector2.zero;

    [Header("Paint Brush UI Cursor")]
    [Tooltip("UI Image that follows the mouse when paint brush is active")]
    public Image paintBrushCursorImage;
    [Tooltip("Offset from mouse position")]
    public Vector2 paintCursorOffset = Vector2.zero;

    [Header("Current State")]
    [SerializeField] private ToolType currentTool = ToolType.None;
    [SerializeField] private Color currentPaintColor = Color.white;

    public ToolType CurrentTool => currentTool;
    public Color CurrentPaintColor => currentPaintColor;

    public event System.Action<ToolType> OnToolChanged;
    public event System.Action<Color> OnPaintColorChanged;

    private Canvas parentCanvas;
    private RectTransform paintCursorRect;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (paintBrushCursorImage != null)
        {
            paintCursorRect = paintBrushCursorImage.GetComponent<RectTransform>();
            parentCanvas = paintBrushCursorImage.GetComponentInParent<Canvas>();
            paintBrushCursorImage.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Update paint cursor position to follow mouse
        if (currentTool == ToolType.PaintBrush && paintBrushCursorImage != null && paintCursorRect != null)
        {
            if (Mouse.current != null)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                paintCursorRect.position = mousePos + paintCursorOffset;
            }
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
        // Handle paint brush UI cursor
        if (paintBrushCursorImage != null)
        {
            bool showPaintCursor = currentTool == ToolType.PaintBrush;
            paintBrushCursorImage.gameObject.SetActive(showPaintCursor);

            if (showPaintCursor)
            {
                // Hide hardware cursor when using UI cursor
                Cursor.visible = false;
                return;
            }
        }

        // Show hardware cursor
        Cursor.visible = true;

        Texture2D cursorTexture = null;

        switch (currentTool)
        {
            case ToolType.Shaver:
                cursorTexture = shaverCursor;
                break;
            default:
                cursorTexture = defaultCursor;
                break;
        }

        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    private void OnDisable()
    {
        // Reset cursor when disabled
        Cursor.visible = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        if (paintBrushCursorImage != null)
        {
            paintBrushCursorImage.gameObject.SetActive(false);
        }
    }
}
