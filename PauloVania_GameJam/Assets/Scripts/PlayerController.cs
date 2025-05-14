using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{

    Rigidbody2D rb;
    Animator playerAnimator;
    SpriteRenderer SR;

    [Header("Movement")]
    public bool Running;
    public bool Jumping;
    public bool facingRight = true;
    public float movementSpeed = 4;
    [Header("Jump Height")]
    public float jumpHeight = 8;
    private Vector2 movementInput = Vector2.zero;
    bool jumped;
    [Header("Attack")]
    [SerializeField] GameObject Attack1_HB;
    bool attack_pressed = false;
    bool attack_active;
    bool attackAction;
    bool isGrounded = true;
    public void onMove(InputAction.CallbackContext context){
        movementInput = context.ReadValue<Vector2>();
    }
    public void onJump(InputAction.CallbackContext context){
        jumped = context.action.triggered;
    }
    public void onAttack(InputAction.CallbackContext context){
        attackAction = context.action.triggered;
    }
    void Falling(){

    }
    void Jump(){
        if(jumped && isGrounded){
            rb.AddForce(transform.up*jumpHeight*100);
            isGrounded = false;
            playerAnimator.SetTrigger("Jumping");
        }
    }
    void Attack1(){
        if(attackAction){
            if(!attack_pressed && !attack_active){
                attack_pressed = true;
                playerAnimator.SetTrigger("AttackTrigger");
                attack_active = true;
            }
        }
        else{attack_pressed = false;}
    }
    void Move(){
        rb.linearVelocity = new Vector2(movementInput.x*movementSpeed, rb.linearVelocity.y);
        if(Mathf.Abs(movementInput.x) > 0.01f){
            playerAnimator.SetBool("Running", true);
        }
        else{
            playerAnimator.SetBool("Running", false);
        }
        SR.flipX = rb.linearVelocity.x < 0.0f;
        
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SR = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Jump();
        Move();
        Attack1();
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("VelocityY", rb.linearVelocity.y);

        if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("PlayerAttack1")){
            attack_active = true;
            Attack1_HB.SetActive(true);
        }
        else{
            attack_active = false;
            Attack1_HB.SetActive(false);
        }
    }
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Floor"){
            isGrounded = true;
        }
    }
}
