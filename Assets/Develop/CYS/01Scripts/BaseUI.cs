using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private Dictionary<string, GameObject> gameObjectDic;
    private Dictionary<(string, System.Type), Component> componentDic;

    protected virtual void Awake()
    {
        Bind();
    }
    private void Bind()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>(true);
        gameObjectDic = new Dictionary<string, GameObject>(transforms.Length << 2);
        componentDic = new Dictionary<(string, System.Type), Component>();
        foreach (Transform child in transforms)
        {
            // Ȥ�� �̸� ���ļ� �߰� �ȵǸ� �� �� �ֵ��� �α� ����.
            // bool isSuccess =  gameObjectDic.TryAdd(child.gameObject.name, child.gameObject);
            // if (isSuccess == false)
            // {
            //     Debug.LogWarning($"�̹� {child.gameObject.name} Object�� �־ �߰����� �ʽ��ϴ�. ");
            // }

            if (gameObjectDic.ContainsKey(child.gameObject.name))
            {
                Debug.LogWarning($"�̹� {child.gameObject.name} Object�� �־ �߰����� �ʽ��ϴ�. ");
                continue;
            }
            gameObjectDic[child.gameObject.name] = child.gameObject;
        }
    }

    // �̸��� name�� UI ���ӿ�����Ʈ ��������
    // GetUI("Key") : Key �̸��� ���ӿ�����Ʈ ��������
    public GameObject GetUI(in string name)
    {
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        return gameObject;
    }
    // �̸��� name�� UI���� ������Ʈ T ��������
    // GetUI<Image>("Key") : Key �̸��� ���ӿ�����Ʈ���� Image ������Ʈ ��������.
    public T GetUI<T>(in string name) where T : Component
    {
        // Ex) Button ���ӿ�����Ʈ �ȿ� Image ������Ʈ�� Ű : Button_Image
        // Ex) Chest ���ӿ�����Ʈ �ȿ� Transform ������Ʈ�� Ű : Chest_Transform
        (string, System.Type) key = (name, typeof(T)); // �ݺ����Ǵϱ� key ĳ��

        // 1. Component ��ųʸ��� �̹� ������(ã�ƺ����� �ִ°��) : �̹� ã�Ҵ��� �ֱ�
        componentDic.TryGetValue(key, out Component component);
        if (component != null)
            return component as T;

        // 2. Component ��ųʸ��� ���� ������(ã�ƺ����� ���°��) : ã�� �� ��ųʸ� �߰�.(Binding)
        // gameObject �� �̸��� ���ӿ�����Ʈ�� �������� ������ ã�ƺ���
        gameObjectDic.TryGetValue(name, out GameObject gameObject);
        if (gameObject == null)  // ������ ��ȯ
            return null;

        // gameObject ã�ƺ���
        component = gameObject.GetComponent<T>();
        if (component == null) // ������Ʈ�� �������� ��ȯ ��¡����
            return null;

        componentDic.TryAdd(key, component); // ��ã���� T�� ��ȯ 
        return component as T;
    }

}
