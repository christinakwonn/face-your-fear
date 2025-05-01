using extOSC;
using TMPro;
using UnityEngine;

public class SendReceive : MonoBehaviour
{

	[SerializeField] private OSCTransmitter transmitter;
    [SerializeField] private OSCReceiver receiver;
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TMP_Text display;


    void Start()
    {
        receiver.Bind("/ReceivedMessage", MessageReceived);
    }
    public void SentMessage()
    {
        if(float.TryParse(input.text, out float result))
        {
            var message = new OSCMessage("/SentMessage");
            message.AddValue(OSCValue.Float(result));
            transmitter.Send(message);   
        }
        else
        {
            display.text = "Error not numerical";
        }
 
    }

    protected void MessageReceived(OSCMessage message)
    {
        display.text = message.Values[0].FloatValue.ToString();
        print($"Message Received : {message.Values[0].FloatValue}" );
    }
}
