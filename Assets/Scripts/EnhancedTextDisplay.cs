using UnityEngine;
using TMPro;
using System.Collections;

public class EnhancedTextDisplay : MonoBehaviour
{
    [Header("Text Style")]
    public TextStyle textStyle = TextStyle.Body;
    public TextColor textColor = TextColor.Primary;
    
    [Header("Animation")]
    public bool enableTypewriter = false;
    public float typewriterSpeed = 0.05f;
    public bool enablePulse = false;
    public float pulseSpeed = 2f;
    public float pulseIntensity = 0.1f;
    
    [Header("Effects")]
    public bool enableShadow = true;
    public bool enableOutline = false;
    public bool enableGlow = false;
    
    private TextMeshProUGUI textComponent;
    private string fullText;
    private Vector3 originalScale;
    
    public enum TextStyle
    {
        Heading1,      // Large titles
        Heading2,      // Section headers  
        Heading3,      // Subsection headers
        Body,          // Normal text
        Caption,       // Small text
        Button,        // Button text
        Score,         // Score display
        Timer          // Timer display
    }
    
    public enum TextColor
    {
        Primary,       // Main text color (white/black)
        Secondary,     // Secondary text (gray)
        Success,       // Success messages (green)
        Warning,       // Warning messages (yellow)
        Danger,        // Error messages (red)
        Info,          // Info messages (blue)
        Score,         // Score text (gold/yellow)
        Timer          // Timer text (white/red when low)
    }
    
    void Start()
    {
        InitializeText();
        ApplyTextStyle();
        
        if (enableTypewriter && !string.IsNullOrEmpty(fullText))
        {
            StartTypewriterEffect();
        }
    }
    
    void Update()
    {
        if (enablePulse)
        {
            ApplyPulseEffect();
        }
    }
    
    void InitializeText()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        
        if (textComponent == null)
        {
            textComponent = gameObject.AddComponent<TextMeshProUGUI>();
        }
        
