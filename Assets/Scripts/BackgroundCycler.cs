using UnityEngine;
using System.Collections;

public class BackgroundCycler : MonoBehaviour
{
    [Header("Background Sprites")]
    [Tooltip("Drag your day, sunset, and night sprites here")]
    public Sprite daySprite;
    public Sprite sunsetSprite; 
    public Sprite nightSprite;
    
    [Header("Cycle Settings")]
    public bool enableCycling = true;
    [Tooltip("Time in seconds for a full day/night cycle")]
    public float fullCycleDuration = 90f;
    [Tooltip("How long each transition takes")]
    public float transitionDuration = 2f;
    
    [Header("Manual Control")]
    [Tooltip("Set to true to manually control background instead of auto-cycling")]
    public bool manualControl = false;
    
    private SpriteRenderer spriteRenderer;
    private Material backgroundMaterial;
    private MeshRenderer meshRenderer;
    private float cycleTimer = 0f;
    private int currentPhase = 0; // 0=Day, 1=Sunset, 2=Night
    private bool isTransitioning = false;
    
    void Start()
    {
        // Try to find components on this GameObject or its children
        spriteRenderer = GetComponent<SpriteRenderer>();
        meshRenderer = GetComponent<MeshRenderer>();
        
        // If we have a MeshRenderer (like your ScrollingBackground), work with its material
        if (meshRenderer != null)
        {
            backgroundMaterial = meshRenderer.material;
        }
        
        // Start with day background
        SetBackground(0);
        
        Debug.Log($"BackgroundCycler initialized. Found MeshRenderer: {meshRenderer != null}, SpriteRenderer: {spriteRenderer != null}");
    }
    
    void Update()
    {
        if (!enableCycling || manualControl || isTransitioning || GameManager.isLevelOver) 
            return;
            
        UpdateCycle();
    }
    
    void UpdateCycle()
    {
        cycleTimer += Time.deltaTime;
        
        // Calculate which phase we should be in
        float cycleProgress = cycleTimer / fullCycleDuration;
        int targetPhase = Mathf.FloorToInt(cycleProgress * 3f) % 3; // 3 phases: 0, 1, 2
        
        // If phase changed, transition
        if (targetPhase != currentPhase)
        {
            StartCoroutine(TransitionToPhase(targetPhase));
        }
        
        // Reset cycle timer to prevent overflow
        if (cycleTimer >= fullCycleDuration)
        {
            cycleTimer = 0f;
        }
    }
    
    IEnumerator TransitionToPhase(int newPhase)
    {
        isTransitioning = true;
        // Simple instant transition for now
        // You could add fade effects here later
        SetBackground(newPhase);
        currentPhase = newPhase;
        
        yield return new WaitForSeconds(0.1f);
        
        isTransitioning = false;
    }
    
    void SetBackground(int phase)
    {
        Sprite targetSprite = GetSpriteForPhase(phase);
        if (targetSprite == null) return;
        
        // Set sprite based on what components we have
        if (meshRenderer != null && backgroundMaterial != null)
        {
            // For MeshRenderer (your ScrollingBackground setup)
            backgroundMaterial.mainTexture = targetSprite.texture;
            Debug.Log($"Set mesh renderer texture to {GetPhaseName(phase)}");
        }
        else if (spriteRenderer != null)
        {
            // For SpriteRenderer
            spriteRenderer.sprite = targetSprite;
            Debug.Log($"Set sprite renderer to {GetPhaseName(phase)}");
        }
    }
    
    Sprite GetSpriteForPhase(int phase)
    {
        switch (phase)
        {
            case 0: return daySprite;
            case 1: return sunsetSprite;
            case 2: return nightSprite;
            default: return daySprite;
        }
    }
    
    string GetPhaseName(int phase)
    {
        switch (phase)
        {
            case 0: return "Day";
            case 1: return "Sunset";
            case 2: return "Night";
            default: return "Unknown";
        }
    }
    
    // Public methods for manual control
    [ContextMenu("Set Day")]
    public void SetDay()
    {
        SetBackground(0);
        currentPhase = 0;
        cycleTimer = 0f;
    }
    
    [ContextMenu("Set Sunset")]
    public void SetSunset()
    {
        SetBackground(1);
        currentPhase = 1;
        cycleTimer = fullCycleDuration * 0.33f;
    }
    
    [ContextMenu("Set Night")]
    public void SetNight()
    {
        SetBackground(2);
        currentPhase = 2;
        cycleTimer = fullCycleDuration * 0.66f;
    }
    
    [ContextMenu("Test Cycle")]
    public void TestCycle()
    {
        StartCoroutine(TestCycleCoroutine());
    }
    
    IEnumerator TestCycleCoroutine()
    {
        Debug.Log("Testing background cycle...");
        SetDay();
        yield return new WaitForSeconds(2f);
        SetSunset();
        yield return new WaitForSeconds(2f);
        SetNight();
        yield return new WaitForSeconds(2f);
        SetDay();
        Debug.Log("Background cycle test complete!");
    }
}