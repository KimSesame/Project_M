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

public class PlayerEntry : BaseUI
{
    [Header ("�ѱ�")]
    [SerializeField] TMP_FontAsset kFont;
    [Header("�ؽ�Ʈ����")]
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _readyText;
    [SerializeField] TMP_Text _readyPopText;
    [Header("�����ư����")]
    [SerializeField] Button _readyButton;
    [SerializeField] GameObject _readyButtonText;
    [SerializeField] GameObject _readyTextBox;

    // ������
    [Header("������")]
    public int TeamNumber; // ���⼭ �������ؼ� �÷��̾�����
    [SerializeField] GameObject[] _teamButtons;
    // �� �������
    [SerializeField] private Color[] _teamColors;           // ������ �迭
    [SerializeField] private RawImage _teamColorIndicator;  // ���õ� ������ ǥ��

    // ĳ���Ͱ���
    [Header("ĳ���� ����")]
    [SerializeField] Texture[] _charTexture;
    [SerializeField] RawImage _charRawImage;
    GameObject _playerImage;
    public int charNumber;
    // Local Player reference
    private Player _player;
    private readonly Color readyColor = new Color(1, 0.8f, 0, 1);
    private readonly Color notReadyColor = Color.white;

    private void Update()
    {   // �׽�Ʈ ������ �����
        if(Input.GetKeyDown(KeyCode.T))
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
        
        // �����ư ���� (��ŸƮ��ư��)
        _readyTextBox = GetUI("ReadyTextBox");
        _readyText = GetUI<TMP_Text>("ReadyText");
        _readyText.font = kFont;
            // �����ؽ�Ʈ�ڽ� �ؿ� �����ư (���� ����۾��� �����ϸ� ������Ǳ����ѱ���)++ó���� ���������ϴٺ����̷��Ե�
            // ���������Ϸ��ٰ� ���һ��ؼ� �ϴ� �׳� �α����.
            _readyButton = GetUI<Button>("ReadyButton");
            _readyButtonText = GetUI("ReadyButtonText");
             GetUI<TMP_Text>("ReadyButtonText").font = kFont;
            _readyButton.onClick.AddListener(Ready);
        
        // �����ϸ� �÷��̾� ���� ������ READY�ؽ�Ʈ
        _readyPopText = GetUI<TMP_Text>("ReadyPopText");
        _readyPopText.font = kFont;

        charNumber = PhotonNetwork.LocalPlayer.GetCharacter();
        // _playerImage = GetUI("PlayerImage");
        // _charRawImage = (RawImage)_playerImage.GetComponent<RawImage>();
        // _charRawImage.texture = _charTexture[charNumber];
    }

    public void SetPlayer(Player player)
    {
        // �÷��̾� ��������
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
            // ���� �ڸ��� �ִٰ� �ٽ� ���� ���� ���� �ȵǴ� �κ� ���� �ٽ� �����ϱ�.
            _nameText.color = notReadyColor;
        }
        _readyButton.gameObject.SetActive(true);
        _readyButton.interactable = player == PhotonNetwork.LocalPlayer;
        // �÷��̾ �������� Ȯ�� -> �����ư player =isLocal �� ����

        // ����ư�� Ȱ��ȭ / �ٸ�������� ��Ȱ��ȭ
        if (_readyButton.interactable)
        {
            _readyTextBox.SetActive(true);
        }
        else
        {
            _readyTextBox.SetActive(false);
            _readyPopText.text ="";
        }
        {
            UpdateReadyState();
        }
        // UpdateCharcter()�� �̿��� �÷��̾� ���� ����
        {
            UpdateCharacter(player.GetCharacter());
            UpdateTeam(player.GetTeam());
        }
       // if (charNumber != player.GetCharacter())
       // {
       //     UpdateCharacter(player.GetCharacter());
       // }
        
    }
    private void UpdateReadyState()
    {
        if (_player.GetReady())
        {
            _readyText.text = "Ready";
            _readyText.color = readyColor;
            _readyPopText.text = "READY";
        }
        else
        {
            _readyText.text = "Ready";
            _readyText.color = notReadyColor;
            _readyPopText.text = "";
        }
    }
    public void SetEmpty()
    {
        _readyPopText.text = "";
        _readyText.text = "";
        _nameText.text = "None";
        // ����ִ� �̸� ������ ������ ������� �����ϱ�
        if(_nameText != null) _nameText.color = Color.white;
        if(_readyButton != null) _readyButton.gameObject.SetActive(false);
        if(_readyTextBox != null) _readyTextBox.SetActive(false);
        // �÷��̾� ĭ�� ������� ������, �ش� ĭ�� �̹��� �����ϱ�
        if(_charRawImage != null)
        {
            _charRawImage.texture = null;
            _charRawImage.color = new Color(0, 0, 0, 0);  // 4��° ����, 0�� �־ transparency���� 0����
        }
        if (_teamColorIndicator != null) _teamColorIndicator.color = Color.clear;
        
    }

    public void Ready()
    {
        // !���� -> ���� || ���� -> !���� 
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        ready = !ready;

        PhotonNetwork.LocalPlayer.SetReady(ready);
        UpdateReadyState();
        if (ready)
        {
            // PhotonNetwork.LocalPlayer.SetReady(true);
            // _readyText.text = "Ready";
            Debug.Log($"�غ����: {ready}");
            _readyButtonText.SetActive(false);
        }
        else
        {
            // PhotonNetwork.LocalPlayer.SetReady(false);
            // _readyText.text = "";
            _readyButtonText.SetActive(true);
        }
    }

    /// <summary>
    /// ĳ���� ���� ���� ������Ʈ
    /// </summary>
    /// <param name="characterID"></param>
    public void UpdateCharacter(int characterID)
    {
        if (_charRawImage == null || _charTexture == null)
        {
            Debug.LogWarning("ĳ���� �̹����� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }
        if (characterID >= 0 && characterID < _charTexture.Length)
        {
            _charRawImage.texture = _charTexture[characterID];
            // ���� �ʱ�ȭ�ϱ� (���İ� 1�� ����)
            _charRawImage.color = new Color(1, 1, 1, 1); // ���, ������
            Debug.Log($"������Ʈ�� ĳ���� �̹���: {_charRawImage.texture.name}, Character ID: {characterID}");
        }
        else 
        {
            Debug.LogWarning($"������ characterID: {characterID}, ������Ʈ�� �ȵƽ��ϴ�.");
        }

    }


    /// <summary>
    /// ������ ������Ʈ
    /// </summary>
    /// <param name="teamNumber">����ȣ</param>
    public void UpdateTeam(int teamNumber)
    {
        if (teamNumber >= 0 && teamNumber < _teamColors.Length)
        {
            _teamColorIndicator.color = _teamColors[teamNumber];
        }
    }
    public void OnTeamColorSelected(int teamNumber)
    {
        PhotonNetwork.LocalPlayer.SetTeam(teamNumber);
        UpdateTeam(teamNumber);
        Debug.Log($"�÷��̾� {_player.NickName}, ����ȣ: {teamNumber}, ����: {teamNumber}");
    }
}
