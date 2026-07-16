using UnityEngine;
using Unity.Cinemachine;

public class LegacyMouseLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineCamera virtualCamera; // drag Virtual Camera di sini
    private CinemachinePanTilt panTilt;

    [Header("Settings")]
    [SerializeField] private float sensitivity = 5f;
    [SerializeField] private bool invertY = false;

    void Start()
    {
        // Ambil komponen PanTilt dari Virtual Camera secara otomatis
        panTilt = virtualCamera.GetComponent<CinemachinePanTilt>();
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * (invertY ? 1f : -1f);

        // Tambahin nilai mouse ke axis yang sudah ada (bukan replace, biar akumulatif)
        panTilt.PanAxis.Value += mouseX;
        panTilt.TiltAxis.Value += mouseY;

        // Clamp Tilt manual, biar nggak bisa nunduk/mendongak lewat batas wajar
        panTilt.TiltAxis.Value = Mathf.Clamp(
            panTilt.TiltAxis.Value,
            panTilt.TiltAxis.Range.x,
            panTilt.TiltAxis.Range.y
        );
    }
}