using UnityEngine;
using UnityEngine.UI;

public class ChallengeManager : MonoBehaviour
{
    public Toggle challenge1Toggle;
    public AudioSource checkSound;
    public GameObject nextSceneButton;


    void Start()
    {
        challenge1Toggle.isOn = false;
        nextSceneButton.SetActive(false);

    }

    public void CompleteChallenge1()
    {
        if (!challenge1Toggle.isOn)
        {
            challenge1Toggle.isOn = true;
            checkSound.Play();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CompleteChallenge1();
        }

        if (challenge1Toggle.isOn && !nextSceneButton.activeSelf)
        {
            nextSceneButton.SetActive(true);
        }
    }


}
