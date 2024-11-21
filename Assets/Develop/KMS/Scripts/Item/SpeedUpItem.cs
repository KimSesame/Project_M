using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpItem : ItemBase
{
    public float speedIncreaseAmount = 2.0f;    // �ӵ� ������

    public override void ApplyEffect(GameObject player)
    {
        Debug.Log("���ǵ� �������� �����߽��ϴ�.");
        PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
        if (playerStatus)
        {
            Debug.Log("���ǵ尡 ���� �մϴ�.");
            //playerStatus.speed += speedIncreaseAmount;
        }
    }
}
