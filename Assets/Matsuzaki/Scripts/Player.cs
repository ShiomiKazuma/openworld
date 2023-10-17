using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private InputKeyboard _inputKeyboard;
    [SerializeField] private float _moveSpeed = 5.0f;

    private void Awake()
    {
        _inputKeyboard.OnStartEvent = () => _animator.SetBool("IsMove", true);
        _inputKeyboard.OnCancelEvent = () => _animator.SetBool("IsMove", false);
    }
    private void Update()
    {
        if(_inputKeyboard.IsInput)
        {
            Move(_inputKeyboard.Vector);
        }
    }
    private void Move(Vector2 vector)
    {
        var moveVector = new Vector3(vector.x, 0f, vector.y) * _moveSpeed * Time.deltaTime;
        var movePosition = transform.position + moveVector;
        transform.LookAt(movePosition);
        transform.position = movePosition;
    }
}
