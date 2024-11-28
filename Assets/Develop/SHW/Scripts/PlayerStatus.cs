using UnityEngine;


public class PlayerStatus : MonoBehaviour
{
    [SerializeField] public float speed;       // �÷��̾� �ӵ�
    [SerializeField] public int power;         // ��ź�Ŀ�
    [SerializeField] public int bombCount;     // ��ź�� 

    public bool isBubble;                      // ����￡ ���� ����

    // �÷��̾� ������ ����
    [SerializeField] public Color[] colors;
    // ���ѹ��� ����
    // -> �÷��̾ �����ؼ� ĳ���� ������ ����?
    [SerializeField] public int teamNum;


    private void Awake()
    {
    }

    private void Update()
    {
        LimitSpeed();
    }

    public void LimitSpeed()
    {
        if(speed > 10)
        {
            speed = 10;
        }
    }

}
