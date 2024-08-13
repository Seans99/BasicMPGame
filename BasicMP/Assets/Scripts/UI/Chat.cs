using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Chat : NetworkBehaviour
{
    private TextMeshProUGUI _ChatBox;

    void Start()
    {
        _ChatBox = GameObject.FindGameObjectWithTag("ChatBox").GetComponent<TextMeshProUGUI>();
    }

    void OnMsgOne(InputValue value)
    {
        SendChatMessage("Hello");
    }

    void OnMsgTwo(InputValue value)
    {
        SendChatMessage("Bye");
    }

    void OnMsgThree(InputValue value)
    {
        SendChatMessage("Haha");
    }

    void OnMsgFour(InputValue value)
    {
        SendChatMessage("Well done");
    }

    private void SendChatMessage(string message)
    {
        if (IsOwner)
        {
            FixedString128Bytes fixedMessage = new(message);
            SubmitMessageRPC(fixedMessage, OwnerClientId);
        }
    }

    [Rpc(SendTo.Server)]
    void SubmitMessageRPC(FixedString128Bytes message, ulong senderClientId)
    {
        UpdateMessageClientRPC(message, senderClientId);
    }

    [ClientRpc]
    void UpdateMessageClientRPC(FixedString128Bytes message, ulong senderClientId)
    {
        if (_ChatBox != null)
        {
            string prefix = senderClientId == 0 ? "P1: " : $"P2: ";
            _ChatBox.text = prefix + message.ToString();
        }
    }
}
