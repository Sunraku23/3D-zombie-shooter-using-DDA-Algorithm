using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera aimCamera;      // drag your Main Camera here
    [SerializeField] private Animator playerAnimator; // drag the player's Animator

    [Header("Gun Settings")]
    [SerializeField] private float range = 100f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private LayerMask hittableLayers; // set to exclude the Player layer

    void Update()
    {
        // Legacy Input Manager — left click to fire
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Trigger the fire animation regardless of hit/miss
        playerAnimator.SetTrigger("Shoot");

        // Ray starts at the CAMERA, not the gun muzzle
        Ray ray = new Ray(aimCamera.transform.position, aimCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, range, hittableLayers))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 0.5f);
            // Try to damage whatever we hit, if it can take damage
            if (hit.collider.TryGetComponent<IDamageable>(out var damageable))
            {
                // Draw a RED line from camera to the actual hit point (confirms real contact)

                damageable.TakeDamage(damage);
            }
        }
        else
        {
            // Draw a GRAY line for the full range (confirms the ray fired but hit nothing)
            Debug.DrawRay(ray.origin, ray.direction * range, Color.gray, 0.5f);
        }
    }
}