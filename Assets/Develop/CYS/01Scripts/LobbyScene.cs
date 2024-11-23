using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

public class LobbyScene : MonoBehaviourPunCallbacks
{
    public enum Panel { Login, Lobby, Room } // ���θ޴��� �κ� �����־ ��..???

    [SerializeField] LoginPanel _loginPanel;
    // [SerializeField] MainPanel _mainPanel;
    [SerializeField] LobbyPanel _lobbyPanel;
    [SerializeField] RoomPanel _roomPanel;

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
        else if (PhotonNetwork.InLobby)
        {
            SetActivePanel(Panel.Lobby);
        }
        else
        {
            SetActivePanel(Panel.Login);
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
    }
    public override void OnLeftRoom()
    {
        Debug.Log("�� ���� ����");
        SetActivePanel(Panel.Lobby);
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

}

