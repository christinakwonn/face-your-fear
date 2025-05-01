using UnityEngine;
using UnityEngine.UI;

public class StressMeter : MonoBehaviour
{
    [Range(0, 1)] public float stressLevel; // Replace this with EEG input
    public Image stressFill;

    void Update()
    {
        // Set fill amount based on stress level
        stressFill.fillAmount = stressLevel;

        // Update color
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