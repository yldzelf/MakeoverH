using UnityEngine;

public class ShavingManager : MonoBehaviour
{
    // Diğer scriptlerden buna kolayca ulaşmak için static yapıyoruz
    public static ShavingManager Instance;

    [Header("Ayarlar")]
    public Texture2D razorCursor; // Jilet imlecini buraya sürükleyeceksin
    public bool isShavingActive = false; // Traş modu açık mı?

    void Awake()
    {
        // Singleton yapısı
        if (Instance == null) Instance = this;
    }

    // Bu fonksiyonu UI'daki butona bağlayacağız
    public void ToggleShavingMode()
    {
        isShavingActive = !isShavingActive; // Açiksa kapat, kapalıysa aç

        if (isShavingActive)
        {
            // Cursor'ı Jilet yap (Vector2.zero sol üst köşeden tıklar)
            Cursor.SetCursor(razorCursor, Vector2.zero, CursorMode.Auto);
            Debug.Log("Traş Modu AÇIK");
        }
        else
        {
            // Cursor'ı normale döndür
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            Debug.Log("Traş Modu KAPALI");
        }
    }
}