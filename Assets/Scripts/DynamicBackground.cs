using UnityEngine;

public class DynamicBackground : MonoBehaviour
{
    [Header("Background Sprites")]
    public Sprite daySprite;
    public Sprite sunsetSprite;
    public Sprite nightSprite;
    
    [Header("Timing Settings")]
    public float totalCycleTime = 120f; // Total time for full day/night cycle (2 minutes)
    public float dayDuration = 40f; // How long day lasts
    public float sunsetDuration = 20f; // How long sunset lasts  
    public float nightDuration = 60f; // How long night lasts
    
    [Header("Transition Settings")]
    public float transitionTime = 3f; // How long transitions take
    public bool smoothTransitions = true; // Fade between backgrounds
    
    private SpriteRenderer backgroundRenderer;
    private SpriteRenderer transitionRenderer; // For fade transitions
    private float currentTime = 0f;
    private Sprite currentSprite;
    private bool isTransitioning = false;
    
    public enum TimeOfDay { Day, Sunset, Night }
    private TimeOfDay currentTimeOfDay = TimeOfDay.Day;

    void Start()
    {
        backgroundRenderer = GetComponent<SpriteRenderer>();
        
        if (backgroundRenderer == null)
        {
            Debug.LogError("DynamicBackground needs a SpriteRenderer component!");
            return;
        }
        
        // Create transition renderer for smooth fades
        if (smoothTransitions)
        {
            CreateTransitionRenderer();
        }
        
        // Start with day
        SetBackgroundSprite(daySprite);
        currentSprite = daySprite;
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        UpdateTimeOfDay();
    }
    
    void UpdateTimeOfDay()
    {
        currentTime += Time.deltaTime;
        
        // Reset cycle if we've completed full cycle
        if (currentTime >= totalCycleTime)
        {
            currentTime = 0f;
        }
        
        // Determine current time of day and transition if needed
        TimeOfDay newTimeOfDay = GetTimeOfDayFromTime(currentTime);
        
        if (newTimeOfDay != currentTimeOfDay)
        {
            TransitionToTimeOfDay(newTimeOfDay);
            currentTimeOfDay = newTimeOfDay;
        }
    }
    
    TimeOfDay GetTimeOfDayFromTime(float time)
    {
        if (time <= dayDuration)
        {
            return TimeOfDay.Day;
        }
        else if (time <= dayDuration + sunsetDuration)
        {
            return TimeOfDay.Sunset;
        }
        else
        {
            return TimeOfDay.Night;
        }
    }
    
    void TransitionToTimeOfDay(TimeOfDay timeOfDay)
    {
        Sprite targetSprite = GetSpriteForTimeOfDay(timeOfDay);
        
        if (smoothTransitions && !isTransitioning)
        {
            StartCoroutine(SmoothTransition(targetSprite));
        }
        else
        {
            SetBackgroundSprite(targetSprite);
        }
        
        Debug.Log($"Transitioning to {timeOfDay} at time {currentTime:F1}s");
    }
    
    Sprite GetSpriteForTimeOfDay(TimeOfDay timeOfDay)
    {
        switch (timeOfDay)
        {
            case TimeOfDay.Day: return daySprite;
            case TimeOfDay.Sunset: return sunsetSprite;
            case TimeOfDay.Night: return nightSprite;
            default: return daySprite;
        }
    }
    
    void SetBackgroundSprite(Sprite sprite)
    {
        if (backgroundRenderer != null && sprite != null)
        {
            backgroundRenderer.sprite = sprite;
            currentSprite = sprite;
        }
    }
    
    void CreateTransitionRenderer()
    {
        // Create a second renderer for smooth transitions
        GameObject transitionGO = new GameObject("BackgroundTransition");
        transitionGO.transform.SetParent(transform);
        transitionGO.transform.localPosition = Vector3.zero;
        transitionGO.transform.localScale = Vector3.one;
        
        transitionRenderer = transitionGO.AddComponent<SpriteRenderer>();
        transitionRenderer.sortingOrder = backgroundRenderer.sortingOrder + 1;
        transitionRenderer.color = new Color(1, 1, 1, 0); // Start transparent
    }
    
    System.Collections.IEnumerator SmoothTransition(Sprite targetSprite)
    {
        if (transitionRenderer == null) yield break;
        
        isTransitioning = true;
        
        // Set up transition
        transitionRenderer.sprite = targetSprite;
        Color startColor = new Color(1, 1, 1, 0);
        Color endColor = new Color(1, 1, 1, 1);
        
        float elapsed = 0f;
        
        // Fade in new background
        while (elapsed < transitionTime)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / transitionTime;
            
            transitionRenderer.color = Color.Lerp(startColor, endColor, progress);
            yield return null;
        }
        
        // Complete transition
        backgroundRenderer.sprite = targetSprite;
        currentSprite = targetSprite;
        transitionRenderer.color = startColor; // Make transition layer transparent again
        
        isTransitioning = false;
    }
    
    // Public methods for external control
    public void SetTimeOfDay(TimeOfDay timeOfDay)
    {
        currentTimeOfDay = timeOfDay;
        TransitionToTimeOfDay(timeOfDay);
    }
    
    public void ResetCycle()
    {
        currentTime = 0f;
        currentTimeOfDay = TimeOfDay.Day;
        SetBackgroundSprite(daySprite);
    }
    
    public TimeOfDay GetCurrentTimeOfDay()
    {
        return currentTimeOfDay;
    }
    
    public float GetCycleProgress()
    {
        return currentTime / totalCycleTime;
    }
}