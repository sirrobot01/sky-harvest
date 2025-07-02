using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class EnhancedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Button Settings")]
    public ButtonStyle buttonStyle = ButtonStyle.Primary;
    public ButtonSize buttonSize = ButtonSize.Medium;
    
    [Header("Colors")]
    public Color primaryColor = new Color(0.2f, 0.6f, 1f, 1f); // Blue
    public Color secondaryColor = new Color(0.4f, 0.4f, 0.4f, 1f); // Gray
    public Color successColor = new Color(0.2f, 0.8f, 0.2f, 1f); // Green
    public Color dangerColor = new Color(0.8f, 0.2f, 0.2f, 1f); // Red
    
    [Header("Animation Settings")]
    public bool enableHoverAnimation = true;
    public bool enableClickAnimation = true;
    public float animationDuration = 0.2f;
    public float hoverScale = 1.05f;
    public float clickScale = 0.95f;
    
    [Header("Visual Effects")]
    public bool enableGlow = true;
    public bool enableShadow = true;
    public bool enableGradient = true;
    
    [Header("Custom Button Assets")]
    [Tooltip("Override sprites from Unity Asset Store")]
    public Sprite primaryButtonSprite;
    public Sprite secondaryButtonSprite;
    public Sprite successButtonSprite;
    public Sprite dangerButtonSprite;
    
    [Header("Asset Loading")]
    public bool useCustomSprites = true; // Enable by default
    public string assetPath = "ButtonAssets/";
    
    private Button button;
    private Image buttonImage;
    private TextMeshProUGUI buttonText;
    private Vector3 originalScale;
    private Color originalColor;
    private bool isHovered = false;
    private bool isPressed = false;
    
    public enum ButtonStyle
    {
        Primary,   // Main action buttons
        Secondary, // Secondary actions
        Success,   // Positive actions (Start, Continue)
        Danger     // Destructive actions (Quit, Delete)
    }
    
    public enum ButtonSize
    {
        Small,     // 120x40
        Medium,    // 160x50  
        Large,     // 200x60
        ExtraLarge // 250x80
    }
    
    void Start()
    {
        InitializeButton();
        SetupButtonAppearance();
        SetupButtonSize();
    }
    
    void InitializeButton()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }
        
        if (buttonImage == null)
        {
            buttonImage = gameObject.AddComponent<Image>();
        }
        
        originalScale = transform.localScale;
        originalColor = buttonImage.color;
    }
    
    void SetupButtonAppearance()
    {
        Color baseColor = GetBaseColor();
        
        // Set up the button's visual appearance
        if (buttonImage != null)
        {
            buttonImage.color = baseColor;
            
            // Create a simple rounded rectangle appearance programmatically
            CreateButtonSprite();
        }
        
        // Set up text appearance
        if (buttonText != null)
        {
            buttonText.color = Color.white;
            buttonText.fontSize = GetFontSize();
            buttonText.fontStyle = FontStyles.Bold;
            buttonText.alignment = TextAlignmentOptions.Center;
            
            // Add text shadow effect
            if (enableShadow)
            {
                // You can enhance this with a separate shadow text component
            }
        }
    }
    
    void SetupButtonSize()
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
            case ButtonSize.Small: return new Vector2(120f, 40f);
            case ButtonSize.Medium: return new Vector2(160f, 50f);
            case ButtonSize.Large: return new Vector2(200f, 60f);
            case ButtonSize.ExtraLarge: return new Vector2(250f, 80f);
            default: return new Vector2(160f, 50f);
        }
    }
    
    float GetFontSize()
    {
        switch (buttonSize)
        {
            case ButtonSize.Small: return 14f;
            case ButtonSize.Medium: return 18f;
            case ButtonSize.Large: return 22f;
            case ButtonSize.ExtraLarge: return 28f;
            default: return 18f;
        }
    }
    
    Color GetBaseColor()
    {
        switch (buttonStyle)
        {
            case ButtonStyle.Primary: return primaryColor;
            case ButtonStyle.Secondary: return secondaryColor;
            case ButtonStyle.Success: return successColor;
            case ButtonStyle.Danger: return dangerColor;
            default: return primaryColor;
        }
    }
    
    void CreateButtonSprite()
    {
        Sprite buttonSprite = null;
        
        if (useCustomSprites)
        {
            // Use custom sprites assigned in Inspector or loaded from Resources
            buttonSprite = GetCustomButtonSprite();
        }
        
        if (buttonSprite == null)
        {
            // Fallback: Create a simple solid color texture for the button
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();
            
            buttonSprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
        }
        
        buttonImage.sprite = buttonSprite;
        buttonImage.type = Image.Type.Sliced; // Enable 9-slice scaling for better resizing
    }
    
    Sprite GetCustomButtonSprite()
    {
        // First, check if sprites are directly assigned in Inspector
        switch (buttonStyle)
        {
            case ButtonStyle.Primary:
                if (primaryButtonSprite != null) return primaryButtonSprite;
                break;
            case ButtonStyle.Secondary:
                if (secondaryButtonSprite != null) return secondaryButtonSprite;
                break;
            case ButtonStyle.Success:
                if (successButtonSprite != null) return successButtonSprite;
                break;
            case ButtonStyle.Danger:
                if (dangerButtonSprite != null) return dangerButtonSprite;
                break;
        }
        
        // Fallback: Try to load from Resources using asset path
        string spriteName = GetSpriteNameForStyle();
        if (!string.IsNullOrEmpty(spriteName))
        {
            Sprite loadedSprite = Resources.Load<Sprite>(assetPath + spriteName);
            if (loadedSprite != null)
            {
                Debug.Log($"Loaded button sprite: {assetPath + spriteName}");
                return loadedSprite;
            }
        }
        
        return null;
    }
    
    string GetSpriteNameForStyle()
    {
        switch (buttonStyle)
        {
            case ButtonStyle.Primary: return "PrimaryButton";
            case ButtonStyle.Secondary: return "SecondaryButton";
            case ButtonStyle.Success: return "SucessButton"; // Note: matches your filename
            case ButtonStyle.Danger: return "DangerButton";
            default: return "PrimaryButton";
        }
    }
    
    // Event Handlers
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPressed && enableHoverAnimation)
        {
            isHovered = true;
            StartCoroutine(AnimateButton(hoverScale, GetHoverColor()));
        }
        
        // Play hover sound
        if (AudioManager.instance != null)
        {
            // You can add a hover sound here
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed)
        {
            isHovered = false;
            StartCoroutine(AnimateButton(1f, originalColor));
        }
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (enableClickAnimation)
        {
            isPressed = true;
            StartCoroutine(AnimateButton(clickScale, GetPressedColor()));
        }
        
        // Play click sound
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySelectClick();
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        
        if (isHovered)
        {
            StartCoroutine(AnimateButton(hoverScale, GetHoverColor()));
        }
        else
        {
            StartCoroutine(AnimateButton(1f, originalColor));
        }
    }
    
    Color GetHoverColor()
    {
        Color baseColor = GetBaseColor();
        return new Color(baseColor.r * 1.2f, baseColor.g * 1.2f, baseColor.b * 1.2f, baseColor.a);
    }
    
    Color GetPressedColor()
    {
        Color baseColor = GetBaseColor();
        return new Color(baseColor.r * 0.8f, baseColor.g * 0.8f, baseColor.b * 0.8f, baseColor.a);
    }
    
    IEnumerator AnimateButton(float targetScale, Color targetColor)
    {
        Vector3 startScale = transform.localScale;
        Color startColor = buttonImage.color;
        float elapsed = 0f;
        
        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / animationDuration;
            
            // Smooth animation curve
            progress = Mathf.SmoothStep(0f, 1f, progress);
            
            // Animate scale
            transform.localScale = Vector3.Lerp(startScale, originalScale * targetScale, progress);
            
            // Animate color
            buttonImage.color = Color.Lerp(startColor, targetColor, progress);
            
            yield return null;
        }
        
        // Ensure final values
        transform.localScale = originalScale * targetScale;
        buttonImage.color = targetColor;
    }
    
    // Public methods for external control
    public void SetButtonStyle(ButtonStyle style)
    {
        buttonStyle = style;
        SetupButtonAppearance();
    }
    
    public void SetButtonSize(ButtonSize size)
    {
        buttonSize = size;
        SetupButtonSize();
        SetupButtonAppearance();
    }
    
    public void SetButtonText(string text)
    {
        if (buttonText != null)
        {
            buttonText.text = text;
        }
    }
    
    // Animation presets
    [ContextMenu("Test Hover")]
    void TestHover()
    {
        StartCoroutine(AnimateButton(hoverScale, GetHoverColor()));
    }
    
    [ContextMenu("Test Click")]
    void TestClick()
    {
        StartCoroutine(TestClickSequence());
    }
    
    [ContextMenu("Test Sprite Loading")]
    void TestSpriteLoading()
    {
        Debug.Log($"Button: {gameObject.name}, Style: {buttonStyle}, Use Custom: {useCustomSprites}");
        
        if (useCustomSprites)
        {
            string spriteName = GetSpriteNameForStyle();
            string fullPath = assetPath + spriteName;
            Sprite testSprite = Resources.Load<Sprite>(fullPath);
            
            if (testSprite != null)
            {
                Debug.Log($"✓ Successfully loaded sprite: {fullPath}");
                Debug.Log($"Sprite size: {testSprite.texture.width}x{testSprite.texture.height}");
            }
            else
            {
                Debug.LogError($"✗ Failed to load sprite: {fullPath}");
            }
        }
    }
    
    [ContextMenu("Force Refresh Sprite")]
    void ForceRefreshSprite()
    {
        SetupButtonAppearance();
        Debug.Log($"Refreshed sprite for {buttonStyle} button");
    }
    
    IEnumerator TestClickSequence()
    {
        StartCoroutine(AnimateButton(clickScale, GetPressedColor()));
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(AnimateButton(1f, originalColor));
    }
}