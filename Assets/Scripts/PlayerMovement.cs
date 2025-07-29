using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public Physics physics;
    /** The speed the player moves at */
    public float moveSpeed;
    /** The force that the player jumps with */
    public float jumpForce;
    /** A reference to the player rigidbody */
    private Rigidbody rb;
    /** The height of the grounded raycast */
    [SerializeField] private float groundedRayHeight;
    //public PlayerManager playerManager;
    /** The force of gravity to be applied to the player */
    [SerializeField] float gravityScale;

    /** The amount of coyote time that the player has */
    [SerializeField] private float maxCoyoteTime;
    /** The time since the player last left the ground */
    private float coyoteTime;
    /** Whether the player has jumped since they last left the ground */
    private bool jumped;

    /** A reference to the player's blob shadow */
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

        // FOR JEREMY TO USE
        Debug.Log(InputManager.cameraInput);
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
