using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Bubble : MonoBehaviourPun
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject bubble;     

    private PlayerStatus _status;           // �÷��̾� ����
    private Animator _animator;             // �ִϸ��̼� ���
    private WaterBombPlacer _placer;        // ��ǳ�� ��ġ

    private void Awake()
    {
        _status = player.GetComponent<PlayerStatus>();
        _animator = player.GetComponent<Animator>();
        _placer = player.GetComponent<WaterBombPlacer>();
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.BOMB_LOCKED);

        _animator.SetBool("isBubble", true);
        _placer.enabled = false;
        // n�� �� �ڵ� ���
        StartCoroutine(BubbleRoutine());
    }

    IEnumerator BubbleRoutine()
    {
        // 5�� �ڿ� ������ ������ �ۼ�
        yield return new WaitForSeconds(5f);
        // ĳ���� ���
        Dead();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹ü �÷��̾��� ���
        if (other.gameObject.layer == 3)
        {
            if (other.gameObject.GetComponent<PlayerStatus>().isBubble == true)
            {
                Debug.Log("���� ���� �÷��̾� �浹");
                return;
            }

            // �浹ü�� �÷��̾��� ���� �Ǵ�
            Color otherColor = other.gameObject.GetComponent<PlayerStatus>().color;
            Color playerColor = player.GetComponent<PlayerStatus>().color;

            // ���� ����� ��ġ�� ���
            if (playerColor == otherColor)
            {
                Debug.Log("���� �� �浹 Ȯ��");
                Save();
            }
            // ���� ����� ��ġ�� ���
            if (playerColor != otherColor)
            {
                Dead();
            }
        }
    }

    [PunRPC]
    public void Save()
    {
        StopAllCoroutines();
        bubble.SetActive(false);
        _animator.SetBool("isBubble", false);
        _status.isBubble = false;
        _placer.enabled = true;
    }

    public void Dead()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.BOMB_DEAD);
        GameManager.Instance.DecreaseTeammate(_status.teamNum);
        bubble.SetActive(false);
        _animator.SetBool("isDead", true);
        Destroy(player, 1f);
    }
}
