using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using Photon.Realtime;

public class LobbyPanel : BaseUI
{
    [SerializeField] TMP_FontAsset kFont;

    /*  LobbyPanel Objects List:
     *  + LobbyPanel (GameObject)
     *      - CreateRoomButton <Button>
     *          - CreateRoomText <TMP_Text>
     *      - QuickStartButton <Button>
     *          - QuickStartText <TMP_Text>
     *      - RoomListPanel    <Button>
     *          - RoomListViewport
     *              - RoomListContent
     *          - RoomListScrollbar
     *              - RLSBSlidingArea
     *                  - RLSBSAHandle
     *      
     */
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
    // [SerializeField] TMP_Dropdown _userInfoDropdown;
    // TODO : ���� �����ͺ��̽� �������� ���� �� �������� �ǳ��غ��� �����ϱ�.

    // mainPanel

    // [SerializeField] GameObject _lobbyPanel;
    [SerializeField] GameObject _mainPanel;  //�ڿ� �׳� ��溮��
    [SerializeField] Button _logOutButton;
    [SerializeField] TMP_Text _logOutText;
    // -----------------------------------------------



    [SerializeField] GameObject _lobbyPanel;
    [SerializeField] Button _createRoomButton;
    [SerializeField] TMP_Text _createRoomText;

    [SerializeField] Button _quickStartButton;
    [SerializeField] TMP_Text _quickStartText;

    [SerializeField] GameObject _roomListPanel;
    [SerializeField] RectTransform _roomListContent;
    [SerializeField] RoomEntry _roomEntryPrefab;

    [SerializeField] GameObject _createRoomPanel;
    [SerializeField] TMP_InputField _roomNameInputField;
    [SerializeField] TMP_InputField _maxPlayerInputField;

    public int InputSelected;

    private bool _isMakingRoom = false;

    private Dictionary<string, RoomEntry> roomDictionary = new Dictionary<string, RoomEntry>();

    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        // from mainPanel

        _mainPanel = GetUI("MainPanel");

        // LogOutButton
        _logOutButton = GetUI<Button>("LogOutButton");
        _logOutButton.onClick.AddListener(LogOut);
        _logOutText = GetUI<TMP_Text>("LogOutText");
        _logOutText.font = kFont;
        _logOutText.fontSizeMin = 14;
        _logOutText.fontSize = 36;
        _logOutText.fontSizeMax = 72;


        _lobbyPanel = GetUI("LobbyPanel");
        // �游��� ��ư
        _createRoomButton = GetUI<Button>("CreateRoomButton");
        _createRoomButton.onClick.AddListener(CreateRoomMenu);
        _createRoomText = GetUI<TMP_Text>("CreateRoomText");
        _createRoomText.fontSizeMin = 14;
        _createRoomText.fontSize = 36;
        _createRoomText.fontSizeMax = 72;
        _createRoomText.text = "�游���";
        _createRoomText.font = kFont;

        // �������۹�ư
        _quickStartButton = GetUI<Button>("QuickStartButton");
        _quickStartButton.onClick.AddListener(RandomMatching); 
        _quickStartText = GetUI<TMP_Text>("QuickStartText");
        _quickStartText.fontSizeMin = 14;
        _quickStartText.fontSize = 36;
        _quickStartText.fontSizeMax = 72;
        _quickStartText.text = "��������";
        _quickStartText.font = kFont;

        _roomListPanel = GetUI("RoomListPanel");
        _roomListContent = GetUI<RectTransform>("RoomListContent");

