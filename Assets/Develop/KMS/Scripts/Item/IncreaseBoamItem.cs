using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseBoamItem : ItemBase
{
    public int balloonCountIncrease = 1;  // �߰��� ��ǳ�� ����

    public override void ApplyEffect(GameObject player)
    {
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus)
        {
            //playerStatus.bombCount += balloonCountIncrease;
        }
    }
}
