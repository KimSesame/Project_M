using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : ItemBase
{
    public int powerIncrease = 1;  // ��ǳ�� �Ŀ�(���Ĺ���)

    private void Awake()
    {
        itemName = "��ǳ�� �Ŀ� ���� ������";
        itemType = E_ITEMTYPE.InstantItem;
    }

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("�Ŀ� �������� �����߽��ϴ�.");
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus)
        {
            Debug.Log("�Ŀ��� ���� �մϴ�.");
            playerStatus.power += powerIncrease;
        }
    }
}
