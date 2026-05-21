using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BotToTargetMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _targetReachedDistance = 1f;

    private Transform _target;
    private Vector3 _direction;
    private Rigidbody _rigidbody;

    public event Action<Transform> TargetReached;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            SetDirection();
            MoveForward();

            Vector3 distance = _target.position - transform.position;

            if (distance.sqrMagnitude < _targetReachedDistance * _targetReachedDistance)
            {
                Transform reachedTarget = _target;

                _target = null;
                _rigidbody.velocity = Vector3.zero;

                TargetReached?.Invoke(reachedTarget);

                return;
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private void SetDirection()
    {
        _direction = (_target.transform.position - transform.position).normalized;

        SetRotation();
    }

    private void SetRotation()
    {
        transform.rotation = Quaternion.LookRotation(_direction);
    }

    private void MoveForward()
    {
        Vector3 velocity = _direction * _speed;
        velocity.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velocity;
    }
}