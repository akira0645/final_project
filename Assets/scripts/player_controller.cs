using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//player need to have rb
[RequireComponent(typeof(Rigidbody2D),typeof(TouchingSpaceDirection))]

public class player_controller : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpImpuse=7f;
    public int jumpnum = 0;
    Vector2 moveInput;
    TouchingSpaceDirection touchingSpaceDirection;

    public float CuttentMoveSpeed { 
        get
        {
            if(CanMove)
            {
                if (IsMoving && !touchingSpaceDirection.IsOnWall)
                {
                    if (IsRunning)
                    {
                        return runSpeed;
                    }
                    else
                    {
                        return walkSpeed;
                    }
                }
                else
                {//airwalkspeed
                    return 0;
                }
            }
            else
            {//movement lock
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving { 
        get 
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
     }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _IsFacingRight = true;
    public bool IsFacingRight {
        get
        {
            return _IsFacingRight;
        }
        private set
        {
            if(_IsFacingRight!=value)
            {//flip
                transform.Rotate(0f, 180f, 0f);
            }
            _IsFacingRight = value;
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive 
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    Rigidbody2D rb;
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingSpaceDirection = GetComponent<TouchingSpaceDirection>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CuttentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x>0 && !IsFacingRight)
        {//foward
            IsFacingRight = true;
        }
        else if(moveInput.x<0 && IsFacingRight)
        {//flip
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //check if alive 
        /*if (context.started &&touchingSpaceDirection.IsGrounded &&CanMove)
         * if (context.started &&touchingSpaceDirection.IsGrounded &&CanMove)
        {
            jumpnum = 0;
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpuse);
        }
         */
        //¤G¬q¸õ
        if (context.started &&touchingSpaceDirection.IsGrounded &&CanMove)
        {
            jumpnum = 0;
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpuse);
        }
        if (context.started && !touchingSpaceDirection.IsGrounded && jumpnum!=1)
        {
            jumpnum++;
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpuse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }
}
