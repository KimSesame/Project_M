using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;        // ��������
using Firebase.Database;    // �����ͺ��̽�
using Firebase.Extensions;

public class BackendManager : MonoBehaviour
{
    public static BackendManager Instance { get; private set; }

    private FirebaseApp _app;
    public static FirebaseApp App { get { return Instance._app; } }
    
    private FirebaseAuth _auth;
    public static FirebaseAuth Auth { get {return Instance._auth; } }

    private FirebaseDatabase _database;
    public static FirebaseDatabase Database { get { return Instance._database; } }

    private void Awake()
    {
        SetSingleton();
    }
    void Start()
    {
        CheckDependency();
    }


    private void SetSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ȣȯ�� üũ & ����
    /// �񵿱��, Async (Task, MultiThread������� üũ)
    /// �񵿱�� �۾��� ��������(�α��ε�����) �̰� �ϰڴ�.
    /// CheckAndFixDependenciesAsync() �ϰ��� ContinueWithOnMainThread(task =>...
    /// </summary>
    private void CheckDependency()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if(task.Result == DependencyStatus.Available)
            {
                _app = FirebaseApp.DefaultInstance;
                _auth = FirebaseAuth.DefaultInstance;
                _database = FirebaseDatabase.DefaultInstance;

                Debug.Log("Firebase ����غ�Ϸ�. ");

            }
            else
            {
                Debug.LogError ($"Cannot resolve all Firebase dependencies: {task.Result}");
                _app = null;
                _auth = null;
                _database = null;
            }
        });
    }
}
