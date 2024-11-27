using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviourPun, IExplosionInteractable
{
    private PlayerStatus _status;           // �÷��̾� ���� ������

    [SerializeField] Rigidbody rigid;       // �̵��� ���� rigidbody

    [SerializeField] Animator animator;     // �÷��̾� �ִϸ��̼� ����

    [SerializeField] GameObject bubble;     // ���ٱ⿡ ���� ���� �����

    // ����� �ȿ� ������ �� �ӵ� ������ ����
    private float bubbleSpeed = 0.5f;

    // ���� ����� 
    [SerializeField] public Color color;
    [SerializeField] Renderer bodyRenderer;

    private void Awake()
    {
        _status = GetComponent<PlayerStatus>();
        _status.isBubble = false;
        bubble.SetActive(false);

        // SetColor();
        photonView.RPC("SetColor", RpcTarget.All);
    }

    private void Update()
    {
        if (photonView.IsMine == false)
            return;

        Move();
    }

    public void Move()
    {
        Vector3 moveDir = new Vector3();
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.z = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveDir.x, 0, moveDir.z).normalized;

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
        if (_status.isBubble == true)
        {
            rigid.velocity = moveDir.normalized * bubbleSpeed;
        }
        else
        {
            rigid.velocity = moveDir.normalized * _status.speed;
        }

        // �Է��� ��� ������ ����
        if (moveDir.magnitude > 0.1)
        {
            transform.forward = moveDir;
        }
    }

    [PunRPC]
    public void SetColor()
    {
        // (�ٸ��� ����)
        int num = photonView.Owner.GetPlayerNumber();
        // (�ӽ�) ������ ����
        int num2 = num % 2;

        for (int i = 0; i < bodyRenderer.materials.Length; i++)
        {
            bodyRenderer.materials[i].color = _status.colors[num];
            color = _status.colors[num];
            //bodyRenderer.materials[i].color = color;
        }

    }
    public bool Interact()
    {
        photonView.RPC("BubbledRPC", RpcTarget.All);

        return true;
    }

    [PunRPC]
    public void BubbledRPC()
    {
        _status.isBubble = true;
        bubble.SetActive(true);
    }

    private Vector3 moveDirection;
    private void OnCollisionStay(Collision collision)
    {
        if (!photonView.IsMine) return;

        // �б� ������ ������Ʈ�� �浹 ��
        PushableObject pushable = collision.gameObject.GetComponent<PushableObject>();
        if (pushable != null)
        {
            // ���� + �÷��̾� ���� =  ������
            // ������ = �������
            // ���� + �÷��̾� ���� x = �������

            // �浹 ������ �浹 ���� ���
            Vector3 collisionDirection = (collision.transform.position - transform.position).normalized;
            
            // �̵� ����� �浹 ������ ��������.
            float dotProduct = Vector3.Dot(moveDirection, collisionDirection);

            // �÷��̾��� �̵� ����� �ӵ�
            Vector3 playerVelocity = rigid.velocity;
            float velocityMagnitude = playerVelocity.magnitude; // �̵� �ӵ��� ũ�� (���� ���)

            // TODO: ���� �°� �����̱�.

            if (dotProduct > 0.5f /*&& velocityMagnitude >= 0.5f*/) // ���� ���絵�� ���� ��� (1�� �������� ������ ��ġ)
            {
                // ������Ʈ �б�
                pushable.Push(moveDirection);
                Debug.Log($"playerVelocity  {playerVelocity.magnitude} ");
            }
            else
            {
                Debug.Log($"������ ��ġ���� �ʾ� ������Ʈ�� �� �� �����ϴ�.  {dotProduct},  {playerVelocity.magnitude}");
            }
        }
    }
}

