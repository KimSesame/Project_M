using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class PlayerController : MonoBehaviourPun, IExplosionInteractable
{
    private PlayerStatus _status;           // �÷��̾� ���� ������

    [SerializeField] Rigidbody rigid;       // �̵��� ���� rigidbody

    [SerializeField] Animator animator;     // �÷��̾� �ִϸ��̼� ����

    [SerializeField] GameObject bubble;     // ���ٱ⿡ ���� ���� �����

    [SerializeField] int playerNumber;

    // ����� �ȿ� ������ �� �ӵ� ������ ����
    private float bubbleSpeed = 0.5f;

    // ���� ����� 
    [SerializeField] Color color;
    [SerializeField] Renderer bodyRenderer;

    private void Awake()
    {
        playerNumber = PhotonNetwork.LocalPlayer.ActorNumber - 1;
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

        // Debug.Log($"{moveDir}");
        // �Է��� ��� ������ ����
        if (moveDir.magnitude > 0.1)
        {
            transform.forward = moveDir;
        }
    }

    [PunRPC]
    public void SetColor()
    {
        // �÷��̾� �ѹ��� ���� ���� ���� �ο�
        int num = photonView.Owner.GetPlayerNumber();
        //color = _status.colors[num];

        for (int i = 0; i < bodyRenderer.materials.Length; i++)
        {
            bodyRenderer.materials[i].color = _status.colors[num];
        }
    }

    public bool Interact()
    {
        // Debug.Log("����￡ ����!");

        //_status.isBubble = true;
        //bubble.SetActive(true);

        photonView.RPC("BubbledRPC", RpcTarget.All);

        return true;
    }

    [PunRPC]
    public void BubbledRPC()
    {
        _status.isBubble = true;
        bubble.SetActive(true);
    }
}
