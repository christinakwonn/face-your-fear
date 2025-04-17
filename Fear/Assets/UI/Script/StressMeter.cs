using UnityEngine;

public class StressMeter : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0, -0.3f, 1.5f); // Adjust this to position UI nicely in view
    public float followSpeed = 5f;

    void LateUpdate()
    {
        Vector3 targetPosition = cameraTransform.position + cameraTransform.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);

        Quaternion targetRotation = Quaternion.LookRotation(transform.position - cameraTransform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * followSpeed);
    }
}

public class eegData : MonoBehaviour
{
    public UnityEngine.UI.Slider stressSlider;
    public float fearLevel; // from EEG script

    void Update()
    {
        // Update with real EEG fear value (e.g., 0 to 1)
        stressSlider.value = Mathf.Lerp(stressSlider.value, fearLevel, Time.deltaTime * 5f);
    }

    // Call this from your EEG integration
    public void UpdateFearLevel(float newFearValue)
    {
        fearLevel = Mathf.Clamp01(newFearValue);
    }
}