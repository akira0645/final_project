using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//player need to have rb
[RequireComponent(typeof(Rigidbody2D),typeof(TouchingSpaceDirection),typeof(AudioSource))]

public class player_controller : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpImpuse=7f;
    public int jumpnum = 0;
    Vector2 moveInput;
    TouchingSpaceDirection touchingSpaceDirection;
    Damageable damageable;
    public GameObject woodPrefab;
    public GameObject stonePrefab;
    public GameObject hp_bar;
    public int ammo_wood;
    public int ammo_stone;
    public Text woods;
    public Text stones;
    public AudioClip SE_player_shoot;
    public AudioClip SE_player_hurt;
    public AudioClip SE_player_death;
    public AudioClip SE_player_junp;
    AudioSource audioSource;

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
            /*if(_IsFacingRight!=value)
            {//flip
                transform.Rotate(0f, 180f, 0f);
            }*/
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
        damageable=GetComponent<Damageable>();
        touchingSpaceDirection = GetComponent<TouchingSpaceDirection>();
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        woods.text =""+ ammo_wood;
        stones.text =""+ ammo_stone;
        float pre = ((float)damageable.Health / (float)damageable.MaxHealth);
        hp_bar.transform.localScale = new Vector3(pre, hp_bar.transform.localScale.y, hp_bar.transform.localScale.z);
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
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(moveInput.x<0 && IsFacingRight)
        {//flip
            IsFacingRight = false;
            GetComponent<SpriteRenderer>().flipX = true;
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
            audioSource.PlayOneShot(SE_player_junp);
        }
        if (context.started && !touchingSpaceDirection.IsGrounded && jumpnum!=1)
        {
            jumpnum++;
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpuse);
            audioSource.PlayOneShot(SE_player_junp);
        }
    }

    public void OnAttack01(InputAction.CallbackContext context)
    {
        if(context.started&&ammo_wood>0)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
            if (IsFacingRight)
            {
                woodPrefab.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                woodPrefab.GetComponent<SpriteRenderer>().flipX = true;
            }
            ammo_wood--;
            Instantiate(woodPrefab, this.transform.position, Quaternion.identity);
            //audioSource.PlayOneShot(SE_player_shoot);
        }
    }
    public void OnAttack02(InputAction.CallbackContext context)
    {
        if (context.started&&ammo_stone>0)
        {
            if (IsFacingRight)
            {
                stonePrefab.GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                stonePrefab.GetComponent<SpriteRenderer>().flipX = true;
            }
            animator.SetTrigger(AnimationStrings.attackTrigger);
            ammo_stone--;
            Instantiate(stonePrefab, this.transform.position, Quaternion.identity);
            //audioSource.PlayOneShot(SE_player_shoot);
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //enemy
        if(collision.contacts[0].normal.x == -1 && (collision.gameObject.tag == "knight"||collision.gameObject.tag == "wc1" || collision.gameObject.tag == "wc2"))//left
        {
            if (collision.gameObject.tag == "wc1")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(10);
                if (!touchingSpaceDirection.IsOnWall && !touchingSpaceDirection.TestingIsCelling(-2f))
                {
                    this.transform.position = new Vector3(transform.gameObject.transform.position.x - 2f, transform.position.y, transform.position.z);
                    // rb.AddForce(new Vector2(50, 50));
                }
            }
            if (collision.gameObject.tag == "wc2")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(20);
                if (!touchingSpaceDirection.IsOnWall && !touchingSpaceDirection.TestingIsCelling(-2.5f))
                {
                    this.transform.position = new Vector3(transform.gameObject.transform.position.x - 2.5f, transform.position.y, transform.position.z);
                    // rb.AddForce(new Vector2(50, 50));
                }
            }
            if (collision.gameObject.tag == "knight")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(20);
                if (!touchingSpaceDirection.IsOnWall && !touchingSpaceDirection.TestingIsCelling(-2f))
                {
                    this.transform.position = new Vector3(transform.gameObject.transform.position.x - 2f, transform.position.y, transform.position.z);
                    // rb.AddForce(new Vector2(50, 50));
                }
            }
            animator.SetTrigger("hurt");
            audioSource.PlayOneShot(SE_player_hurt);
        }
        else if (collision.contacts[0].normal.x == 1&& (collision.gameObject.tag == "knight"||collision.gameObject.tag=="wc1"|| collision.gameObject.tag=="wc2"))//right
        {
            if (collision.gameObject.tag == "wc1")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(10);
                if (!touchingSpaceDirection.IsOnWall && !touchingSpaceDirection.TestingIsCelling(2f))
                {
                    this.transform.position = new Vector3(transform.gameObject.transform.position.x + 2f, transform.position.y, transform.position.z);
                    //rb.AddForce(new Vector2(50, 0));
                }
            }
            if (collision.gameObject.tag == "wc2")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(20);

                if (!touchingSpaceDirection.IsOnWall && !touchingSpaceDirection.TestingIsCelling(2.5f))
                {
                    this.transform.position = new Vector3(transform.gameObject.transform.position.x + 2.5f, transform.position.y, transform.position.z);
                    //rb.AddForce(new Vector2(50, 0));
                }
            }
            if (collision.gameObject.tag == "knight")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(20);
                if (!touchingSpaceDirection.IsOnWall && !touchingSpaceDirection.TestingIsCelling(-2f))
                {
                    this.transform.position = new Vector3(transform.gameObject.transform.position.x - 2f, transform.position.y, transform.position.z);
                    // rb.AddForce(new Vector2(50, 50));
                }
            }
            animator.SetTrigger("hurt");
            audioSource.PlayOneShot(SE_player_hurt);
        }
        else if(collision.contacts[0].normal.y >0)
        {
            if (collision.gameObject.tag == "wc1")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(5);
                //print(collision.contacts[0].normal.y+"YYYYYYYYYYYYYYYYYYYY");
                animator.SetTrigger("hurt");
                audioSource.PlayOneShot(SE_player_hurt);
            }
            if (collision.gameObject.tag == "wc2")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(5);
                animator.SetTrigger("hurt");
                audioSource.PlayOneShot(SE_player_hurt);
            }
            if (collision.gameObject.tag == "knight")
            {
                print(collision.gameObject.name);
                //collision.gameObject.SendMessage("Apply", 10);
                damageable.Hit(20);
                audioSource.PlayOneShot(SE_player_hurt);
            }
        }


        //other
        if (collision.gameObject.tag == "food")
        {
            print(collision.gameObject.name);
            animator.SetTrigger("getRecover");
            int totalHealth = damageable.Health+25;
            if (totalHealth< damageable.MaxHealth)
            {
                damageable.SetHealth(totalHealth);
            }
            else
            {
                print("healthFull");
            }
            Destroy(collision.gameObject);
            //collision.gameObject.SendMessage("Apply", 10);
        }
        if (collision.gameObject.tag == "ammo_wood")
        {
            print(collision.gameObject.name);
            ammo_wood+=5;
            animator.SetTrigger("getAmmo");
            Destroy(collision.gameObject);
            //collision.gameObject.SendMessage("Apply", 10);
        }
        if (collision.gameObject.tag == "ammo_stone")
        {
            print(collision.gameObject.name);
            ammo_stone+=3;
            animator.SetTrigger("getAmmo");
            Destroy(collision.gameObject);
            //collision.gameObject.SendMessage("Apply", 10);
        }
        if (collision.gameObject.tag == "gift")
        {
            print(collision.gameObject.name);
            ammo_stone += 15;
            ammo_wood += 50;
            animator.SetTrigger("getAmmo");
            Destroy(collision.gameObject);
            //collision.gameObject.SendMessage("Apply", 10);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        print("ryyy");
        if(collision.gameObject.tag =="portal")
        {
            collision.gameObject.transform.GetComponent<Transsport>().ChangeScene("BossScenes");
        }
    }
}
