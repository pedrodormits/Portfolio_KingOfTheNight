using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerInput : MonoBehaviour
{
    #region variables
    [Header("Components")]
    protected Rigidbody _rb;
    protected PlayerAnimation _playerAnim;

    [Header("Movement")]
    [SerializeField] protected float _speed;
    [SerializeField] protected float _jumpForce;

    [HideInInspector] public bool IsGrounded;

    protected bool _jumpRequested;

    [Header("Combat")]
    [SerializeField] protected float _lightAttackDuration;
    [SerializeField] protected float _heavyAttackDuration;
    [SerializeField] protected float _specialAbility1Duration;
    [SerializeField] protected float _specialAbility2Duration;
    [SerializeField] protected float _specialAbility3Duration;

    protected bool _isAttacking;
    
    [Header("Cooldowns")]
    [SerializeField] protected float _lightAttackCooldown;
    [SerializeField] protected float _heavyAttackCooldown;
    [SerializeField] protected float _specialAbility1Cooldown;
    [SerializeField] protected float _specialAbility2Cooldown;
    [SerializeField] protected float _specialAbility3Cooldown;

    private float _lastLightAttackTime = -Mathf.Infinity;
    private float _lastHeavyAttackTime = -Mathf.Infinity;
    private float _lastSpecial1Time = -Mathf.Infinity;
    private float _lastSpecial2Time = -Mathf.Infinity;
    private float _lastSpecial3Time = -Mathf.Infinity;
    #endregion
    
    protected virtual void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerAnim = GetComponent<PlayerAnimation>();
    }

    protected virtual void Update()
    {
        ExecuteJump();
        ExecuteLightAttack();
        ExecuteHeavyAttack();
        ExecuteSpecialAbility1();
        ExecuteSpecialAbility2();
        ExecuteSpecialAbility3();
    }

    protected virtual void FixedUpdate()
    {
        Move();
        Jump();
    }

    protected virtual void Move()
    {
        if (!_isAttacking)
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0,
                Input.GetAxis("Vertical")).normalized;
            _rb.velocity = new Vector3(input.x * _speed, _rb.velocity.y,
                input.z * _speed);

            if (input.magnitude > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(input);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    targetRotation, Time.deltaTime * 10f);
            }
        }
        else
        {
            _rb.velocity = new Vector3(0, _rb.velocity.y, 0);
        }
    }

    protected virtual void ExecuteJump()
    {
        if (!_isAttacking && Input.GetButtonDown("Jump") && IsGrounded)
            _jumpRequested = true;
    }

    protected virtual void Jump()
    {
        if (_jumpRequested)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            IsGrounded = false;
            _jumpRequested = false;
        }
    }

    protected virtual void ExecuteLightAttack()
    {
        if (IsGrounded && Input.GetButtonDown("Fire1") && Time.time >= _lastLightAttackTime + _lightAttackCooldown)
        {
            _lastLightAttackTime = Time.time;
            StartCoroutine(LightAttack());
        }
    }

    IEnumerator LightAttack()
    {
        _isAttacking = true;
        _playerAnim.PlayLightAttackAnimation();
        yield return new WaitForSeconds(_lightAttackDuration);
        _isAttacking = false;
    }

    protected virtual void ExecuteHeavyAttack()
    {
        if (IsGrounded && Input.GetButtonDown("Fire2") && Time.time >= _lastHeavyAttackTime + _heavyAttackCooldown)
        {
            _lastHeavyAttackTime = Time.time;
            StartCoroutine(HeavyAttack());
        }
    }

    IEnumerator HeavyAttack()
    {
        _isAttacking = true;
        _playerAnim.PlayHeavyAttackAnimation();
        yield return new WaitForSeconds(_heavyAttackDuration);
        _isAttacking = false;
    }

    protected virtual void ExecuteSpecialAbility1()
    {
        if (IsGrounded && Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= _lastSpecial1Time + _specialAbility1Cooldown)
        {
            _lastSpecial1Time = Time.time;
            StartCoroutine(SpecialAbility1());
        }
    }

    protected virtual IEnumerator SpecialAbility1()
    {
        _isAttacking = true;
        _playerAnim.PlaySpecialAbility1Animation();
        yield return new WaitForSeconds(_specialAbility1Duration);
        _isAttacking = false;
    }

    protected virtual void ExecuteSpecialAbility2()
    {
        if (IsGrounded && Input.GetKeyDown(KeyCode.E) && Time.time >= _lastSpecial2Time + _specialAbility2Cooldown)
        {
            _lastSpecial2Time = Time.time;
            StartCoroutine(SpecialAbility2());
        }
    }

    protected virtual IEnumerator SpecialAbility2()
    {
        _isAttacking = true;
        _playerAnim.PlaySpecialAbility2Animation();
        yield return new WaitForSeconds(_specialAbility2Duration);
        _isAttacking = false;
    }

    protected virtual void ExecuteSpecialAbility3()
    {
        if (IsGrounded && Input.GetKeyDown(KeyCode.F) && Time.time >= _lastSpecial3Time + _specialAbility3Cooldown)
        {
            _lastSpecial3Time = Time.time;
            StartCoroutine(SpecialAbility3());
        }
    }

    protected virtual IEnumerator SpecialAbility3()
    {
        _isAttacking = true;
        _playerAnim.PlaySpecialAbility3Animation();
        yield return new WaitForSeconds(_specialAbility3Duration);
        _isAttacking = false;
    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = true;
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = false;
    }
}