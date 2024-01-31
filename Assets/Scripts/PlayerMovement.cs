using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : MonoBehaviour
{
    private float _walkSpeed = 5f;

    private Vector3 up => Vector3.forward + Vector3.right;
    private Vector3 right => Vector3.back + Vector3.right;

    private Vector3 _lastInput;
    private Vector3 _lastVelocity;

    private CharacterController _cc;

    private Transform _modelTransform;

    private bool _canRun = false;

    private Player _owner;

    // Start is called before the first frame update
    void Start()
    {
        _owner = GetComponent<Player>();
        if(_owner == null || !_owner.IsLocalPlayer)
        {
            return;
        }

        _cc = GetComponent<CharacterController>();
        _modelTransform = transform.Find("Model");

        _canRun = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canRun)
            return;

        CheckInput();
        Move();
        RotateModel();
    }

    private void CheckInput()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var curInput = up * vertical + right * horizontal;

        _lastInput = Vector3.Lerp(_lastInput, curInput, Time.deltaTime * 45);
    }

    private void Move()
    {
        var frameVelocity = _lastInput * _walkSpeed * Time.deltaTime;

        _lastVelocity = frameVelocity;

        _lastVelocity.y += Physics.gravity.y * Time.deltaTime;

        _cc.Move(_lastVelocity);
    }

    private void RotateModel()
    {
        if (_lastInput == Vector3.zero)
            return;

        var angle = Vector3.SignedAngle(Vector3.forward, _lastInput, Vector3.up);

        var prevRot = _modelTransform.rotation;
        var newRot = Quaternion.Euler(0, angle, 0);

        _modelTransform.rotation = Quaternion.Lerp(prevRot, newRot, Time.deltaTime * 5 * _lastInput.magnitude);
    }
}
