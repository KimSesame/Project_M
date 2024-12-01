using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviourPun
{
    public List<ItemBase> inventory = new List<ItemBase>();

    // ������ ���� �̺�Ʈ
    public event Action<bool, ItemBase> OnItemChanged;

    /// <summary>
    /// ������ �����ϴ� �޼���.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemBase item)
    {
        if (item.itemType == E_ITEMTYPE.ActiveItem)
        {
            if (inventory.Count < 1)
            {
                EquipItem(item);
            }
            else
            {
                Debug.Log("�κ��丮�� ���� á���ϴ�.");
                ItemBase oldItem = inventory[0];
                inventory.RemoveAt(0);

                // ���� ������ ���� ����ȭ
                oldItem.photonView.RPC(nameof(ItemBase.Interact), RpcTarget.AllBuffered);

                Debug.Log($"���� ������ {oldItem.itemName}�� �����Ǿ����ϴ�.");
                EquipItem(item);
            }
        }
    }

    /// <summary>
    /// �������� �κ��丮�� ��ġ�ϵ��� �ϴ� �޼���.
    /// </summary>
    /// <param name="item"></param>
    private void EquipItem(ItemBase item)
    {
        inventory.Add(item);
        // �������� ��ġ�� ��� Ŭ���̾�Ʈ���� �̵�
        if (PhotonNetwork.IsMasterClient)
        {
            item.photonView.RPC(nameof(ItemBase.MoveItemToInventory_RPC), RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        Debug.Log($"������ {item.itemName}�� �κ��丮�� �߰��Ǿ����ϴ�.");

        // UI ���� �̺�Ʈ ȣ�� (������ �߰�)
        OnItemChanged?.Invoke(true, item);
    }

    /// <summary>
    /// �ش� �������� ����ϴ� �޼���.
    /// </summary>
    /// <param name="index"></param>
    public void UseItem(int index)
    {
        if (index < inventory.Count)
        {
            ItemBase item = inventory[index];

            // ������ ���
            inventory[index].ApplyEffect(this.gameObject);
            inventory.RemoveAt(index);

            // UI ���� �̺�Ʈ ȣ�� (������ ����)
            OnItemChanged?.Invoke(false, null);

            // ���忡�� ���� ��û
            if (!PhotonNetwork.IsMasterClient)
            {
                photonView.RPC(nameof(RequestItemDestruction_RPC), RpcTarget.MasterClient, item.photonView.ViewID);
            }
            else
            {
                PhotonNetwork.Destroy(item.gameObject);
            }
        }
        else
        {
            Debug.Log($"�κ��丮�� �������� �����ϴ�.");
        }
    }

    /// <summary>
    /// �������� ���� ������ Ŭ���̾�Ʈ���� ������ ��û�ϴ� �޼���.
    /// </summary>
    /// <param name="itemViewID"></param>
    [PunRPC]
    public void RequestItemDestruction_RPC(int itemViewID)
    {
        PhotonView itemView = PhotonView.Find(itemViewID);
        if (itemView != null && itemView.IsMine)
        {
            PhotonNetwork.Destroy(itemView.gameObject);
        }
    }
}
