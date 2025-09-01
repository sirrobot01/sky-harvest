using UnityEngine;
using System.Collections;

public class EnhancedBackgroundManager : MonoBehaviour
{
    [Header("Background Sprites")]
    public Sprite dayBackground;
    public Sprite sunsetBackground;
    public Sprite nightBackground;
    
    [Header("Cycle Settings")]
    public bool enableDayNightCycle = true;
    public float cycleDuration = 60f; // How long each full cycle takes (in seconds)
    public float transitionDuration = 3f; // How long transitions take
    
    [Header("Parallax Settings")]
    public bool enableParallax = true;
    public float parallaxSpeed = 0.5f; // How fast background moves relative to game speed
    
    [Header("Scrolling Settings")]
    public float baseScrollSpeed = 4f;
    public bool followDragonSpeed = true;
    
    [Header("References")]
    public MeshRenderer backgroundRenderer;
    public Transform backgroundTransform;
    
    private Material backgroundMaterial;
    private PlayerDragonController dragon;
    private float cycleTimer = 0f;
    private bool isTransitioning = false;
    
    // Background phases
    public enum BackgroundPhase
    {
        Day,
        Sunset, 
        Night,
        Sunrise
    }
    
    private BackgroundPhase currentPhase = BackgroundPhase.Day;
    
    void Start()
    {
        InitializeBackground();
        
        // Find the dragon for speed reference
        dragon = FindFirstObjectByType<PlayerDragonController>();
        
        // Start with day background
        if (dayBackground != null)
        {
            SetBackgroundSprite(dayBackground);
        }
    }
    
    void InitializeBackground()
    {
        // Get components if not assigned
        if (backgroundRenderer == null)
            backgroundRenderer = GetComponent<MeshRenderer>();
        
        if (backgroundTransform == null)
            backgroundTransform = transform;
            
        if (backgroundRenderer != null)
        {
            backgroundMaterial = backgroundRenderer.material;
        }
        
        // Set up the material for scrolling
        if (backgroundMaterial != null)
        {
            // Enable texture scrolling
            backgroundMaterial.mainTextureOffset = Vector2.zero;
        }
    }
    
    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        UpdateScrolling();
        
        if (enableDayNightCycle)
        {
            UpdateDayNightCycle();
        }
    }
    
    void UpdateScrolling()
    {
        if (backgroundMaterial == null) return;
        
        // Calculate scroll speed
        float scrollSpeed = baseScrollSpeed;
        
        if (followDragonSpeed && dragon != null)
        {
            scrollSpeed *= PlayerDragonController.scrollSpeedMultiplier;
        }
        
        if (enableParallax)
        {
            scrollSpeed *= parallaxSpeed;
        }
        
        // Update texture offset for scrolling effect
        Vector2 currentOffset = backgroundMaterial.mainTextureOffset;
        currentOffset.x += scrollSpeed * Time.deltaTime;
        
        // Wrap the offset to prevent floating point precision issues
        if (currentOffset.x >= 1f)
        {
            currentOffset.x -= 1f;
        }
        else if (currentOffset.x <= -1f)
        {
            currentOffset.x += 1f;
        }
        
        backgroundMaterial.mainTextureOffset = currentOffset;
    }
    
    void UpdateDayNightCycle()
    {
        if (isTransitioning) return;
        
        cycleTimer += Time.deltaTime;
        
        // Calculate cycle progress (0 to 1)
        float cycleProgress = (cycleTimer / cycleDuration) % 1f;
        
        // Determine which phase we should be in
        BackgroundPhase targetPhase = GetPhaseFromProgress(cycleProgress);
        
        // If phase has changed, start transition
        if (targetPhase != currentPhase)
        {
            StartCoroutine(TransitionToPhase(targetPhase));
        }
    }
    
    BackgroundPhase GetPhaseFromProgress(float progress)
    {
        // Divide the cycle into 4 equal phases
        if (progress < 0.25f)
            return BackgroundPhase.Day;
        else if (progress < 0.5f)
            return BackgroundPhase.Sunset;
        else if (progress < 0.75f)
            return BackgroundPhase.Night;
        else
            return BackgroundPhase.Sunrise; // Sunrise is like day but different timing
    }
    
    IEnumerator TransitionToPhase(BackgroundPhase newPhase)
    {
        isTransitioning = true;
        
        Sprite newSprite = GetSpriteForPhase(newPhase);
        if (newSprite == null)
        {
            isTransitioning = false;
            yield break;
        }
        
        // For now, do an instant transition
        // You could add fading effects here later
        SetBackgroundSprite(newSprite);
        currentPhase = newPhase;
        
        Debug.Log($"Background changed to: {newPhase}");
        
        yield return new WaitForSeconds(0.1f); // Brief pause
        
        isTransitioning = false;
    }
    
    Sprite GetSpriteForPhase(BackgroundPhase phase)
    {
        switch (phase)
        {
            case BackgroundPhase.Day:
            case BackgroundPhase.Sunrise:
                return dayBackground;
            case BackgroundPhase.Sunset:
                return sunsetBackground;
            case BackgroundPhase.Night:
                return nightBackground;
            default:
                return dayBackground;
        }
    }
    
    void SetBackgroundSprite(Sprite sprite)
    {
        if (backgroundMaterial != null && sprite != null)
        {
            backgroundMaterial.mainTexture = sprite.texture;
        }
    }
    
    // Public methods for manual control
    public void SetToDayBackground()
    {
        if (dayBackground != null)
        {
            SetBackgroundSprite(dayBackground);
            currentPhase = BackgroundPhase.Day;
        }
    }
    
    public void SetToSunsetBackground()
    {
        if (sunsetBackground != null)
        {
            SetBackgroundSprite(sunsetBackground);
            currentPhase = BackgroundPhase.Sunset;
        }
    }
    
    public void SetToNightBackground()
    {
        if (nightBackground != null)
        {
            SetBackgroundSprite(nightBackground);
            currentPhase = BackgroundPhase.Night;
        }
    }
    
    public void SetScrollSpeed(float speed)
    {
        baseScrollSpeed = speed;
    }
    
    public void SetParallaxSpeed(float speed)
    {
        parallaxSpeed = speed;
    }
    
    // For debugging
    [ContextMenu("Test Day")]
    void TestDay() { SetToDayBackground(); }
    
    [ContextMenu("Test Sunset")]
    void TestSunset() { SetToSunsetBackground(); }
    
    [ContextMenu("Test Night")]
    void TestNight() { SetToNightBackground(); }
    
    [ContextMenu("Reset Cycle")]
    void ResetCycle() 
    { 
        cycleTimer = 0f; 
        isTransitioning = false;
        currentPhase = BackgroundPhase.Day;
        SetToDayBackground();
    }
}