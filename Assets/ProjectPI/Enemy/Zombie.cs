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
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator; // BARU: drag Animator component zombie di sini

    [Header("Death Settings")]
    [SerializeField] private float destroyDelay = 3f; // BARU: sesuaikan sama durasi animasi Die

    private NavMeshAgent agent;
    private State currentState = State.Chase;
    private int currentHealth;
    private float nextAttackTime;

    // BARU: cache nama parameter jadi hash int, lebih efisien daripada pakai string tiap frame
    private static readonly int SpeedParam = Animator.StringToHash("Speed");
    private static readonly int AttackParam = Animator.StringToHash("Attack");
    private static readonly int DeadParam = Animator.StringToHash("IsDead");

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentState == State.Dead) return;

        float distance = Vector3.Distance(transform.position, player.position);

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
                agent.SetDestination(transform.position);
                TryAttack();
                break;
        }

        // BARU: kirim kecepatan aktual ke Animator tiap frame, dengan smoothing (dampTime = 0.1f)
        // agent.velocity.magnitude = kecepatan real NavMeshAgent, bukan raw input (zombie kan AI, bukan dikontrol manusia)
        float normalizedSpeed = agent.velocity.magnitude / agent.speed; // hasilnya 0-1, cocok buat Blend Tree threshold
        animator.SetFloat(SpeedParam, normalizedSpeed, 0.1f, Time.deltaTime);
    }

    void TryAttack()
    {
        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            animator.SetTrigger(AttackParam); // BARU: trigger animasi attack, one-shot
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
            animator.SetBool(DeadParam, true); // BARU: masuk state Die, stay disana
            Debug.Log("Zombie died");

            Destroy(gameObject, destroyDelay); // BARU: hapus objek setelah animasi Die selesai diputar
        }
    }
}