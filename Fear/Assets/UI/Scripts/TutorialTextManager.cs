using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialTextManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject textPanel;
    public CanvasGroup textGroup;
    public TextMeshProUGUI tutorialText;
    public GameObject stressMeterUI;
    public GameObject continueButton;

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float displayDuration = 4f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip calmSound;
    public AudioClip stressSound;
    public AudioClip dangerSound;

    [TextArea]
    public string[] tutorialLines;

    private void Start()
    {
        StartCoroutine(PlayTutorial());
    }

    IEnumerator PlayTutorial()
    {
        foreach (string line in tutorialLines)
        {
            yield return StartCoroutine(ShowText(line));
        }

        // Fade out text panel
        yield return StartCoroutine(FadeCanvasGroup(textGroup, false));
        textPanel.SetActive(false);

        // Show stress meter
        stressMeterUI.SetActive(true);

        // Show stress explanation message
        tutorialText.text = "This is your stress meter. Keep it low to stay in control.";
        textPanel.SetActive(true);
        yield return StartCoroutine(FadeCanvasGroup(textGroup, true));
        yield return new WaitForSeconds(displayDuration);
        yield return StartCoroutine(FadeCanvasGroup(textGroup, false));
        textPanel.SetActive(false);

        // Show continue button
        continueButton.SetActive(true);
    }

    IEnumerator ShowText(string line)
    {
        tutorialText.text = line;

        // Fade in
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            textGroup.alpha = t / fadeDuration;
            yield return null;
        }

        textGroup.alpha = 1f;

        // Play sound & feedback
        TriggerAudioAndFeedback(line);

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        for (float t = fadeDuration; t >= 0; t -= Time.deltaTime)
        {
            textGroup.alpha = t / fadeDuration;
            yield return null;
        }

        textGroup.alpha = 0f;
    }

    IEnumerator FadeCanvasGroup(CanvasGroup group, bool fadeIn)
    {
        float target = fadeIn ? 1f : 0f;
        float start = group.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            group.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        group.alpha = target;
    }

    void TriggerAudioAndFeedback(string line)
    {
        string lowerLine = line.ToLower();

        if (lowerLine.Contains("breathing"))
        {
            audioSource?.PlayOneShot(calmSound);
        }
        else if (lowerLine.Contains("fear") || lowerLine.Contains("stress"))
        {
            audioSource?.PlayOneShot(stressSound);
            SendHaptic();
        }
        else if (lowerLine.Contains("danger"))
        {
            audioSource?.PlayOneShot(dangerSound);
            SendStrongHaptic();
        }
    }

    void SendHaptic()
    {
        Debug.Log("Light Haptic Triggered");
    }

    void SendStrongHaptic()
    {
        Debug.Log("Strong Haptic Triggered");
    }
}
