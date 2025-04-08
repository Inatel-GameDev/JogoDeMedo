using UnityEngine;
using Mirror;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    public Transform cameraTransform;
    public float walkSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    string playerName = "Player";
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (!isLocalPlayer && cameraTransform != null)
            cameraTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        HandleMovement();
        CmdSendPosition(transform.position);
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * walkSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Envia a posição atual ao servidor
    [Command]
    void CmdSendPosition(Vector3 pos)
    {
        RpcUpdatePosition(pos);
    }

    // Replica a posição pros outros jogadores
    [ClientRpc]
    void RpcUpdatePosition(Vector3 pos)
    {
        if (isLocalPlayer) return;
        transform.position = pos;
    }

    public void SetNome(string nome)
    {
        playerName = nome;
    }

}