        // �游����г�
        _createRoomPanel = GetUI("CreateRoomPanel");
        // ���̸�_Text
        GetUI<TMP_Text>("RoomName");
        GetUI<TMP_Text>("RoomName").fontSizeMin = 14;
        GetUI<TMP_Text>("RoomName").fontSize = 36;
        GetUI<TMP_Text>("RoomName").fontSizeMax = 72;
        GetUI<TMP_Text>("RoomName").text = "���̸�";
        // �ִ��ο�_Text
        GetUI<TMP_Text>("CapText");
        GetUI<TMP_Text>("CapText").fontSizeMin = 14;
        GetUI<TMP_Text>("CapText").fontSize = 36;
        GetUI<TMP_Text>("CapText").fontSizeMax = 72;
        GetUI<TMP_Text>("CapText").text = "�ִ��ο�";
        // ���̸�����_IputField
        _roomNameInputField = GetUI<TMP_InputField>("RoomNameInputField");
        GetUI<TMP_Text>("RoomNamePlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("RoomNamePlaceholder").fontSize = 22;
        GetUI<TMP_Text>("RoomNamePlaceholder").fontSizeMax = 58;
        GetUI<TMP_Text>("RoomNamePlaceholder").text = "���̸��� �����ּ���";
        // ���̸�����_IputFieldText
        GetUI<TMP_Text>("RoomNameText").fontSizeMin = 14;
        GetUI<TMP_Text>("RoomNameText").fontSize = 22;
        GetUI<TMP_Text>("RoomNameText").fontSizeMax = 58;
        // �ִ��ο�����_IputField
        _maxPlayerInputField = GetUI<TMP_InputField>("MaxPlayerInputField");
        GetUI<TMP_Text>("MaxPlayerPlaceholder");
        GetUI<TMP_Text>("MaxPlayerPlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("MaxPlayerPlaceholder").fontSize = 22;
        GetUI<TMP_Text>("MaxPlayerPlaceholder").fontSizeMax = 58;
        GetUI<TMP_Text>("MaxPlayerPlaceholder").text = "2~8";
        // �ִ��ο�����_IputFieldText
        GetUI<TMP_Text>("MaxPlayerText").fontSizeMin = 14;
        GetUI<TMP_Text>("MaxPlayerText").fontSize = 22;
        GetUI<TMP_Text>("MaxPlayerText").fontSizeMax = 58;




        GetUI<Button>("CreateRoomtButton").onClick.AddListener(CreateRoomConfirm);
        GetUI<Button>("CreateRoomCancelButton").onClick.AddListener(CreateRoomCancel);


        GetUI<Button>("ExitButton").onClick.AddListener(QuitGame);
        // UpdateRoomList();
        // TestLog();
    }
    private void Update()
    {
        if (_isMakingRoom)
        {
            TabInputField();
        }
    }
    /// <summary>
    /// TabInputField
    /// Int ������ InputField �ϳ��������ؼ� ��Ű ������ ++ �ǰ�
    /// �ִ� ��ġ�� �Ѿ�� ó������ ���ư�����
    /// </summary>
    public void TabInputField()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            InputSelected++;
            if (InputSelected > 1)
                InputSelected = 0;
            SelectInputField();
        }
    }
    /// <summary>
    /// ���콺�� Ŭ���ؼ� �ϸ� �����ִ�
    /// </summary>
    public void SelectInputField()
    {
        switch (InputSelected)
        {
            case 0:
                _roomNameInputField.Select();
                break;
            case 1:
                _maxPlayerInputField.Select();
                break;

        }
    }
    public void RoomNameSelected() => InputSelected = 0;
    public void MaxPlayerSelected() => InputSelected = 1;


    private void TestLog()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
        {
            Debug.Log("�÷��̾ �α����� �ùٸ����ʽ��ϴ�.");
            return;
        }
        Debug.Log("Lobby Panel �׽�Ʈ�α�");
        Debug.Log($"Display Name: \t {user.DisplayName}");
        Debug.Log($"Email Address: \t {user.Email}");
        Debug.Log($"Email Verification: \t {user.IsEmailVerified}");
        Debug.Log($"User ID: \t\t {user.UserId}");
        Debug.Log("");
    }
    public void CreateRoomMenu()
    {
        _createRoomPanel.SetActive(true);
        _isMakingRoom = true;
        _roomNameInputField.text = $"Room {Random.Range(1000, 10000)}";
        _maxPlayerInputField.text = "8";
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    public void CreateRoomCancel()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _createRoomPanel.SetActive(false);
        _isMakingRoom = false;
    }
    public void CreateRoomConfirm()
    {
        string roomName = _roomNameInputField.text;
        if (roomName == "")
        {
            // "���̸�"�� ������ �־�� ���� �� ����.
            Debug.LogWarning("�� �̸��� �����ؾ� ���� ������ �� �ֽ��ϴ�.");
            return;
        }

        int maxPlayer = int.Parse(_maxPlayerInputField.text);
        maxPlayer = Mathf.Clamp(maxPlayer, 1, 8);

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = maxPlayer;

        PhotonNetwork.CreateRoom(roomName, options);
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        // �� �г� �����ߵ�
    }
    public void RandomMatching()
    {
        Debug.Log("���� ��Ī ��û");

        // ��� �ִ� ���� ������ ���� �ʴ� ���
        // PhotonNetwork.JoinRandomRoom();
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        // ��� �ִ� ���� ������ ���� ���� ���� ���� ���
        string name = $"Room {Random.Range(1000, 10000)}";
        RoomOptions options = new RoomOptions() { MaxPlayers = 8 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomName: name, roomOptions: options);
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // ���� ����� ��� + ���� ������� ��� + ������ �Ұ����� ���� ���
            if (info.RemovedFromList == true || info.IsVisible == false || info.IsOpen == false)
            {
                // ���� ��Ȳ : �κ� ���ڸ��� ������� ���� ���
                if (roomDictionary.ContainsKey(info.Name) == false)
                    continue;

                Destroy(roomDictionary[info.Name].gameObject);
                roomDictionary.Remove(info.Name);
            }

            // ���ο� ���� ������ ���
            else if (roomDictionary.ContainsKey(info.Name) == false)
            {
                RoomEntry roomEntry = Instantiate(_roomEntryPrefab, _roomListContent);
                roomDictionary.Add(info.Name, roomEntry);
                roomEntry.SetRoomInfo(info);
            }

            // ���� ������ ����� ���
            else if (roomDictionary.ContainsKey((string)info.Name) == true)
            {
                RoomEntry roomEntry = roomDictionary[info.Name];
                roomEntry.SetRoomInfo(info);
            }
        }
    }
    public void ClearRoomEntries()
    {
        foreach (string name in roomDictionary.Keys)
        {
            Destroy(roomDictionary[name].gameObject);
        }
        roomDictionary.Clear();
    }

    // From mainPanel
    public void LogOut()
    {
        Debug.Log("�α׾ƿ� �׽�Ʈ �α�");
        PhotonNetwork.Disconnect();
    }


    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif

    }
}
