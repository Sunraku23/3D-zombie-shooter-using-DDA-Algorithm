using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float range = 50f;
    [SerializeField] private float fireRate = 0.3f; // jeda antar tembakan
    [SerializeField] private LayerMask hittableLayers;

    [Header("References")]
    [SerializeField] private Camera playerCamera; // drag Main Camera di sini

    private float nextFireTime = 0f;

    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        // Raycast dari tengah layar (crosshair) ke depan
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red, 1f);

        if (Physics.Raycast(ray, out hit, range, hittableLayers))
        {
            Debug.Log("Kena: " + hit.collider.name);

            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }
        }
    }
}