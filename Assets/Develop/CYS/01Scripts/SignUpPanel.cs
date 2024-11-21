using Firebase.Auth;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class SignUpPanel : BaseUI
{
    [SerializeField] TMP_InputField _signUpIDInputField;
    [SerializeField] TMP_InputField _signUpPWInputField;
    [SerializeField] TMP_InputField _PWConfirmInputField;


    [SerializeField] GameObject _checkPopup;
    [SerializeField] GameObject _alreadyExistMsg;
    [SerializeField] GameObject _availableAddressMsg;
    
    AuthError error = AuthError.EmailAlreadyInUse;
    private void OnEnable()
    {
        Init();
    }
    private void Init()
    {

        // ID & PW
        // TMP_Text
        GetUI<TMP_Text>("SignUpIDText");
        GetUI<TMP_Text>("SignUpPWText");
        GetUI<TMP_Text>("PWConfirmText");

        // TMP_InputField
        _signUpIDInputField = GetUI<TMP_InputField>("SignUpIDInputField");
        GetUI<TMP_Text>("SignUpIDPlaceholder").text = "example@gmail.com";
        _signUpPWInputField = GetUI<TMP_InputField>("SignUpPWInputField");
        GetUI<TMP_Text>("SUPWinputPlaceholder").text = "Cannot be too simple like\n qwer1234";
        _PWConfirmInputField = GetUI<TMP_InputField>("PWConfirmInputField");
        GetUI<TMP_Text>("PWConfirmplaceholder").text = "Must to match with the password.";

        // Button
        GetUI<Button>("SignUpConfirmButton").onClick.AddListener(SignUp);  // Init �ٲ����
        GetUI<Button>("SignUpCancelButton").onClick.AddListener(Close);  // Init �ٲ����

        // CheckPopup
        _checkPopup = GetUI("CheckPopup");
        _alreadyExistMsg = GetUI("AlreadyExistMsg");
        _availableAddressMsg = GetUI("AvailableAddressMsg");
        GetUI<Button>("CheckPopupButton").onClick.AddListener(Close);
    }

    void Update()
    {
        // ������Ʈ���� ������ �ִ°� ���� �ٸ���������� ����
        // �ϴ� ���Եǰ� ��Ȱ��ȭ�Ǹ� ���� �����ϱ� ���Դ�ÿ��� ���ư��°�������...
        CheckAvailability();
    }


    public void SignUp()
    {
        Debug.Log("SignUpButton �׽�Ʈ �α� : ȸ������!");

        string _email = _signUpIDInputField.text;
        string _password = _signUpPWInputField.text;
        string _confirmPW = _PWConfirmInputField.text;

        if(_email.IsNullOrEmpty() || _password.IsNullOrEmpty() || _confirmPW.IsNullOrEmpty())
        {
            // TODO : �ð��̵ȴٸ� �˾�â���� ����..?
            Debug.LogWarning("�Է����� ���� ���� �ֽ��ϴ�. \n�ٽ��ѹ� Ȯ�����ּ���");
            return;
        }
        if (_password != _confirmPW)
        {
            Debug.LogWarning("�н����尡 ��ġ���� �ʽ��ϴ�.");
        }
        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Firebase.FirebaseException exception = task.Exception.InnerException as Firebase.FirebaseException;
                switch ((AuthError)exception.ErrorCode)
                {

                    case AuthError.EmailAlreadyInUse:
                        Debug.LogWarning($"�̸����� �̹� ������Դϴ�.");
                        _checkPopup.SetActive(true);
                        break;
                }

                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            _checkPopup.SetActive(true);
            AuthResult result = task.Result;
            Debug.Log($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
            gameObject.SetActive(false);
        });

    }

    /// <summary>
    /// ��밡���� �̸������� üũ :
    /// ����/�Ұ��ɿ� ���� �ٸ� �޽��� ���
    /// </summary>
    public void CheckAvailability()
    {
        if (error == AuthError.EmailAlreadyInUse)
        {
            _alreadyExistMsg.SetActive(true);
            _availableAddressMsg.SetActive(false);
        }
        else
        {
            _availableAddressMsg.SetActive(true);
            _alreadyExistMsg.SetActive(false);
        }
    }

    public void Close()
    {
        Debug.Log("�ݱ��ư");
        gameObject.SetActive(false);
    }
}
