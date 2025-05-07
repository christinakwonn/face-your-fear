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

    private bool calmTriggered = false;
    private bool stressTriggered = false;
    private bool dangerTriggered = false;

    void Start()
    {
        textPanel.SetActive(false);
        textGroup.alpha = 0f;
    }

    void Update()
    {
        if (stressMeter == null) return;

        // Convert stressLevel (0–1) to 0–100 scale
        float currentStress = stressMeter.stressLevel * 100f;

        if (!calmTriggered && currentStress <= calmThreshold)
        {
            calmTriggered = true;
            StartCoroutine(TriggerText(calmMessage));
        }
        else if (!stressTriggered && currentStress > calmThreshold && currentStress <= stressThreshold)
        {
            stressTriggered = true;
            StartCoroutine(TriggerText(stressMessage));
        }
        else if (!dangerTriggered && currentStress > dangerThreshold)
        {
            dangerTriggered = true;
            StartCoroutine(TriggerText(dangerMessage));
        }
    }

    IEnumerator TriggerText(string message)
    {
        tutorialText.text = message;
        textPanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(textGroup, true));

        PlayAudioForMessage(message);

        yield return new WaitForSeconds(displayDuration);

        yield return StartCoroutine(FadeCanvasGroup(textGroup, false));
        textPanel.SetActive(false);
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
