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
    // ���� ����� 
    [SerializeField] public Color color;
    [SerializeField] Renderer bodyRenderer;

    // ���ѹ��� ����
    // -> �÷��̾ �����ؼ� ĳ���� ������ ����?
    [SerializeField] public int teamNum;

    [SerializeField] GameObject canvas;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            // Set team number as the selected team number from custom properties
            photonView.RPC(nameof(SetTeamNum), RpcTarget.AllViaServer, PhotonNetwork.LocalPlayer.GetTeam());


            canvas.SetActive(true);
        }
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            GameManager.Instance.LocalPlayerStatus = this;

            // Set color as team color
            photonView.RPC(nameof(SetColor), RpcTarget.AllViaServer);
        }
    }

    private void Update()
    {
        LimitStatus();
    }

    [PunRPC]
    private void SetTeamNum(int n) => teamNum = n;

    [PunRPC]
    public void SetColor()
    {
        // Change color as team color
        color = colors[teamNum];
        for (int i = 0; i < bodyRenderer.materials.Length; i++)
        {
            bodyRenderer.materials[i].color = color;
        }

        // Notify to GameManager
        GameManager.Instance.IncreaseTeammate(teamNum);
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