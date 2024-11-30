using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartItem : ItemBase
{
    [SerializeField] private GameObject _dartPrefab;  // ��Ʈ ������

    private void Awake()
    {
        itemName = "��Ʈ ������";
        itemType = E_ITEMTYPE.ActiveItem;
    }

    public override void ApplyEffect(GameObject player)
    {
        // MuzzlePoint ��������
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController == null || playerController.muzzlePoint == null)
        {
            Debug.LogError("MuzzlePoint�� �������� �ʾҽ��ϴ�.");
            return;
        }

        // ��Ʈ�� MuzzlePoint���� ����
        Vector3 spawnPosition = playerController.muzzlePoint.position;
        Quaternion spawnRotation = Quaternion.LookRotation(playerController.muzzlePoint.forward);

        // ��Ʈ�� ��Ʈ��ũ �󿡼� ����
        GameObject dart = PhotonNetwork.Instantiate($"Item/{_dartPrefab.name}", spawnPosition, spawnRotation);

        Debug.Log("��Ʈ ������ ���: ��Ʈ�� �߻��߽��ϴ�.");
    }
}
