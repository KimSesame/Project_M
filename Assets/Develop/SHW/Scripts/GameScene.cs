using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� �÷��̿� �ʼ������� ���Խ��� �� ���� �� ��ũ��Ʈ
/// (241126) ���� �÷��̾��� �������� ����ϰ� ������ �߰����� ������ �ʿ��մϴ�.
/// </summary>
public class GameScene : MonoBehaviour
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

        // �÷��̾� �ѹ� ����
        playerNum = PhotonNetwork.LocalPlayer.ActorNumber - 1;

        // �ణ�� ������ �� ���� ����
        StartCoroutine(StartDelayRoutine());
    }

    IEnumerator StartDelayRoutine()
    {
        yield return new WaitForSeconds(1f);
        TestGameStart();
    }

    public void TestGameStart()
    {
        Debug.Log("���� ����");

        // �׽�Ʈ�� ���� ���� �κ�
        spawner.PlayerSpawn(playerNum);
    }
}
