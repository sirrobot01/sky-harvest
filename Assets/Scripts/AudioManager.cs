using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    [Header("Audio Sources")]
    public AudioSource wingFlapSource;
    public AudioSource fruitCollectSource;
    public AudioSource collisionSource;
    public AudioSource uiSoundSource;
    
    [Header("Audio Clips")]
    public AudioClip wingFlapClip;
    public AudioClip collisionClip;
    public AudioClip selectClickClip;
    public AudioClip powerUpClip;

    [Header("Fruit Sound Clips")]
    public AudioClip appleSound;
    public AudioClip grapeSound;
    public AudioClip watermelonSound;
    public AudioClip dragonfruitSound;
    
    // Singleton pattern for easy access
    public static AudioManager instance;
    
    void Awake() {
        instance = this;
    }
    
    public void PlayWingFlap() {
        wingFlapSource.PlayOneShot(wingFlapClip);
    }
    
    public void PlayFruitSound(Fruit.FruitType fruitType) 
    {
        AudioClip clipToPlay = null;
        
        switch(fruitType) 
        {
            case Fruit.FruitType.Apple: clipToPlay = appleSound; break;
            case Fruit.FruitType.Grape: clipToPlay = grapeSound; break;
            case Fruit.FruitType.Watermelon: clipToPlay = watermelonSound; break;
            case Fruit.FruitType.Dragonfruit: clipToPlay = dragonfruitSound; break;
        }
        
        if (clipToPlay != null) 
        {
            fruitCollectSource.PlayOneShot(clipToPlay);
        }
    }
    
    public void PlayCollision() {
        collisionSource.PlayOneShot(collisionClip);
    }
    
    public void PlaySelectClick() {
        if (uiSoundSource != null && selectClickClip != null) {
            uiSoundSource.PlayOneShot(selectClickClip);
        }
    }
    
    public void PlayPowerUp() {
        if (uiSoundSource != null && powerUpClip != null) {
            uiSoundSource.PlayOneShot(powerUpClip);
        }
    }

}