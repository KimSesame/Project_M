using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpwaner : MonoBehaviourPunCallbacks
{
    [SerializeField] private SpawnPointManager _spawnPointManager;
    public GameObject map;

    // KMS ��� ������ ���� ����Ʈ �ε���
    private List<int> _shuffledIndices = new List<int>();

    private void Start()
    {
        map.SetActive(true);

        // ���� ������ ����Ʈ ����.
        _spawnPointManager.LoadSpawnPoints();

        // �����͸� ���� ������ ����Ʈ �ε��� ���� �÷��̾�鿡�� �ε��� ����.
        if (PhotonNetwork.IsMasterClient)
        {
            ShuffleSpawnIndices();
            AssignSpawnIndicesToPlayers();
        }

        // ĳ���� ID ����ȭ�� ��ٸ� �� ����
        StartCoroutine(WaitForCharacterIdAndSpawn());
    }

    /// <summary>
    /// ���� ����Ʈ�� �ε����� ����
    /// </summary>
    private void ShuffleSpawnIndices()
    {
        // �� ����� ���� �ִ� ���� �ʱ�ȭ.
        _shuffledIndices.Clear();
        for (int i = 0; i < _spawnPointManager.spawnPoints.Count; i++)
        {
            _shuffledIndices.Add(i);
        }

        // Fisher-Yates ����(������ ���� �˰���)
        for (int i = _shuffledIndices.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = _shuffledIndices[i];
            _shuffledIndices[i] = _shuffledIndices[randomIndex];
            _shuffledIndices[randomIndex] = temp;
        }
    }

    /// <summary>
    /// �� �÷��̾�� ���� ���� �ε��� �Ҵ�
    /// </summary>
    private void AssignSpawnIndicesToPlayers()
    {
        var players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length && i < _shuffledIndices.Count; i++)
        {
            players[i].SetSpawnIndex(_shuffledIndices[i]);
        }

        Debug.Log("���� �ε����� ��� �÷��̾�� �Ҵ�Ǿ����ϴ�.");
    }

    private IEnumerator WaitForCharacterIdAndSpawn()
    {
        while (!PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(CustomProperty.CHARACTER))
        {
            Debug.Log("ĳ���� ID�� ��ٸ��� ��");
            yield return null;
        }

        Debug.Log($"ĳ���� ID�� ã��: {PhotonNetwork.LocalPlayer.CustomProperties[CustomProperty.CHARACTER]}");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == PhotonNetwork.LocalPlayer && changedProps.ContainsKey(CustomProperty.SPAWNPOINT))
        {
            Debug.Log($"���� �ε��� ���ŵ�: {targetPlayer.GetSpawnIndex()}");
            SpawnPlayer();
        }
    }

    /// <summary>
    ///  ĳ���� ���� ��ȯ �� ���� ����.
    /// </summary>
    public void SpawnPlayer()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.GetSpawnIndex();

        if (spawnIndex < 0 || spawnIndex >= _spawnPointManager.spawnPoints.Count)
        {
            Debug.LogError("�߸��� ���� �ε����Դϴ�!");
            return;
        }

        Vector3 spawnPoint = _spawnPointManager.spawnPoints[spawnIndex];
        Debug.Log($"���� ��ġ: {spawnPoint}");

        // ĳ���� ID ��������
        int characterId = GetCharacterId();

        // ĳ���� ������ �̸� ����
        string prefabName = GetCharacterPrefabName(characterId);

        // ĳ���� ����
        PhotonNetwork.Instantiate(prefabName, spawnPoint, Quaternion.identity);
    }

    private int GetCharacterId()
    {
        // Photon�� Custom Properties���� ĳ���� ID ��������
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomProperty.CHARACTER, out object characterId))
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
