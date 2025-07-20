using UnityEngine;
using TMPro;

public class FontTester : MonoBehaviour
{
    [Header("Font Testing")]
    [Tooltip("Test your imported fonts")]
    public bool testFontsOnStart = true;
    
    [Header("Sample Texts")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public TextMeshProUGUI scoreText;
    
    void Start()
    {
        if (testFontsOnStart)
        {
            TestFontLoading();
        }
    }
    
    [ContextMenu("Test Font Loading")]
    public void TestFontLoading()
    {
        Debug.Log("=== TESTING FONT IMPORTS ===");
        
        // Test if fonts are properly imported
        TMP_FontAsset[] allFonts = Resources.FindObjectsOfTypeAll<TMP_FontAsset>();
        
        Debug.Log($"Found {allFonts.Length} TextMeshPro font assets:");
        foreach (TMP_FontAsset font in allFonts)
        {
            Debug.Log($"• {font.name}");
            
            // Check for your specific fonts
            if (font.name.ToLower().Contains("orbitron"))
            {
                Debug.Log($"  ✓ Orbitron font found: {font.name}");
            }
            if (font.name.ToLower().Contains("rubik"))
            {
                Debug.Log($"  ✓ Rubik font found: {font.name}");
            }
        }
        
        // Test theme font assignment
        TestThemeFonts();
    }
    
    void TestThemeFonts()
    {
        UIThemeManager theme = UIThemeManager.Instance;
        if (theme == null)
        {
            Debug.LogError("No UIThemeManager found! Create DefaultUITheme asset first.");
            return;
        }
        
        Debug.Log("=== TESTING THEME FONT ASSIGNMENTS ===");
        
        if (theme.headingFont != null)
            Debug.Log($"✓ Heading Font: {theme.headingFont.name}");
        else
            Debug.LogWarning("✗ Heading Font not assigned in theme");
            
        if (theme.bodyFont != null)
            Debug.Log($"✓ Body Font: {theme.bodyFont.name}");
        else
            Debug.LogWarning("✗ Body Font not assigned in theme");
            
        if (theme.buttonFont != null)
            Debug.Log($"✓ Button Font: {theme.buttonFont.name}");
        else
            Debug.LogWarning("✗ Button Font not assigned in theme");
            
        if (theme.scoreFont != null)
            Debug.Log($"✓ Score Font: {theme.scoreFont.name}");
        else
            Debug.LogWarning("✗ Score Font not assigned in theme");
    }
    
    [ContextMenu("Apply Sample Text")]
    public void ApplySampleText()
    {
        // Create sample text to see fonts in action
        if (titleText != null)
        {
            titleText.text = "SKY HARVEST";
            titleText.fontSize = 48f;
        }
        
        if (bodyText != null)
        {
            bodyText.text = "Use Arrow Keys to Move\nPress Space to Jump";
            bodyText.fontSize = 18f;
        }
        
        if (scoreText != null)
        {
            scoreText.text = "SCORE: 12,500\nTIME: 02:30";
            scoreText.fontSize = 24f;
        }
        
        Debug.Log("Applied sample text. Check your scene to see font rendering.");
    }
    
    [ContextMenu("Show Font Recommendations")]
    public void ShowFontRecommendations()
    {
        Debug.Log("=== FONT USAGE RECOMMENDATIONS ===");
        Debug.Log("ORBITRON (Sci-fi, clean numbers):");
        Debug.Log("  • Orbitron-Bold → Headings, Titles");
        Debug.Log("  • Orbitron-Regular → Scores, Timers, Numbers");
        Debug.Log("");
        Debug.Log("RUBIK (Modern, readable):");
        Debug.Log("  • Rubik-Bold → Button Text");
        Debug.Log("  • Rubik-Regular → Body Text, Instructions");
        Debug.Log("");
        Debug.Log("PERFECT FOR YOUR GAME:");
        Debug.Log("  • Game Title: Orbitron-Bold, 48px");
        Debug.Log("  • Score Display: Orbitron-Regular, 24px");
        Debug.Log("  • Timer: Orbitron-Regular, 20px");
        Debug.Log("  • Button Text: Rubik-Bold, 18px");
        Debug.Log("  • Instructions: Rubik-Regular, 16px");
    }
}