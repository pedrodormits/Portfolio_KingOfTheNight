using System.Collections;
using UnityEngine;

public class Vampire : PlayerInput
{
    #region variables
    [Header("Nightfall Step")]
    private int _extraAirJumps = 1;
    private int _remainingAirJumps;

    [Header("Chiroptera Drift")]
    [SerializeField] private float _driftSpeed;

    private bool _isDrifting;

    [Header("Umbral Seeker")]
    [SerializeField] private Transform _batLauncher;
    [SerializeField] private GameObject _bat;
    
    [Header("Crimson Gaze")]
    [SerializeField] private float _stunRange;
    [SerializeField] private float _stunDuration;
    #endregion

    #region Awake
    protected override void Awake()
    {
        base.Awake();
        _remainingAirJumps = _extraAirJumps;
    }
    #endregion
    
    #region Movement
    protected override void Move()
    {
        if (_isDrifting)
            return;

        base.Move();
    }
    #endregion

    #region Nightfall Step
    protected override void ExecuteJump()
    {
        if (_isAttacking || !Input.GetButtonDown("Jump"))
            return;

        if (IsGrounded)
            _jumpRequested = true;

        else if (_remainingAirJumps > 0)
        {
            _jumpRequested = true;
            _remainingAirJumps--;
        }
    }
    #endregion
    
    #region Chiroptera Drift
    protected override IEnumerator SpecialAbility1()
    {
        _isDrifting = true;
        _playerAnim.PlaySpecialAbility1Animation();
        yield return new WaitForSeconds(_specialAbility1Duration);
        _isDrifting = false;
    }

    public void PerformDrift()
    {
        Vector3 driftDirection = transform.forward;
        _rb.velocity = new Vector3(driftDirection.x * _driftSpeed, _rb.velocity.y,
            driftDirection.z * _driftSpeed);
    }
    #endregion
    
    #region Umbral Seeker
    protected override IEnumerator SpecialAbility2()
    {
        _isAttacking = true;
        _playerAnim.PlaySpecialAbility2Animation();
        yield return new WaitForSeconds(_specialAbility2Duration);
        _isAttacking = false;
    }

    public void SendBat()
    {
        Instantiate(_bat, _batLauncher.position, _batLauncher.rotation);
    }
    #endregion
    
    #region Crimson Gaze
    protected override IEnumerator SpecialAbility3()
    {
        _isAttacking = true;
        _playerAnim.PlaySpecialAbility3Animation();
        yield return new WaitForSeconds(_specialAbility3Duration);
        _isAttacking = false;
    }

    public void CastGaze()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _stunRange);
        
        foreach (var hit in hitColliders)
        {
            if (hit.TryGetComponent<IStunnable>(out var stunnable))
            {
                stunnable.Stun(_stunRange);
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stunRange);
    }
    #endregion
    
    #region Ground Collision
    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            _remainingAirJumps = _extraAirJumps;
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
    #endregion 
}