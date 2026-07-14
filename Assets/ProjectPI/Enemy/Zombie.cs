using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour, IDamageable
{
    private enum State { Chase, Attack, Dead }

    [Header("Stats")]
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("References")]
    [SerializeField] private Transform player; // drag Player di sini (sementara, nanti bisa auto-find)

    private NavMeshAgent agent;
    private State currentState = State.Chase;
    private int currentHealth;
    private float nextAttackTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Transisi state berdasarkan jarak
        if (distance <= attackRange)
            currentState = State.Attack;
        else
            currentState = State.Chase;

        switch (currentState)
        {
            case State.Chase:
                agent.SetDestination(player.position);
                break;
            case State.Attack:
                agent.SetDestination(transform.position); // berhenti di tempat
                TryAttack();
                break;
        }
    }

    void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            Debug.Log("Zombie attack player for " + attackDamage);
            // TODO: panggil player.TakeDamage() setelah Player punya IDamageable juga
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Zombie HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            currentState = State.Dead;
            agent.isStopped = true;
            Debug.Log("Zombie died");
            // TODO: play death animation, destroy after delay
        }
    }
}