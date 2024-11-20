using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDestruction : MonoBehaviour
{
    [Header("�μ����� �� ����")]
    public int fragmentsCount = 10;         // �μ����� ���� ����
    public float explosionForce = 30f;      // ���߹���
    public float explosionRadius = 5f;      // ���߹ݰ�
    public GameObject fragmentPrefab;       // �μ��� ����
    public Transform parentContainer;       // �μ��� �������� ���� ��

    /// <summary>
    /// ���� ������Ʈ�� ������� �μ��� ������ ���߷� ���ư� �޼���.
    /// </summary>
    public void DestroyObject()
    {
        List<GameObject> fragments = CreateFragments();

        foreach(var fragment in fragments)
        {
            // ����鿡�� ���� �������� ���߷� ����.
            Rigidbody rb = fragment.GetComponent<Rigidbody>();
            
            if(rb)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// �μ��� ���� ���� �޼���.
    /// </summary>
    private List<GameObject> CreateFragments()
    {
        List<GameObject> fragments = new List<GameObject>();

        for (int i = 0; i < fragmentsCount; i++)
        {
            // ������ ��ġ �����Ͽ� ��������.
            //      Random.insideUnitSphere = �ݰ� 1�� ���� �� ���� ������ ������ ��ȯ�մϴ�.
            Vector3 randomPos = transform.position + Random.insideUnitSphere * 0.5f;
            GameObject fragment = Instantiate(fragmentPrefab, randomPos, Random.rotation); //Rpc�� ���� ����.
            
            // ������ ũ�⸦ �������� ����(�پ缺 �ο�)
            float randomScale = Random.Range(0.1f, 0.4f);
            fragment.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // ������ ������ �Ӽ� �߰�
            Rigidbody rb = fragment.AddComponent<Rigidbody>();
            rb.mass = 0.1f;

            // �θ� ������Ʈ ����
            if (parentContainer)
            {
                fragment.transform.SetParent(parentContainer);
            }

            // ������ ������ ����Ʈ�� ��ȯ.
            fragments.Add(fragment);
        }

        return fragments;
    }
}
