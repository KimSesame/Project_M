using Photon.Pun;
using System.Linq;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviourPunCallbacks
{
    private SpawnPointManager spawnPointManager;
    private Vector3[] randomPoints;
    private Vector3 spawnPoint;

    private int charNum;        // ĳ���� �ѹ�

    private void Awake()
    {
        PlayerSpawn(PhotonNetwork.LocalPlayer.ActorNumber - 1);
    }

    public void PlayerSpawn(int num)
    {
        // �÷��̾� ���� ����Ʈ ����
        // �÷��̾� ���̵� ã�Ƽ� 
        // ���̵� �Ҵ�Ǵ� ��ȣ�� ���� ����Ʈ ��ġ
        spawnPointManager = GameObject.Find("MapContainer").GetComponent<SpawnPointManager>();
        spawnPoint = spawnPointManager.spawnPoints[num];

        // ���� ����
        
        //int randomNum = Random.Range(0, spawnPoints.Length);
        //for (int i = 0; i == spawnPoints.Length; i++)
        //{

        //}

        // ������Ƽ ������ �Ϸ�Ǹ� �ּ� �����ؼ� ���
        // PhotonNetwork.LocalPlayer.GetTeam(out charNum);

        // ���� ĳ���� �׽�Ʈ��
        // num �ڸ��� charNum�� �־ ĳ���� �Ҵ�
        if (num == 0)
        {
            PhotonNetwork.Instantiate("PlayerAdult", spawnPoint, Quaternion.identity);
        }
        else if (num == 1)
        {
            PhotonNetwork.Instantiate("PlayerGirl", spawnPoint, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity);
        }
    }
}
