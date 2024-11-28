using Photon.Pun;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviourPun
{
    private SpawnPointManager spawnPointManager;
    private Vector3 spawnPoint;

    private int charNum;

    public void PlayerSpawn(int num)
    {
        // �÷��̾� ���� ����Ʈ ����
        // �÷��̾� ���̵� ã�Ƽ� 
        // ���̵� �Ҵ�Ǵ� ��ȣ�� ���� ����Ʈ ��ġ
        spawnPointManager = GameObject.Find("MapContainer").GetComponent<SpawnPointManager>();
        spawnPoint = spawnPointManager.spawnPoints[num];

        // ���ҽ��� �������ʿ� ����ٸ� ������ �ּҷ� �ۼ� (�� : GameObject/Player)
        // PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity);

        // ������Ƽ ������ �Ϸ�Ǹ� �ּ� �����ؼ� ���
        // PhotonNetwork.LocalPlayer.GetTeam(out charNum);

        // ���� ĳ���� �׽�Ʈ��
        // num �ڸ��� charNum�� �־ ĳ���� �Ҵ�
        if (num == 0)
        {
            PhotonNetwork.Instantiate("PlayerAdult", spawnPoint, Quaternion.identity);
        }
        else if (num == 2)
        {
            PhotonNetwork.Instantiate("PlayerGirl", spawnPoint, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity);
        }
    }
}
