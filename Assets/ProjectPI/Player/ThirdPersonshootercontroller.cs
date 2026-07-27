using UnityEngine;
using Unity.Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;


public class ThirdPersonshootercontroller : MonoBehaviour
{
    [SerializeField]
    private CinemachineCamera aimVirtualcamera;
    [SerializeField]
    private float PlayerSens;
    [SerializeField]
    private float Aimsens;
    [SerializeField]
    private LayerMask aimColiderMask = new LayerMask();
    [SerializeField]
    private Transform debugTransform;
    [SerializeField]
    private Transform bullet;
    [SerializeField]
    private Transform bulletSpawner;
    [SerializeField] private Transform vfxred;
    [SerializeField] private Transform vfxgreen;

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private Animator animator;

  



    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPost = new Vector2(Screen.width /2f, Screen.height /2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPost);
        Transform hitTransfrom = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColiderMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransfrom = raycastHit.transform;
        }
        if (starterAssetsInputs.aim)
        {
            aimVirtualcamera.gameObject.SetActive(true);
            thirdPersonController.Setsensivity(Aimsens);
            thirdPersonController.SetRotateOnmove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            Vector3 worldAimtarget = mouseWorldPosition;
            worldAimtarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimtarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualcamera.gameObject.SetActive(false);
            thirdPersonController.Setsensivity(PlayerSens);
            thirdPersonController.SetRotateOnmove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (starterAssetsInputs.Shoot)
        {
            if (hitTransfrom != null)
            {
                if (hitTransfrom.GetComponent<Target>() != null)
                {
                    Instantiate(vfxgreen, raycastHit.point, Quaternion.identity);
                }
                else
                {
                    Instantiate(vfxred, raycastHit.point, Quaternion.identity);
                }
            }

            //Vector3 aimDir = (mouseWorldPosition - bulletSpawner.position).normalized;
            //Instantiate(bullet, bulletSpawner.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.Shoot = false;
        }

    }
}
