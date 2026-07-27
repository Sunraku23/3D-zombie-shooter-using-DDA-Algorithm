using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private Transform vfxred;
    [SerializeField] private Transform vfxgreen;

    [SerializeField] private float speed = 40f; // exposed so you can tune it in Inspector
    private Rigidbody bulletRb;
    

    private void Awake()
    {
        bulletRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        // linearVelocity = movement through space, in Unity 6 naming
        bulletRb.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
          Destroy(gameObject); // destroy the whole bullet object, not just the Rigidbody
    }
}