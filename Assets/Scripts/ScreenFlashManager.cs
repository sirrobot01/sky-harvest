using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFlashManager : MonoBehaviour
{
    [Header("Flash Settings")]
    [Range(0f, 1f)]
    public float damageFlashIntensity = 0.3f;
    [Range(0f, 1f)]
    public float damageFlashDuration = 0.15f;
    public Color damageFlashColor = Color.red;
    
    [Range(0f, 1f)]
    public float goldFlashIntensity = 0.2f;
    [Range(0f, 1f)]
    public float goldFlashDuration = 0.25f;
    public Color goldFlashColor = Color.yellow;
    
    [Range(0f, 1f)]
    public float healFlashIntensity = 0.15f;
    [Range(0f, 1f)]
    public float healFlashDuration = 0.3f;
    public Color healFlashColor = Color.green;
    
    [Range(0f, 1f)]
    public float deathFlashIntensity = 0.6f;
    [Range(0f, 2f)]
    public float deathFlashDuration = 0.8f;
    public Color deathFlashColor = Color.red;
    
    public static ScreenFlashManager instance;
    
    private Canvas flashCanvas;
    private Image flashImage;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        CreateFlashUI();
    }
    
    void CreateFlashUI()
    {
        // Create a Canvas for the flash overlay
        GameObject canvasObj = new GameObject("ScreenFlashCanvas");
        canvasObj.transform.SetParent(transform);
        
        flashCanvas = canvasObj.AddComponent<Canvas>();
        flashCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        flashCanvas.sortingOrder = 1000; // Very high priority to render on top
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Create the flash image
        GameObject flashObj = new GameObject("FlashImage");
        flashObj.transform.SetParent(flashCanvas.transform, false);
        
        flashImage = flashObj.AddComponent<Image>();
        flashImage.color = new Color(1, 1, 1, 0); // Start transparent
        
        // Make it cover the entire screen
        RectTransform rectTransform = flashImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
    
    public void FlashDamage()
    {
        StartFlash(damageFlashColor, damageFlashIntensity, damageFlashDuration);
    }
    
    public void FlashGold()
    {
        StartFlash(goldFlashColor, goldFlashIntensity, goldFlashDuration);
    }
    
    public void FlashHeal()
    {
        StartFlash(healFlashColor, healFlashIntensity, healFlashDuration);
    }
    
    public void FlashDeath()
    {
        StartFlash(deathFlashColor, deathFlashIntensity, deathFlashDuration);
    }
    
    public void FlashCustom(Color color, float intensity, float duration)
    {
        StartFlash(color, intensity, duration);
    }
    
    void StartFlash(Color color, float intensity, float duration)
    {
        if (flashImage == null) return;
        
        // Stop any current flash
        StopAllCoroutines();
        
        StartCoroutine(FlashCoroutine(color, intensity, duration));
    }
    
    IEnumerator FlashCoroutine(Color color, float intensity, float duration)
    {
        // Set the flash color with intensity
        Color flashColor = color;
        flashColor.a = intensity;
        flashImage.color = flashColor;
        
        float elapsed = 0f;
        
        // Fade out the flash
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // Use unscaled time
            float alpha = Mathf.Lerp(intensity, 0f, elapsed / duration);
            
            flashColor.a = alpha;
            flashImage.color = flashColor;
            
            yield return null;
        }
        
        // Ensure it's completely transparent
        flashColor.a = 0f;
        flashImage.color = flashColor;
    }
    
    // For debugging
    [ContextMenu("Test Damage Flash")]
    void TestDamageFlash()
    {
        FlashDamage();
    }
    
    [ContextMenu("Test Gold Flash")]
    void TestGoldFlash()
    {
        FlashGold();
    }
    
    [ContextMenu("Test Heal Flash")]
    void TestHealFlash()
    {
        FlashHeal();
    }
    
    [ContextMenu("Test Death Flash")]
    void TestDeathFlash()
    {
        FlashDeath();
    }
}