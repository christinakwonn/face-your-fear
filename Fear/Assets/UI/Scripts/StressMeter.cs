using UnityEngine;
using UnityEngine.UI; 

public class StressMeter : MonoBehaviour
{
    [Range(0, 1)] public float stressLevel = 0f;
    public Image stressFill;
    public Image stressOverlay;

    void Update()
    {
        if (stressFill == null || stressOverlay == null)
        {
            Debug.LogWarning("Stress UI Images not assigned.");
            return;
        }

        // Normalize gamma input once
        float normalizedGamma = Mathf.Clamp01(Receive.gammaValue * 1000000f);
        stressLevel = normalizedGamma;

        // Update stress meter fill
        stressFill.fillAmount = stressLevel;
        stressFill.color = GetStressColor(stressLevel);

        // Update overlay
        UpdateStressOverlay(stressLevel);
    }


    void UpdateStressOverlay(float value)
    {
        // Green to Red
        Color color = Color.Lerp(Color.green, Color.red, value);

        // Transparent to 60% opacity
        float alpha = Mathf.Lerp(0f, 0.6f, value);
        color.a = alpha;

        stressOverlay.color = color;
    }


    Color GetStressColor(float stress)
    {
        if (stress < 0.5f)
            return Color.Lerp(Color.green, Color.yellow, stress * 2);
        else
            return Color.Lerp(Color.yellow, Color.red, (stress - 0.5f) * 2);
    }
}
