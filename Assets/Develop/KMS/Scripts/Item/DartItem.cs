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

        // ��Ʈ�� ���� ��ġ�� ����
        Vector3 spawnPosition = playerController.muzzlePoint.position;
        Vector3 spawnDirection = playerController.muzzlePoint.forward;
        Debug.Log($"spawnDirection {spawnDirection}");

        // Y�� ȸ���� ���
        float yRotation = Mathf.Atan2(spawnDirection.x, spawnDirection.z) * Mathf.Rad2Deg; // Z�� ���� ���� ���
        Quaternion spawnRotation = Quaternion.Euler(0, yRotation, 0); // X, Z���� 0���� �����ϰ� Y�ุ ȸ��
        Debug.Log($"yRotation {yRotation}");

        // ��Ʈ��ũ �󿡼� ��Ʈ�� ����
        GameObject dart = PhotonNetwork.Instantiate($"Item/{_dartPrefab.name}", spawnPosition, spawnRotation);

        // ��� Ŭ���̾�Ʈ���� ��Ʈ �ʱ�ȭ
        PhotonView dartPhotonView = dart.GetComponent<PhotonView>();
        dartPhotonView.RPC(nameof(DartProjectile.Initialize), RpcTarget.AllBuffered, spawnPosition, spawnDirection, PhotonNetwork.Time);

        Debug.Log("��Ʈ ������ ���: ��Ʈ�� �߻��߽��ϴ�.");
    }
}
