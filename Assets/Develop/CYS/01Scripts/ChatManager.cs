using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Chat;
using ExitGames.Client.Photon;
using System;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{

    private ChatClient _chatClient;
    private string _userName;
    private string _currentChannelName;

    public TMP_InputField inputFieldChat;
    public TMP_Text currentChannelText;
    public TMP_Text outputText;

    // Use this for initialization
    private void OnEnable()
    {
        Application.runInBackground = true;

        // �г��Ӽ���
        _userName = PhotonNetwork.LocalPlayer.NickName;
        _currentChannelName = "Channel 001";

        _chatClient = new ChatClient(this);

        // true �� �ƴ� ��� ������ ��׶���� �� �� ���� ����
        _chatClient.UseBackgroundWorkerForSending = true;
        _chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new Photon.Chat.AuthenticationValues(_userName));
        AddLine(string.Format("Try CONNECTION", _userName));
    }
    // ���� ���� Ȩ���������� ����Ǿ� �ִ� ����
    // chatClient.Service() �� Update ���� ȣ���ϴ���
    // �ʿ信 ���� chatClient.Service() �� �ݵ�� ȣ�� �ؾ��Ѵ�
    private void Update()
    {
        OnEnterSend();
        _chatClient.Service();
    }

    #region NoNeedToShowAlltheTime
    // ���� ä�� ���¸� ������� UI.Text
    public void AddLine(string lineString)
    {
        outputText.text += lineString + "\r\n";
    }

    // ���ø����̼��� ����Ǿ��� �� ȣ��
    public void OnApplicationQuit()
    {
        if (_chatClient != null)
        {
            _chatClient.Disconnect();
        }
    }

    // DebugLevel �� ���� �� enum Ÿ�Կ� ���� �޼����� ����Ѵ�
    public void DebugReturn(ExitGames.Client.Photon.DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    // ������ ������ ������
    public void OnConnected()
    {
        AddLine("Connected to Server (ChatManager).");

        // ������ ä�θ����� ����
        _chatClient.Subscribe(new string[] { _currentChannelName }, 10);
    }

    // �������� ������ ������
    public void OnDisconnected()
    {
        AddLine("Disconnected from server (ChatManager).");
    }

    // ���� Ŭ���̾�Ʈ�� ���¸� ���
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("Enter Channel ({0})", string.Join(",", channels)));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("Exit Channel ({0})", string.Join(",", channels)));
    }

    // Update() �� chatClient.Service() ��
    // �� ȣ�� �� OnGetMessages �� ȣ���Ѵ�.
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(_currentChannelName))
        {
            // update text
            this.ShowChannel(_currentChannelName);
        }
    }
    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this._chatClient.TryGetChannel(channelName, out channel);
        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }

        this._currentChannelName = channelName;
        // ������ TMP_TEXT (display? panel? ������ UI)�� ����
        // ä�ο� ���� �� ��� ä�� �޼����� �ҷ��´�.
        // ���� �̸��� ä�� ������ �Ѳ����� �ҷ�������.
        this.currentChannelText.text = channel.ToStringMessages();
        Debug.Log("ShowChannel: " + _currentChannelName);
    }




    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("status : " + string.Format("{0} is {1}, Msg : {2} ", user, status, message));
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    // �ӼӸ� �޼ҵ�
    // TODO ���߿� �ϰԵȴٸ� �غ���
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        Debug.Log("OnPrivateMessage : " + message);
    }

    /// <summary>
    /// �ν������� InputField ���� �Է¹��� �޼����� ���� �� ���
    /// </summary>
    public void OnEnterSend()
    {
    //    Debug.Log("����ġ��ä�ó�����");
    //    this.SendChatMessage(this.inputFieldChat.text);
    //    this.inputFieldChat.text = "";
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            Debug.Log("����ġ��ä�ó�����");
            this.SendChatMessage(this.inputFieldChat.text);
            this.inputFieldChat.text = "";
        }
    }
    /// <summary>
    /// ���� �Լ� ��ư��
    /// </summary>
    public void OnEnterSendButton()
    {
        Debug.Log("����ġ��ä�ó�����");
        this.SendChatMessage(this.inputFieldChat.text);
        this.inputFieldChat.text = "";
    }


    // �Է��� ä���� ������ �����Ѵ�.
    private void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }
        this._chatClient.PublishMessage(_currentChannelName, inputLine);
    }
}
