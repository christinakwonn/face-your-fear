using UnityEngine;
using UnityEngine.UI; 

public class StressMeter : MonoBehaviour
{
    [Range(0, 1)] public float stressLevel = 0f;
    public Image stressFill;

    void Update()
    {
        if (stressFill == null)
        {
            Debug.LogWarning("Stress UI Images not assigned.");
            return;
        }

        // Normalize gamma input once
        //float normalizedGamma = Mathf.Clamp01(Receive.gammaValue * 1000000f);
        //stressLevel = normalizedGamma;

        // Update stress meter fill
        stressFill.fillAmount = stressLevel;
        stressFill.color = GetStressColor(stressLevel);

    }



    Color GetStressColor(float stress)
    {
        if (stress < 0.5f)
            return Color.Lerp(Color.green, Color.yellow, stress * 2);
        else
            return Color.Lerp(Color.yellow, Color.red, (stress - 0.5f) * 2);
    }
}
