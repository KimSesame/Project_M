using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiddleItem : ItemBase
{
    private void Awake()
    {
        itemName = "�ٴ� ������";
        itemType = E_ITEMTYPE.ActiveItem;
    }

    public override void ApplyEffect(GameObject player)
    {
        Bubble bubble = player.GetComponentInChildren<Bubble>();
        if (bubble != null)
        {
            // Bubble�� Save �Լ� ȣ��
            bubble.photonView.RPC(nameof(bubble.Save), RpcTarget.All);
            Debug.Log("�ٴ� ������ ���: ����� ���¿��� Ż��");
        }
        else
        {
            Debug.Log("�ٴ� ��� ����: ����� ���°� �ƴ�.");
        }
    }
}
