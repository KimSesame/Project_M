using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour, IExplosionInteractable
{
    public string itemName;         // ������ �̸�
    public bool isPickup = false;   // �������� �Ⱦ��Ǿ����� ����

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾�� �浹��
        if (other.CompareTag("Player"))
        {
            Debug.Log($"{other.name}�� �������� �����߽��ϴ�.");
            isPickup = true;
            ApplyEffect(other.gameObject);
            OnPickedUp();
        }
        // ���ٱ�� �浹��
        // TODO : Interface�� ����� �ξ��⿡ �ش� Interface�� �Է� �� �����ϵ��� �����Ѵ�.
        //else if (other.CompareTag("WaterStream"))   // ���Ƿ� ���� �Է��� tag
        //{
        //    OnHitByWaterStream();
        //}
    }

    /// <summary>
    /// �������� ȿ���� �÷��̾ �����ϴ� �޼��� (��ӹ޾� ����)
    /// </summary>
    public abstract void ApplyEffect(GameObject player);

    /// <summary>
    /// �������� �Ⱦ��� �� �߰������� ó���� ����
    /// </summary>
    protected virtual void OnPickedUp()
    {
        // �Ⱦ� �� ������ ������Ʈ ����
        Destroy(gameObject);
    }

    /// <summary>
    /// ���ٱ⿡ ���� �������� ���ŵǴ� ó��
    /// </summary>
    protected virtual void OnHitByWaterStream()
    {
        Debug.Log($"{itemName}��(��) ���ٱ⿡ ���� ���ŵǾ����ϴ�.");
        Destroy(gameObject);
    }

    public virtual bool Interact()
    {
        OnHitByWaterStream();
        return true;
    }
}