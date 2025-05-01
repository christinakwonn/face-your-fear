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
            Debug.LogWarning("Stress fill Image is not assigned.");
            return;
        }

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
