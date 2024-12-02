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

    SoundManager soundManager = SoundManager.Instance;

    private void OnEnable()
    {
        Init();
        TestLogin();
    }
    private void OnDisable()
    {
        soundManager.StopBGM();
    }
    private void Update()
    {
        TabInputField();
        // ����Ű���� �α��� ��ư �Է�
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            soundManager.PlaySFX(SoundManager.E_SFX.BOMB_EXPLOSION);
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
        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    // ���� Ŭ���ϸ� �ٲ�µ� InputSelected�� �ȹٲ�ϱ�
    // ���� �ż���� InputSelected�ٲ�Լ���
    public void EmailSelected()
    {
        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
        InputSelected = 0;
    } 

    public void PwSelected()
    {
        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
        InputSelected = 1; 
    }



    //  private void Start()
    //  {
    //      Init();
    //  }
    private void Init()
    {
        soundManager.StopBGM();
        soundManager.PlayBGM(SoundManager.E_BGM.LOGIN);


        // TMP_Text
        GetUI<TMP_Text>("IDText").font = kFont;
        GetUI<TMP_Text>("PWText").font = kFont;
        
        // TMP_InputField
        _emailInputField = GetUI<TMP_InputField>("IDInputField");
        GetUI<TMP_Text>("IDInputPlaceholder").text = "example@gmail.com";
        GetUI<TMP_Text>("IDInputPlaceholder").font = kFont;
        _pwInputField = GetUI<TMP_InputField>("PWInputField");
        GetUI<TMP_Text>("PWInputPlaceholder").text = "Type your password in";
        GetUI<TMP_Text>("PWInputPlaceholder").font = kFont;

        // Button
        GetUI<Button>("LoginButton").onClick.AddListener(Login);
        GetUI<Button>("SignUpButton").onClick.AddListener(GoToSignUp);
        GetUI<Button>("ResetPWButton").onClick.AddListener(ResetPW);

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
        _restPwIDInputField = GetUI<TMP_InputField>("RestPwIDInputField");
        GetUI<Button>("RestPwConfirmButton").onClick.AddListener(SendResetPwEmail);
        GetUI<Button>("RestPwCancelButton").onClick.AddListener(CancelFindingPW);
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
        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    public void Login()
    {
        Debug.Log("LoginButton �׽�Ʈ �α�");
        
        string email = _emailInputField.text;
        if (email == "")
        {
            Debug.Log("Email is empty, please put your eamil");
        }
        string password = _pwInputField.text;
        if (password == "")
        {
            Debug.Log("Password is empty, please put your eamil");
        }

        
        BackendManager.Auth.SignInWithEmailAndPasswordAsync(email, password)
           .ContinueWithOnMainThread(task =>
           {
               if (task.IsCanceled)
               {
                   Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                   return;
               }
               if (task.IsFaulted)
               {
                   Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                   return;
               }

               AuthResult result = task.Result;
               Debug.Log($"User signed in successfully: {result.User.DisplayName} ({result.User.UserId})");
               CheckUserInfo();
           });

        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
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
        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
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
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Password reset email sent successfully.");
            _resetPwPanel.SetActive(false);
        });
        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
    }
    public void CancelFindingPW()
    {
        soundManager.PlaySFX(SoundManager.E_SFX.CLICK);
        _resetPwPanel.SetActive(false);
    }


}
