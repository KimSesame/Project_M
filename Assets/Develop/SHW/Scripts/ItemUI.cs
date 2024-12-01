using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private Inventory inventory;

    [Header("Items")]
    [SerializeField] Image niddle;
    [SerializeField] Image dart;

    private void Awake()
    {
        inventory = gameObject.GetComponent<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("Inventory ��ũ��Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    private void OnEnable()
    {
        // �κ��丮 �̺�Ʈ ���
        // inventory.OnItemChanged += UpdateUI;
    }

    private void OnDisable()
    {
        // �̺�Ʈ ����
        // inventory.OnItemChanged -= UpdateUI;
    }

    private void UpdateUI(bool hasItem, ItemBase item)
    {
        // ��� ������ ������ �ʱ�ȭ
        ItemUIOFF();

        if (hasItem && item != null)
        {
            // ������ �̸��� ���� ������ Ȱ��ȭ
            switch (item.itemName)
            {
                case "�ٴ� ������":
                    niddle.gameObject.SetActive(true);
                    break;

                case "��Ʈ ������":
                    dart.gameObject.SetActive(true);
                    break;

                default:
                    Debug.LogWarning($"�� �� ���� ������: {item.itemName}");
                    break;
            }
        }
    }

    public void ItemUIOFF()
    {
        niddle.gameObject.SetActive(false);
        dart.gameObject.SetActive(false);
    }
}
