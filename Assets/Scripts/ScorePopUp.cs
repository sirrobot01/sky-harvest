using TMPro;
using UnityEngine;

public class ScorePopUp : MonoBehaviour
{
    public float moveSpeed = 0.5f; // Speed in world units.
    public float fadeDuration = 1f;

    private TextMeshProUGUI textMesh;
    private float timer;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null)
        {
            Debug.LogError("ScorePopUp Prefab Error: Could not find a TextMeshProUGUI component!", this.gameObject);
            Destroy(gameObject); 
            return;
        }
        
        timer = fadeDuration;
    }

    void Start()
    {
        // Make the pop-up face the camera so the text is always readable.
        if (Camera.main != null)
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    void Update()
    { 
        // Move the object upwards in world space.
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        // Fade out the pop-up over its duration.
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Color color = textMesh.color;
            color.a = timer / fadeDuration; 
            textMesh.color = color;
        }
    }

    public void SetText(string text)
    {
        if (textMesh == null)
        {
           textMesh = GetComponentInChildren<TextMeshProUGUI>();
        }
        textMesh.text = text;
        textMesh.ForceMeshUpdate(true);
    }
}

