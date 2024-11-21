using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaterBomb : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _range = 1;
    [SerializeField] private LayerMask _layerMask;

    [Header("Explosion Effect")]
    [SerializeField] GameObject _effect;

    private WaitForSeconds _delay;
    private ObjectPool<WaterBomb> _objectPool;

    public ObjectPool<WaterBomb> ObjectPool { set { _objectPool = value; } }

    private void OnEnable()
    {
        Deactivate();
    }

    private void Start()
    {
        _delay = new WaitForSeconds(_lifeTime);
    }

    private void OnDisable()
    {
        if(_deactiveCoroutine != null)
        {
            StopCoroutine(_deactiveCoroutine);
            _deactiveCoroutine = null;
        }
    }

    private void Deactivate() => StartCoroutine(DeactivateRoutine());

    private Coroutine _deactiveCoroutine;
    IEnumerator DeactivateRoutine()
    {
        yield return _delay;

        CreateExplosion();

        _objectPool.Release(this);
    }

    private void CreateExplosion()
    {
        // Center
        Instantiate(_effect, transform.position, Quaternion.identity);

        // 4-way(up, down, right, left)
        for (int i = 1; i <= _range; i++)
        {
            Instantiate(_effect, transform.position + i * transform.forward, Quaternion.identity);
            Instantiate(_effect, transform.position - i * transform.forward, Quaternion.identity);
            Instantiate(_effect, transform.position + i * transform.right, Quaternion.identity);
            Instantiate(_effect, transform.position - i * transform.right, Quaternion.identity);
        }
    }


    /// <summary>
    /// ��ǳ���� ��ġ�� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="placerPosition">ǳ���� ��ġ�ϴ� ������Ʈ�� position</param>
    /// <returns>�ش� ��ġ�� ��ǳ�� ��ġ ���� ����</returns>
    public bool SetLocation(Vector3 placerPosition)
    {
        Vector3 location = new Vector3(Mathf.RoundToInt(placerPosition.x), 0, Mathf.RoundToInt(placerPosition.z));

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
