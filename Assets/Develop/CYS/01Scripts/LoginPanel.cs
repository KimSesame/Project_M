using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Extensions;
using Photon.Pun;
using System;

public class LoginPanel : BaseUI
{
    [SerializeField] TMP_FontAsset kFont;


    [SerializeField] TMP_InputField _emailInputField;   // 0
    [SerializeField] TMP_InputField _pwInputField;      // 1

    public int InputSelected;

    // [SerializeField] Image _nicknamePanel;
    // [SerializeField] Image _verificationPanel;

    // �ؿ� ���� ��ũ��Ʈ ���� �����ϱ�???
    [SerializeField] GameObject _signUpPanel;
    [SerializeField] GameObject _verificationPanel;
    [SerializeField] GameObject _nicknamePanel;

    [SerializeField] GameObject _resetPwPanel;
    TMP_InputField _restPwIDInputField;

    [SerializeField] GameObject _notificationPanel;
    // string _notification;

    private void Start()
    {
        Init();
        // TestLogin();
        TestLogin();
        SoundManager.Instance.StopBGM();
        SoundManager.Instance.PlayBGM(SoundManager.E_BGM.LOGIN);
    }

    private void OnEnable()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(SoundManager.E_BGM.LOGIN);
        }
    }

    private void OnDisable()
    {
        
    }
    private void Update()
    {
        TabInputField();
        // ����Ű���� �α��� ��ư �Է�
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
            Login();
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
            case 0: _emailInputField.Select();
                break;
            case 1: _pwInputField.Select();
                break;
        }
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    // ���� Ŭ���ϸ� �ٲ�µ� InputSelected�� �ȹٲ�ϱ�
    // ���� �ż���� InputSelected�ٲ�Լ���
    public void EmailSelected()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        InputSelected = 0;
    } 

    public void PwSelected()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        InputSelected = 1; 
    }



    //  private void Start()
    //  {
    //      Init();
    //  }
    private void Init()
    {
        // TMP_Text
        // ID_Text
        GetUI<TMP_Text>("IDText").font = kFont;
        GetUI<TMP_Text>("IDText").fontSizeMin = 14;
        GetUI<TMP_Text>("IDText").fontSize = 36;
        GetUI<TMP_Text>("IDText").fontSizeMax = 72;
        // PW_Text
        GetUI<TMP_Text>("PWText").font = kFont;
        GetUI<TMP_Text>("PWText").fontSizeMin = 14;
        GetUI<TMP_Text>("PWText").fontSize = 36;
        GetUI<TMP_Text>("PWText").fontSizeMax = 72;
        
        GetUI<TMP_Text>("IDText").text = "�̸���";
        GetUI<TMP_Text>("PWText").text = "��й�ȣ";

        // TMP_InputField
        // ID
        _emailInputField = GetUI<TMP_InputField>("IDInputField");
        GetUI<TMP_Text>("IDInputPlaceholder").text = "example@gmail.com";
        GetUI<TMP_Text>("IDInputPlaceholder").font = kFont;
        GetUI<TMP_Text>("IDInputPlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("IDInputPlaceholder").fontSize = 22;
        GetUI<TMP_Text>("IDInputPlaceholder").fontSizeMax = 58;
        GetUI<TMP_Text>("IDInputText").fontSizeMin = 14;
        GetUI<TMP_Text>("IDInputText").fontSize = 22;
        GetUI<TMP_Text>("IDInputText").fontSizeMax = 58;

        // PW
        _pwInputField = GetUI<TMP_InputField>("PWInputField");
        GetUI<TMP_Text>("PWInputPlaceholder").text = "��й�ȣ�� �Է��ϼ���";
        GetUI<TMP_Text>("PWInputPlaceholder").font = kFont;
        GetUI<TMP_Text>("PWInputPlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("PWInputPlaceholder").fontSize = 22;
        GetUI<TMP_Text>("PWInputPlaceholder").fontSizeMax = 58;
        GetUI<TMP_Text>("PWInputText").fontSizeMin = 14;
        GetUI<TMP_Text>("PWInputText").fontSize = 22;
        GetUI<TMP_Text>("PWInputText").fontSizeMax = 58;

        // Button
        // LoginButton
        GetUI<Button>("LoginButton").onClick.AddListener(Login);
        GetUI<TMP_Text>("LoginButtonText").fontSizeMin = 14;
        GetUI<TMP_Text>("LoginButtonText").fontSize = 36;
        GetUI<TMP_Text>("LoginButtonText").fontSizeMax = 72;
        GetUI<TMP_Text>("LoginButtonText").text = "�α���";
        // SignUpButton
        GetUI<Button>("SignUpButton").onClick.AddListener(GoToSignUp);
        GetUI<TMP_Text>("SignUpText").fontSizeMin = 14;
        GetUI<TMP_Text>("SignUpText").fontSize = 36;
        GetUI<TMP_Text>("SignUpText").fontSizeMax = 72;
        GetUI<TMP_Text>("SignUpText").text = "ȸ������";
        // ResetPWButton
        GetUI<Button>("ResetPWButton").onClick.AddListener(ResetPW);
        GetUI<TMP_Text>("ResetPWText").fontSizeMin = 14;
        GetUI<TMP_Text>("ResetPWText").fontSize = 36;
        GetUI<TMP_Text>("ResetPWText").fontSizeMax = 72;
        GetUI<TMP_Text>("ResetPWText").text = "��й�ȣ ã��";

        // VerificationPanel
        _verificationPanel = GetUI("VerificationPanel");
        GetUI<TMP_Text>("WaitingText").font = kFont;
        GetUI<Button>("VerificationCancelButton");

        // NicknamePanel
        _nicknamePanel = GetUI("NicknamePanel");
        GetUI<TMP_Text>("NicknameText").font = kFont;
        GetUI<TMP_InputField>("NicknameInputField");
        GetUI<Button>("ConfirmButton");

        _signUpPanel = GetUI("SignUpPanel");


        // ResetPasswordPanel
        _resetPwPanel = GetUI("ResetPwPanel");
        GetUI<TMP_Text>("RestPwIDText").fontSizeMin = 14;
        GetUI<TMP_Text>("RestPwIDText").fontSize = 36;
        GetUI<TMP_Text>("RestPwIDText").fontSizeMax = 72;
        GetUI<TMP_Text>("RestPwIDText").text = "�̸���";
        // placeInput
        GetUI<TMP_Text>("RestPwIDInputTextPlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("RestPwIDInputTextPlaceholder").fontSize = 22;
        GetUI<TMP_Text>("RestPwIDInputTextPlaceholder").fontSizeMax = 58;
        GetUI<TMP_Text>("RestPwIDInputTextPlaceholder").text = "�̸����� �Է��ϼ���";
        // RestPwIDInputText
        GetUI<TMP_Text>("RestPwIDInputText").fontSizeMin = 14;
        GetUI<TMP_Text>("RestPwIDInputText").fontSize = 22;
        GetUI<TMP_Text>("RestPwIDInputText").fontSizeMax = 58;

        _restPwIDInputField = GetUI<TMP_InputField>("RestPwIDInputField");
        GetUI<Button>("RestPwConfirmButton").onClick.AddListener(SendResetPwEmail);
        GetUI<TMP_Text>("RestPwConfirmText").text = "�ʱ�ȭ ���Ϲ߼�";
        GetUI<TMP_Text>("RestPwConfirmText").fontSizeMin = 14;
        GetUI<TMP_Text>("RestPwConfirmText").fontSize = 22;
        GetUI<TMP_Text>("RestPwConfirmText").fontSizeMax = 58;

        GetUI<Button>("RestPwCancelButton").onClick.AddListener(CancelFindingPW);
        GetUI<TMP_Text>("RestPwCancelText").text = "���";
        GetUI<TMP_Text>("RestPwCancelText").fontSizeMin = 14;
        GetUI<TMP_Text>("RestPwCancelText").fontSize = 36;
        GetUI<TMP_Text>("RestPwCancelText").fontSizeMax = 72;

        // �˸�â
        _notificationPanel = GetUI("NotificationPanel");
        // �˸� �޼���
        GetUI<TMP_Text>("NotificationText").font = kFont;
        GetUI<TMP_Text>("NotificationText").fontSizeMin = 14;
        GetUI<TMP_Text>("NotificationText").fontSize = 22;
        GetUI<TMP_Text>("NotificationText").fontSizeMax = 40;
      //  GetUI<TMP_Text>("NotificationText").text = _notification;
        // Ȯ��/�ݱ��ư
        GetUI<Button>("NotificationButton").onClick.AddListener(CloseNotification);





        // �����ư
        GetUI<Button>("ExitButton").onClick.AddListener(QuitGame);
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
    

    /// <summary>
    /// �׽�Ʈ ���ϰ��Ϸ��� �α��� �ڵ�
    /// </summary>
    public void TestLogin()
    {
        _emailInputField.text = "ysc1350@gmail.com";
        _pwInputField.text = "q1w2e3r4";
    }


    public void SubscirbesEvents()
    {
       // GetUI<Button>("LoginButton").onClick.AddListener(Login);
    }
    public void GoToSignUp()
    {
        Debug.Log("ȸ������ â ����");
        _signUpPanel.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    public void Login()
    {
        Debug.Log("LoginButton �׽�Ʈ �α�");
        
        string email = _emailInputField.text;
        if (email == "")
        {
            Debug.Log("�̸����� �Է� �� �ּ���.");
            GetUI<TMP_Text>("NotificationText").text = "�̸����� �Է� �� �ּ���.";
            OpenNotifiaction();
        }
        string password = _pwInputField.text;
        if (password == "")
        {
            Debug.Log("��й�ȣ�� �Է� �ϼ���.");
            GetUI<TMP_Text>("NotificationText").text = "��й�ȣ�� �Է� �ϼ���.";
            OpenNotifiaction();
        }

        
        BackendManager.Auth.SignInWithEmailAndPasswordAsync(email, password)
           .ContinueWithOnMainThread(task =>
           {
               if (task.IsCanceled)
               {
                   Debug.LogWarning("SignInWithEmailAndPasswordAsync was canceled.");
                   GetUI<TMP_Text>("NotificationText").text = "�α��� ������ ��ҵǾ����ϴ�. ";
                   OpenNotifiaction();
                   return;
               }
               if (task.IsFaulted)
               {
                   Debug.LogWarning("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                   GetUI<TMP_Text>("NotificationText").text = $"�ùٸ� ���̵�/��й�ȣ��\n �Է����ּ���.";
                   OpenNotifiaction();
                   return;
               }

               AuthResult result = task.Result;
               Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
               CheckUserInfo();
           });

        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    public void CheckUserInfo()
    {
        FirebaseUser user = BackendManager.Auth.CurrentUser;
        if (user == null)
            return;         // Ȥ�ó� �α��ξȵǼ� ������ ������ return

        Debug.Log($"Display Name: \t {user.DisplayName}");
        Debug.Log($"Email Address: \t {user.Email}");
        Debug.Log($"Email Verification: \t {user.IsEmailVerified}");
        Debug.Log($"User ID: \t\t {user.UserId}");

        if (user.IsEmailVerified == false)
        {
            // �̸������� �ȵ����� ����â Ȱ��ȭ
            _verificationPanel.gameObject.SetActive(true);
        }
        else if (user.DisplayName == "")
        {
            // �г��� ������ �г��� �����â Ȱ��ȭ
            _nicknamePanel.gameObject.SetActive(true);
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = user.DisplayName;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void ResetPW()
    {
        _resetPwPanel.SetActive(true);
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    /// <summary>
    /// ��й�ȣ �缳��
    /// InputField�� ���� �ּҷ� ��й�ȣ �缳�� �̸��� ������
    /// </summary>
    public void SendResetPwEmail()
    {
        string email = _restPwIDInputField.text;
        BackendManager.Auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogWarning("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogWarning("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Password reset email sent successfully.");
            _resetPwPanel.SetActive(false);
        });
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    public void CancelFindingPW()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _resetPwPanel.SetActive(false);
    }
    public void OpenNotifiaction()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _notificationPanel.SetActive(true);
    }
    public void CloseNotification()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _notificationPanel.SetActive(false);
    }

}
