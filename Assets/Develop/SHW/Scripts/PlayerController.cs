using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;       // �÷��̾� �ӵ�
    [SerializeField] float power;       // ��ź�Ŀ�
    [SerializeField] int bombCount;     // ��ź�� 

    [SerializeField] Rigidbody rigid;

    [SerializeField] Animator animator;

    private void Start()
    {

    }

    private void Update()
    {
        // �÷��̾� �������� �ϰ��
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetBoom();
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
        rigid.velocity = moveDir.normalized * speed;
        transform.forward = moveDir;
    }

    public void SetBoom()
    {
        // TODO : ��ź ��ġ
    }
}
