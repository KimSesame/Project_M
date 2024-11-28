using Photon.Realtime;
using System.Diagnostics;
using PhotonHashtable = ExitGames.Client.Photon.Hashtable;

public static class CustomProperty
{
    public const string READY = "Ready";
    public const string LOAD = "load";

    // (������) �� Ŀ���� ������Ƽ
    public const string TEAM = "Team";

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
    public static void GetTeam(this Player player, out int num)
    {
        PhotonHashtable customProperty = player.CustomProperties;
        // Ű���� �ִ� ���
        if (customProperty.ContainsKey(TEAM))
        {
            // �Է��� ������ Ű���� �Ҵ�?
            num = (int)customProperty[TEAM];
            return;
        }
        else
        {
            // Ű���� ���� ��� ���Ƿ� 0�� ������ ������ġ?
            num = 0;
            return;
        }
    }
}
