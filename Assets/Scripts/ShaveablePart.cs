using UnityEngine;
using UnityEngine.EventSystems;

public class ShaveablePart : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if shaving tool is active via ToolManager
        if (ShavingManager.Instance != null && ShavingManager.Instance.IsShavingActive)
        {
            Destroy(gameObject);
        }
    }
}