using UnityEngine;
using TMPro;

public class FontAssetHelper : MonoBehaviour
{
    [Header("Font Recommendations")]
    [Tooltip("Download these free fonts for your game UI")]
    
    [Header("Google Fonts (Free) - Download from fonts.google.com")]
    public string[] recommendedFonts = {
        "Orbitron - Perfect for scores, timers (sci-fi feel)",
        "Exo 2 - Clean, modern UI font (all text)",
        "Rajdhani - Geometric, readable (headings)",
        "Share Tech Mono - Monospace (scores, timers)",
        "Audiowide - Bold display font (titles)",
        "Quantico - Military/tech style (action games)"
    };
    
    [Header("Font Usage Guide")]
    public string[] fontUsageGuide = {
        "TITLES/HEADINGS: Orbitron Bold, Audiowide",
        "BODY TEXT: Exo 2 Regular, Rajdhani Medium", 
        "SCORES/TIMERS: Share Tech Mono, Orbitron Light",
        "BUTTONS: Rajdhani Bold, Exo 2 Bold",
        "CONTROLS/INFO: Exo 2 Regular, Rajdhani Regular"
    };
    
    [Header("TextMeshPro Setup")]
    [Tooltip("Drag your downloaded font files here to create SDF assets")]
    public Font[] customFonts;
    
    [ContextMenu("Create Font Assets")]
    public void CreateFontAssets()
    {
        Debug.Log("=== FONT ASSET CREATION GUIDE ===");
        Debug.Log("1. Download fonts from fonts.google.com");
        Debug.Log("2. Import font files (.ttf) into Unity");
        Debug.Log("3. Window → TextMeshPro → Font Asset Creator");
        Debug.Log("4. Select font file as Source Font File");
        Debug.Log("5. Set Atlas Resolution to 512x512 or 1024x1024");
        Debug.Log("6. Click 'Generate Font Atlas'");
        Debug.Log("7. Save as SDF font asset");
        Debug.Log("8. Use in UIThemeManager component");
    }
    
    [ContextMenu("Apply Recommended Fonts to Theme")]
    public void ApplyRecommendedFontsToTheme()
    {
        UIThemeManager theme = UIThemeManager.Instance;
        if (theme == null)
        {
            Debug.LogError("No UIThemeManager found! Create DefaultUITheme asset first.");
            return;
        }
        
        Debug.Log("=== FONT APPLICATION GUIDE ===");
        Debug.Log("Manually assign these font types in UIThemeManager:");
        Debug.Log("• Heading Font: Bold display font (Orbitron, Audiowide)");
        Debug.Log("• Body Font: Readable text font (Exo 2, Rajdhani)");
        Debug.Log("• Button Font: Bold UI font (Rajdhani Bold, Exo 2 Bold)");
    }
    
    [ContextMenu("Show Font Size Recommendations")]
    public void ShowFontSizeRecommendations()
    {
        Debug.Log("=== RECOMMENDED FONT SIZES ===");
        Debug.Log("TITLES (H1): 48-72px - Use bold display fonts");
        Debug.Log("HEADINGS (H2/H3): 28-36px - Use medium weight");
        Debug.Log("BODY TEXT: 16-20px - Use regular weight");
        Debug.Log("BUTTONS: 18-24px - Use bold weight");
        Debug.Log("SCORES: 24-32px - Use monospace or bold");
        Debug.Log("TIMERS: 20-28px - Use monospace");
        Debug.Log("CAPTIONS: 12-16px - Use light/regular weight");
    }
    
    // Helper method to test font loading
    public void TestFontLoading()
    {
        TMP_FontAsset[] allFonts = Resources.LoadAll<TMP_FontAsset>("");
        
        Debug.Log($"Found {allFonts.Length} TextMeshPro fonts:");
        foreach (TMP_FontAsset font in allFonts)
        {
            Debug.Log($"• {font.name}");
        }
        
        if (allFonts.Length == 0)
        {
            Debug.LogWarning("No TextMeshPro fonts found! Import or create some SDF fonts.");
        }
    }
}