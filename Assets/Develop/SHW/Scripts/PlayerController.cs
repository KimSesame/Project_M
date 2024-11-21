using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerStatus _status;

    [SerializeField] Rigidbody rigid;

    [SerializeField] Animator animator;

    private void Awake()
    {
        _status = GetComponent<PlayerStatus>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        // �÷��̾� �������� �ϰ��
        Move();

        // ��ź ��ġ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetBoom();
        }

        // (�׽�Ʈ) �÷��̾� ��ǳ���� ����
        if(Input.GetKeyDown(KeyCode.Keypad0))
        {
            BindBubble();
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

    public void SetBoom()
    {
        // TODO : ��ź ��ġ
    }

    // ��ǳ���� ������ ���
    public void BindBubble()
    {

    }

    // ǳ���� �浹�� �����Ͽ� ��ǳ���� ������ �Ѵ�.
    private void OnCollisionEnter(Collision collision)
    {
        BindBubble();
    }
}
