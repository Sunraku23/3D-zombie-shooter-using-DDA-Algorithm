using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    // event: siapapun bisa subscribe tanpa PlayerHealth perlu tau siapa mereka
    public event Action OnPlayerDied;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        // TODO: kurangin currentHealth, cek kalau <= 0
    }
}