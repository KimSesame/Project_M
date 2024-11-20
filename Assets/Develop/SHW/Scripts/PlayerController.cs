using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;       // �÷��̾� �ӵ�
    [SerializeField] float power;       // ��ź�Ŀ�
    [SerializeField] int bombCount;     // ��ź�� 

    [SerializeField] Animator animator;

    private void Start()
    {

    }

    private void Update()
    {
        // �÷��̾� �������� �ϰ��
        Move();
        // TODO : ��ź ��ġ
    }

    public void Move()
    {
        Vector3 moveDir = new Vector3();
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");

        if (moveDir.x != 0 || moveDir.z != 0)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }

        // �������� �ʾ��� �� 
        if (moveDir == Vector3.zero)
            return;


        // ���� �Է� �� �ٸ� ���� ������ ����
        if(moveDir.x !=0)
        {
            moveDir.z = 0;
        }
        else if(moveDir.z != 0) 
        {
            moveDir.x = 0;
        }

        transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);
        transform.forward = moveDir.normalized;
    }
}
