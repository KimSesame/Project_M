using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviourPun
{
    private SpawnPointManager spawnPointManager;
    private List<Vector3> spawnPoints = new List<Vector3>();
    private Vector3 spawnPoint;

    public void PlayerSpawn(int num)
    {
        // �÷��̾� ���� ����Ʈ ����
        // �÷��̾� ���̵� ã�Ƽ� 
        // ���̵� �Ҵ�Ǵ� ��ȣ�� ���� ����Ʈ ��ġ
        spawnPointManager = GameObject.Find("MapContainer").GetComponent<SpawnPointManager>();
        spawnPoints = spawnPointManager.spawnPoints;   
         spawnPoint = spawnPoints[num];
        

        // ���ҽ��� �������ʿ� ����ٸ� ������ �ּҷ� �ۼ� (�� : GameObject/Player)
        PhotonNetwork.Instantiate("Player", spawnPoint, Quaternion.identity);
    }
}
