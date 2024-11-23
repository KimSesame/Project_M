using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : BaseUI
{
    [SerializeField] Image _roomMap;
    [SerializeField] TMP_Text _roomTitle;
    [SerializeField] TMP_Text _roomCapacity;
    [SerializeField] Button _roomJoinButton;
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
        _roomCapacity.text = $"{info.PlayerCount} / {info.MaxPlayers}";
        _roomJoinButton.interactable = info.PlayerCount < info.MaxPlayers;
    }
    private void Init()
    {
        _roomTitle = GetUI<TMP_Text>("RoomTitle");
        _roomCapacity = GetUI<TMP_Text>("RoomCapacity");
        _roomJoinButton = GetUI<Button>("RoomJoinButton");
        _roomJoinButton.onClick.AddListener(JoinRoom);
    }

    public void JoinRoom()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.JoinRoom(_roomTitle.text);
    }

}
