using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialTextManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI tutorialText;
    public CanvasGroup textGroup;

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

        // You can trigger gameplay here, like enabling controls or switching scenes
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

        textGroup.alpha = 1;

        // Play audio + optional haptics depending on content
        TriggerAudioAndFeedback(line);

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        for (float t = fadeDuration; t >= 0; t -= Time.deltaTime)
        {
            textGroup.alpha = t / fadeDuration;
            yield return null;
        }

        textGroup.alpha = 0;
    }

    void TriggerAudioAndFeedback(string line)
    {
        if (line.ToLower().Contains("breathing"))
        {
            audioSource.PlayOneShot(calmSound);
        }
        else if (line.ToLower().Contains("fear"))
        {
            audioSource.PlayOneShot(stressSound);
            SendHaptic();
        }
        else if (line.ToLower().Contains("danger"))
        {
            audioSource.PlayOneShot(dangerSound);
            SendStrongHaptic();
        }
    }

    // Placeholder — replace with XR haptic calls if using XR Toolkit
    void SendHaptic()
    {
        Debug.Log("Light Haptic Triggered");
    }

    void SendStrongHaptic()
    {
        Debug.Log("Strong Haptic Triggered");
    }
}
