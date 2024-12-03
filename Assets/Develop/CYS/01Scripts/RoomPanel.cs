using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using static UnityEditor.Experimental.GraphView.GraphView;


public class RoomPanel : BaseUI
{
    [Header("���̸� �ο���")]
    [SerializeField] TMP_Text _roomTitle;
    [SerializeField] TMP_Text _roomCapacity;
    // ������, �ο��� ������ ���� ������
    private string _roomName, _roomMembers;


    [Header("�ѱ���Ʈ")]
    [SerializeField] TMP_FontAsset kFont;
    [Header("�÷��̾����")]
    [SerializeField] PlayerEntry[] _playerEntries;
    [SerializeField] Button _startButton;

    // Map ����
    [Header("�ʰ���")]
    // [SerializeField] List <string> mapList;
    private List<string> mapList = new List<string>();
    GameObject _mapImage;
    [SerializeField] Texture[] _mapTexture;
    [SerializeField] RawImage _mapRawImage;
    [SerializeField] TMP_Text _mapTitleText;    // ���̸�
    public int _miniMap = 0;                    // ���г� �� �����(�����ư��) 0���ͽ��� _miniMap 0 == mapNumber 1
    GameObject _mapListPanel;
    GameObject _map01, _map02, _map03, _map04, _map05, _map06;
    Button _map01Button, _map02Button, _map03Button, _map04Button, _map05Button, _map06Button;
    public int mapNumber = 1;


    // ������
    [Header("������")]
    public int TeamNumber; // ���⼭ �������ؼ� �÷��̾�����
    [SerializeField] GameObject _teamChoicePanel;
    [SerializeField] GameObject[] _teamButtons;
    [SerializeField] GameObject[] _teamColor;
    public int teamColorNum;
    Image _teamImage;



    // ĳ���Ͱ���
    [Header("ĳ���Ͱ���")]
    [SerializeField] GameObject _charChoicePanel;
    [SerializeField] GameObject[] _character;
    [SerializeField] Texture[] _charTexture;
    [SerializeField] RawImage _charRawImage;
    GameObject _playerImage;
    public int charNumber; // ������� ĳ���� ����â ĳ���͹�ȣ


    [Header("�����ư����")]
    [SerializeField] Button _readyButton;
    [SerializeField] GameObject _readyTextBox;
    [SerializeField] TMP_Text _readyText;
    private readonly Color readyColor = new Color(1, 0.8f, 0, 1);
    private readonly Color notReadyColor = Color.white;
    private Player _player;
    PlayerEntry _playerEntry;
    [SerializeField] TMP_Text _readyPopText;

