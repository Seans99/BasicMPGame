using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chat : MonoBehaviour
{
    private TextMeshProUGUI _ChatBox;

    void Start()
    {
        _ChatBox = GameObject.FindGameObjectWithTag("ChatBox").GetComponent<TextMeshProUGUI>();
    }

    void OnMsgOne(InputValue value)
    {
        FixedString128Bytes message = new("Hello");
        SubmitMessageRPC(message);
    }

    void OnMsgTwo(InputValue value)
    {
        FixedString128Bytes message = new("Bye");
        SubmitMessageRPC(message);
    }

    void OnMsgThree(InputValue value)
    {
        FixedString128Bytes message = new("Haha");
        SubmitMessageRPC(message);
    }

    void OnMsgFour(InputValue value)
    {
        FixedString128Bytes message = new("Well done");
        SubmitMessageRPC(message);
    }

    [Rpc(SendTo.Server)]
    public void SubmitMessageRPC(FixedString128Bytes message)
    {
        UpdateMessageRPC(message);
    }

    [Rpc(SendTo.Server)]
    public void UpdateMessageRPC(FixedString128Bytes message)
    {
        _ChatBox.text = message.ToString();
    }
}
