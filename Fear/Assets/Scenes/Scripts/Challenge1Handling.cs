using UnityEngine;
using UnityEngine.UI;

public class Challenge1Handling : MonoBehaviour
{
    [SerializeField] Toggle challengeToggle;
    [SerializeField] GameObject bananaObj;
    [SerializeField] bool challengeOneComplete = false;

    private void Update() {
        if (bananaObj == null) challengeOneComplete = true; // bananaObj is destroyed, complete challenge

        if (challengeOneComplete) challengeToggle.isOn = true;
    }
}
