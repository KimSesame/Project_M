using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviourPun, IExplosionInteractable
{
    private PlayerStatus _status;           // �÷��̾� ���� ������

    [SerializeField] Rigidbody rigid;       // �̵��� ���� rigidbody

    [SerializeField] Animator animator;     // �÷��̾� �ִϸ��̼� ����

    [SerializeField] GameObject bubble;     // ���ٱ⿡ ���� ���� �����

    [SerializeField] GameObject arrow;      // �÷��̾ ����ų ȭ��ǥ

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
    }
    private void Start()
    {
        // ���� ����
        photonView.RPC("SetColor", RpcTarget.All);
        GameManager.Instance.IncreaseTeammate(_status.teamNum);
    }

    private void Update()
    {
        if (photonView.IsMine == false)
        {
            arrow.SetActive(false);
            return;
        }

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
        for (int i = 0; i < bodyRenderer.materials.Length; i++)
        {
            // �÷��̾� teamNum ������ �����ٸ� num2 �ڸ��� _status.teamNum�� ������ �˴ϴ�.
            // Debug.Log($"�Ͽ�) TeamNum {_status.teamNum}");
            bodyRenderer.materials[i].color = _status.colors[_status.teamNum];
            // �� �ν��� ���� ĳ���� ���� ����
            color = _status.colors[_status.teamNum];
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

            if (dotProduct > 0.5f) // ���� ���絵�� ���� ��� (1�� �������� ������ ��ġ)
            {
                // ������Ʈ �б�
                pushable.Push(photonView, moveDirection);
            }
            else
            {
                Debug.Log($"������ ��ġ���� �ʾ� ������Ʈ�� �� �� �����ϴ�.  {dotProduct}");
            }
        }
    }
}

