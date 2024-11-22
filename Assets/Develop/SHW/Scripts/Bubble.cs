using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject bubble;

    private PlayerStatus _status;
    private Animator _animator;

    private void Awake()
    {
        _status = player.GetComponent<PlayerStatus>();
        _animator = player.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _animator.SetBool("isBubble", true);
        // n�� �� ����� ��Ȱ��ȭ
        StartCoroutine(BubbleRoutine());
    }

    IEnumerator BubbleRoutine()
    {
        // Debug.Log("�ڷ�ƾ ����");
        // (�ӽ�) 5�� �ڿ� ������ ������ �ۼ�
        yield return new WaitForSeconds(5f);
        bubble.SetActive(false);
        _animator.SetBool("isDead",true);
        // ĳ���� ���
        Destroy(player, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� ����� ��ġ�� ���
        if (other.gameObject.name == "testTeam")
        {
            Debug.Log("���� �� �浹 Ȯ��");
            StopAllCoroutines();
            bubble.SetActive(false);
            _animator.SetBool("isBubble", false);
            _status.isBubble = false;
        }
        // ���� ����� ��ġ�� ���
        if (other.gameObject.name == "testEnemy")
        {
            bubble.SetActive(false);
            //_animator.SetBool("isBubble", false);
            _animator.SetBool("isDead", true);
            Destroy(player,1f);
        }
    }
}
