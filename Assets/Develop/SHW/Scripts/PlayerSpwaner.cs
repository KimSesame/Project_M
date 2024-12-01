using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviourPunCallbacks
{
    [SerializeField] private SpawnPointManager _spawnPointManager;
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

        if(_spawnPointManager.spawnPoints.Count == 0)
        {
            _spawnPointManager.LoadSpawnPoints();
        }

        spawnPoint = _spawnPointManager.spawnPoints[num];

        // ������Ƽ ������ �Ϸ�Ǹ� �ּ� �����ؼ� ���
        charNum =  PhotonNetwork.LocalPlayer.GetCharacter();

        // ���� ĳ���� �׽�Ʈ��
        // num �ڸ��� charNum�� �־ ĳ���� �Ҵ�
        if (charNum == 0)
        {
            PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity);
        }
        else if (charNum == 1)
        {
            PhotonNetwork.Instantiate("PlayerAdult", spawnPoint, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate("PlayerGirl", spawnPoint, Quaternion.identity);
        }
    }
}
