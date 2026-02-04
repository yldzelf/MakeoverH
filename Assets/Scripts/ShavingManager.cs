using UnityEngine;

public class ShavingManager : MonoBehaviour
{
    public static ShavingManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Listen for tool changes to update our state
        if (ToolManager.Instance != null)
        {
            ToolManager.Instance.OnToolChanged += OnToolChanged;
        }
    }

    void OnDestroy()
    {
        if (ToolManager.Instance != null)
        {
            ToolManager.Instance.OnToolChanged -= OnToolChanged;
        }
    }

    private void OnToolChanged(ToolType newTool)
    {
        // Log state changes for debugging
        if (newTool == ToolType.Shaver)
        {
            Debug.Log("Traş Modu AÇIK");
        }
        else if (IsShavingActive)
        {
            Debug.Log("Traş Modu KAPALI");
        }
    }

    // Property to check if shaving is active (uses ToolManager)
    public bool IsShavingActive
    {
        get
        {
            return ToolManager.Instance != null &&
                   ToolManager.Instance.CurrentTool == ToolType.Shaver;
        }
    }

    // Connect this to your UI button
    public void ToggleShavingMode()
    {
        if (ToolManager.Instance == null)
        {
            Debug.LogWarning("ToolManager not found in scene!");
            return;
        }

        if (IsShavingActive)
        {
            // Turn off shaving
            ToolManager.Instance.ClearTool();
        }
        else
        {
            // Turn on shaving (this will cancel any other active tool)
            ToolManager.Instance.SetShaver();
        }
    }

    // Call this to activate shaving directly (non-toggle)
    public void ActivateShaving()
    {
        if (ToolManager.Instance != null)
        {
            ToolManager.Instance.SetShaver();
        }
    }

    // Call this to deactivate shaving
    public void DeactivateShaving()
    {
        if (ToolManager.Instance != null && IsShavingActive)
        {
            ToolManager.Instance.ClearTool();
        }
    }
}