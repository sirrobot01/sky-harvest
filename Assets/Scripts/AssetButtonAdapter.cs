using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

[RequireComponent(typeof(Button))]
public class AssetButtonAdapter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Asset Button Settings")]
    public EnhancedButton.ButtonStyle buttonStyle = EnhancedButton.ButtonStyle.Primary;
    public EnhancedButton.ButtonSize buttonSize = EnhancedButton.ButtonSize.Medium;
    
    [Header("Asset Components")]
    [Tooltip("The main Image component from your asset")]
    public Image assetButtonImage;
    [Tooltip("Any animator component from your asset")]
    public Animator assetAnimator;
    [Tooltip("Any additional effects from your asset")]
    public ParticleSystem buttonEffects;
    
    [Header("Animation Triggers")]
    [Tooltip("Animator trigger names from your asset")]
    public string hoverTrigger = "OnHover";
    public string clickTrigger = "OnClick";
    public string normalTrigger = "OnNormal";
    
    [Header("Enhanced Features")]
    public bool enableEnhancedFeatures = true;
    public float scaleMultiplier = 1.05f;
    
    private Button button;
    private Vector3 originalScale;
    private bool isHovered = false;
    
    void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
        
        // If no asset components assigned, try to find them automatically
        if (assetButtonImage == null)
            assetButtonImage = GetComponent<Image>();
        if (assetAnimator == null)
            assetAnimator = GetComponent<Animator>();
        if (buttonEffects == null)
            buttonEffects = GetComponentInChildren<ParticleSystem>();
            
        ConfigureButtonSize();
    }
    
    void ConfigureButtonSize()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform == null) return;
        
        Vector2 size = GetSizeForButtonSize();
        rectTransform.sizeDelta = size;
    }
    
    Vector2 GetSizeForButtonSize()
    {
        switch (buttonSize)
        {
            case EnhancedButton.ButtonSize.Small: return new Vector2(120f, 40f);
            case EnhancedButton.ButtonSize.Medium: return new Vector2(160f, 50f);
            case EnhancedButton.ButtonSize.Large: return new Vector2(200f, 60f);
            case EnhancedButton.ButtonSize.ExtraLarge: return new Vector2(250f, 80f);
            default: return new Vector2(160f, 50f);
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        
        // Trigger asset animator
        if (assetAnimator != null && !string.IsNullOrEmpty(hoverTrigger))
        {
            assetAnimator.SetTrigger(hoverTrigger);
        }
        
        // Enhanced scale effect
        if (enableEnhancedFeatures)
        {
            transform.localScale = originalScale * scaleMultiplier;
        }
        
        // Play particle effects
        if (buttonEffects != null)
        {
            buttonEffects.Play();
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        
        // Trigger asset animator
        if (assetAnimator != null && !string.IsNullOrEmpty(normalTrigger))
        {
            assetAnimator.SetTrigger(normalTrigger);
        }
        
        // Reset scale
        if (enableEnhancedFeatures)
        {
            transform.localScale = originalScale;
        }
        
        // Stop particle effects
        if (buttonEffects != null)
        {
            buttonEffects.Stop();
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        // Trigger asset animator
        if (assetAnimator != null && !string.IsNullOrEmpty(clickTrigger))
        {
            assetAnimator.SetTrigger(clickTrigger);
        }
        
        // Enhanced click effect
        if (enableEnhancedFeatures)
        {
            transform.localScale = originalScale * 0.95f;
        }
        
        // Play click sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySelectClick();
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        // Return to appropriate state
        if (isHovered)
        {
            OnPointerEnter(eventData);
        }
        else
        {
            OnPointerExit(eventData);
        }
    }
    
    public void SetButtonText(string text)
    {
        TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null)
        {
            textComponent.text = text;
        }
        else
        {
            // Fallback to regular Text
            Text regularText = GetComponentInChildren<Text>();
            if (regularText != null)
            {
                regularText.text = text;
            }
        }
    }
    
    // Integration with theme system
    public void ApplyTheme(UIThemeManager theme)
    {
        if (theme == null || assetButtonImage == null) return;
        
        // Apply theme colors to asset button
        Color themeColor = GetThemeColorForStyle(theme);
        assetButtonImage.color = themeColor;
        
        // Apply theme font to text
        TextMeshProUGUI textComponent = GetComponentInChildren<TextMeshProUGUI>();
        if (textComponent != null && theme.buttonFont != null)
        {
            textComponent.font = theme.buttonFont;
            textComponent.fontSize = theme.buttonSize;
        }
    }
    
    Color GetThemeColorForStyle(UIThemeManager theme)
    {
        switch (buttonStyle)
        {
            case EnhancedButton.ButtonStyle.Primary: return theme.primaryColor;
            case EnhancedButton.ButtonStyle.Secondary: return theme.secondaryColor;
            case EnhancedButton.ButtonStyle.Success: return theme.successTextColor;
            case EnhancedButton.ButtonStyle.Danger: return theme.dangerTextColor;
            default: return theme.primaryColor;
        }
    }
}