using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Firebase.Auth;


public class RoomPanel : BaseUI
{
    [SerializeField] PlayerEntry[] _playerEntries;
    [SerializeField] Button _startButton;

    private void OnEnable()
    {
        UpdatePlayers();

        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);


        // ������� üũ�α�
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        Debug.Log($"�������: {ready}");

        Init();

        // TestLog();

    }
    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;
    }
    private void Init()
    {
        GetUI<Button>("PreviousButton").onClick.AddListener(LeaveRoom);
        _startButton = GetUI<Button>("StartButton");
        _startButton.onClick.AddListener(StartGame);
    }
    private void TestLog()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;
        Debug.Log("���г� �׽�Ʈ�α�");
        Debug.Log($"Display Name: \t {user.DisplayName}");
        Debug.Log($"Email Address: \t {user.Email}");
        Debug.Log($"Email Verification: \t {user.IsEmailVerified}");
        Debug.Log($"User ID: \t\t {user.UserId}");
        Debug.Log("");
    }
   
    public void UpdatePlayers()
    {
        foreach (PlayerEntry entry in _playerEntries)
        {
            entry.SetEmpty();
        }
        // ���� �濡 �ִ� ��� �÷��̾� ��������
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Number �Ҵ� ���� -1  �̴ϱ�, �� �÷��̾�� �Ҵ����� �ʴ´�.
            if (player.GetPlayerNumber() == -1)
                continue;

            int number = player.GetPlayerNumber();
            _playerEntries[number].SetPlayer(player);
        }
        // ���⼭ ����̻��� �ȵǰԲ�, ���ǹ��� �ɸ� ����̻����~ �ϰ� �� �� �ִ�.
        
        // ������ �����ϴ븸 ���� �� �ְ��ϱ�
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _startButton.interactable = CheckAllReady();
        }
        else
        {
            _startButton.interactable = false;
        }
    }
    public void UpdatePlayerProperty(Player targetPlayer, Hashtable properties)
    {
        if(properties.ContainsKey(CustomProperty.READY))
        {
            UpdatePlayers();
        }
    }
    public void EnterPlayer(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}���� �����Ͽ����ϴ�.");
        UpdatePlayers();
    }
    public void ExitPlayer(Player oldPlayer)
    {
        Debug.Log($"{oldPlayer.NickName}���� �����Ͽ����ϴ�.");
        UpdatePlayers();
    }
    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // �� ���鼭 �Ѹ��̶� false�� ����
            if(player.GetReady() == false)
                return false;
        }   // �� ���� �� �����
        return true;
    }
    public void StartGame()
    {
        PhotonNetwork.LoadLevel("KMS_ICE_Scene"); // ���� �����ϸ鼭 �̸����� ����
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


}
