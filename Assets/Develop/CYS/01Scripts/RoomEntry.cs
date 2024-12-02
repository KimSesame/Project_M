using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : BaseUI
{
    [Header("�ѱ���Ʈ")]
    [SerializeField] TMP_FontAsset kFont;

    
    [SerializeField] TMP_Text _roomTitle;
    [SerializeField] TMP_Text _roomCapacity;
    [SerializeField] Button _roomJoinButton;

    [Header("�ʰ���")]
    [SerializeField] GameObject _roomImage;
    [SerializeField] RawImage _roomMap;
    [SerializeField] Texture[] _mapTexture;
    private int _defaultMap = 1;
    public int mapNum;

    // RoomStatus - Availability to join (waiting / started)
    // RoomInfo, protected bool isOpen = true; (IsOpen)


    // ���� �̰͵� �׳� '�� �� �ִ���' �� ���Ѱǵ�
    // not open �̸� �����µ� �κ� �븮��Ʈ���� ����x
    // not open �̸� ������ġ���� �ȵ�. �����ϸ� !open. ���� ó���� �� ������ ����

    // RoomSetting - Availability to join (private / public) 
    // RoomInfo, protected bool isVisible (IsVisible) ?? �̰� �� �� ������ ������
    // IsVisible �̰Ŵ�, �׳� lobby����Ʈ�� �ȳ����°� ����.

    // �׷��� ���� �Ѵ� �濡 �� �� �ֳ�/���� �ϱ�
    // �׳� IsOpen�̸� ǥ�ø� �ٸ��������ؼ� �ص� ������������
    // RoomStatus, waiting => isOpen || started => !isOpen
    // RoomSetting, public => isOpen || private => !isOpen

    private void OnEnable()
    {
        Init();
        
    }
    public void SetRoomInfo(RoomInfo info)
    {
        _roomTitle.text = info.Name;
        _roomCapacity.text = $"{info.PlayerCount}/{info.MaxPlayers}";
        _roomJoinButton.interactable = info.PlayerCount < info.MaxPlayers;
       // mapNum = PhotonNetwork.CurrentRoom.GetMap();
       // _roomMap.texture = _mapTexture[mapNum];
    }
    private void Init()
    {
        _roomTitle = GetUI<TMP_Text>("RoomTitle");
        _roomTitle.font = kFont;
        _roomCapacity = GetUI<TMP_Text>("RoomCapacity");
        _roomCapacity.font = kFont;
        GetUI<TMP_Text>("RoomJoinButtonText").font = kFont;
        GetUI<TMP_Text>("RoomSetting").font = kFont;
        GetUI<TMP_Text>("RoomStatus").font = kFont;
        _roomJoinButton = GetUI<Button>("RoomJoinButton");
        _roomJoinButton.onClick.AddListener(JoinRoom);
        _roomImage = GetUI("RoomMap");
        _roomMap = (RawImage)_roomImage.GetComponent<RawImage>();
        mapNum = _defaultMap;
        _roomMap.texture = _mapTexture[mapNum];


    }

    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(_roomTitle.text);
    }

}
