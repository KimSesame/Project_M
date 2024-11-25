using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviourPunCallbacks
{
    public const string RoomName = "TestRoom";

    private void Start()
    {
        // �г��ӵ� �ϴ� ���� ���� �ο�
        PhotonNetwork.LocalPlayer.NickName = $"Player {Random.Range(1000, 10000)}";
        // �������ڸ��� �����ҰŴϱ�
        PhotonNetwork.ConnectUsingSettings();
    }

    // �׳� �������� ������ ������ ���ӽ�Ű��
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 8;
        options.IsVisible = false;

        // �� �̸�, �ɼ�, �κ�Ÿ���� ��������
        // �κ�� �ǳ� �۰Ŵ� TypedLobby.Default �� ���� (null�� �θ� ����)
        PhotonNetwork.JoinOrCreateRoom(RoomName, options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        StartCoroutine(StartDelayRoutine());
    }

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
        PlayerSpawn();
    }

    private void PlayerSpawn()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));

        // ���ҽ��� �������ʿ� ����ٸ� ������ �ּҷ� �ۼ� (�� : GameObject/Player)
        PhotonNetwork.Instantiate("Player", randomPos, Quaternion.identity);
    }
}
