using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform; // drag Main Camera di sini

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 10f; // makin besar = makin cepat "nempel" ke arah kamera

    void Update()
    {
        // 1. Ambil arah depan kamera (Vector3), tapi nol-in sumbu Y
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f; // biar player nggak ikut nunduk/mendongak
        camForward.Normalize(); // panjang vector dijadiin 1, biar arah aja yang dipakai

        // 2. Ubah arah (Vector3) itu jadi rotasi (Quaternion) yang "menghadap ke sana"
        Quaternion targetRotation = Quaternion.LookRotation(camForward);

        // 3. Putar rotasi player secara bertahap menuju targetRotation (bukan langsung snap)
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}