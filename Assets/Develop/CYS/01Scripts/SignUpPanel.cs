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
    [SerializeField] TMP_InputField _signUpIDInputField;    // 0
    [SerializeField] TMP_InputField _signUpPWInputField;    // 1
    [SerializeField] TMP_InputField _PWConfirmInputField;   // 2

    public int InputSelected;

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
        GetUI<TMP_Text>("SUPWinputPlaceholder").text = "Cannot be too simple like\n qwer1234 & longer than 6 chars";
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
        TabInputField();
        // ������Ʈ���� ������ �ִ°� ���� �ٸ���������� ����
        // �ϴ� ���Եǰ� ��Ȱ��ȭ�Ǹ� ���� �����ϱ� ���Դ�ÿ��� ���ư��°�������...
        CheckAvailability();
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
            if (InputSelected > 2)
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
            case 0:
                _signUpIDInputField.Select();
                break;
            case 1:
                _signUpPWInputField.Select();
                break;
             case 2:
                _PWConfirmInputField.Select();
                break;

        }
    }
    // ���� Ŭ���ϸ� �ٲ�µ� InputSelected�� �ȹٲ�ϱ�
    // ���� �ż���� InputSelected�ٲ�Լ���
    public void EmailSelected() => InputSelected = 0;
    public void PwSelected() => InputSelected = 1;
    public void ConfirmationSelected() => InputSelected = 2;


    public void SignUp()
    {
        Debug.Log("SignUpButton �׽�Ʈ �α� : ȸ�����Թ�ư ����");

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
                /* �ϴ� �������� �˾�â�����ϴ°� ����
                Firebase.FirebaseException exception = task.Exception.InnerException as Firebase.FirebaseException;
                switch ((AuthError)exception.ErrorCode)
                {

                    case AuthError.EmailAlreadyInUse:
                        Debug.LogWarning($"�̸����� �̹� ������Դϴ�.");
                        _checkPopup.SetActive(true);
                        break;
                }
                */

                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            // üũ�˾� _checkPopup.SetActive(true);

            // Firebase user has been created.
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
