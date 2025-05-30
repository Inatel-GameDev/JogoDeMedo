using UnityEngine;

public class JogadorAtivo : Estado
{
    [SerializeField] private Jogador jogador;
    public float jogadorHeight = 2f;
    public LayerMask whatIsGround;
    [SerializeField] private bool grounded;
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airMultipler;
    [SerializeField] private bool readyToJump = true;
    [SerializeField] public Transform orientation;
    public float groundDrag;
    private float horizontalInput;
    private Vector3 moveDirection;
    private float verticalInput;
    [SerializeField] private AudioSource audioAndar;

    public override void Enter()
    {
        readyToJump = true;
    }

    public override void FixedDo()
    {
        /*// Ajuste importante no raycast:
        Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f;
        float raycastDistance = jogadorHeight / 2f + 0.3f;

        grounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, (jogadorHeight / 2f) + 0.5f, whatIsGround);
        Debug.DrawRay(raycastOrigin, Vector3.down * raycastDistance, grounded ? Color.green : Color.red);

        jogador.rb.linearDamping = grounded ? groundDrag : 0;

        if (jogador.rb.linearVelocity.x != 0 || jogador.rb.linearVelocity.z != 0)
        {
            if (!audioAndar.isPlaying)*/
        grounded = Physics.Raycast(transform.position, Vector3.down, jogadorHeight / 2f, whatIsGround);
        Debug.DrawRay(transform.position, Vector3.down * (jogadorHeight / 2), grounded ? Color.green : Color.red);
        if (grounded)
            jogador.rb.linearDamping = groundDrag;
        else
            jogador.rb.linearDamping = 0;
        if (jogador.rb.linearVelocity.x != 0 || jogador.rb.linearVelocity.z != 0){
            if(!audioAndar.isPlaying)
                audioAndar.Play();
        }
        else
        {
            audioAndar.Pause();
        }

        MyInput();
        Movejogador();
        SpeedControl();
    }

    public override void Exit()
    {
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void Movejogador()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (grounded)
        {
            jogador.rb.AddForce(moveDirection.normalized * (jogador.velocidade * 10f), ForceMode.Force);
            jogador.rb.linearDamping = groundDrag;
        }
        else if (!grounded)
        {
            jogador.rb.AddForce(moveDirection.normalized * (jogador.velocidade * 10f * airMultipler), ForceMode.Force);
        }

        if (transform.position.y <= -10)
            jogador.Morte();
    }

    private void SpeedControl()
    {
        var flatVel = new Vector3(jogador.rb.linearVelocity.x, jogador.rb.linearVelocity.y, jogador.rb.linearVelocity.z);

        if (flatVel.magnitude > jogador.velocidade)
        {
            var limitedVel = flatVel.normalized * jogador.velocidade;
            jogador.rb.linearVelocity = new Vector3(limitedVel.x, jogador.rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        jogador.rb.linearVelocity = new Vector3(jogador.rb.linearVelocity.x / 0.5f, 0f, jogador.rb.linearVelocity.z / 0.5f);
        jogador.rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
