using UnityEngine;

public class JogadorAndando : Estado
{
    [SerializeField] private Jogador jogador;
    public float jogadorHeight;
    public LayerMask whatIsGround;
    [SerializeField] private bool grounded;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airMultipler;
    [SerializeField] private bool readyToJump = true;
    [SerializeField] public Transform orientation;
    public float groundDrag;
    private float horizontalInput;
    private Vector3 moveDirection;
    private float verticalInput;
    [SerializeField] private  AudioSource audioAndar;
    


    public override void Enter()
    {
        // jogador.anim.play_animation("idle");
        readyToJump = true;
        moveSpeed = jogador.getVelocidade();
    }
    


    public override void FixedDo()
    {
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
        else {
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
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     jogador.MoveCelular();
        // }
        
        if (Input.GetKeyDown(KeyCode.E) && jogador.inventario.itemPerto != null)
        {
            jogador.inventario.AdicionarItem();
        }
        // usar  Item 
        // Dropar Item 

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
        if (grounded){
            jogador.rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
            jogador.rb.linearDamping = groundDrag;
        }
        else if (!grounded)
            jogador.rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultipler), ForceMode.Force);
        
        if(transform.position.y <= -10 )
            jogador.Morte();
    }

    private void SpeedControl()
    {
        var flatVel = new Vector3(jogador.rb.linearVelocity.x, jogador.rb.linearVelocity.y, jogador.rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            var limitedVel = flatVel.normalized * moveSpeed;
            jogador.rb.linearVelocity = new Vector3(limitedVel.x, jogador.rb.linearVelocity.y, limitedVel.z);
        }

        // if (flatVel.magnitude > 0)
        //     jogador.anim.play_animation("walking");
        // else
        //     jogador.anim.play_animation("idle");
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
