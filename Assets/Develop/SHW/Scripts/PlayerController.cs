using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;       // �÷��̾� �ӵ�
    [SerializeField] float power;       // ��ź�Ŀ�
    [SerializeField] int bombCount;     // ��ź�� 

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

        // �������� �ʾ��� �� 
        if (moveDir == Vector3.zero)
            return;

        // �밢 �Է� �� 
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))
            {
                // TODO : ���� �Է½� ���� �̵��� ����. ���� �̵��ϴ� �������� ��� �̵��ϴ� ������ 
                return;
            }
        }

        transform.Translate(moveDir.normalized * speed * Time.deltaTime, Space.World);
        transform.forward = moveDir.normalized;
    }
}
