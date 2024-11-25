using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDestruction : MonoBehaviourPun, IExplosionInteractable
{
    [Header("�μ����� �� ����")]
    public int fragmentsCount = 10;         // �μ����� ���� ����
    public float explosionForce = 30f;      // ���߹���
    public float explosionRadius = 5f;      // ���߹ݰ�
    public GameObject fragmentPrefab;       // �μ��� ����
    public Transform parentContainer;       // �μ��� �������� ���� ��

    private List<Collider> playerColliders; // Player �±׸� ���� ��� ĳ������ Collider ����Ʈ

    [Header("������ ���� ����")]
    public GameObject[] itemPrefabs;        // ������ ������ ������
    public float itemSpawnChance = 0.3f;    // ������ ���� Ȯ��

    /// <summary>
    /// �׽�Ʈ�� �޼���
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            DestroyObject();
        }
    }

    /// <summary>
    /// ���� ������Ʈ�� ������� �μ��� ������ ���߷� ���ư� �޼���.
    /// ��Ʈ��ũ ���۽� �ش� �κ��� Rpc�� ��ȯ�ؼ� �����ؾ��Ѵ�.
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

            // ��� Player���� �浹 ����
            Collider fragmentCollider = fragment.GetComponent<Collider>();
            if (fragmentCollider != null)
            {
                Debug.Log("����");

                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                foreach (GameObject player in players)
                {
                    Collider playerCollider = player.GetComponent<Collider>();
                    if (playerCollider != null)
                    {
                        Physics.IgnoreCollision(fragmentCollider, playerCollider);
                    }
                }
            }
        }

        // ������ ���� Ȯ���� ���� ����
        SpawnItem();

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
            float randomScale = Random.Range(0.5f, 1f);
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

    /// <summary>
    /// ���� �μ����� �����Ǵ� ���� ������
    /// </summary>
    private void SpawnItem()
    {
        // ���� Ȯ���� ������ ����
        if (Random.value <= itemSpawnChance)
        {
            // ������ ������ �� �ϳ��� ���� ����
            int randomIndex = Random.Range(0, itemPrefabs.Length);
            GameObject item = PhotonNetwork.Instantiate($"Item/{itemPrefabs[randomIndex].name}", transform.position, Quaternion.identity);

            if(parentContainer)
            {
                item.transform.SetParent(parentContainer);
            }

            Debug.Log("������ ����: " + item.name);
        }
    }

    public bool Interact()
    {
        DestroyObject();
        return false;
    }
}
