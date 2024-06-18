using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingSpaceDirection))]
public class Knight : MonoBehaviour
{
    public float walkSpeed = 9f;
    Rigidbody2D rb;

    public enum WalkableDirection { Right, Left };
    public enum KnightState { Idle, Wait, Walk, Attack1, Attack2 };
    public KnightState state;
    private WalkableDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;
    TouchingSpaceDirection touchingSpaceDirections;
    Damageable damageable;
    public GameObject hp_bar_bg;
    public GameObject hp_bar;
    public int max_hp = 100;
    Animator animator;
    public AudioClip SE_player_shoot;
    AudioSource audioSource;
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
        if (touchingSpaceDirections.IsOnWall && touchingSpaceDirections.IsGrounded)
        {
            FilpDirection();
        }
        rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.velocity.y);
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
