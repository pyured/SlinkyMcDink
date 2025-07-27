using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Physics physics;
    /** The speed the player moves at */
    public float moveSpeed;
    /** The force that the player jumps with */
    public float jumpForce;
    private Rigidbody rb;
    [SerializeField] private float groundedRayHeight;
    public PlayerManager playerManager;
    [SerializeField] float gravityScale;

    [SerializeField] private float maxCoyoteTime;
    private float coyoteTime;
    private bool jumped;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (IsGrounded())
        {
            coyoteTime = 0;
            if (rb.velocity.y < 0)
            {
                jumped = false;
            }
        }
        else
        {
            coyoteTime += Time.deltaTime;
        }

        if (InputManager.buttonMap["Jump"].PressedThisFrame() && (IsGrounded() || (coyoteTime <= maxCoyoteTime && !jumped)))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumped = true;
        }

        Debug.Log(coyoteTime);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(InputManager.movementInput.x * moveSpeed, rb.velocity.y, InputManager.movementInput.y * moveSpeed);
        PlayerGravity();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.down * groundedRayHeight);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(rb.transform.position, Vector3.down, groundedRayHeight, LayerMask.GetMask("Ground"));
    }

    void PlayerGravity()
    {
        if (!IsGrounded())
        {
            rb.AddForce(Vector3.down * gravityScale);
        }
    }
}
