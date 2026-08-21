using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;

    private CharacterController controller;
    private float verticalVelocity;

    // Cache hash parameter, sama kaya di ZombieAI
    private static readonly int SpeedParam = Animator.StringToHash("Speed");

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 1. Baca input mentah dari keyboard (WASD / Arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        // 2. Ubah input jadi arah gerak RELATIF terhadap kamera
        // (bukan relatif world axis, biar "W" selalu "maju sesuai kamera", bukan maju sesuai sumbu Z dunia)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        // 3. Tentukan kecepatan aktual berdasarkan input magnitude + mode jalan/lari
        float inputMagnitude = Mathf.Clamp01(new Vector2(horizontal, vertical).magnitude);
        float targetSpeed = (isRunning ? runSpeed : walkSpeed) * inputMagnitude;

        // 4. Gravity manual (CharacterController tidak otomatis kena gravity kaya Rigidbody)
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f; // kecil aja, biar tetap "nempel" ke ground
        verticalVelocity += gravity * Time.deltaTime;

        // 5. Gabungkan gerak horizontal + vertical, lalu eksekusi lewat CharacterController
        Vector3 finalMove = moveDirection * targetSpeed;
        finalMove.y = verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);

        // 6. Kirim speed ternormalisasi ke Animator (sama pola kaya ZombieAI kemarin)
        float normalizedSpeed = targetSpeed / runSpeed; // 0 = diam, 1 = full run
        animator.SetFloat(SpeedParam, normalizedSpeed, 0.1f, Time.deltaTime);
    }
}