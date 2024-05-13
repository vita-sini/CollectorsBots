using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float scanDelay = 0.1f;

    private Queue<Resource> _resourse = new Queue<Resource>();

    private float radius = 360f;
    private float maxDistance = Mathf.Infinity;

    private void Start()
    {
        StartCoroutine(Scanning());
    }

    public Resource GiveResource()
    {
        if (_resourse.Count > 0)
        {
            return _resourse.Dequeue();
        }

        return null;
    }

    private IEnumerator Scanning()
    {
        var waitSeconds = new WaitForSeconds(scanDelay);

        while (true)
        {
            yield return waitSeconds;

            RaycastHit[] raycastHits = Physics.SphereCastAll(transform.position, radius, Vector3.one, maxDistance, _layerMask);

            foreach (RaycastHit hit in raycastHits)
            {
                if (hit.collider.TryGetComponent(out Resource resource))
                {
                    if (!_resourse.Contains(resource) && resource.IsBusy == false)
                    {
                        _resourse.Enqueue(resource);
                    }
                }
            }

            yield return waitSeconds;
        }
    }
}
