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

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (InputManager.buttonMap["Jump"].PressedThisFrame() && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        Debug.Log(InputManager.buttonMap["Roll"].Active());
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
