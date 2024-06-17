using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingSpaceDirection : MonoBehaviour
{
    public float groundDistance=0.05f;
    public float wallDistance = 0.000001f;
    public float ceilingDistance = 0.05f;

    public ContactFilter2D castFilter;
    public CapsuleCollider2D touchingCol;

    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isGrounded;

    public bool IsGrounded { 
        get
        {
            return _isGrounded;
        }
        private set
        {
            _isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded,value);
        }
    }

    [SerializeField]
    private bool _isOnwall;

    public bool IsOnWall
    {
        get
        {
            return _isOnwall;
        }
        private set
        {
            _isOnwall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    [SerializeField]
    private bool _isOnCeiling;

    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

    public bool IsOnCeiling
    {
        get
        {
            return _isOnCeiling;
        }
        private set
        {
            _isOnCeiling = value;
            animator.SetBool(AnimationStrings.isOnCeiling, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded=touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance)>0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }
    public bool TestingIsCelling(float d)
    {
        return (touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance+d) > 0);
    }
}
