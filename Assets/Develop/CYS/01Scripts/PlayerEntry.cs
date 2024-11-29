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
    [SerializeField] TMP_FontAsset kFont;
    // [SerializeField] GameObject _playerImage; ("PlayerImage") ���߿� 
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _readyText;
    [SerializeField] Button _readyButton;
    [SerializeField] GameObject _readyButtonText;
    [SerializeField] GameObject _readyTextBox;
    [SerializeField] TMP_Text _readyPopText;

    // ������
    public int TeamNumber; // ���⼭ �������ؼ� �÷��̾�����
    [SerializeField] GameObject _teamChoicePanel;
    [SerializeField] GameObject[] _teamButtons;

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
        _nameText = GetUI<TMP_Text>("PlayerNameText");
        _nameText.font = kFont;
        _readyText = GetUI<TMP_Text>("ReadyText");
        _readyText.font = kFont;
        _readyButton = GetUI<Button>("ReadyButton");
        _readyButtonText = GetUI("ReadyButtonText");
        GetUI<TMP_Text>("ReadyButtonText").font = kFont;
        _readyButton.onClick.AddListener(Ready);
        _readyTextBox = GetUI("ReadyTextBox");
        _readyPopText = GetUI<TMP_Text>("ReadyPopText");
        _readyPopText.font = kFont;
        GetUI<TMP_Text>("ReadyButtonText").font = kFont;

    }

    public void SetPlayer(Player player)
    {
        if(player.IsMasterClient)
        {
            
            _nameText.text = $"����\n{player.NickName}";
            _nameText.color = new Color(1, .8f, 0, 1);
            // �ϴ� "MASTER" �۾�, ���� �̹�������� �ǳ��� ����
        }
        else
        {
            _nameText.text = player.NickName;
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

        if (player.GetReady())
        {
            _readyText.text = "Ready";
            _readyText.color = new Color(1, .8f, 0, 1);
            // _readyButton.transition.;
            _readyPopText.text = "READY";
        }
        else
        {
            _readyText.text = "Ready";
            _readyText.color = Color.white;
            _readyPopText.text = "";
        }
   
    }
  
    public void SetEmpty()
    {
        _readyPopText.text = "";
        _readyText.text = "";
        _nameText.text = "None";
        _readyButton.gameObject.SetActive(false);
        
    }



    public void Ready()
    {
        // !���� -> ���� || ���� -> !���� 
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        ready = !ready;

        PhotonNetwork.LocalPlayer.SetReady(ready);
        if (ready)
        {
            PhotonNetwork.LocalPlayer.SetReady(true);
            _readyText.text = "Ready";
            Debug.Log($"�غ����: {ready}");
            _readyButtonText.SetActive(false);
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetReady(false);
            _readyText.text = "";
            _readyButtonText.SetActive(true);
        }
    }

}
