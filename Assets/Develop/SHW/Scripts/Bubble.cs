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
        // n초 후 물방울 비활성화
        StartCoroutine(BubbleRoutine());
    }

    IEnumerator BubbleRoutine()
    {
        // Debug.Log("코루틴 시작");
        // (임시) 5초 뒤에 터지는 것으로 작성
        yield return new WaitForSeconds(5f);
        bubble.SetActive(false);
        _animator.SetBool("isDead",true);
        // 캐릭터 사망
        Destroy(player, 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 팀이 방울을 터치할 경우
        if (other.gameObject.name == "testTeam")
        {
            Debug.Log("같은 팀 충돌 확인");
            StopAllCoroutines();
            bubble.SetActive(false);
            _animator.SetBool("isBubble", false);
            _status.isBubble = false;
        }
        // 적이 방울을 터치할 경우
        if (other.gameObject.name == "testEnemy")
        {
            bubble.SetActive(false);
            //_animator.SetBool("isBubble", false);
            _animator.SetBool("isDead", true);
            Destroy(player,1f);
        }
    }
}
