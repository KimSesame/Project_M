using Photon.Pun;
using UnityEngine;

public class DartProjectile : MonoBehaviourPun
{
    [SerializeField] private float _speed = 15f;    // ��Ʈ�� �ӵ�
    [SerializeField] private float _lifetime = 5f;  // ��Ʈ�� ����
    private Vector3 _initialDirection;              // ��Ʈ�� �ʱ� ����
    private double _creationTime;                   // ��Ʈ ���� �� ���� �ð�
    private Vector3 _startPosition;                 // ��Ʈ ���� ��ġ

    private void Start()
    {
        if (photonView.IsMine)
        {
            _initialDirection = transform.forward;
            _startPosition = transform.position;

            // ������ �ð��� ���(������ RPC�� �Ⱦ��� ����ȭ ����.)
            _creationTime = PhotonNetwork.Time;
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

    private void OnEnable()
    {
        if (!photonView.IsMine)
        {
            // ���� ���� ���
            AdjustForLag();
        }
    }

    private void AdjustForLag()
    {
        // ���� �������� ������� ����� �ð� ���
        double elapsedTime = PhotonNetwork.Time - _creationTime;

        // ��Ʈ�� ��ġ�� ��� �ð��� ���� ����
        transform.position = _startPosition + _initialDirection * _speed * (float)elapsedTime;

        Debug.Log($"Dart ���� ���� ����. ��� �ð�: {elapsedTime}s, �� ��ġ: {transform.position}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("WaterBomb"))
        {
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
        else
        {
            Debug.Log("��ǳ���� �ƴ϶� ������ �ȵ˴ϴ�.");
        }
    }
}