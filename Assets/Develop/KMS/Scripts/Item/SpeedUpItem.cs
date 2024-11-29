using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : ItemBase
{
    public float speedIncreaseAmount = 1.0f;    // �ӵ� ������

    private void Awake()
    {
        itemName = "ĳ���� ���ǵ� ������";
        itemType = E_ITEMTYPE.InstantItem;
    }

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("���ǵ� �������� �����߽��ϴ�.");
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus)
        {
            Debug.Log("���ǵ尡 ���� �մϴ�.");
            playerStatus.speed += speedIncreaseAmount;
        }
    }
}
