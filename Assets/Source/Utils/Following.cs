using UnityEngine;

public class Following : MonoBehaviour
{
    [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, -5f);
    [SerializeField] private float _smoothSpeed = 5f;

    private Transform _target;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null)
            return;

        Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}

