using UnityEngine;

public class PlayerController : MonoBehaviour, IExplosionInteractable
{
    private PlayerStatus _status;           // �÷��̾� ���� ������

    [SerializeField] Rigidbody rigid;       // �̵��� ���� rigidbody

    [SerializeField] Animator animator;     // �÷��̾� �ִϸ��̼� ����

    [SerializeField] GameObject bubble;     // ���ٱ⿡ ���� ���� �����

    // ����� �ȿ� ������ �� �ӵ� ������ ����
    private float bubbleSpeed = 0.5f;

    private void Awake()
    {
        _status = GetComponent<PlayerStatus>();
        _status.isBubble = false;
        bubble.SetActive(false);
    }

    private void Update()
    {
        // TODO : �÷��̾� ���������� ����� ���ǹ� �߰� �ʿ�
        Move();
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
        //if (moveDir.magnitude < 0.1)
        //{
        //    rigid.velocity = Vector3.zero;
        //}

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
        if (_status.isBubble == true)
        {
            rigid.velocity = moveDir.normalized * bubbleSpeed;
        }
        else
        {
            rigid.velocity = moveDir.normalized * _status.speed;
        }
        transform.forward = moveDir;
    }

    public bool Interact()
    {
        // Debug.Log("����￡ ����!");

        _status.isBubble = true;
        bubble.SetActive(true);

        return false;
    }
}
