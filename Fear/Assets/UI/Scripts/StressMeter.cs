using UnityEngine;
using UnityEngine.UI; 

public class StressMeter : MonoBehaviour
{
    [Range(0, 1)] public float stressLevel = 0f;
    public Image stressFill;
    public Image stressOverlay;

    void Update()
    {
        if (stressFill == null)
        {
            Debug.LogWarning("Stress fill Image is not assigned.");
            return;
        }

        stressLevel = Mathf.Clamp01(Receive.gammaValue); // normalized to 0–1

        // Update UI
        stressFill.fillAmount = stressLevel;
        stressFill.color = GetStressColor(stressLevel);

        float normalizedGamma = Mathf.Clamp01(Receive.gammaValue * 1000000f);
        stressLevel = normalizedGamma;

        UpdateStressOverlay(stressLevel);

    }

    void UpdateStressOverlay(float value)
    {
   
        Color color = Color.Lerp(Color.green, Color.red, value);

        float alpha = Mathf.Lerp(0f, 0.6f, value); // 0–60% visible
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
