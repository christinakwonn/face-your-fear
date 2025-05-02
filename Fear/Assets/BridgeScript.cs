using UnityEngine;

public class BridgeScript : MonoBehaviour
{
    [SerializeField] float bridgeSwayMultiplier;
    Vector3 startingPosition;
    public float gammaValue;
    void Start()
    {
        startingPosition = transform.position;
    }


    void Update()
    {
        gammaValue = Receive.gammaValue;
        float swayAmount = Mathf.Sin(Time.time) * gammaValue * bridgeSwayMultiplier;
        transform.position = startingPosition + new Vector3(swayAmount, 0f, 0f);
    }
}
