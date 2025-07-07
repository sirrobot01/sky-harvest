using System.Collections;
using UnityEngine;

public class PlayerDragonController : MonoBehaviour
{
    public Sprite normalSprite;
    public Sprite fastSprite;
    public Sprite slowSprite;
    public Sprite hurtSprite;
    public Sprite deathSprite;

    public int health = 3;
    public int maxHealth = 5; // Maximum health the dragon can have
    public float speedMultiplier = 1f;
    public bool isInvincible = false;
    public bool isStunned = false;
    public bool isDead = false;

    private SpriteRenderer spriteRenderer;

    // Lane system
    [HideInInspector] public float[] laneYPositions = { -4f, -2f, 0f, 2f, 4f };
    [HideInInspector] public int currentLaneIndex = 2;

    // Speed
    [HideInInspector] public static float scrollSpeedMultiplier = 1f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetToNormal();
        transform.position = new Vector3(transform.position.x, laneYPositions[currentLaneIndex], transform.position.z);
        
        // Dragon trail effect removed
        // if (ParticleEffectManager.instance != null)
        // {
        //     ParticleEffectManager.instance.StartDragonTrail(transform);
        // }
        
        // Add dragon movement animator
        DragonMovementAnimator animator = GetComponent<DragonMovementAnimator>();
        if (animator == null)
        {
            gameObject.AddComponent<DragonMovementAnimator>();
        }
    }

    void Update()
    {
        if (GameManager.isLevelOver) return;
        
        if (isDead) return;

        if (!isStunned)
        {
            HandleLaneInput();
            HandleSpeedInput();
        }
    }

    private void HandleLaneInput()
{
    if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && currentLaneIndex < laneYPositions.Length - 1)
    {
        currentLaneIndex++;
        MoveToCurrentLane();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWingFlap();
        }
    }
    else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currentLaneIndex > 0)
    {
        currentLaneIndex--;
        MoveToCurrentLane();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayWingFlap();
        }
    }
}

    private void HandleSpeedInput()
    {
    // Use explicit key detection instead of axis for more reliable speed control
    if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
    {
        scrollSpeedMultiplier = 2f;
        SetToFast();
    }
    else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
    {
        scrollSpeedMultiplier = 0.5f;
        SetToSlow();
    }
    else
    {
        scrollSpeedMultiplier = 1f;
        SetToNormal();
    }
}

    private void MoveToCurrentLane()
    {
        // Movement animation is now handled by DragonMovementAnimator
        // Just updating the currentLaneIndex is enough
    }

    public void SetToNormal() => spriteRenderer.sprite = normalSprite;
    public void SetToFast() => spriteRenderer.sprite = fastSprite;
    public void SetToSlow() => spriteRenderer.sprite = slowSprite;
    public void SetToHurt() => spriteRenderer.sprite = hurtSprite;
    public void SetToDead() => spriteRenderer.sprite = deathSprite;

    private float invincibleTime = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") && !isStunned && !isInvincible)
        {
            // Debug collision detection
            float distance = Vector2.Distance(transform.position, collision.transform.position);
            Debug.Log($"Dragon hit {collision.name} at distance: {distance:F2}");
            
            TakeDamage();
        }
    }

    private IEnumerator HandleHitStun()
    {
        isStunned = true; // 🚫 freeze movement

        scrollSpeedMultiplier = 0f;
        PlayerDragonController.scrollSpeedMultiplier = 0f;
        SetToHurt(); // Hurt sprite

        yield return new WaitForSeconds(0.5f); // Freeze duration

        // Begin invincibility
        isInvincible = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.5f); // Fade sprite

        SetToNormal(); // Resume normal sprite (but keep faded)
        isStunned = false; // ✅ allow movement again

        yield return new WaitForSeconds(invincibleTime);

        // Restore visuals
        isInvincible = false;
        PlayerDragonController.scrollSpeedMultiplier = 1f;
        scrollSpeedMultiplier = 1f;
        spriteRenderer.color = Color.white;
    }

    public void TakeDamage()
    {
        if (isDead) return;

        Debug.Log("Player took damage!");

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayCollision();
        }
        
        // Camera shake on damage
        if (CameraShakeManager.instance != null)
        {
            CameraShakeManager.instance.ShakeOnDamage();
        }
        
        // Screen flash on damage
        if (ScreenFlashManager.instance != null)
        {
            ScreenFlashManager.instance.FlashDamage();
        }
        
        // Trigger hit animation
        DragonMovementAnimator animator = GetComponent<DragonMovementAnimator>();
        if (animator != null)
        {
            animator.PlayHitAnimation();
        }

        health--;
        Debug.Log("Dragon took damage! Health is now: " + health);

        if (health <= 0)
        {
            Debug.Log("Dragon has died.");
            isDead = true;
            SetToDead();
            scrollSpeedMultiplier = 0f;
            PlayerDragonController.scrollSpeedMultiplier = 0f;
            spriteRenderer.color = Color.white; // Reset just in case
            
            // Camera shake on death (stronger than damage)
            if (CameraShakeManager.instance != null)
            {
                CameraShakeManager.instance.ShakeOnDeath();
            }
            
            // Screen flash on death
            if (ScreenFlashManager.instance != null)
            {
                ScreenFlashManager.instance.FlashDeath();
            }
            
            // Trigger game over
            GameManager.isLevelOver = true;
            GameEndManager.TriggerGameOver();
            
            return;
        }

        StartCoroutine(HandleHitStun());
    }


}


