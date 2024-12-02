using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviourPun, IExplosionInteractable
{
    public E_ITEMTYPE itemType;     // ���� �������� Ÿ��

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
                // ������ ȹ�� ����
                SoundManager.Instance.PlaySFX(SoundManager.E_SFX.GET_ITEM);

                // �ﰢ ��� ������
                if (itemType == E_ITEMTYPE.InstantItem)
                {
                    photonView.RPC(nameof(OnPickedUp_RPC), RpcTarget.AllBuffered, playerView.ViewID);
                }
                // ���� �ʿ� ������(�κ��丮�� �߰�)
                else if (itemType == E_ITEMTYPE.ActiveItem)
                {
                    playerView.GetComponent<Inventory>().AddItem(this);

                    // �������� ��� Ŭ���̾�Ʈ���� �̵���Ŵ
                    photonView.RPC(nameof(MoveItemToInventory_RPC), RpcTarget.AllBuffered, playerView.ViewID);
                }
                //photonView.RPC(nameof(OnPickedUp_RPC), RpcTarget.AllBuffered, playerView.ViewID);
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
    /// �������� �κ��丮�� �̵�
    /// </summary>
    [PunRPC]
    public virtual void MoveItemToInventory_RPC(int playerViewID)
    {
        if (PhotonView.Find(playerViewID) != null)
        {
            // ������ ��ġ �̵�
            transform.position = new Vector3(100, 0, 100);
        }
    }

    /// <summary>
    /// ���ٱ⿡ ���� �������� ���ŵǴ� ó��
    /// </summary>
    protected virtual void OnHitByWaterStream()
    {
        Debug.Log($"{itemName}��(��) ���ٱ⿡ ���� ���ŵǾ����ϴ�.");

        if (photonView.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }

    public virtual bool Interact()
    {
        OnHitByWaterStream();
        return true;
    }
}
