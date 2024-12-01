using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviourPunCallbacks
{
    [SerializeField] private SpawnPointManager _spawnPointManager;
    public GameObject map;

    // KMS ��� ������ ���� ����Ʈ ���
    private List<Vector3> _availableSpawnPoints = new List<Vector3>();

    private void Start()
    {
        map.SetActive(true);
        // ���� ����Ʈ �ε�
        if (_spawnPointManager.spawnPoints.Count == 0)
        {
            _spawnPointManager.LoadSpawnPoints();
        }

        // ��� ������ ���� ����Ʈ �ʱ�ȭ
        _availableSpawnPoints = new List<Vector3>(_spawnPointManager.spawnPoints);

        // ĳ���� ID ����ȭ�� ��ٸ� �� ����
        StartCoroutine(WaitForCharacterIdAndSpawn());
    }

    private IEnumerator WaitForCharacterIdAndSpawn()
    {
        while (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Character"))
        {
            Debug.Log("ĳ���� ID�� ��ٸ��� ��");
            yield return null; // ���� ������ ���
        }

        Debug.Log($"ĳ���� ID�� ã��: {PhotonNetwork.LocalPlayer.CustomProperties["Character"]}");
        SpawnPlayer();
    }

    /// <summary>
    ///  ĳ���� ���� ��ȯ �� ���� ����.
    /// </summary>
    public void SpawnPlayer()
    {
        // ���� ��ġ ����
        Vector3 spawnPoint = GetRandomSpawnPoint();

        // ĳ���� ID ��������
        int characterId = GetCharacterId();

        // ĳ���� ������ �̸� ����
        string prefabName = GetCharacterPrefabName(characterId);

        // ĳ���� ����
        PhotonNetwork.Instantiate(prefabName, spawnPoint, Quaternion.identity);
    }

    /// <summary>
    /// ���� ������ �Ŵ����� ��ġ�� �����ϰ� �����ϴ� �޼���.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomSpawnPoint()
    {
        // ��� ������ ���� ����Ʈ�� ������ ���� ó��
        if (_availableSpawnPoints.Count == 0)
        {
            Debug.LogError("���� ����Ʈ�� �����մϴ�.");
            return Vector3.zero;
        }

        // ������ ���� ����Ʈ ����
        int randomIndex = Random.Range(0, _availableSpawnPoints.Count);
        Vector3 selectedSpawnPoint = _availableSpawnPoints[randomIndex];

        Debug.Log($"���� ��ġ�� {selectedSpawnPoint}");

        // ���õ� ���� ����Ʈ�� ����Ʈ���� �����Ͽ� �ߺ� ����
        _availableSpawnPoints.RemoveAt(randomIndex);

        return selectedSpawnPoint;
    }

    private int GetCharacterId()
    {
        // Photon�� Custom Properties���� ĳ���� ID ��������
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Character", out object characterId))
        {
            return (int)characterId;
        }

        // �⺻�� (0)
        Debug.LogWarning("ĳ���� ID�� �������� �ʾҽ��ϴ�. �⺻��(0)�� ����մϴ�.");
        return 0;
    }

    private string GetCharacterPrefabName(int characterId)
    {
        // ĳ���� ID�� ���� ������ �̸� ��ȯ
        switch (characterId)
        {
            case 0:
                return "Player";        // Player ������
            case 1:
                return "PlayerAdult";   // PlayerAdult ������
            case 2:
                return "PlayerGirl";    // PlayerGirl ������
            default:
                Debug.LogWarning("�߸��� ĳ���� ID�Դϴ�. �⺻ ĳ���͸� �����մϴ�.");
                return "Player"; // �⺻ ĳ���� ������
        }
    }

    #region �⺻ �÷��̾� ����
    /*
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
     */
    #endregion
}
