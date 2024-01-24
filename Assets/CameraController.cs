using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 _offset;

    [SerializeField]
    private Transform _target;

    private void Start()
    {
        if (_target == null)
            return;

        transform.parent = null;

        _offset = transform.position - _target.position;
    }

    private void Update()
    {
        if (_target == null)
            return;

        Vector3 targetPos = _target.position + _offset;
        Vector3 lerp = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 35);
        transform.position = lerp;
    }
}
