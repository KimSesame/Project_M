using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
//using System.Drawing;
using TMPro;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerEntry : BaseUI
{
    // [SerializeField] GameObject _playerImage; ("PlayerImage") ���߿� 
    [SerializeField] TMP_Text _nameText;
    [SerializeField] TMP_Text _readyText;
    [SerializeField] Button _readyButton;

    private void OnEnable()
    {
        Init();        
    }
    private void Init()
    {
      //  _nameText = GetUI<TMP_Text>("PlayerNameText");
      //  _readyText = GetUI<TMP_Text>("ReadyText");
      //  _readyButton = GetUI<Button>("ReadyButton");

    }
    public void SetPlayer(Player player)
    {
        if(player.IsMasterClient)
        {
            _nameText.text = $"MASTER\n{player.NickName}";
            // �ϴ� "MASTER" �۾�, ���� �̹�������� �ǳ��� ����
        }
        else
        {
            _nameText.text = player.NickName;
        }
        _readyButton.gameObject.SetActive(true);
        _readyButton.interactable = player == PhotonNetwork.LocalPlayer;
        // �÷��̾ �������� Ȯ�� -> �����ư player =isLocal �� ����

        if (player.GetReady())
        {
            _readyText.text = "Ready";
            // _readyButton.transition.;
        }
        else
        {
            _readyText.text = "";
        }
    }

    public void SetEmpty()
    {
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
        }
        else
        {
            PhotonNetwork.LocalPlayer.SetReady(false);
            _readyText.text = "";
        }
    }

}
