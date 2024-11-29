using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartItem : ItemBase
{
    [SerializeField] private float _maxRange = 10f; // ��Ʈ�� ���� �� �ִ� �ִ� �Ÿ�
    [SerializeField] private LayerMask _waterBombLayer; // ��ǳ���� �����ϴ� ���̾�


    private void Awake()
    {
        itemName = "��Ʈ ������";
        itemType = E_ITEMTYPE.ActiveItem;
    }

    public override void ApplyEffect(GameObject player)
    {
        RaycastHit hit;
        Vector3 playerPosition = player.transform.position;
        Vector3 forwardDirection = player.transform.forward;

        // ����ĳ��Ʈ�� ��ǳ�� ����
        if (Physics.Raycast(playerPosition + Vector3.up * 0.5f, forwardDirection, out hit, _maxRange, _waterBombLayer))
        {
            WaterBomb waterBomb = hit.collider.GetComponent<WaterBomb>();
            if (waterBomb != null)
            {
                // ��ǳ�� Interact ȣ��
                waterBomb.Interact();
                Debug.Log("��Ʈ ������ ���: ��ǳ���� �Ͷ߷Ƚ��ϴ�.");
            }
            else
            {
                Debug.Log("��Ʈ ������ ��� ����: ��ǳ���� ã�� ���߽��ϴ�.");
            }
        }
        else
        {
            Debug.Log("��Ʈ ������ ��� ����: ���� ���� ��ǳ���� �����ϴ�.");
        }
    }
}
