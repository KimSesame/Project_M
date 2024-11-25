using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public GameObject _chatContent;
    public TMP_InputField _chatInputField;

    PhotonView _photonView;

    GameObject _chatDisplay;

    string _userName;
    // �̰� �ᱹ �κ������ �� �̷���� ��ɵ��̴ϱ�
    // �׳� LobbyScene : MonoBehaviourPunCallbacks ���� �۵��ص� ���� ��������

    private void OnEnable()
    {
        
    }
    void Start()
    {
       // PhotonNetwork.ConnectUsingSettings();
        _chatDisplay = _chatContent.transform.GetChild(0).gameObject;
        _photonView = GetComponent<PhotonView>();
        Debug.Log("ChatManager�׽�Ʈ �����@Start");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && _chatInputField.isFocused == false)
        {
            _chatInputField.ActivateInputField();
        }
    }
    public override void OnConnectedToMaster()
    {
        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
      
        int _randomKey = Random.Range(0, 100);
        _userName = "user" + _randomKey;
      
        PhotonNetwork.LocalPlayer.NickName = _userName;
        PhotonNetwork.JoinOrCreateRoom("Room1", options, null);
      
        Debug.Log("OnConnectedToMatser ���� �׽�Ʈ");
   
        // �� �Լ��̹� LobbyScene���� �Ἥ ���� ���⼭ ���ص� �ʿ��ϴ� ������ �ű�� �̽��ϱ�
    }

    // �ϴ� ������ ���ִ°� ������ �κ�� �ٲܼ� ������ �ٲٱ�
    public override void OnJoinedRoom()
    {
        AddChatMessage("connect user : " + PhotonNetwork.LocalPlayer.NickName);

        // �� ���°ŵ� �뿡 ���� ���� �ϰ��ϸ�..
    }
    public void OnEndEditEvent()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("ä�ÿ��� �׽�Ʈ");
            string strMessage = _userName + " : " + _chatInputField.text;

            // target �޴��� ��ο��� inputField�� ������� 
            _photonView.RPC("RPC_Chat", RpcTarget.All, strMessage);
            _chatInputField.text = "";
        }
    }
    void AddChatMessage(string message)
    {
        GameObject goText = Instantiate(_chatDisplay, _chatContent.transform);
        goText.GetComponent<TextMeshProUGUI>().text = message;
        _chatDisplay.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    [PunRPC]
    void RPC_Chat(string message)
    {
        AddChatMessage(message);
    }
}
