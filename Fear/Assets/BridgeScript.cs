using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    [SerializeField] float bridgeSwayMultiplier;
    Vector3 startingPosition;
    void Start()
    {
        startingPosition = transform.position;
    }


    void Update()
    {
        float swayAmount = Mathf.Sin(Time.time) * Receive.gammaValue * bridgeSwayMultiplier;
        transform.position = startingPosition + new Vector3(swayAmount, 0f, 0f);
    }
}
