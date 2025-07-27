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

    [SerializeField] private GameObject blobShadow;

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

        UpdateBlobShadow();

        //Debug.Log(coyoteTime);
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

    // TODO: Have this update to ground rotation
    void UpdateBlobShadow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 100f))
        {
            blobShadow.SetActive(true);
            blobShadow.transform.position = hit.point + Vector3.up * 0.05f;
            //blobShadow.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal );
        }
        else
        {
            blobShadow.SetActive(false);
            blobShadow.transform.position = transform.position + Vector3.down * 3f;
        }
    }
}
