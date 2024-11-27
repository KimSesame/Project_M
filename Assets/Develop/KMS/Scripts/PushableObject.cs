using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PushableObject : MonoBehaviourPun
{
    [SerializeField] private float _pushSpeed = 3f; // �̴� �ӵ�

    private Vector3 targetPosition;                 // ��ǥ ��ġ
    private bool isMoving = false;                  // �̵� ������ ����

    private ProceduralDestruction destructionScript;

    private void Awake()
    {
        // �ʱ� ��ġ�� ������ ����
        targetPosition = RoundToGrid(transform.position);
        transform.position = targetPosition;
    }

    public void Push(Vector3 direction)
    {
        if (!isMoving)
        {
            Vector3 potentialTarget = targetPosition + direction;

            // �̵� ���� ���� üũ
            if (CanMoveTo(potentialTarget))
            {
                targetPosition = potentialTarget;
                isMoving = true;

                // RPC ȣ��� ��� Ŭ���̾�Ʈ���� �̵� ����ȭ
                photonView.RPC(nameof(PushRPC), RpcTarget.All, targetPosition);
            }
        }
    }

    [PunRPC]
    private void PushRPC(Vector3 newTargetPosition)
    {
        StartCoroutine(MoveObject(newTargetPosition));
    }

    private IEnumerator MoveObject(Vector3 newTargetPosition)
    {
        while (Vector3.Distance(transform.position, newTargetPosition) > 0.01f)
        {
            // ��ġ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, newTargetPosition, _pushSpeed * Time.deltaTime);
            yield return null;
        }

        // ��ǥ ��ġ�� ����
        transform.position = RoundToGrid(newTargetPosition);
        isMoving = false;
    }

    private bool CanMoveTo(Vector3 position)
    {
        // �浹 üũ (��: �÷��̾�, �� ��)
        // �ش� ��ġ�� �浹ü�� �ִ��� Ȯ��
        Collider[] colliders = Physics.OverlapSphere(position, 0.4f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle") || collider.CompareTag("Box") || collider.CompareTag("Player"))
            {
                // �̵� �Ұ���
                return false;
            }
        }
        return true;
    }

    private Vector3 RoundToGrid(Vector3 position)
    {
        // ��ġ�� ���� ��ǥ�� ����
        return new Vector3(Mathf.Round(position.x), position.y, Mathf.Round(position.z));
    }
}
