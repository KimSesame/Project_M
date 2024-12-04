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
    [SerializeField] TMP_FontAsset kFont;

    [SerializeField] TMP_InputField _signUpIDInputField;    // 0
    [SerializeField] TMP_InputField _signUpPWInputField;    // 1
    [SerializeField] TMP_InputField _PWConfirmInputField;   // 2

    public int InputSelected;

    [SerializeField] GameObject _checkPopup;
    [SerializeField] TMP_Text _checkPopupMsg;
    
    AuthError error = AuthError.EmailAlreadyInUse;
    private void OnEnable()
    {
        Init();
    }
    private void Init()
    {
        // ID & PW
        // TMP_Text
        GetUI<TMP_Text>("SignUpIDText").font = kFont;
        GetUI<TMP_Text>("SignUpIDText").text = "�̸���";
        GetUI<TMP_Text>("SignUpIDText").fontSizeMin = 14;
        GetUI<TMP_Text>("SignUpIDText").fontSize = 36;
        GetUI<TMP_Text>("SignUpIDText").fontSizeMax = 72;

        GetUI<TMP_Text>("SignUpPWText").font = kFont;
        GetUI<TMP_Text>("SignUpPWText").text = "��й�ȣ";
        GetUI<TMP_Text>("SignUpPWText").fontSizeMin = 14;
        GetUI<TMP_Text>("SignUpPWText").fontSize = 36;
        GetUI<TMP_Text>("SignUpPWText").fontSizeMax = 72;

        GetUI<TMP_Text>("PWConfirmText").font = kFont;
        GetUI<TMP_Text>("PWConfirmText").text = "��й�ȣ Ȯ��";
        GetUI<TMP_Text>("PWConfirmText").fontSizeMin = 14;
        GetUI<TMP_Text>("PWConfirmText").fontSize = 36;
        GetUI<TMP_Text>("PWConfirmText").fontSizeMax = 72;

        // TMP_InputField
        _signUpIDInputField = GetUI<TMP_InputField>("SignUpIDInputField");
        GetUI<TMP_Text>("SignUpIDPlaceholder").text = "example@gmail.com";
        GetUI<TMP_Text>("SignUpIDPlaceholder").font = kFont;
        GetUI<TMP_Text>("SignUpIDPlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("SignUpIDPlaceholder").fontSize = 22;
        GetUI<TMP_Text>("SignUpIDPlaceholder").fontSizeMax = 58;
        // ���̵� �Է¶� ����
        GetUI<TMP_Text>("SignUpIDInputText").font = kFont;
        GetUI<TMP_Text>("SignUpIDInputText").fontSizeMin = 14;
        GetUI<TMP_Text>("SignUpIDInputText").fontSize = 22;
        GetUI<TMP_Text>("SignUpIDInputText").fontSizeMax = 58;

        // ��й�ȣ �Է�
        _signUpPWInputField = GetUI<TMP_InputField>("SignUpPWInputField");
        GetUI<TMP_Text>("SUPWinputPlaceholder").text = "��й�ȣ�� �Է��ϼ���";
        GetUI<TMP_Text>("SUPWinputPlaceholder").font = kFont;
        GetUI<TMP_Text>("SUPWinputPlaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("SUPWinputPlaceholder").fontSize = 22;
        GetUI<TMP_Text>("SUPWinputPlaceholder").fontSizeMax = 58;
        // ��й�ȣ �Է¶� ����
        GetUI<TMP_Text>("SUPWInputText").font = kFont;
        GetUI<TMP_Text>("SUPWInputText").fontSizeMin = 14;
        GetUI<TMP_Text>("SUPWInputText").fontSize = 22;
        GetUI<TMP_Text>("SUPWInputText").fontSizeMax = 58;

        // ��й�ȣ Ȯ�ζ�
        _PWConfirmInputField = GetUI<TMP_InputField>("PWConfirmInputField");
        GetUI<TMP_Text>("PWConfirmplaceholder").text = "���� �����ؾ� �մϴ�.";
        GetUI<TMP_Text>("PWConfirmplaceholder").font = kFont;
        GetUI<TMP_Text>("PWConfirmplaceholder").fontSizeMin = 14;
        GetUI<TMP_Text>("PWConfirmplaceholder").fontSize = 22;
        GetUI<TMP_Text>("PWConfirmplaceholder").fontSizeMax = 58;
        // ��й�ȣ Ȯ�ζ� �Է¶� ����
        GetUI<TMP_Text>("PWConfirmInputText").font = kFont;
        GetUI<TMP_Text>("PWConfirmInputText").fontSizeMin = 14;
        GetUI<TMP_Text>("PWConfirmInputText").fontSize = 22;
        GetUI<TMP_Text>("PWConfirmInputText").fontSizeMax = 58;





        // Button
        GetUI<Button>("SignUpConfirmButton").onClick.AddListener(SignUp);  // Init �ٲ����
        GetUI<Button>("SignUpCancelButton").onClick.AddListener(Close);  // Init �ٲ����
        GetUI<TMP_Text>("SignUpConfirmText").font = kFont;
        GetUI<TMP_Text>("SignUpCancelText").font = kFont;

        // CheckPopup
        _checkPopup = GetUI("CheckPopup");
        _checkPopupMsg = GetUI<TMP_Text>("CheckPopupMsg");
        _checkPopupMsg.font = kFont;
        _checkPopupMsg.fontSizeMin = 14;
        _checkPopupMsg.fontSize = 22;
        _checkPopupMsg.fontSizeMax = 58;
        GetUI<TMP_Text>("CheckPopupButtonText").font = kFont;
        GetUI<TMP_Text>("CheckPopupButtonText").fontSizeMin = 14;
        GetUI<TMP_Text>("CheckPopupButtonText").fontSize = 22;
        GetUI<TMP_Text>("CheckPopupButtonText").fontSizeMax = 58;
        GetUI<Button>("CheckPopupButton").onClick.AddListener(ClosePopup);
    }

    void Update()
    {
        TabInputField();
        // ������Ʈ���� ������ �ִ°� ���� �ٸ���������� ����
        // �ϴ� ���Եǰ� ��Ȱ��ȭ�Ǹ� ���� �����ϱ� ���Դ�ÿ��� ���ư��°�������...
        // CheckAvailability();
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

            _checkPopup.SetActive(true);
            _checkPopupMsg.text = "�Է����� ���� ���� �ֽ��ϴ�. \n�ٽ��ѹ� Ȯ�����ּ���";
            return;
        }
        if (_password != _confirmPW)
        {
            Debug.LogWarning("�н����尡 ��ġ���� �ʽ��ϴ�.");
            _checkPopup.SetActive(true);
            _checkPopupMsg.text = "�н����尡 ��ġ���� �ʽ��ϴ�.";
            return;
        }
        BackendManager.Auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                _checkPopup.SetActive(true);
                _checkPopupMsg.text = "ȸ�������� ��ҵǾ����ϴ�.";
                return;
            }
            if (task.IsFaulted)
            {
                // �ϴ� �������� �˾�â�����ϴ°� ����
                Firebase.FirebaseException exception = task.Exception.InnerException as Firebase.FirebaseException;
                switch ((AuthError)exception.ErrorCode)
                {

                    case AuthError.EmailAlreadyInUse:
                        Debug.LogWarning($"�̸����� �̹� ������Դϴ�.");
                        _checkPopup.SetActive(true);
                        _checkPopupMsg.text = "�̸����� �̹� ������Դϴ�.";
                        break;
                    case AuthError.WeakPassword:
                        Debug.LogWarning($"��й�ȣ�� �ʹ� �����ϴ�.");
                        _checkPopup.SetActive(true);
                        _checkPopupMsg.text = "��й�ȣ�� �ʹ� �����ϴ�. \n�ٸ���й�ȣ�� ������ּ���.";
                        break;
                    case AuthError.WrongPassword:
                        Debug.LogWarning($"�߸��� ��й�ȣ�Դϴ�.: {exception.ErrorCode}");
                        _checkPopupMsg.text = "��й�ȣ�� Ʋ���ϴ�.. \n�ٸ��� �Է��ϼ���.";
                        break;

                }
                

                Debug.LogWarning("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                _checkPopup.SetActive(true);
                _checkPopupMsg.text = $"{task.Exception} ������ �Ͼ���ϴ�.";
                return;
            }
            // üũ�˾� _checkPopup.SetActive(true);

            // Firebase user has been created.
            AuthResult result = task.Result;
            Debug.Log($"Firebase user created successfully: {result.User.DisplayName} ({result.User.UserId})");
            _checkPopupMsg.text = $"ȸ�������� ���ϵ帳�ϴ�! \n{result.User.UserId}�� ���������� Ȯ�����ּ���.";
            gameObject.SetActive(false);
        });
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);

    }
    public void ClosePopup()
    {
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        _checkPopup.SetActive(false);
    }

    public void Close()
    {
        Debug.Log("�ݱ��ư");
        SoundManager.Instance.PlaySFX(SoundManager.E_SFX.CLICK);
        gameObject.SetActive(false);
    }
}
