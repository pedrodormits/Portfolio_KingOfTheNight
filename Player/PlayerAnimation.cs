using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Components")]
    private Animator _anim;
    private Rigidbody _rb;
    private PlayerInput _playerInput;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        UpdateIsGrounded();
        PlayIdleMoveAnimations();
        PlayJumpFallAnimations();
    }

    private void UpdateIsGrounded()
    {
        _anim.SetBool("isGrounded", _playerInput.IsGrounded);
    }

    private void PlayIdleMoveAnimations()
    {
        Vector3 horizontalVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        _anim.SetFloat("velocityMagnitude", horizontalVelocity.magnitude);
    }

    private void PlayJumpFallAnimations()
    {
        _anim.SetFloat("verticalVelocity", _rb.velocity.y);
    }

    public void PlayLightAttackAnimation() => _anim.SetTrigger("lightAttack");

    public void PlayHeavyAttackAnimation() => _anim.SetTrigger("heavyAttack");

    public void PlaySpecialAbility1Animation() => _anim.SetTrigger("specialAbility1");

    public void PlaySpecialAbility2Animation() => _anim.SetTrigger("specialAbility2");

    public void PlaySpecialAbility3Animation() => _anim.SetTrigger("specialAbility3");

    public void PlayHurtAnimation() => _anim.SetTrigger("takeDamage");

    public void PlayDeathAnimation() => _anim.SetTrigger("die");
}