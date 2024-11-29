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
            bubble.StopAllCoroutines();
            bubble.bubble.SetActive(false);
            bubble.player.GetComponent<Animator>().SetBool("isBubble", false);
            bubble.player.GetComponent<PlayerStatus>().isBubble = false;
            bubble.player.GetComponent<WaterBombPlacer>().enabled = true;
        }
        else
        {
            Debug.Log("�ٴ� ��� ����: ����� ���°� �ƴ�.");
        }
    }
}
