using Photon.Realtime;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
    public const string READY = "Ready";
    public const string LOAD = "load";

    // (������) �� Ŀ���� ������Ƽ
    public const string TEAM = "Team";
    public const string CHARACTER = "Character";

    // ��Ŀ����������Ƽ
    public const string MAP = "Map";
    public const string SPAWNPOINT = "SpawnPoint";

    public static void SetMap(this Room room, int map)
    {
        PhotonHashtable customRoomProperty = new PhotonHashtable();
        customRoomProperty[MAP] = map;
        room.SetCustomProperties(customRoomProperty);
    }
    public static int GetMap(this Room room)
    {
        PhotonHashtable customRoomProperty = room.CustomProperties;
        if (customRoomProperty.ContainsKey(MAP))
        {
            return (int)customRoomProperty[MAP];
        }
        else
        {
            // Scene 1���� 1�����̴ϱ�, 0�� �κ��
            return 1;
        }
    }


    public static void SetReady(this Player player, bool ready)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[READY] = ready;
        player.SetCustomProperties(customProperty);
    }
    public static bool GetReady(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(READY))
        {
            return (bool)customProperty[READY];
        }
        else
        {
            return false;
        }
    }

    public static void SetLoad(this Player player, bool load)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[LOAD] = load;
        player.SetCustomProperties(customProperty);
    }
    public static bool GetLoad(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(LOAD))
        {
            return (bool)customProperty[LOAD];
        }
        else
        {
            return false;
        }
    }

    // �� ���� Ŀ���� ������Ƽ
    /// <summary>
    /// ���� �����ϴ� �Լ� �Դϴ�.
    /// num�� ���� ��ȣ�� �־��ּ���(0~7��)
    /// PlayerEntry�� SetPlayer���� ������� ��, �� ���۵��� Ȯ���߽��ϴ�.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="num"></param>
    public static void SetTeam(this Player player, int num)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[TEAM] = num;
        player.SetCustomProperties(customProperty);
    }
    public static int GetTeam(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        // Ű���� �ִ� ���
        if (customProperty.ContainsKey(TEAM))
        {
            // �Է��� ������ Ű���� �Ҵ�?
            return (int)customProperty[TEAM];
        }
        else
        {
            // Ű���� ���� ��� ���Ƿ� 0�� ������ ������ġ?
            return 0;
        }
    }

    public static void SetCharacter(this Player player, int num)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[CHARACTER] = num;
        player.SetCustomProperties(customProperty);
    }
    public static int GetCharacter(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(CHARACTER))
        {
            return (int)customProperty[CHARACTER];
        }
        else
        {
            return 0;
        }
    }

    public static void SetSpawnIndex(this Player player, int spawnPoint)
    {
        PhotonHashtable customProperty = new PhotonHashtable();
        customProperty[SPAWNPOINT] = spawnPoint;
        player.SetCustomProperties(customProperty);
    }
    public static int GetSpawnIndex(this Player player)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        if (customProperty.ContainsKey(SPAWNPOINT))
        {
            return (int)customProperty[SPAWNPOINT];
        }
        else
        {
            return -1;
        }
    }
}
