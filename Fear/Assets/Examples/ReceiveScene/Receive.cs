using extOSC;
using TMPro;
using UnityEngine;

public class Receive : MonoBehaviour
{
    [SerializeField] private OSCReceiver receiver;
    //[SerializeField] private TMP_Text display;

    public static float gammaValue = 0;

    void Start()
    {
        receiver.Bind("/gamma", MessageReceived);
    }

    protected void MessageReceived(OSCMessage message)
    {
        gammaValue = message.Values[0].FloatValue / 1000000000;
        //display.text = message.Values[0].FloatValue.ToString();
        // print($"Message Received : {message.Values[0].FloatValue}" );
        //print($"Message Received : {message}" );
    }
}
