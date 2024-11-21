using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaterBomb : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private LayerMask _layerMask;

    private WaitForSeconds _delay;
    private IObjectPool<WaterBomb> _objectPool;

    public IObjectPool<WaterBomb> ObjectPool { set { _objectPool = value; } }

    private void OnEnable()
    {
        Deactivate();
    }

    private void Start()
    {
        _delay = new WaitForSeconds(_lifeTime);
    }

    private void Deactivate() => StartCoroutine(DeactivateRoutine());

    IEnumerator DeactivateRoutine()
    {
        yield return _delay;

        _objectPool.Release(this);
    }


    /// <summary>
    /// ��ǳ���� ��ġ�� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="placerPosition">ǳ���� ��ġ�ϴ� ������Ʈ�� position</param>
    /// <returns>�ش� ��ġ�� ��ǳ�� ��ġ ���� ����</returns>
    public bool SetLocation(Vector3 placerPosition)
    {
        Vector3 offset = new(0, 0.5f, 0);
        Vector3 location = new Vector3(Mathf.RoundToInt(placerPosition.x), 0, Mathf.RoundToInt(placerPosition.z)) + offset;

        // Inspect validation of location
        Collider[] others = Physics.OverlapSphere(location, 0.3f, _layerMask);
        if (others.Length > 0)
        {
            _objectPool.Release(this);
            return false;
        }

        // Move to location
        transform.position = location;
        return true;
    }
}
