using UnityEngine;

public class AnimatedHighlight : MonoBehaviour
{
    public float animationDuration = 1.0f;
    public float minEmission = 0.0f;
    public float maxEmission = 1.0f;
    public Color highlightColor = Color.white;

    private Material material;
    private float animationTime = 0.0f;
    public bool isHighlighting = false;
    private Color originalColor;

    private bool H;

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.GetColor("_EmissionColor");
    }

    private void Update()
    {   
        if (isHighlighting)
        {
            animationTime += Time.deltaTime;

            if (H)
            {
                float emission = Mathf.Lerp(minEmission, maxEmission, animationTime / animationDuration);
                material.SetColor("_EmissionColor", highlightColor * emission);
            }
            else
            {
                float emission = Mathf.Lerp(maxEmission, minEmission, animationTime / animationDuration);
                material.SetColor("_EmissionColor", highlightColor * emission);
            }

            if (animationTime >= animationDuration)
            {
                H = !H;
                animationTime = 0.0f;
            }
        }
        else
        {
            animationTime = 0.0f;
            material.SetColor("_EmissionColor", originalColor);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }
}
