using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NicknamePanel : BaseUI
{
    [SerializeField] TMP_InputField _nickNameInputField;
    [SerializeField] GameObject _nicknamePanel;
    [SerializeField] Button _confirmButton;

    private void OnEnable()
    {
        Init();
    }
    private void Init()
    {
        _nicknamePanel = GetUI("NicknamePanel");
        GetUI<TMP_Text>("NicknameText");
        GetUI<TMP_Text>("NicknameText").text = "�г����� �����ּ���";
        GetUI<TMP_Text>("NicknameText").fontSizeMin = 14;
        GetUI<TMP_Text>("NicknameText").fontSize = 36;
        GetUI<TMP_Text>("NicknameText").fontSizeMax = 72;

        _nickNameInputField = GetUI<TMP_InputField>("NicknameInputField");
        _nickNameInputField.text = "â�Ƿ��� �����ϼ���!";
        GetUI<TMP_Text>("NicknamePlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("NicknamePlaceholder").fontSize = 22;
        GetUI<TMP_Text>("NicknamePlaceholder").fontSizeMax = 58;

        _confirmButton = GetUI<Button>("NicknameConfirmButton");
        GetUI<TMP_Text>("NicknameConfirmText").text = "Ȯ��";
        GetUI<TMP_Text>("NicknameConfirmText").fontSizeMin = 14;
        GetUI<TMP_Text>("NicknameConfirmText").fontSize = 22;
        GetUI<TMP_Text>("NicknameConfirmText").fontSizeMax = 58;
        _confirmButton.onClick.AddListener(Confirm);
    }
    public void Confirm()
    {
        Debug.Log("���߹�ư �۵�Ȯ�� �α�");
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        string nickName = _nickNameInputField.text;
        if (nickName == "")
        {
            Debug.LogWarning("�г����� �������ּ���.");
            return;
        }

        UserProfile profile = new UserProfile();
        profile.DisplayName = nickName;

        BackendManager.Auth.CurrentUser.UpdateUserProfileAsync(profile)
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");


                // �ϴ� �׳� üũ�� �α� 
                Debug.Log($"Display Name :\t {user.DisplayName}");
                Debug.Log($"Email :\t\t {user.Email}");
                Debug.Log($"Email Verified:\t  {user.IsEmailVerified}");
                Debug.Log($"User ID: \t\t  {user.UserId}");


                PhotonNetwork.LocalPlayer.NickName = nickName;
                PhotonNetwork.ConnectUsingSettings();
                gameObject.SetActive(false);
            });
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }

}
