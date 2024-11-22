using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using Photon.Realtime;

public class MainPanel : BaseUI
{
    /* Objects in UI
     * + UserInfoDropdown (ĳ��������â / dropdownButton) ��ÿ��� ��ư������ �ö󰡱�
     *  - UserInfoLabel
     *  - UserInfoArrow (downArrow Image)
     *  - UserInfoTemplate
     *      - UserInfoViewport
     *          - UserInfoContent
     *              - UserInfoList
     *                  - UserInfoListBackground   // ����� ���ټ���??
     *                  - UserInfoListCheckmark
     *                  - UserInfoListLabel
     *
     * + LobbyPanel // ��� ���� ��ũ��Ʈ �� �ٿ��� �
     * 
     * + LogOutButton
     *  - LogOutText
     * 
     */
    [SerializeField] TMP_Dropdown _userInfoDropdown;
    // TODO : ���� �����ͺ��̽� �������� ���� �� �������� �ǳ��غ��� �����ϱ�.

    [SerializeField] GameObject _lobbyPanel;

    [SerializeField] Button _logOutButton;
    [SerializeField] TMP_Text _logOutText;
    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        _lobbyPanel = GetUI("LobbyPanel");
        _logOutButton = GetUI<Button>("LogOutButton");
        _logOutText = GetUI<TMP_Text>("LogOutText");
        JoinLobby();
    }

    public void JoinLobby()
    {
        // �����ϸ鼭 �׳� �κ�â ������
        Debug.Log("�κ����� �׽�Ʈ �α�");
        _lobbyPanel.SetActive(true);
        PhotonNetwork.JoinLobby();
    }
    public void LogOut()
    {
        Debug.Log("�α׾ƿ� �׽�Ʈ �α�");
        PhotonNetwork.Disconnect();
    }

}
