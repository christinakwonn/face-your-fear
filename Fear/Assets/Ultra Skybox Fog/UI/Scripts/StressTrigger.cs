using System.Collections;
using UnityEngine;
using TMPro;

public class StressTrigger : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject textPanel;
    public CanvasGroup textGroup;
    public TextMeshProUGUI tutorialText;

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float displayDuration = 4f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip calmSound;
    public AudioClip stressSound;
    public AudioClip dangerSound;


    [Header("Stress Thresholds")]
    public float calmThreshold = 20f;
    public float stressThreshold = 50f;
    public float dangerThreshold = 80f;

    [Header("Stress Meter Reference")]
    public StressMeter stressMeter; // Correct class name

    [Header("Messages")]
    [TextArea] public string calmMessage = "Take a deep breath. You are calm.";
    [TextArea] public string stressMessage = "Your stress level is rising. Focus.";
    [TextArea] public string dangerMessage = "Danger! You're losing control!";

    private bool isShowing = false;
    private float lastStress = -1f; 

    void Start()
    {
        textPanel.SetActive(false);
        textGroup.alpha = 0f;
    }

    void Update()
    {
        if (stressMeter == null) return;

        float currentStress = stressMeter.stressLevel * 100f;

        // Only proceed if stress actually changed
        if (Mathf.Approximately(currentStress, lastStress)) return;

        // From above to calm
        if (lastStress > calmThreshold && currentStress <= calmThreshold)
            StartCoroutine(TriggerText(calmMessage));

        // From outside to stress range
        else if ((lastStress <= calmThreshold || lastStress > stressThreshold) &&
                 (currentStress > calmThreshold && currentStress <= stressThreshold))
            StartCoroutine(TriggerText(stressMessage));

        // From below to danger
        else if (lastStress <= dangerThreshold && currentStress > dangerThreshold)
            StartCoroutine(TriggerText(dangerMessage));

        lastStress = currentStress;
    }

    IEnumerator TriggerText(string message)
    {
        if (isShowing) yield break;
        isShowing = true;

        tutorialText.text = message;
        textPanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(textGroup, true));

        PlayAudioForMessage(message);

        yield return new WaitForSeconds(displayDuration);

        yield return StartCoroutine(FadeCanvasGroup(textGroup, false));
        textPanel.SetActive(false);
        isShowing = false;

    }


    void PlayAudioForMessage(string message)
    {
        string lower = message.ToLower();

        if (lower.Contains("calm") && calmSound != null)
        {
            audioSource?.PlayOneShot(calmSound);
        }
        else if (lower.Contains("stress") && stressSound != null)
        {
            audioSource?.PlayOneShot(stressSound);
        }
        else if (lower.Contains("danger") && dangerSound != null)
        {
            audioSource?.PlayOneShot(dangerSound);
        }
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, bool fadeIn)
    {
        float targetAlpha = fadeIn ? 1f : 0f;
        float startAlpha = group.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        group.alpha = targetAlpha;
    }
}
