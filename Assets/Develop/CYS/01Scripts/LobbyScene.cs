using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Lobby, Room } // ���θ޴��� �κ� �����־ ��..???

    [SerializeField] LoginPanel _loginPanel;
    // [SerializeField] MainPanel _mainPanel;
    [SerializeField] LobbyPanel _lobbyPanel;
    [SerializeField] RoomPanel _roomPanel;


    // ChatFunction
    public GameObject _chatContent;
    public TMP_InputField _chatInputField;

    PhotonView _photonView;

    GameObject _chatDisplay;

    string _userName;

    TMP_Text _roomChatDisplay;

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (PhotonNetwork.InRoom == true)
        {
            SetActivePanel(Panel.Room);
        }
        else if (PhotonNetwork.IsConnected)
        {
           // PhotonNetwork.InLobby;
            SetActivePanel(Panel.Lobby);
        }
        else if (PhotonNetwork.InLobby)  // �̰� ��Ȱ��ȭ �ص� ������.. ������� �׽�Ʈ�غ�����
        {
            SetActivePanel(Panel.Lobby);
        }
        else
        {
            SetActivePanel(Panel.Login);
        }

        // From ChatManager
        // PhotonNetwork.ConnectUsingSettings(); 
        // �� �Լ��� connects to a dedicated server that provides rooms, matchmaking, and communication
        // ���� ��Ȳ������ �ٷ� ������ ����ǹ����� �� �� ����.
        _chatDisplay = _chatContent.transform.GetChild(0).gameObject;
        _photonView = GetComponent<PhotonView>();
        Debug.Log("ChatManager�׽�Ʈ �����@Start");

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && _chatInputField.isFocused == false)
        {
            _chatInputField.ActivateInputField();
        }
    }

    private void SetActivePanel(Panel panel)
    {
        _loginPanel.gameObject.SetActive(panel == Panel.Login);
        //_mainPanel.gameObject.SetActive(panel == Panel.Menu);
        _lobbyPanel.gameObject.SetActive(panel == Panel.Lobby);
        _roomPanel.gameObject.SetActive(panel == Panel.Room);
        // �����׵��ϴ±����� ���̵��� ���� ���鼭 ���� ����

    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("���ӿ� �����ߴ�! (OnConnectedToMaster)");
       // SetActivePanel(Panel.Menu);
        SetActivePanel(Panel.Lobby);
        PhotonNetwork.JoinLobby();

        _userName = PhotonNetwork.LocalPlayer.NickName;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"������ �����. cause : {cause} \n OnDisconnected");
        SetActivePanel(Panel.Login);
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ���� ����");
        SetActivePanel(Panel.Lobby);

        //Chat ���� FromChatManager
        AddChatMessage($"{PhotonNetwork.LocalPlayer.NickName} has joined");

        // ���� �����ؾߵǼ� �ϴ� �̷������� �Ǹ�ȵ�
    }
   // public override void OnLeftLobby()
   // {
   //     Debug.Log("�κ� ���� ����");
   //     _lobbyPanel.ClearRoomEntries();
   //     SetActivePanel(Panel.Menu);
   // }
   // �κ� ���ξ��̶� ���� �ʿ䰡 ����.


    public override void OnJoinedRoom()
    {
        Debug.Log("�� ���� ���� \n OnJoinedRoom");
        SetActivePanel(Panel.Room);
        
        // ClearChatMessages();
    }
    public override void OnLeftRoom()
    {
        Debug.Log("�� ���� ����");
        SetActivePanel(Panel.Lobby);
        ClearChatMessages();
    }
    /// <summary>
    /// ��������Ƽ ������Ʈ
    /// </summary>
    /// <param name="changedProperty"></param>
    public override void OnRoomPropertiesUpdate(Hashtable changedProperty)
    {
        // ���� ������ ���� ������Ƽ�� ������Ʈ�� ȣ���
        _roomPanel.UpdateRoomProperty(changedProperty);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // ���� ����� ������ �ִ� ��� �������� ������ ������
        // ���ǻ���
        // 1. ó�� �κ� ���� �� : ��� �� ����� ����
        // 2. ���� �� �� ����� ����Ǵ� ��� : ����� �� ��ϸ� ����
        _lobbyPanel.UpdateRoomList(roomList);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        _roomPanel.EnterPlayer(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        _roomPanel.ExitPlayer(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        _roomPanel.UpdatePlayerProperty(targetPlayer, changedProps);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"{newMasterClient.NickName} �÷��̾ ������ �Ǿ����ϴ�.");

        // PhotonNetwork.SetMasterClient(); �����ִ� ��ɵ� ���� (���常 �� �� ����)
    }
    // TODO: RoomPanel �����ؾ���
    public override void OnCreatedRoom()
    {
        Debug.Log("�� ���� ���� \n OnCreatedRoom");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"�� ���� ����, ���� : {message} \n OnCreateRoomFailed");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"�� ���� ����, ���� : {message}");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogWarning($"���� ��Ī ����, ���� : {message}");
    }


    // ä�ð���
    private void ChatOn()
    {
        if (_chatInputField.text == "" && Input.GetKey(KeyCode.Return))
        {
            _chatInputField.Select();
        }
    }
    public void OnEndEditEvent()
    {
       if (_chatInputField.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("ä�ÿ��� �׽�Ʈ");
            string strMessage = _userName + " : " + _chatInputField.text;

            // target �޴��� ��ο��� inputField�� ������� 
            _photonView.RPC("RPC_Chat", RpcTarget.All, strMessage);
            _chatInputField.text = "";
       }
    }
    public void OnEndEditEventButton()
    {
        // if (Input.GetKeyDown(KeyCode.Return))
        // {
        Debug.Log("ä�ù�ư �׽�Ʈ");
        string strMessage = _userName + " : " + _chatInputField.text;

        // target �޴��� ��ο��� inputField�� ������� 
        _photonView.RPC("RPC_Chat", RpcTarget.All, strMessage);
        _chatInputField.text = "";
        // }
    }


    // From ChatManager ä�ð��� �Լ���
    void AddChatMessage(string message)
    {
        GameObject goText = Instantiate(_chatDisplay, _chatContent.transform);
        goText.GetComponent<TextMeshProUGUI>().text = message;
        _chatDisplay.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }
    [PunRPC]
    void RPC_Chat(string message)
    {
        AddChatMessage(message);
        
    }
    // ���γ����ٿ��°ŷ� �� ä���� �������� �ȵǴ� RPC�� �ϸ� �ȵɰ� ����.
    void ClearChatMessages()
    {
        // ���ö� �ѹ� Ŭ�����ϰ� ���ӽ����Ҷ��� �ѹ��ؾߵ�

        // �ڿ� Ŭ���ִ¾ֵ鸸 �����ߵ�
        string objName = "RoomChatDisplay";
        foreach (Transform child in _chatContent.transform)
        {   
            if (child.name != objName)
            {
                Destroy(child.gameObject);
            }
        }
    }


}

