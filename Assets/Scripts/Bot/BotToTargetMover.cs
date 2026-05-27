using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class BotToTargetMover : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _targetReachedDistance = 1f;

    private Transform _target;
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
            Vector3 direction = SetDirection(_target.transform.position);

            SetRotation(direction);
            MoveForward(direction);

            Vector3 distance = _target.position - transform.position;

            if (distance.sqrMagnitude < _targetReachedDistance * _targetReachedDistance)
            {
                Transform reachedTarget = _target;

                _target = null;
                _rigidbody.velocity = Vector3.zero;

                TargetReached?.Invoke(reachedTarget);
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    private Vector3 SetDirection(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        return direction;
    }

    private void SetRotation(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void MoveForward(Vector3 direction)
    {
        Vector3 velocity = direction * _speed;
        velocity.y = _rigidbody.velocity.y;

        _rigidbody.velocity = velocity;
    }
}