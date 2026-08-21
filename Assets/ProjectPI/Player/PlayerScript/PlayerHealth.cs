using System;
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
        currentHealth -= amount ;
        currentHealth = Mathf.Max(currentHealth, 0); // biar gak minus, jelek buat UI health bar nanti

        Debug.Log("Player HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnPlayerDied?.Invoke(); // fire event, siapapun yg subscribe akan dikasih tau
        }
    }
}