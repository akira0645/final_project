using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingSpaceDirection))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 9f;
    Rigidbody2D rb;

    public enum WalkableDirection { Right, Left };
    public enum KnightState { Idle, Wait, Walk, Attack1, Attack2 };
    public KnightState state;
    private WalkableDirection _walkDirection;
    public Vector2 WalkDirectionVector = Vector2.right;
    TouchingSpaceDirection touchingSpaceDirections;
    Damageable damageable;
    public GameObject hp_bar_bg;
    public GameObject hp_bar;
    public int max_hp = 100;
    Animator animator;
    public AudioClip SE_player_shoot;
    AudioSource audioSource;
    public GameObject sw1;
    public GameObject sw2A;
    public GameObject sw2B;
    public GameObject sw2C;
    public GameObject sw2D;

    [SerializeField]
    private bool _walkLock;

    public bool walkLock
    {
        get
        {
            return _walkLock;
        }
        private set
        {
            _walkLock = value;
            animator.SetBool("walkLock", value);
        }
    }

    [SerializeField]
    private bool _Onattack1;

    public bool Onattack1
    {
        get
        {
            return _Onattack1;
        }
        private set
        {
            _Onattack1 = value;
            animator.SetBool("Onattack1", value);
        }
    }

    [SerializeField]
    private bool _Onattack2;

    public bool Onattack2
    {
        get
        {
            return _Onattack2;
        }
        private set
        {
            _Onattack2 = value;
            animator.SetBool("Onattack2", value);
        }
    }

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                //gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x*-1, gameObject.transform.localScale.y);
                //transform.Rotate(0f, 180f, 0f);
                if (value == WalkableDirection.Right)
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                    WalkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                    WalkDirectionVector = Vector2.left;
                }

            }

            _walkDirection = value;
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
        touchingSpaceDirections = GetComponent<TouchingSpaceDirection>();
        audioSource = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if(!walkLock)
        {
            if (touchingSpaceDirections.IsOnWall && touchingSpaceDirections.IsGrounded)
            {
                FilpDirection();
            }
            rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
        }
            
    }

    private void FilpDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.Log("enemy walk error!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        damageable.SetHealth(max_hp);
        InvokeRepeating("startOnAttack01", 2f, 30f);
        InvokeRepeating("OnAttack01", 5f, 30f);
        InvokeRepeating("OnAttack01c", 10, 30f);
        InvokeRepeating("endOnAttack01", 11f, 30f);

        InvokeRepeating("startOnAttack02", 15f, 30f);
        InvokeRepeating("OnAttack02", 18f, 30f);
        InvokeRepeating("OnAttack021", 18.5f, 30f);
        InvokeRepeating("OnAttack02c", 23, 30f);
        InvokeRepeating("endOnAttack02", 24f, 30f);
    }

    void startOnAttack01()
    {
        Onattack1 = true;
        walkLock = true;

    }
     void OnAttack01()
    {
        sw1.GetComponent<BulletSpawner>().SetLocker();

    }
    void OnAttack01c()
    {
        sw1.GetComponent<BulletSpawner>().Lock();
    }
    void endOnAttack01()
    {
        Onattack1 = false;
        walkLock = false;

    }

    void startOnAttack02()
    {
        Onattack2 = true;
        walkLock = true;

    }
    void OnAttack02()
    {
        sw2A.GetComponent<BulletSpawner>().SetLocker();
        sw2B.GetComponent<BulletSpawner>().SetLocker();

    }
    void  OnAttack021()
    {
        sw2C.GetComponent<BulletSpawner>().SetLocker();
        sw2D.GetComponent<BulletSpawner>().SetLocker();
    }
    void OnAttack02c()
    {
        sw2A.GetComponent<BulletSpawner>().Lock();
        sw2B.GetComponent<BulletSpawner>().Lock();
        sw2C.GetComponent<BulletSpawner>().Lock();
        sw2D.GetComponent<BulletSpawner>().Lock();
    }
    void endOnAttack02()
    {
        Onattack2 = false;
        walkLock = false;

    }


    // Update is called once per frame
    void Update()
    {
        hp_bar_bg.GetComponent<SpriteRenderer>().flipX = false;
        float pre = ((float)damageable.Health / (float)damageable.MaxHealth);
        hp_bar_bg.transform.localScale = new Vector3(pre, hp_bar_bg.transform.localScale.y, hp_bar_bg.transform.localScale.z);
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "wood")
        {
            float pre = ((float)damageable.Health / (float)damageable.MaxHealth);
            print("WC" + pre);
            audioSource.PlayOneShot(SE_player_shoot);
            damageable.Hit(20);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "stone")
        {
            print("wwwwwwwwwwwwwwwwwwwwwwwwwwwC" + damageable.Health);
            damageable.Hit(50);
            audioSource.PlayOneShot(SE_player_shoot);
            Destroy(collision.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag == "wc1" || gameObject.tag == "knight")
        {
            if (collision.contacts[0].normal.x == -1 && !touchingSpaceDirections.TestingIsCelling(-3f) && (collision.gameObject.tag != "food") && (collision.gameObject.tag != "ammo_wood") && (collision.gameObject.tag != "ammo_stone"))
            {
                this.transform.position = new Vector3(transform.gameObject.transform.position.x - 3f, transform.position.y, transform.position.z);
            }
            else if (collision.contacts[0].normal.x == 1 && !touchingSpaceDirections.TestingIsCelling(+3f) && (collision.gameObject.tag != "food") && (collision.gameObject.tag != "ammo_wood") && (collision.gameObject.tag != "ammo_stone"))
            {
                this.transform.position = new Vector3(transform.gameObject.transform.position.x + 3f, transform.position.y, transform.position.z);
            }
            /*else if (collision.contacts[0].normal.y==-1 && (collision.gameObject.tag == "player") )
            {
                damageable.Hit(damageable.MaxHealth/2);
                print(collision.contacts[0].normal.y + "YYYYYYYYYYYYYYYYYYYY"+ damageable.MaxHealth / 2);
            }*/
        }
    }
}
