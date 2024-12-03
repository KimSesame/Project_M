using Photon.Pun;
using UnityEngine;

public class DartProjectile : MonoBehaviourPun
{
    [SerializeField] private float _speed = 15f;    // ��Ʈ�� �ӵ�
    [SerializeField] private float _lifetime = 5f;  // ��Ʈ�� ����
    [SerializeField] private LayerMask _playerLayer;// �÷��̾� ���̾� ����
    private Vector3 _direction;                     // ��Ʈ�� �̵� ����
    private Vector3 _startPosition;                 // ��Ʈ�� ���� ��ġ
    private double _creationTime;                   // ��Ʈ ���� �� ���� �ð�

    private bool _isInitialized = false;            // �ʱ�ȭ ����

    private void Start()
    {
        // ������ ���ο� ������� �ı��� ��� Ŭ���̾�Ʈ���� �ڵ����� �߻�
        Destroy(gameObject, _lifetime);
    }

    private void Update()
    {
        if (_isInitialized)
        {
            // ��Ʈ�� �̵�
            transform.position += _direction * _speed * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        if (!_isInitialized)
        {
            AdjustForLag(); // ���� ���� ����
        }
    }

    /// <summary>
    /// ���� ������ �����Ͽ� ��Ʈ ��ġ�� �����մϴ�.
    /// </summary>
    private void AdjustForLag()
    {
        if (!photonView.IsMine)
        {
            // ���� �ð� �������� ��� �ð� ���
            double elapsedTime = PhotonNetwork.Time - _creationTime;

            // ��Ʈ�� ��ġ�� ��� �ð��� ���� �̵�
            transform.position += _direction * _speed * (float)elapsedTime;
            Debug.Log($"���� ���� ���� �Ϸ�: {elapsedTime}s ���, �� ��ġ: {transform.position}");
        }
    }

    /// <summary>
    /// ��Ʈ�� �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="startPosition">���� ��ġ</param>
    /// <param name="direction">�̵� ����</param>
    /// <param name="creationTime">���� �� ���� �ð�</param>
    [PunRPC]
    public void Initialize(Vector3 startPosition, Vector3 direction, double creationTime)
    {
        _startPosition = startPosition;
        _direction = direction.normalized;
        _creationTime = creationTime;
        transform.position = startPosition;
        transform.rotation = Quaternion.LookRotation(direction);
        _isInitialized = true;

        Debug.Log($"��Ʈ �ʱ�ȭ �Ϸ�: ���� ��ġ {startPosition}, ���� {direction}, ���� �ð� {creationTime}");
    }

    private void OnTriggerEnter(Collider other)
    {
        // �÷��̾� ���̾�� �浹 �� ����
        if (((1 << other.gameObject.layer) & _playerLayer) != 0)
        {
            Debug.Log("�÷��̾���� �浹 ����");
            return;
        }

        // �浹 �� ���� ó��
        WaterBomb waterBomb = other.GetComponent<WaterBomb>();
        if (waterBomb != null)
        {
            waterBomb.Interact(); // ��ǳ�� �Ͷ߸���
        }

        Destroy(gameObject); // ��Ʈ �ı�
    }
}
