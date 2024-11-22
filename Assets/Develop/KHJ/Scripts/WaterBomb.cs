using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WaterBomb : MonoBehaviour, IExplosionInteractable
{
    [SerializeField] private float _lifeTime;
    [SerializeField] private int _range = 1;
    [SerializeField] private LayerMask _waterBombLayerMask;
    [SerializeField] private LayerMask _judgeLayerMask;

    [Header("Explosion Effect")]
    [SerializeField] GameObject _effect;

    private WaitForSeconds _delay;
    private ObjectPool<WaterBomb> _objectPool;

    private bool _isExploded;

    public ObjectPool<WaterBomb> ObjectPool { set { _objectPool = value; } }
    public int Range { set { _range = value; } }

    private void OnEnable()
    {
        _isExploded = false;

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

        Explode();
    }

    private void Explode()
    {
        _isExploded = true;

        int upEnd = _range;
        int downEnd = _range;
        int rightEnd = _range;
        int leftEnd = _range;

        // Judge explosion hit
        ProceedWaterStream(transform.forward, _range);
        ProceedWaterStream(-transform.forward, _range);
        ProceedWaterStream(transform.right, _range);
        ProceedWaterStream(-transform.right, _range);

        // Visual Effect
        // center
        Instantiate(_effect, transform.position, Quaternion.identity);
        // up
        for (int i = 1; i <= upEnd; i++)
            Instantiate(_effect, transform.position + i * transform.forward, Quaternion.identity);
        // down
        for (int i = 1; i <= downEnd; i++)
            Instantiate(_effect, transform.position - i * transform.forward, Quaternion.identity);
        // right
        for (int i = 1; i <= rightEnd; i++)
            Instantiate(_effect, transform.position + i * transform.right, Quaternion.identity);
        // end
        for (int i = 1; i <= leftEnd; i++)
            Instantiate(_effect, transform.position - i * transform.right, Quaternion.identity);

        // Return to pool
        _objectPool.Release(this);
    }

    private void ProceedWaterStream(Vector3 direction, int range)
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 offset = new Vector3(0, 0.5f, 0);
        bool isContinue = true;

        for(int i = 0; i < range; i++)
        {
            if (Physics.Raycast(origin + offset, direction, out hit, 1f, _judgeLayerMask))
            {
                // Find IExplosionInteractable
                IExplosionInteractable interactable = null;
                Transform curTransform = hit.transform;
                while (curTransform != null)
                {
                    interactable = curTransform.GetComponent<IExplosionInteractable>();
                    if (interactable != null)
                        break;

                    curTransform = curTransform.parent;
                }

                if (interactable == null)
                    break;

                // Interact
                isContinue = interactable.Interact();
                if (!isContinue)
                    break;
            }

            offset += direction;
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
        Collider[] others = Physics.OverlapSphere(location, 0.3f, _waterBombLayerMask);
        if (others.Length > 0)
        {
            _objectPool.Release(this);
            return false;
        }

        // Move to location
        transform.position = location;
        return true;
    }

    public bool Interact()
    {
        if (!_isExploded)
        {
            Explode();
        }
        return false;
    }
}
