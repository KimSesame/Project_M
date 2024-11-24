using Photon.Pun.Demo.SlotRacer.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject wallPrefab;       // ������ ��
    public List<GameObject> obstaclePrefabs;   // �ı� ������ ��ֹ�
    public GameObject spawnPointPrefab; // �÷��̾� ���� ����
    public GameObject groundPlane;      // �ٴ� Plane
    public Vector3 offset;              // �� ��ü ��ġ ����

    private int[,] mapData = {
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 3, 0, 2, 0, 2, 0, 2, 0, 2, 0, 3, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 1, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 1, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 1, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 3, 0, 2, 0, 2, 0, 2, 0, 2, 0, 3, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
    };

    private List<Vector3> spawnPoints = new List<Vector3>(); // ���� ��ġ ����Ʈ
    private GameObject mapContainer; // ���� ��� ������Ʈ�� ��� �����̳�

    /// <summary>
    /// �÷��̾ �����Ǵ� ��ġ�� ��ȯ�ϴ� �޼���.
    /// </summary>
    /// <returns></returns>
    public List<Vector3> GetSpawnPoints()
    {
        return spawnPoints;
    }

    /// <summary>
    /// �� �����Ϳ� ���Ͽ� �����ϵ��� �ϴ� �޼���.
    /// </summary>
    public void GenerateMap()
    {
        ClearMap();
        CreateGroundPlane(); // �ٴ� Plane ����

        // �� �����̳� ����
        mapContainer = new GameObject("MapContainer");
        mapContainer.transform.parent = transform;

        for (int z = 0; z < mapData.GetLength(0); z++)
        {
            // Z�� �������� �� �����̳ʿ� ����
            GameObject zParent = new GameObject($"Z_{z}");
            zParent.transform.parent = mapContainer.transform;

            for (int x = 0; x < mapData.GetLength(1); x++)
            {
                Vector3 position = new Vector3(x + offset.x, offset.y, -z + offset.z);
                Vector3 spawnPos = new Vector3(x + offset.x, -1f + offset.y, -z + offset.z);
                GameObject tileObject = null;

                switch (mapData[z, x])
                {
                    case 1: // ��
                        tileObject = Instantiate(wallPrefab, position, Quaternion.identity, zParent.transform);
                        break;
                    case 2: // �ı� ������ ��ֹ�
                        if (obstaclePrefabs != null && obstaclePrefabs.Count > 0)
                        {
                            GameObject randomObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
                            tileObject = Instantiate(randomObstacle, position, Quaternion.identity, zParent.transform);
                        }
                        break;
                    case 3: // ���� ����
                        tileObject = Instantiate(spawnPointPrefab, spawnPos, Quaternion.identity, zParent.transform);
                        spawnPoints.Add(position);
                        break;
                }

                if (tileObject != null)
                {
                    tileObject.name = $"Tile_x({x})_z({z})";
                }
            }
        }
    }

    /// <summary>
    /// �� ������ �ٴ��� Plane���� ũ�⸦ �����ؼ� �����ϴ� �޼���.
    /// </summary>
    public void CreateGroundPlane()
    {
        Vector3 planePosition = new Vector3
            (
                (mapData.GetLength(1) - 1) / 2f + offset.x,
                offset.y, 
                -(mapData.GetLength(0) - 1) / 2f + offset.z
            );

        GameObject ground = Instantiate(groundPlane, planePosition, Quaternion.identity, transform);
        ground.name = "GroundPlane";

        ground.transform.localScale = new Vector3(mapData.GetLength(1) / 10f, 1, mapData.GetLength(0) / 10f);
    }

    /// <summary>
    /// ������� ���� �����Ҽ� �ֵ��� �� �޼���.
    /// </summary>
    public void ClearMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            // Destroy�� ���� �����ӿ��� ����.(Play ��忡���� ����)
            // DestroyImmediate�� ��ü�� ��� ������ �� ���.(Play ���� ������ ��� �� �� ����)
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}


#region Test Map Data
/*
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 3, 0, 0, 2, 2, 2, 2, 0, 0, 3, 0, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 0, 0, 2, 2, 0, 2, 0, 0, 2, 2, 0, 1 },
        { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 },
        { 1, 0, 2, 0, 2, 0, 0, 0, 2, 0, 2, 0, 1 },
        { 1, 2, 1, 2, 1, 0, 0, 0, 1, 2, 1, 2, 1 },
        { 1, 2, 2, 2, 2, 0, 0, 0, 2, 2, 2, 2, 1 },
        { 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1 },
        { 1, 0, 2, 2, 2, 0, 2, 2, 2, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 3, 0, 0, 2, 2, 2, 2, 0, 0, 3, 0, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
 */
#endregion

#region Map01
/*
    { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
        { 1, 3, 0, 2, 0, 2, 0, 2, 0, 2, 0, 3, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 1, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 1, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 1, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 2, 0, 2, 0, 2, 0, 2, 0, 2, 0, 2, 1 },
        { 1, 0, 2, 1, 2, 1, 2, 1, 2, 1, 2, 0, 1 },
        { 1, 3, 0, 2, 0, 2, 0, 2, 0, 2, 0, 3, 1 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }

 */
#endregion