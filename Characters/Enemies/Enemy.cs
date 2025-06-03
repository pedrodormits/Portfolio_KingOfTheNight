using UnityEngine;

public class Enemy : MonoBehaviour, IStunnable
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Enemy _enemy;
    private Vector3 _target;
    
    private bool _isStunned;
    private float _stunTimer;
    
    public bool IsStunned =>_isStunned;
    
    private void Update()
    {
        if (_isStunned)
        {
            _stunTimer -= Time.deltaTime;
            if (_stunTimer <= 0f)
                _isStunned = false;
        }

        if (!_isStunned)
        {
            _target = pointB.position;
            MoveTowardsTarget();
        }
    }
    
    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            _target = _target == pointA.position ? pointB.position : pointA.position;
        }
    }

    public void Stun(float duration)
    {
        _isStunned = true;
        _stunTimer = duration;
    }
}