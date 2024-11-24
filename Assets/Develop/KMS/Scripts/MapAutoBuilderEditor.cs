// Unity ������ ȯ�濡���� ������.
// ���� ���� ȯ�濡���� ���ܵǾ� ���ʿ��� ������ �� ������ ����.
#if UNITY_EDITOR
using UnityEditor; // ������ Ȯ�� ����� ����.
using UnityEngine;

// ��ũ��Ʈ�� Ŀ���� Inspector�� ����.
[CustomEditor(typeof(GridManager))]
public class MapEditor : Editor
{
    /// <summary>
    /// Inspector â�� GUI�� �׸��� ���� ȣ��Ǵ� �޼���.
    /// ����� ���� ��ư, �ؽ�Ʈ �ʵ� ���� ������ �����ϴ�.
    /// </summary>
    public override void OnInspectorGUI()
    {
        // �⺻ Inspector �Ӽ����� ���� + ����� ���� GUI.
        DrawDefaultInspector();

        GridManager mapData = (GridManager)target;

        // Inspectorâ�� "Generate Map"��ư�� �����Ѵ�.
        if (GUILayout.Button("Generate Map"))
        {
            mapData.GenerateMap();
        }

        // Inspectorâ�� "Clear Map"��ư�� �����Ѵ�.
        if (GUILayout.Button("Clear Map"))
        {
            mapData.ClearMap();
        }
    }
}
#endif