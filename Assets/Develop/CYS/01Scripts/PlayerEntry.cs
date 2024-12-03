using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
//using System.Drawing;
using TMPro;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.EventSystems;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerEntry : BaseUI
{
    [Header("�ѱ�")]
    [SerializeField] TMP_FontAsset kFont;
    [Header("�ؽ�Ʈ����")]
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _readyPopText;
    [Header("�����ư����")]

    // ������
    [Header("������")]
    public int TeamNumber; // ���⼭ �������ؼ� �÷��̾�����
    // KMS ���� ���� �迭 �� ǥ�� �̹���.
    [SerializeField] public Color[] _teamColors;           // �� ���� �迭
    [SerializeField] public RawImage _teamColorIndicator;  // ���õ� �� ���� ǥ��
    

    // ĳ���Ͱ���
    [Header("ĳ���� ����")]
    [SerializeField] Texture[] _charTexture;
    [SerializeField] RawImage _charRawImage;
    GameObject _playerImage;


    // KMS Local Player�� ���۷���
    private Player _player;
    private readonly Color readyColor = new Color(1, 0.8f, 0, 1);
    private readonly Color notReadyColor = Color.white;

    private void Update()
    {   // �׽�Ʈ ������ �����
        if (Input.GetKeyDown(KeyCode.T))
        {

            Debug.Log($"�����Ͻ� ����ȣ: {PhotonNetwork.LocalPlayer} from PlayerEntryScript");
        }
    }

    private void OnEnable()
    {
        Init();
    }
    private void Init()
    {
        // �÷��̾� �̸�
        _nameText = GetUI<TMP_Text>("PlayerNameText");
        _nameText.font = kFont;
        _nameText.fontSizeMin = 14;
        _nameText.fontSize = 22;
        _nameText.fontSizeMax = 50;
        _teamColorIndicator = GetUI<RawImage>("TeamColorBox");

        // �����ϸ� �÷��̾� ���� ������ READY�ؽ�Ʈ
        _readyPopText = GetUI<TMP_Text>("ReadyPopText");
        _readyPopText.font = kFont;

    }

    public void SetPlayer(Player player)
    {
        // KMS �÷��̾� ��������.
        _player = player;

        if (player.IsMasterClient)
        {

            _nameText.text = $"����\n{player.NickName}";
            _nameText.color = new Color(1, .8f, 0, 1);
            // �ϴ� "MASTER" �۾�, ���� �̹�������� �ǳ��� ����
        }
        else
        {
            _nameText.text = player.NickName;

            // KMS ���� �ڸ��� �ִٰ� �ٽ� ���� ���� ������ �ȵǾ��� �κ� ����.
            _nameText.color = Color.white;
        }

        {
            UpdateReadyState();
        }

        {
            UpdateCharacter(player.GetCharacter());
            UpdateTeam(player.GetTeam());
        }
    }

    /// <summary>
    /// KMS �غ� ���� ������Ʈ
    /// </summary>
    public void UpdateReadyState()
    {
        if (_player.GetReady())
        {

            _readyPopText.text = "READY";
        }
        else
        {

            _readyPopText.text = "";
        }
    }

    /// <summary>
    /// KMS ĳ���� ���� ���� ������Ʈ
    /// </summary>
    public void UpdateCharacter(int characterId)
    {
        if (_charRawImage == null || _charTexture == null)
        {
            Debug.LogWarning("ĳ���� �̹����� �Ҵ���� ����");
            return;
        }

        if (characterId >= 0 && characterId < _charTexture.Length)
        {
            _charRawImage.texture = _charTexture[characterId];

            // ���� �ʱ�ȭ (���� ���� 1�� ����)
             _charRawImage.color = new Color(1, 1, 1, 1); // ���, ������

            Debug.Log($"������Ʈ �� ĳ���� �̹���: {_charRawImage.texture.name}, Character ID: {characterId}");
        }
        else
        {
            Debug.LogWarning($"������ character ID: {characterId}. ������Ʈ�� �ȵ�.");
        }
    }

    /// <summary>
    /// KMS �� ���� ������Ʈ
    /// </summary>
    /// <param name="teamNumber">�� ��ȣ</param>
    public void UpdateTeam(int teamNumber)
    {
        if (teamNumber >= 0 && teamNumber < _teamColors.Length)
        {
            _teamColorIndicator.color = _teamColors[teamNumber];
        }
    }

    public void SetEmpty()
    {
        _readyPopText.text = "";
        _nameText.text = "����";
        // KMS ����ִ� �̸� ������ ������ ������� ����.
        if (_nameText != null) _nameText.color = Color.white;


        // KMS �÷��̾� ĭ�� ������� �Ǿ����� �ش� ĭ�� �̹��� ����.
        if (_charRawImage != null)
        {
            _charRawImage.texture = null;
            _charRawImage.color = new Color(0, 0, 0, 0); // ������ ����
        }
        if (_teamColorIndicator != null) _teamColorIndicator.color = new Color(0, .61f, 1f, 1f);
    }

    public void Ready()
    {
        // !���� -> ���� || ���� -> !���� 
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        ready = !ready;

        PhotonNetwork.LocalPlayer.SetReady(ready);
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        // KMS ���� �κа���.
        {
            UpdateReadyState();

            if (ready)
            {
                Debug.Log($"�غ����: {ready}");
            }
        }
    }


    /// <summary>
    /// KMS �� ���� ���� ó�� ��ư(������ ��ư���� ȣ�� ���ּ���.)
    /// </summary>
    /// <param name="teamNumber">������ �� ��ȣ</param>
    private void OnTeamColorSelected(int teamNumber)
    {
        PhotonNetwork.LocalPlayer.SetTeam(teamNumber);

        UpdateTeam(teamNumber);

        Debug.Log($"�÷��̾� {_player.NickName} �� ��ȣ: {teamNumber}, ����: {_teamColors[teamNumber]}");
    }
}
