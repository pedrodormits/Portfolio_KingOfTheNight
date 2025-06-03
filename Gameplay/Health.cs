using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Components")]
    private PlayerInput _playerInput;
    private PlayerAnimation _playerAnimation;
    
    [Header("Health")]
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _maxHealth;
    [SerializeField] private float _disableInputDuration;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);

        if (_playerInput)
            StartCoroutine(DisableInputTemporarily());

        if (_currentHealth <= 0)
            Die();
    }

    private IEnumerator DisableInputTemporarily()
    {
        _playerInput.enabled = false;
        _playerAnimation?.PlayHurtAnimation();
        yield return new WaitForSeconds(_disableInputDuration);

        if (_currentHealth > 0)
            _playerInput.enabled = true;
    }

    private void Die()
    {
        if (_playerInput)
        {
            _playerInput.enabled = false;
            _playerAnimation.PlayDeathAnimation();
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }
}