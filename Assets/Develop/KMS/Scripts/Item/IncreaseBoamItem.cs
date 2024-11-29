using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseBoamItem : ItemBase
{
    public int balloonCountIncrease = 1;  // �߰��� ��ǳ�� ����

    private void Awake()
    {
        itemName = "��ǳ�� ���� ������";
        itemType = E_ITEMTYPE.InstantItem;
    }

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("ǳ���� ���� ���� �������� �����߽��ϴ�.");
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus)
        {
            Debug.Log("ǳ���� ������ ���� �մϴ�.");
            playerStatus.bombCount += balloonCountIncrease;
        }
    }
}
