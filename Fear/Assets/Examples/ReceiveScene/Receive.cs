using extOSC;
using TMPro;
using UnityEngine;

public class Receive : MonoBehaviour
{
    [SerializeField] private OSCReceiver receiver;

    public static float gammaValue;

    void Start()
    {
        receiver.Bind("/gamma", MessageReceived);
    }

    protected void MessageReceived(OSCMessage message)
    {
        gammaValue = message.Values[0].FloatValue / 1000000000; // sets to a normal number range
        print($"Gamma Value : {gammaValue}" );
        //print($"Message Received : {message.Values[0].FloatValue}" );
    }
}
