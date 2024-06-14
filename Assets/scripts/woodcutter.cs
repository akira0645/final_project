using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(TouchingSpaceDirection))]
public class woodcutter : MonoBehaviour
{
    public float walkSpeed=9f;
    Rigidbody2D rb;

    public enum WalkableDirection {Right,Left};
    private WalkableDirection  _walkDirection;
    private Vector2 WalkDirectionVector=Vector2.right;
    TouchingSpaceDirection touchingSpaceDirections;
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }                                                                     
        set {
            if (_walkDirection != value)
            {
                //gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x*-1, gameObject.transform.localScale.y);
                transform.Rotate(0f, 180f, 0f);
                if (value==WalkableDirection.Right)
                {
                    WalkDirectionVector = Vector2.right;
                }else if(value == WalkableDirection.Left)
                {
                    WalkDirectionVector = Vector2.left;
                }

            }

            _walkDirection = value; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingSpaceDirections = GetComponent<TouchingSpaceDirection>();
    }
    private void FixedUpdate()
    {
        if(touchingSpaceDirections.IsOnWall&& touchingSpaceDirections.IsGrounded)
        {
            FilpDirection();
        }
        rb.velocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.velocity.y);
    }

    private void FilpDirection()
    {
        if(WalkDirection==WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }else if(WalkDirection==WalkableDirection.Left)
        {
            WalkDirection=WalkableDirection.Right;
        }
        else
        {
            Debug.Log("enemy walk error!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