        fullText = textComponent.text;
        originalScale = transform.localScale;
    }
    
    void ApplyTextStyle()
    {
        if (textComponent == null) return;
        
        // Apply font size and style based on TextStyle
        ApplyFontSettings();
        
        // Apply color based on TextColor
        ApplyColorSettings();
        
        // Apply effects
        ApplyTextEffects();
    }
    
    void ApplyFontSettings()
    {
        switch (textStyle)
        {
            case TextStyle.Heading1:
                textComponent.fontSize = 30f;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.alignment = TextAlignmentOptions.Center;
                break;
                
            case TextStyle.Heading2:
                textComponent.fontSize = 28f;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.alignment = TextAlignmentOptions.Center;
                break;
                
            case TextStyle.Heading3:
                textComponent.fontSize = 22f;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.alignment = TextAlignmentOptions.Left;
                break;
                
            case TextStyle.Body:
                textComponent.fontSize = 18f;
                textComponent.fontStyle = FontStyles.Normal;
                textComponent.alignment = TextAlignmentOptions.Left;
                break;
                
            case TextStyle.Caption:
                textComponent.fontSize = 14f;
                textComponent.fontStyle = FontStyles.Normal;
                textComponent.alignment = TextAlignmentOptions.Left;
                break;
                
            case TextStyle.Button:
                textComponent.fontSize = 20f;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.alignment = TextAlignmentOptions.Center;
                break;
                
            case TextStyle.Score:
                textComponent.fontSize = 32f;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.alignment = TextAlignmentOptions.TopRight;
                break;
                
            case TextStyle.Timer:
                textComponent.fontSize = 28f;
                textComponent.fontStyle = FontStyles.Bold;
                textComponent.alignment = TextAlignmentOptions.Center;
                break;
        }
    }
    
    void ApplyColorSettings()
    {
        Color targetColor = GetColorForStyle();
        textComponent.color = targetColor;
    }
    
    Color GetColorForStyle()
    {
        switch (textColor)
        {
            case TextColor.Primary: return Color.white;
            case TextColor.Secondary: return new Color(0.7f, 0.7f, 0.7f);
            case TextColor.Success: return new Color(0.2f, 0.8f, 0.2f);
            case TextColor.Warning: return new Color(1f, 0.8f, 0f);
            case TextColor.Danger: return new Color(0.8f, 0.2f, 0.2f);
            case TextColor.Info: return new Color(0.2f, 0.6f, 1f);
            case TextColor.Score: return new Color(1f, 0.8f, 0f); // Gold
            case TextColor.Timer: return Color.white;
            default: return Color.white;
        }
    }
    
    void ApplyTextEffects()
    {
        if (enableShadow)
        {
            // Enable shadow using TextMeshPro's material approach (safe fallback)
            // Just enable outline as a shadow alternative
            if (textComponent.outlineWidth == 0)
            {
                textComponent.outlineWidth = 0.1f;
                textComponent.outlineColor = new Color(0, 0, 0, 0.5f);
            }
        }
        
        if (enableOutline)
        {
            // Add outline effect
            textComponent.outlineWidth = 0.2f;
            textComponent.outlineColor = Color.black;
        }
        
        if (enableGlow)
        {
            // Add glow effect using outline with lighter color
            textComponent.outlineWidth = 0.3f;
            textComponent.outlineColor = new Color(1f, 1f, 1f, 0.3f);
        }
    }
    
    void StartTypewriterEffect()
    {
        textComponent.text = "";
        StartCoroutine(TypewriterCoroutine());
    }
    
    IEnumerator TypewriterCoroutine()
    {
        for (int i = 0; i <= fullText.Length; i++)
        {
            textComponent.text = fullText.Substring(0, i);
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
    
    void ApplyPulseEffect()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
        transform.localScale = originalScale * pulse;
    }
    
    // Public methods for dynamic updates
    public void SetText(string newText)
    {
        fullText = newText;
        
        if (enableTypewriter)
        {
            StartTypewriterEffect();
        }
        else if (textComponent != null)
        {
            textComponent.text = newText;
        }
    }
    
    public void SetTextStyle(TextStyle style)
    {
        textStyle = style;
        ApplyTextStyle();
    }
    
    public void SetTextColor(TextColor color)
    {
        textColor = color;
        ApplyColorSettings();
    }
    
    public void AnimateIn()
    {
        StartCoroutine(AnimateInCoroutine());
    }
    
    public void AnimateOut()
    {
        StartCoroutine(AnimateOutCoroutine());
    }
    
    IEnumerator AnimateInCoroutine()
    {
        transform.localScale = Vector3.zero;
        float elapsed = 0f;
        float duration = 0.5f;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;
            progress = Mathf.SmoothStep(0f, 1f, progress);
            
            transform.localScale = Vector3.Lerp(Vector3.zero, originalScale, progress);
            yield return null;
        }
        
        transform.localScale = originalScale;
    }
    
    IEnumerator AnimateOutCoroutine()
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;
        float duration = 0.3f;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float progress = elapsed / duration;
            
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, progress);
            yield return null;
        }
        
        transform.localScale = Vector3.zero;
    }
    
    // Special methods for score and timer
    public void UpdateScore(int score)
    {
        if (textStyle == TextStyle.Score)
        {
            SetText($"Score: {score:N0}");
            StartCoroutine(ScoreUpdateAnimation());
        }
    }
    
    public void UpdateTimer(float timeRemaining)
    {
        if (textStyle == TextStyle.Timer)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            SetText($"{minutes:00}:{seconds:00}");
            
            // Change color when time is low (with null check)
            if (textComponent != null)
            {
                if (timeRemaining <= 30f)
                {
                    textComponent.color = Color.red;
                }
                else if (timeRemaining <= 60f)
                {
                    textComponent.color = Color.yellow;
                }
                else
                {
                    textComponent.color = Color.white;
                }
            }
        }
    }
    
    IEnumerator ScoreUpdateAnimation()
    {
        Vector3 startScale = originalScale;
        Vector3 targetScale = originalScale * 1.2f;
        
        // Scale up
        float elapsed = 0f;
        while (elapsed < 0.1f)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsed / 0.1f);
            yield return null;
        }
        
        // Scale back down
        elapsed = 0f;
        while (elapsed < 0.2f)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(targetScale, startScale, elapsed / 0.2f);
            yield return null;
        }
        
        transform.localScale = startScale;
    }
    
    // Context menu for testing
    [ContextMenu("Test Animate In")]
    void TestAnimateIn() { AnimateIn(); }
    
    [ContextMenu("Test Animate Out")]
    void TestAnimateOut() { AnimateOut(); }
    
    [ContextMenu("Test Typewriter")]
    void TestTypewriter() { StartTypewriterEffect(); }
}