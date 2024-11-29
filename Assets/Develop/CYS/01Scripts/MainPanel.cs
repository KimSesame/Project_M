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
        _lobbyPanel.SetActive(true); 

    }

    private void Init()
    {
        _lobbyPanel = GetUI("LobbyPanel");
        _logOutButton = GetUI<Button>("LogOutButton");
        _logOutButton = GetUI<Button>("LogOutButton");
        _logOutButton.onClick.AddListener(LogOut);

        // TestLog();

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

   // private void TestLog()
   // {
   //     FirebaseUser user = BackendManager.Auth.CurrentUser;
   //     if (user == null)
   //     {
   //         Debug.Log("�÷��̾ �α����� �ùٸ����ʽ��ϴ�.");
   //         return;
   //     }
   //     Debug.Log("���г� �׽�Ʈ�α�");
   //     Debug.Log($"Display Name: \t {user.DisplayName}");
   //     Debug.Log($"Email Address: \t {user.Email}");
   //     Debug.Log($"Email Verification: \t {user.IsEmailVerified}");
   //     Debug.Log($"User ID: \t\t {user.UserId}");
   //     Debug.Log("");
   // }

}
