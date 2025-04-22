using extOSC;
using TMPro;
using UnityEngine;

public class Send : MonoBehaviour
{
	[SerializeField] private OSCTransmitter transmitter;
    [SerializeField] private TMP_InputField input;


    public void SentMessage()
    {
        var message = new OSCMessage("/SentMessage");
        message.AddValue(OSCValue.String(input.text));
        transmitter.Send(message);    
        Debug.Log($"Message Sent : {input.text}");
    }
}
