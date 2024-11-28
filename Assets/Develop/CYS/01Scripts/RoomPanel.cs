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
    public int chosenMap;

    

   // enum MapButtons { _map01Button, _map02Button, _map03Button, _map04Button, _map05Button, _map06Button }
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

    }
    private void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayers;
        _startButton.onClick.RemoveListener(StartGame);
    }
    private void Init()
    {
        GetUI<Button>("PreviousButton").onClick.AddListener(LeaveRoom);
        GetUI<TMP_Text>("PreviousButtonText").font = kFont;
        _startButton = GetUI<Button>("StartButton");
        GetUI<TMP_Text>("StartButtonText").font = kFont;
        _startButton.onClick.AddListener(StartGame);

        // �ʼ��� ����
        GetMapList();
        _mapImage = GetUI("MapImage");
        _mapRawImage = (RawImage)_mapImage.GetComponent<RawImage>();
        _mapRawImage.texture = _mapTexture[_miniMap];



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

        GetUI<TMP_Text>("MapNameText01").text = (mapList[1]);
        GetUI<TMP_Text>("MapNameText02").text = (mapList[2]);
        GetUI<TMP_Text>("MapNameText03").text = (mapList[3]);
        GetUI<TMP_Text>("MapNameText04").text = (mapList[4]);
        GetUI<TMP_Text>("MapNameText05").text = (mapList[5]);
        GetUI<TMP_Text>("MapNameText06").text = (mapList[6]);
    }
    private void Update()
    {
        // SelectMap();
        if (Input.GetKeyDown(KeyCode.P))
        {
            
        }
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
            case "MapSelectButton01":
                mapNumber = 0;
                break;
            case "MapSelectButton02":
                mapNumber = 1;
                break;
            case "MapSelectButton03":
                mapNumber = 2;
                break;
            case "MapSelectButton04":
                mapNumber = 3;
                break;
            case "MapSelectButton05":
                mapNumber = 4;
                break;
            case "MapSelectButton06":
                mapNumber = 5;
                break;

        }
        // �� �����ϸ� => Button������
        // �׳� ������ ___�ϴ� �Լ� ���� �ű⼭ ���ϰ�
        // �� ��ȣ��
        // if (SelectedMap == _map01Button.name)
        // {
        //     Debug.Log("�ʴ����°��׽�Ʈ\n 1�����Ƚ��ϴ�.");
        //     SelectedMap = "";
        // }


        //  for (int i = 0; i < mapList.Count; i++)
        //  {
        //      if ( == _map01Button)
        //      mapNumber = i;
        //      switch (mapNumber)
        //      {
        //          case 1: mapNumber = 0;
        //              break;
        //          case 2: mapNumber = 1; 
        //              break;
        //          case 3: mapNumber = 2;
        //              break;
        //          case 4: mapNumber = 3;
        //              break;
        //          case 5: mapNumber = 4;
        //              break;
        //          case 6: mapNumber = 5;
        //              break;
        //
        //      }
        //  }
    }
    private void ChoseMap()
    { 
    }
    public void Map1Selected()
    {
        mapNumber = 1;
        _miniMap = mapNumber-1;
        _mapListPanel.SetActive(false);
        _mapRawImage.texture = _mapTexture[_miniMap];
    }
    public void Map2Selected()
    {
        mapNumber = 2;
        _miniMap = mapNumber - 1;
        _mapListPanel.SetActive(false);
        _mapRawImage.texture = _mapTexture[_miniMap];
    }
    public void Map3Selected()
    {
        mapNumber = 3;
        _miniMap = mapNumber - 1;
        _mapListPanel.SetActive(false);
        _mapRawImage.texture = _mapTexture[_miniMap];
    }
    public void Map4Selected()    
    {
        mapNumber = 4;
        _miniMap = mapNumber-1;
        _mapListPanel.SetActive(false);
        _mapRawImage.texture = _mapTexture[_miniMap];
    }
    public void Map5Selected()    
    {
        mapNumber = 5;
        _miniMap = mapNumber-1;
        _mapListPanel.SetActive(false);
    }
    public void Map6Selected()   
    {
        mapNumber = 6;
        _miniMap = mapNumber - 1;
        _mapListPanel.SetActive(false);
        _mapRawImage.texture = _mapTexture[_miniMap];
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
        
      //  for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
      //  {
      //      mapList.Add(System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)));
      //  }
        PhotonNetwork.LoadLevel(mapList[mapNumber]); // ���� �����ϸ鼭 �̸����� ����
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


}
