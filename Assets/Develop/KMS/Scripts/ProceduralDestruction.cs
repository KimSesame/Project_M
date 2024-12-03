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

    [Header("���ٱ� �߰� ���� ����")]
    public bool isContinue;

    /// <summary>
    /// ���� ������Ʈ�� ������� �μ��� ������ ���߷� ���ư� ������ ������ RPC�޼���.
    /// </summary>
    public void DestroyObject()
    {
        if (!photonView || photonView.ViewID == 0)
        {
            Debug.LogError($"DestroyObject ȣ�� �� PhotonView�� ��ȿ���� ����. {name} ������Ʈ Ȯ�� �ʿ�.");
            return;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(DestroyObjectRPC), RpcTarget.AllBuffered);
        }
    }

    /// <summary>
    /// ���� ������Ʈ�� ������� �μ��� ������ ���߷� ���ư� �޼���.
    /// ��Ʈ��ũ ���۽� �ش� �κ��� Rpc�� ��ȯ�ؼ� �����ؾ��Ѵ�.
    /// </summary>
    [PunRPC]
    private void DestroyObjectRPC(PhotonMessageInfo info)
    {
        if (!this || !gameObject)
        {
            Debug.LogWarning($"DestroyObjectRPC ȣ�� ����: ������Ʈ�� �̹� ������ {name}");
            return;
        }

        float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
        Debug.Log($"DestroyObjectRPC ȣ�� ���� �ð�: {lag}��");

        List<GameObject> fragments = CreateFragments(lag);

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
        // ���� ���� ���� �ִ� �÷��̾ �����ϰ� �ٸ� �÷��̾�� �˸���.
        if(PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(SpawnItemRPC), RpcTarget.AllBuffered, info.SentServerTime);


        // ��Ʈ��ũ���� �� ����
        if (PhotonNetwork.IsMasterClient || photonView.IsMine)
            PhotonNetwork.Destroy(gameObject); 
    }

    /// <summary>
    /// �μ��� ���� ���� �޼���.
    /// </summary>
    private List<GameObject> CreateFragments(float lag)
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
            
            if(fragment.name == "Fragment_Wood(Clone)")
                randomScale = Random.Range(0.1f, 0.3f);
            fragment.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            // ������ ������ �Ӽ� �߰�
            Rigidbody rb = fragment.AddComponent<Rigidbody>();
            rb.mass = 0.1f;

            // ���� �ð���ŭ ������ ��ġ�� ����
            if (rb)
            {
                Vector3 direction = (randomPos - transform.position).normalized;
                rb.position += direction * lag * explosionForce * Time.deltaTime;
            }

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
    /// 
    [PunRPC]
    private void SpawnItemRPC(double sentServerTime)
    {
        float lag = Mathf.Abs((float)(PhotonNetwork.Time - sentServerTime));
        Debug.Log($"SpawnItemRPC ȣ�� ���� �ð�: {lag}��");

        // ������ Ŭ���̾�Ʈ������ ����
        if (!PhotonNetwork.IsMasterClient) return;

        // ���� Ȯ���� ������ ����
        if (Random.value <= itemSpawnChance)
        {
            // ������ ������ �� �ϳ��� ���� ����.
            // �������� �����ɶ� ���� ������Ʈ�� �����ϱ�.
            int randomIndex = Random.Range(0, itemPrefabs.Length);
            Vector3 spawnPosition = transform.position + Vector3.down * lag;
            if (transform.name == "stun_hammer_head_lvl3_LOD1")
                spawnPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);

            GameObject item = PhotonNetwork.InstantiateRoomObject($"Item/{itemPrefabs[randomIndex].name}", spawnPosition, Quaternion.identity);

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
        return isContinue;
    }
}
