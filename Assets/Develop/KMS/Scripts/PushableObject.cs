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
    private PhotonView pushingPlayer = null;        // ���� �а� �ִ� �÷��̾�
    private float contactTime = 0f;                 // �÷��̾���� ���� �ð�
    private const float requiredContactTime = 0.5f; // �б� ���� �ּ� ���� �ð�



    private void Awake()
    {
        // �ʱ� ��ġ�� ������ ����
        targetPosition = RoundToGrid(transform.position);
        transform.position = targetPosition;
    }

    private void Update()
    {
        // �а� �ִ� �÷��̾ ������ ���� �ð� �ʱ�ȭ
        if (pushingPlayer == null)
        {
            contactTime = 0f;
        }
    }

    public void Push(PhotonView playerView, Vector3 direction)
    {
        // �̹� �̵� ���̸� ����
        if (isMoving) return;

        // �÷��̾ ������ �������� ��� �а� �ִ��� Ȯ��
        if (pushingPlayer == null || pushingPlayer != playerView)
        {
            pushingPlayer = playerView;
            contactTime = 0f; // ���ο� �÷��̾ �� ��� ���� �ð� �ʱ�ȭ
        }
        contactTime += Time.deltaTime;

        // �ּ� ���� �ð��� �����Ǿ����� Ȯ��
        if (contactTime >= requiredContactTime)
        {
            Vector3 potentialTarget = targetPosition + direction;

            if (CanMoveTo(potentialTarget))
            {
                targetPosition = potentialTarget;
                isMoving = true;
                contactTime = 0f; // ���� �ð� �ʱ�ȭ

                // RPC ȣ��� ��� Ŭ���̾�Ʈ���� �̵� ����ȭ
                photonView.RPC(nameof(PushRPC), RpcTarget.All, targetPosition);
            }
        }
    }

    [PunRPC]
    private void PushRPC(Vector3 newTargetPosition)
    {
        // �̵� ���¸� ��� Ŭ���̾�Ʈ���� ����ȭ
        isMoving = true;

        StartCoroutine(MoveObject(newTargetPosition));
    }

    private IEnumerator MoveObject(Vector3 newTargetPosition)
    {
        while (Vector3.Distance(transform.position, newTargetPosition) > 0.01f)
        {
            // ��ġ�� �̵�
            Debug.Log("�̵�");
            transform.position = Vector3.MoveTowards(transform.position, newTargetPosition, _pushSpeed * Time.deltaTime);
            yield return null;
        }

        // ��ǥ ��ġ�� ����
        transform.position = RoundToGrid(newTargetPosition);
        isMoving = false;
        pushingPlayer = null; // �б� �Ϸ� �� �а� �ִ� �÷��̾� �ʱ�ȭ
    }

    private bool CanMoveTo(Vector3 position)
    {
        // �浹 üũ (��: �÷��̾�, �� ��)
        // �ش� ��ġ�� �浹ü�� �ִ��� Ȯ��
        Collider[] colliders = Physics.OverlapSphere(position, 0.4f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Obstacle") 
                || collider.CompareTag("Box") 
                || collider.CompareTag("Player") 
                || collider.gameObject.layer == LayerMask.NameToLayer("WaterBomb"))
            {
                Debug.Log($"collider tag {collider.tag}");
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
