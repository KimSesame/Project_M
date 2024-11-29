using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.UI;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using static Photon.Pun.UtilityScripts.PunTeams;


public class RoomPanel : BaseUI
{
    [SerializeField] TMP_FontAsset kFont;
    [SerializeField] PlayerEntry[] _playerEntries;
    [SerializeField] Button _startButton;

    // Map ����
   // [SerializeField] List <string> mapList;
    private List<string> mapList = new List<string>();
    GameObject _mapImage;
    [SerializeField] Texture[] _mapTexture;
    [SerializeField] RawImage _mapRawImage;
    public int _miniMap = 0; // ���г� �� �����(�����ư��) 0���ͽ��� _miniMap 0 == mapNumber 1

    // [SerializeField] Button _mapSelectButton;
    GameObject _mapListPanel;
    GameObject _map01;
    GameObject _map02;
    GameObject _map03;
    GameObject _map04;
    GameObject _map05;
    GameObject _map06;
    Button _map01Button;
    Button _map02Button;
    Button _map03Button;
    Button _map04Button;
    Button _map05Button;
    Button _map06Button;
    public int mapNumber = 1;



    // ������
    public int TeamNumber; // ���⼭ �������ؼ� �÷��̾�����
    [SerializeField] GameObject _teamChoicePanel;
    // [SerializeField] GameObject[] _teamButtons;

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
        GetUI<Button>("PreviousButton").onClick.AddListener(LeaveRoom);
        GetUI<TMP_Text>("PreviousButtonText").font = kFont;
        _startButton = GetUI<Button>("StartButton");
        GetUI<TMP_Text>("StartButtonText").font = kFont;
        _startButton.onClick.AddListener(StartGame);

        // �ʼ��� ����
        _mapImage = GetUI("MapImage");
        _mapRawImage = (RawImage)_mapImage.GetComponent<RawImage>();
        _mapRawImage.texture = _mapTexture[_miniMap];
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
        mapNumber = 2; // ó���� ���� ���� �ٽ� ����������
        // �ǽɵǴ� ������ ����� ����ߴµ� init() OnEnable������ �ȸ��� .����
        Debug.Log($"���� �ʻ��� �α�, ��ư�̴��� �� : {(mapList[mapNumber])}");

        GetUI<TMP_Text>("MapNameText01").text = (mapList[1]);
        GetUI<TMP_Text>("MapNameText02").text = (mapList[2]);
        GetUI<TMP_Text>("MapNameText03").text = (mapList[3]);
        GetUI<TMP_Text>("MapNameText04").text = (mapList[4]);
        GetUI<TMP_Text>("MapNameText05").text = (mapList[5]);
        GetUI<TMP_Text>("MapNameText06").text = (mapList[6]);


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

    }
    private void Update()
    {
        // SelectMap();
        if (Input.GetKeyDown(KeyCode.P))
        {
            
        }
    }

    public void SelectTeam()
    {
        string SelectedTeam = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log($"{SelectedTeam} is selected.");

        // SelectedTeam �̸��� ���� ��ư�� �����ϸ� �׹�ư�� �´� �� �ѹ��� �ο�
        switch (SelectedTeam)
        {
            case "Team1": TeamNumber = 0;
                break;
            case "Team2": TeamNumber = 1;
                break;
            case "Team3": TeamNumber = 2;
                break;
            case "Team4": TeamNumber = 3;
                break;
            case "Team5": TeamNumber = 4;
                break;
            case "Team6": TeamNumber = 5;
                break;
            case "Team7": TeamNumber = 6;
                break;
            case "Team8": TeamNumber = 7;
                break;
        }
        PhotonNetwork.LocalPlayer.SetTeam(TeamNumber);
        Debug.Log($"�����Ͻ� ����ȣ: {PhotonNetwork.LocalPlayer.GetTeam(TeamNumber)}");
       // Debug.Log($"�����Ͻ� ����ȣ: {PhotonNetwork.LocalPlayer.GetTeam(TeamNumber)}");
    }
    void OpenMapList()
    {
        _mapListPanel.SetActive(true);
    }
    void CloseMapList()
    {
        _mapListPanel.SetActive(false);
    }
    void GetMapList()
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            mapList.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
        }
    }
    /// <summary>
    ///  EventSystem.current.currentSelectedGameObject.name��
    ///  ���̸��� ���ؼ� ������ �����ɷ� ��Int ����
    /// </summary>
    public void SelectMap()
    {
        string SelectedMap = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log($"{SelectedMap} is selected.");
        //if (SelectedMap == _map01Button.name)
        //    mapNumber = 1;

        // SelectedMap �̸��� ���� ��ư�� �����ϸ� �׹�ư�� �´� �� �ѹ��� �ο�
        switch (SelectedMap)
        {
            case "MapSelectButton01": mapNumber = 1; 
                break;
            case "MapSelectButton02": mapNumber = 2;
                break;
            case "MapSelectButton03": mapNumber = 3;
                break;
            case "MapSelectButton04": mapNumber = 4;
                break;
            case "MapSelectButton05": mapNumber = 5;
                break;
            case "MapSelectButton06": mapNumber = 6;
                break;
        }
        _miniMap = mapNumber - 1;
        _mapRawImage.texture = _mapTexture[_miniMap];
        _mapListPanel.SetActive(false);
        // �� �����ϸ� => Button������
        // �׳� ������ ___�ϴ� �Լ� ���� �ű⼭ ���ϰ�
        // �� ��ȣ�� �ε� ��
    }

    public void UpdatePlayers()
    {
        foreach (PlayerEntry entry in _playerEntries)
        {
            entry.SetEmpty();
        }
        // ���� �濡 �ִ� ��� �÷��̾� ��������
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Number �Ҵ� ���� -1  �̴ϱ�, �� �÷��̾�� �Ҵ����� �ʴ´�.
            if (player.GetPlayerNumber() == -1)
                continue;

            int number = player.GetPlayerNumber();
            _playerEntries[number].SetPlayer(player);
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
    public void UpdatePlayerProperty(Player targetPlayer, Hashtable properties)
    {
        if(properties.ContainsKey(CustomProperty.READY))
        {
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
            if(player.GetReady() == false)
                return false;
        }   // �� ���� �� �����
        return true;
    }


    public void StartGame()
    {
        Debug.Log(mapList[mapNumber]);
        PhotonNetwork.LoadLevel(mapList[mapNumber]); // ���� �����ϸ鼭 �̸����� ����
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


}
