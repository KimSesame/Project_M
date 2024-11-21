using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;    // ������ ������ �迭
    public Transform[] spawnPoints;     // ������ ���� ��ġ �迭
    public float spawnInterval = 10f;   // ���� ����.

    private void Start()
    {
        InvokeRepeating(nameof(SpawnItem), 0, spawnInterval);
    }

    private void SpawnItem()
    {
        // ������ �����۰� ���� ��ġ ����
        int randomItemIndex = Random.Range(0, itemPrefabs.Length);
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);

        // ������ ����
        Instantiate(itemPrefabs[randomItemIndex], spawnPoints[randomSpawnIndex].position, Quaternion.identity);
    }
}
