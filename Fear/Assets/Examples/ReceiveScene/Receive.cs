using extOSC;
using TMPro;
using UnityEngine;

public class Receive : MonoBehaviour
{
    public static Receive instance { get; private set; }

    [SerializeField] private OSCReceiver receiver;
    [SerializeField] private TMP_Text display;

    public static float gammaValue;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

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
