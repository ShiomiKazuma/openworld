using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private InputKeyboard _inputKeyboard;
    [SerializeField] private float _moveSpeed = 5.0f;
    [SerializeField] private Camera _playerCamera = null;
    Rigidbody _rb;

    private void Awake()
    {
        _inputKeyboard.OnStartEvent = () => _animator.SetBool("IsMove", true);
        _inputKeyboard.OnCancelEvent = () => _animator.SetBool("IsMove", false);
        _rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (_inputKeyboard.IsInput)
        {
            Move(_inputKeyboard.Vector);
        }
    }
    private void Move(Vector2 vector)
    {
        var moveVector = new Vector3(vector.x, 0f, vector.y).normalized;
        //Debug.Log(vector.x + " " + vector.y);
        _rb.AddForce(moveVector * _moveSpeed, ForceMode.Force);
    }
}
