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

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;

    

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPost = new Vector2(Screen.width /2f, Screen.height /2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPost);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColiderMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }
        if (starterAssetsInputs.aim)
        {
            aimVirtualcamera.gameObject.SetActive(true);
            thirdPersonController.Setsensivity(Aimsens);
            thirdPersonController.SetRotateOnmove(false);

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
        }

        if (starterAssetsInputs.Shoot) 
        {
            Vector3 aimDir = (mouseWorldPosition - bulletSpawner.position).normalized;
            Instantiate(bullet, bulletSpawner.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.Shoot = false;
        }

    }
}
