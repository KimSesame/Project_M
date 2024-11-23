using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralDestruction : MonoBehaviour, IExplosionInteractable
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

    private void Start()
    {
        // Player �±׸� ���� ��� ĳ������ Collider�� ĳ��
        if (playerColliders == null)
        {
            CachePlayerColliders();
        }
    }

    /// <summary>
    /// Player �±׸� ���� ��� Collider�� ĳ��
    /// </summary>
    private void CachePlayerColliders()
    {
        playerColliders = new List<Collider>();

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Collider collider = player.GetComponent<Collider>();
            if (collider != null)
            {
                playerColliders.Add(collider);
            }
        }

        if (playerColliders.Count == 0)
        {
            Debug.LogWarning("Player �±׸� ���� Collider�� ã�� ���߽��ϴ�.");
        }
        else
        {
            foreach (Collider collider in playerColliders)
                Debug.Log($"Player �±� : {collider.name}");
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

                foreach (Collider playerCollider in playerColliders)
                {
                    Physics.IgnoreCollision(fragmentCollider, playerCollider);
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
            GameObject item = Instantiate(itemPrefabs[randomIndex], transform.position, Quaternion.identity);

            Debug.Log("������ ����: " + item.name);
        }
    }

    public bool Interact()
    {
        DestroyObject();
        return false;
    }
}
