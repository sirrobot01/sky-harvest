using UnityEngine;
using System.Collections;

public class CameraShakeManager : MonoBehaviour
{
    [Header("Shake Settings")]
    [Range(0f, 2f)]
    public float damageShakeIntensity = 0.3f;
    [Range(0f, 1f)]
    public float damageShakeDuration = 0.2f;
    
    [Range(0f, 2f)]
    public float explosionShakeIntensity = 0.5f;
    [Range(0f, 1f)]
    public float explosionShakeDuration = 0.4f;
    
    [Range(0f, 2f)]
    public float deathShakeIntensity = 0.8f;
    [Range(0f, 2f)]
    public float deathShakeDuration = 1f;
    
    public static CameraShakeManager instance;
    
    private Camera mainCamera;
    private Vector3 originalPosition;
    private bool isShaking = false;
    
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
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
        }
        
        if (mainCamera != null)
        {
            originalPosition = mainCamera.transform.localPosition;
        }
    }
    
    public void ShakeOnDamage()
    {
        if (mainCamera != null)
        {
            StartCoroutine(ShakeCamera(damageShakeIntensity, damageShakeDuration));
        }
    }
    
    public void ShakeOnExplosion()
    {
        if (mainCamera != null)
        {
            StartCoroutine(ShakeCamera(explosionShakeIntensity, explosionShakeDuration));
        }
    }
    
    public void ShakeOnDeath()
    {
        if (mainCamera != null)
        {
            StartCoroutine(ShakeCamera(deathShakeIntensity, deathShakeDuration));
        }
    }
    
    public void ShakeCustom(float intensity, float duration)
    {
        if (mainCamera != null)
        {
            StartCoroutine(ShakeCamera(intensity, duration));
        }
    }
    
    IEnumerator ShakeCamera(float intensity, float duration)
    {
        if (isShaking)
        {
            // If already shaking, just extend the current shake if new one is stronger
            yield break;
        }
        
        isShaking = true;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            // Generate random shake offset
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;
            
            // Apply shake with smooth falloff over time
            float falloff = 1f - (elapsed / duration);
            falloff = falloff * falloff; // Smooth curve
            
            Vector3 shakeOffset = new Vector3(x, y, 0) * falloff;
            mainCamera.transform.localPosition = originalPosition + shakeOffset;
            
            elapsed += Time.unscaledDeltaTime; // Use unscaled time so shake works even if game is paused
            yield return null;
        }
        
        // Return to original position
        mainCamera.transform.localPosition = originalPosition;
        isShaking = false;
    }
    
    // Call this if camera position changes (e.g., camera follows player)
    public void UpdateOriginalPosition()
    {
        if (mainCamera != null && !isShaking)
        {
            originalPosition = mainCamera.transform.localPosition;
        }
    }
    
    // For debugging - test shake in editor
    [ContextMenu("Test Damage Shake")]
    void TestDamageShake()
    {
        ShakeOnDamage();
    }
    
    [ContextMenu("Test Explosion Shake")]
    void TestExplosionShake()
    {
        ShakeOnExplosion();
    }
    
    [ContextMenu("Test Death Shake")]
    void TestDeathShake()
    {
        ShakeOnDeath();
    }
}