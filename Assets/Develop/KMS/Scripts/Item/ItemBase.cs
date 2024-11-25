using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviourPun, IExplosionInteractable
{
    public string itemName;         // ������ �̸�
    public bool isPickup = false;   // �������� �Ⱦ��Ǿ����� ����

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}�� �������� �����߽��ϴ�.");
            PhotonView playerView = other.GetComponent<PhotonView>();

            if (playerView != null && playerView.IsMine)
            {
                photonView.RPC(nameof(OnPickedUp_RPC), RpcTarget.AllBuffered, playerView.ViewID);
            }
        }
    }

    /// <summary>
    /// �������� ȿ���� �÷��̾ �����ϴ� �޼��� (��ӹ޾� ����)
    /// </summary>
    public abstract void ApplyEffect(GameObject player);

    /// <summary>
    /// �������� �Ⱦ��� �� �߰������� ó���� ����
    /// </summary>
    [PunRPC]
    protected virtual void OnPickedUp_RPC(int playerViewID)
    {
        PhotonView playerPhotonView = PhotonView.Find(playerViewID);
        if (playerPhotonView != null)
        {
            GameObject player = playerPhotonView.gameObject;
            ApplyEffect(player);
        }

        // ��� Ŭ���̾�Ʈ���� ����ȭ�� ������ ����
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    /// <summary>
    /// ���ٱ⿡ ���� �������� ���ŵǴ� ó��
    /// </summary>
    protected virtual void OnHitByWaterStream()
    {
        Debug.Log($"{itemName}��(��) ���ٱ⿡ ���� ���ŵǾ����ϴ�.");
        Destroy(gameObject);
    }

    public virtual bool Interact()
    {
        OnHitByWaterStream();
        return true;
    }
}
