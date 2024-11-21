using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject bubble;

    private PlayerStatus _status;

    private void Awake()
    {
        _status = player.GetComponent<PlayerStatus>();
        // n�� �� ����� ��Ȱ��ȭ
        StartCoroutine(BubbleRoutine());
    }

    IEnumerator BubbleRoutine()
    {
        Debug.Log("�ڷ�ƾ ����");
        // (�ӽ�) 3�� �ڿ� ������ ������ �ۼ�
        yield return new WaitForSeconds(5f);
        bubble.SetActive(false);
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
            _status.isBubble = false;
        }
        // ���� ����� ��ġ�� ���
        if (other.gameObject.name == "testEnemy")
        {
            bubble.SetActive(false);
            Destroy(player,1f);
        }
    }
}
