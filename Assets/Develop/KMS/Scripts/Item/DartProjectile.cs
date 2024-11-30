using Photon.Pun;
using UnityEngine;

public class DartProjectile : MonoBehaviourPun
{
    [SerializeField] private float _speed = 15f;     // ��Ʈ�� �ӵ�
    [SerializeField] private float _lifetime = 5f;  // ��Ʈ�� ����
    private Vector3 _initialDirection;

    private void Start()
    {
        if (photonView.IsMine)
        {
            _initialDirection = transform.forward;
            // ��Ʈ ���� �ð� �� �ڵ� �ı�
            Destroy(gameObject, _lifetime);
        }
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            // �ʱ� �������� ��Ʈ�� �̵�
            transform.position += _initialDirection * _speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        // ��ǳ���� �浹���� ���
        WaterBomb waterBomb = other.GetComponent<WaterBomb>();
        if (waterBomb != null)
        {
            // ��ǳ�� �Ͷ߸���
            waterBomb.Interact();

            // ��Ʈ�� �ı�
            PhotonNetwork.Destroy(gameObject);
        }
    }
}