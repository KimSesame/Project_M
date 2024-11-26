using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";
    private PlayerSpwaner spawner;
    private int playerNum;

    private void Start()
    {
        spawner = GetComponent<PlayerSpwaner>();

        // �г��ӵ� �ϴ� ���� ���� �ο�
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        // �������ڸ��� �����ҰŴϱ�
        // PhotonNetwork.ConnectUsingSettings();

        StartCoroutine(StartDelayRoutine());
    }

    //public override void OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    playerNum = newPlayer.ActorNumber;
    //    // spawner.PlayerSpawn(playerNum);
    //}

    // �׳� �������� ������ ������ ���ӽ�Ű��
    //public override void OnConnectedToMaster()
    //{
    //    RoomOptions options = new RoomOptions();
    //    options.MaxPlayers = 8;
    //    options.IsVisible = false;

    //    // �� �̸�, �ɼ�, �κ�Ÿ���� ��������
    //    // �κ�� �ǳ� �۰Ŵ� TypedLobby.Default �� ���� (null�� �θ� ����)
    //    PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    //}

    //public override void OnJoinedRoom()
    //{
    //    // StartCoroutine(StartDelayRoutine());
    //    TestGameStart();
    //}

    // �����ڸ��� �����ϸ� ������ ���� �� ������ ��Ʈ��ũ ������ ���� �ð� �����ֱ�
    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }

    public void TestGameStart()
    {
        Debug.Log("���� ����");

        // �׽�Ʈ�� ���� ���� �κ�
         spawner.PlayerSpawn(PhotonNetwork.LocalPlayer.ActorNumber-1);
    }

}
