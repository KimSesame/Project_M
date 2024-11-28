using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using static Photon.Pun.UtilityScripts.PunTeams;


public class PlayerStatus : MonoBehaviourPun
{
    [SerializeField] public float speed;       // �÷��̾� �ӵ�
    [SerializeField] public int power;         // ��ź�Ŀ�
    [SerializeField] public int bombCount;     // ��ź�� 
    [Header("MaxStatus")]
    [SerializeField] public float maxSpeed;       // �÷��̾� �ӵ�
    [SerializeField] public int maxPower;         // ��ź�Ŀ�
    [SerializeField] public int maxBombCount;     // ��ź�� 

    public bool isBubble;                      // ����￡ ���� ����

    // �÷��̾� ������ ����
    [SerializeField] public Color[] colors;
    // ���ѹ��� ����
    // -> �÷��̾ �����ؼ� ĳ���� ������ ����?
    [SerializeField] public int teamNum;

    private void Awake( )
    {
        // �� �������� ��ȣ�� �Ű����� �ּ� Ǯ� �� ����
        // PhotonNetwork.LocalPlayer.GetTeam(out teamNum);
    }

    private void Update()
    {
        LimitStatus();
    }

    public void LimitStatus()
    {
        // �ӵ�����
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        // ��ǳ�� �Ŀ� ����
        if (power > maxPower)
        {
            power = maxPower;
        }
        // ��ǳ�� ���� ���� 
        if (bombCount > maxBombCount)
        {
            bombCount = maxBombCount;
        }
    }

}
