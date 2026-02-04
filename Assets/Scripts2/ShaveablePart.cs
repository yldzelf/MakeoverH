using UnityEngine;
using UnityEngine.EventSystems; // UI tıklamaları için bu kütüphane ŞART

// MonoBehaviour yanına virgül koyup IPointerClickHandler ekliyoruz
public class ShaveablePart : MonoBehaviour, IPointerClickHandler 
{
    // Bu fonksiyon UI elemanına (Image) tıklandığında otomatik çalışır
    public void OnPointerClick(PointerEventData eventData)
    {
        // Manager'a sor: Traş modu açık mı?
        if (ShavingManager.Instance.isShavingActive)
        {
            // Evet açık, o zaman bu UI görselini yok et
            Destroy(gameObject);
        }
    }
}