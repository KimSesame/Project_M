using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemBase> inventory = new List<ItemBase>();

    public void AddItem(ItemBase item)
    {
        if (item.itemType == ItemBase.E_ITEMTYPE.ActiveItem)
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

                // ���� �ִ� �������� ��Ʈ��ũ �󿡼� ����
                if (oldItem.photonView != null && oldItem.photonView.IsMine)
                {
                    PhotonNetwork.Destroy(oldItem.gameObject);
                }
                else
                {
                    Destroy(oldItem.gameObject); // ���� ������ ����
                }

                Debug.Log($"���� ������ {oldItem.itemName}�� �����Ǿ����ϴ�.");
                EquipItem(item);
            }
        }
    }

    private void EquipItem(ItemBase item)
    {
        inventory.Add(item);
        item.transform.position = new Vector3(100, 0, 100);
        Debug.Log($"������ {item.itemName}�� �κ��丮�� �߰��Ǿ����ϴ�.");
    }


    public void UseItem(int index)
    {
        if (index < inventory.Count)
        {
            inventory[index].ApplyEffect(this.gameObject);
            inventory.RemoveAt(index);
        }
    }
}