    private void OnEnable()
    {
        UpdatePlayers();

        PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayers;

        PhotonNetwork.LocalPlayer.SetReady(false);
        PhotonNetwork.LocalPlayer.SetLoad(false);

        // ������� üũ�α�
        bool ready = PhotonNetwork.LocalPlayer.GetReady();
        Debug.Log($"�������: {ready}");

        Init();

        // TestLog();
        Debug.Log($"���� �ʻ��� �α�, �� : {(mapList[mapNumber])}");
    }
    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;
        _startButton.onClick.RemoveListener(StartGame);
        // ����鼭 StartGameAddListner�� �ƴµ� �׻��·� ����Ǽ� ���۵Ǽ� �ȵǴ°�
        // �������ؼ� ���г� ��Ȱ��ȭ�� ������Ҹ� �߾�� ����.
    }
    private void Init()
    {
        _roomTitle = GetUI<TMP_Text>("RoomTitle");
        _roomTitle.font = kFont;
        _roomCapacity = GetUI<TMP_Text>("RoomCapacity");
        _roomCapacity.font = kFont;
        _roomName = _roomTitle.text;
        _roomMembers = _roomCapacity.text;



        GetUI<Button>("PreviousButton").onClick.AddListener(LeaveRoom);
        GetUI<TMP_Text>("PreviousButtonText").font = kFont;
        _startButton = GetUI<Button>("StartButton");
        GetUI<TMP_Text>("StartButtonText").font = kFont;
        GetUI<TMP_Text>("StartButtonText").fontSizeMin = 14;
        GetUI<TMP_Text>("StartButtonText").fontSize = 22;
        GetUI<TMP_Text>("StartButtonText").fontSizeMax = 58;
        _startButton.onClick.AddListener(StartGame);

        // �ʼ��� ����
        GetMapList();
        GetUI<Button>("MapSelectButton").onClick.AddListener(OpenMapList);
        _mapListPanel = GetUI("MapListPanel");
        GetUI<Button>("MapCancelButton").onClick.AddListener(CloseMapList);
        _map01Button = GetUI<Button>("MapSelectButton01");
        _map02Button = GetUI<Button>("MapSelectButton02");
        _map03Button = GetUI<Button>("MapSelectButton03");
        _map04Button = GetUI<Button>("MapSelectButton04");
        _map05Button = GetUI<Button>("MapSelectButton05");
        _map06Button = GetUI<Button>("MapSelectButton06");
        _map01Button.onClick.AddListener(SelectMap);
        _map02Button.onClick.AddListener(SelectMap);
        _map03Button.onClick.AddListener(SelectMap);
        _map04Button.onClick.AddListener(SelectMap);
        _map05Button.onClick.AddListener(SelectMap);
        _map06Button.onClick.AddListener(SelectMap);
        // MapButtonInit(PhotonNetwork.LocalPlayer);
        _miniMap = 1;  // ���۸� �丶�伳��
        mapNumber = 2; // ó���� ���� ���� �ٽ� ���������� (���۸��丶��...)
        _mapImage = GetUI("MapImage");
        _mapRawImage = (RawImage)_mapImage.GetComponent<RawImage>();
        _mapRawImage.texture = _mapTexture[_miniMap];
        _mapTitleText = GetUI<TMP_Text>("MapTitleText");
        //_mapTitleText.text = (mapList[mapNumber]);
        _mapTitleText.fontSizeMin = 14;
        _mapTitleText.fontSize = 22;
        _mapTitleText.fontSizeMax = 58;
        SetRoomInfo(PhotonNetwork.CurrentRoom);
        KoreanMap();
        Debug.Log($"���� �ʻ��� �α�, ��ư�̴��� �� : {(mapList[mapNumber])}");

        GetUI<TMP_Text>("MapNameText01").text = "�ʽ�Ʈ����";
        GetUI<TMP_Text>("MapNameText02").text = "�丶�佺Ʈ����";
        GetUI<TMP_Text>("MapNameText03").text = "���̽������� 10";
        GetUI<TMP_Text>("MapNameText04").text = "���� 14";
        GetUI<TMP_Text>("MapNameText05").text = "���丮 07";
        GetUI<TMP_Text>("MapNameText06").text = "������Ʈ07";

        // ������
        _teamChoicePanel = GetUI("TeamChoicePanel");
        GetUI<Button>("Team1").onClick.AddListener(SelectTeam);
        GetUI<Button>("Team2").onClick.AddListener(SelectTeam);
        GetUI<Button>("Team3").onClick.AddListener(SelectTeam);
        GetUI<Button>("Team4").onClick.AddListener(SelectTeam);
        GetUI<Button>("Team5").onClick.AddListener(SelectTeam);
        GetUI<Button>("Team6").onClick.AddListener(SelectTeam);
        GetUI<Button>("Team7").onClick.AddListener(SelectTeam);
        GetUI<Button>("Team8").onClick.AddListener(SelectTeam);

        // ĳ���Ͱ���
        _charChoicePanel = GetUI("CharChoicePanel");
        // charNumber = 0; //ó���� ĳ���� 0 (����)
        PhotonNetwork.LocalPlayer.SetCharacter(charNumber);
        GetUI<Button>("Character1").onClick.AddListener(SelectCharacter);
        GetUI<Button>("Character2").onClick.AddListener(SelectCharacter);
        GetUI<Button>("Character3").onClick.AddListener(SelectCharacter);

        _playerImage = GetUI("PlayerImage");
        _charRawImage = (RawImage)_playerImage.GetComponent<RawImage>();
        _charRawImage.texture = _charTexture[charNumber];
        // ���� �̷��� �̹��� �޴°� PlayerEntry������ �ϸ� �ɵ�?? (GetCharacter)�Ἥ?


        // �����ư ���� (��ŸƮ��ư��)
        _readyTextBox = GetUI("ReadyTextBox");
        _readyText = GetUI<TMP_Text>("ReadyText");
        _readyText.font = kFont;
        // �����ؽ�Ʈ�ڽ� �ؿ� �����ư (���� ����۾��� �����ϸ� ������Ǳ����ѱ���)++ó���� ���������ϴٺ����̷��Ե�
        // ���������Ϸ��ٰ� ���һ��ؼ� �ϴ� �׳� �α����.
        _readyButton = GetUI<Button>("ReadyButton");
        _readyButton.onClick.AddListener(Ready);
        // �����ϸ� �÷��̾� ���� ������ READY�ؽ�Ʈ
        _readyPopText = GetUI<TMP_Text>("ReadyPopText");
        _readyPopText.font = kFont;

    }
    private void Update()
    {
        // ������ �ٲ�ų� / �ο����� �ٲ�� ������Ʈ 
        if (_roomName != PhotonNetwork.CurrentRoom.Name ||
            _roomMembers != $"{PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}")
        {
            SetRoomInfo(PhotonNetwork.CurrentRoom);
        }

        // SelectMap();
        if (Input.GetKeyDown(KeyCode.P))
        {


        }
    }
    public void SetPlayer(Player player)
    {
        // KMS �÷��̾� ��������.
        _player = player;
        _readyButton.gameObject.SetActive(true);
        _readyButton.interactable = player == PhotonNetwork.LocalPlayer;
        
        {
 
            UpdateReadyState();
        }

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
            //UpdateReadyState();

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
    // ���� ���� ���� ������ ���°Ű���
    public GameObject[] GetChildren(GameObject parent)
    {
        GameObject[] children = new GameObject[parent.transform.childCount];
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            children[i] = parent.transform.GetChild(i).gameObject;
        }
        return children;
    }
    public void SelectCharacter()
    {
        string SelectedChar = EventSystem.current.currentSelectedGameObject.name;
        switch (SelectedChar)
        {
            case "Character1":
                charNumber = 0;
                break;
            case "Character2":
                charNumber = 1;
                break;
            case "Character3":
                charNumber = 2;
                break;
            default:
                Debug.LogWarning("�߸��� ĳ���� ����!");
                return;
        }
        PhotonNetwork.LocalPlayer.SetCharacter(charNumber);
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        //_charRawImage.texture = _charTexture[charNumber];
        Debug.Log($"ĳ���͹�ȣ: {charNumber}");

        // KMS �÷��̾� ���� ����
        UpdatePlayers();
    }
    public void SelectTeam()
    {
        string SelectedTeam = EventSystem.current.currentSelectedGameObject.name;
        // Debug.Log($"{SelectedTeam} is selected.");

        // SelectedTeam �̸��� ���� ��ư�� �����ϸ� �׹�ư�� �´� �� �ѹ��� �ο�
        switch (SelectedTeam)
        {
            case "Team1":
                TeamNumber = 0;
                break;
            case "Team2":
                TeamNumber = 1;
                break;
            case "Team3":
                TeamNumber = 2;
                break;
            case "Team4":
                TeamNumber = 3;
                break;
            case "Team5":
                TeamNumber = 4;
                break;
            case "Team6":
                TeamNumber = 5;
                break;
            case "Team7":
                TeamNumber = 6;
                break;
            case "Team8":
                TeamNumber = 7;
                break;
        }
        PhotonNetwork.LocalPlayer.SetTeam(TeamNumber);
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        Debug.Log($"�����Ͻ� ����ȣ: {PhotonNetwork.LocalPlayer.GetTeam()}");
        // Debug.Log($"�����Ͻ� ����ȣ: {PhotonNetwork.LocalPlayer.GetTeam(TeamNumber)}");

        // KMS ���� ���� ���ý� �÷��̾� ����
        UpdatePlayers();
    }
    void OpenMapList()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _mapListPanel.SetActive(true);
    }
    void CloseMapList()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _mapListPanel.SetActive(false);
    }
    void GetMapList()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            mapList.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
        
    }


    public void SelectMap()
    {

        string SelectedMap = EventSystem.current.currentSelectedGameObject.name;
        //Debug.Log($"{SelectedMap} is selected.");  // Ŭ����Object �̸� ������
        //if (SelectedMap == _map01Button.name)
        //    mapNumber = 1;

        // SelectedMap �̸��� ���� ��ư�� �����ϸ� �׹�ư�� �´� �� �ѹ��� �ο�
        switch (SelectedMap)
        {
            case "MapSelectButton01":
                mapNumber = 1;
                break;
            case "MapSelectButton02":
                mapNumber = 2;
                break;
            case "MapSelectButton03":
                mapNumber = 3;
                break;
            case "MapSelectButton04":
                mapNumber = 4;
                break;
            case "MapSelectButton05":
                mapNumber = 5;
                break;
            case "MapSelectButton06":
                mapNumber = 6;
                break;
        }
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _mapListPanel.SetActive(false);
        // �� ����
        // �����϶��� �����ϵ��� | ����ƴϸ� �׳� â�ݱ�
        // PhotonNetwork.CurrentRoom.SetMap(mapNumber);
        // �� �����ϸ� => Button������
        // �׳� ������ ___�ϴ� �Լ� ���� �ű⼭ ���ϰ�
        // �� ��ȣ�� �ε� ��
        if (PhotonNetwork.IsMasterClient)
        {
            _miniMap = mapNumber - 1;
            _mapRawImage.texture = _mapTexture[_miniMap];
            PhotonNetwork.CurrentRoom.SetMapNum(mapNumber);
            PhotonNetwork.CurrentRoom.SetMapName(mapList[mapNumber]);
            Debug.Log($"�� ��ȣ {mapNumber}�� �����Ǿ����ϴ�.");
            // KoreanMap();
            //_mapTitleText.text = (mapList[mapNumber]);
        }
    }
    private void KoreanMap()
    {
        if (mapNumber == 1)
            _mapTitleText.text = "�ʽ�Ʈ����";
        else if (mapNumber == 2)
            _mapTitleText.text = "�丶�� ��Ʈ����";
        else if (mapNumber == 3)
            _mapTitleText.text = "���̽������� 10";
        else if (mapNumber == 4)
            _mapTitleText.text = "���� 14";
        else if (mapNumber == 5)
            _mapTitleText.text = "���丮 07";
        else if (mapNumber == 6)
            _mapTitleText.text = "������Ʈ07";
    }

    /// <summary>
    /// �� ���� ���� ������Ʈ
    /// </summary>
    /// <param name="mapNumber"></param>
    public void UpdateMapUI(int mapNumber)
    {
        mapNumber = PhotonNetwork.CurrentRoom.GetMapNum()-1;
        if (_mapRawImage == null || _mapTexture == null) return;

        if (mapNumber >= 0 && mapNumber < _mapTexture.Length)
        {
            _mapRawImage.texture = _mapTexture[mapNumber];
        }
    }
    public void UpdateMapName(string mapName)
    {
        mapName = PhotonNetwork.CurrentRoom.GetMapName();
        if (_mapTitleText.text != mapName)
        {
            _mapTitleText.text = mapName;
        }
        KoreanMap();
    }

    public void UpdatePlayers()
    {
        if (_playerEntries == null || _playerEntries.Length == 0)
        {
            Debug.LogWarning("PlayerEntries �迭�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        foreach (PlayerEntry entry in _playerEntries)
        {
            if (entry != null)
            {
                entry.SetEmpty();
            }
            else
            {
                Debug.LogWarning("PlayerEntry�� null�Դϴ�.");
            }
        }
        // ���� �濡 �ִ� ��� �÷��̾� ��������
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Number �Ҵ� ���� -1  �̴ϱ�, �� �÷��̾�� �Ҵ����� �ʴ´�.
            if (player.GetPlayerNumber() == -1)
                continue;

            int number = player.GetPlayerNumber();
            _playerEntries[number].SetPlayer(player);

            // KMS ĳ���� ���� ������Ʈ
            {
                int characterId = player.GetCharacter();
                _playerEntries[number].UpdateCharacter(characterId);

                // KMS �� ���� ������Ʈ
                int teamNumber = player.GetTeam();
                _playerEntries[number].UpdateTeam(teamNumber);
            }
        }
        // ���⼭ ����̻��� �ȵǰԲ�, ���ǹ��� �ɸ� ����̻����~ �ϰ� �� �� �ִ�.

        // ������ �����ϴ븸 ���� �� �ְ��ϱ�
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            _startButton.interactable = CheckAllReady();
        }
        else
        {
            _startButton.interactable = false;
        }


        // �����ư �����϶��� ���ΰ� ������ �ְ��ϱ�
        // �� PlayerEntry���� ����

    }

    public void UpdateRoomProperty(Hashtable properties)
    {
        // �ʺ���, num & UI
        if (properties.ContainsKey(CustomProperty.MAP))
        {
            UpdateMapUI(mapNumber);
        }
        // ���̸�UI ����
        if (properties.ContainsKey(CustomProperty.MAPNAME))
        {
            UpdateMapName(mapList[mapNumber]);
        }


    }
    /// <summary>
    ///  �÷��̾� ������Ƽ ������Ʈ
    ///  �����Ȳ
    /// </summary>
    /// <param name="targetPlayer"></param>
    /// <param name="properties"></param>
    public void UpdatePlayerProperty(Player targetPlayer, Hashtable properties)
    {
        if (properties.ContainsKey(CustomProperty.READY))
        {
            UpdatePlayers();
        }

        // KMS CHARACTER ���� ó��
        if (properties.ContainsKey(CustomProperty.CHARACTER))
        {
            Debug.Log($"{targetPlayer.NickName}�� ĳ���Ͱ� �����: {targetPlayer.GetCharacter()}");

            // ����� ĳ���� UI ����
            UpdatePlayers();
        }
        if (properties.ContainsKey(CustomProperty.TEAM))
        {
            Debug.Log($"{targetPlayer.NickName}�� ���� ����: {targetPlayer.GetTeam()}");

            // ����� ĳ���� UI ����
            UpdatePlayers();
        }
    }
    public void EnterPlayer(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}���� �����Ͽ����ϴ�.");
        UpdatePlayers();
    }
    public void ExitPlayer(Player oldPlayer)
    {
        Debug.Log($"{oldPlayer.NickName}���� �����Ͽ����ϴ�.");
        UpdatePlayers();
    }
    private bool CheckAllReady()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // �� ���鼭 �Ѹ��̶� false�� ����
            if (player.GetReady() == false)
                return false;
        }   // �� ���� �� �����
        return true;
    }

    public void SetRoomInfo(RoomInfo info)
    {
        _roomTitle.text = info.Name;
        //_roomCapacity.text = $"{info.PlayerCount}/{info.MaxPlayers}";
        int currentMember = info.PlayerCount;
        _roomCapacity.text = $"{PhotonNetwork.PlayerList.Count()}/{info.MaxPlayers}";

    }
    public void StartGame()
    {
        Debug.Log(mapList[mapNumber]);
        PhotonNetwork.LoadLevel(mapList[mapNumber]); // ���� �����ϸ鼭 �̸����� ����
        PhotonNetwork.CurrentRoom.IsOpen = false;
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }

    public void LeaveRoom()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        PhotonNetwork.LeaveRoom();
    }


}
