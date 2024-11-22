using UnityEngine;

public class PlayerController : MonoBehaviour, IExplosionInteractable
{
    private PlayerStatus _status;           // �÷��̾� ���� ������

    [SerializeField] Rigidbody rigid;       // �̵��� ���� rigidbody

    [SerializeField] Animator animator;     // �÷��̾� �ִϸ��̼� ����

    [SerializeField] GameObject bubble;     // ���ٱ⿡ ���� ���� �����

    private float preSpeed;

    private void Awake()
    {
        _status = GetComponent<PlayerStatus>();
        _status.isBubble = false;
        bubble.SetActive(false);
        preSpeed = _status.speed;
    }

    private void Update()
    {
        // TODO : �÷��̾� ���������� ����� ���ǹ� �߰� �ʿ�
        Move();

        if (_status.isBubble == false)
        {
            _status.speed = preSpeed;
        }
    }

    public void Move()
    {
        Vector3 moveDir = new Vector3();
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");

        // �̵��� �ִϸ��̼� ���
        if (moveDir.x != 0 || moveDir.z != 0)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        // ��� �̵� ����
        if (moveDir.magnitude < 0.1)
        {
            rigid.velocity = Vector3.zero;
        }

        // ���� �Է� �� �ٸ� ���� ������ ����
        if (moveDir.x != 0)
        {
            moveDir.z = 0;
        }
        else if (moveDir.z != 0)
        {
            moveDir.x = 0;
        }

        // ������ �ٵ�� �̵�
        rigid.velocity = moveDir.normalized * _status.speed;
        transform.forward = moveDir;
    }

    // �浹 ����
    private void OnCollisionEnter(Collision collision)
    {
        //// ���ٱ⿡ ����� ���
        //if (collision.gameObject.name == "test")
        //{
        //    Debug.Log("����￡ ����!");

        //    _status.isBubble = true;
        //    bubble.SetActive(true);
        //}
    }

    public bool Interact()
    {
        Debug.Log("����￡ ����!");

        _status.isBubble = true;
        bubble.SetActive(true);
        _status.speed = 0.5f;

        return false;
    }
}
