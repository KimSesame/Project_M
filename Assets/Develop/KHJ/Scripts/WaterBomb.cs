using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaterBomb : MonoBehaviour
{
    [SerializeField] private float _lifeTime;

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
    /// ����ź�� ��ġ�� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="placerPosition">��ź�� ��ġ�ϴ� ������Ʈ�� position</param>
    public void SetLocation(Vector3 placerPosition)
    {
        transform.position = placerPosition;
    }
}
