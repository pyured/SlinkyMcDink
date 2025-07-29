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

    /** The amount of coyote time that the player has */
    [SerializeField] private float maxCoyoteTime;
    /** The time since the player last left the ground */
    private float coyoteTime;
    /** Whether the player has jumped since they last left the ground */
    private bool jumped;
    /* This variable is just used to create a delay between jumps to avoid IDJs which messes with player jump height*/
    private bool recentlyJumped;

    /** A reference to the player's blob shadow */
    [SerializeField] private GameObject blobShadow;

    #region Terrain / Platform Gravity Settings
    public bool tubeGravity;
    public GameObject currentTerrain;
    public float rotationSpeed;
    /** The force of gravity to be applied to the player */
    [SerializeField] float gravityScale;
    public Vector3 planetDir;
    public Vector3 normalDir;

    #endregion


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

        if (InputManager.buttonMap["Jump"].PressedThisFrame() && (IsGrounded() || (coyoteTime <= maxCoyoteTime && !jumped)) && !recentlyJumped)
        {
            // rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            // rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            // jumped = true;

            StartCoroutine(Jump());
        }

        UpdateBlobShadow();

        if (tubeGravity)
        {
            SurfaceAlignment();
        }

        // FOR JEREMY TO USE
        Debug.Log(InputManager.cameraInput);
        Debug.Log(GetGravityVector());
    }

    void FixedUpdate()
    {
        if (tubeGravity)
        {

            Vector3 forwardMovement = currentTerrain.GetComponent<MainTerrain>().lineOfGravity.normalized;
            Vector3 tangentMovement = Vector3.Cross(-GetGravityVector(), forwardMovement).normalized;
            Vector3 moveDir = (forwardMovement * InputManager.movementInput.y + tangentMovement * InputManager.movementInput.x).normalized;
            rb.velocity = moveDir * moveSpeed + Vector3.Project(rb.velocity, GetGravityVector());

        }
        else
        {
            rb.velocity = new Vector3(InputManager.movementInput.x * moveSpeed, rb.velocity.y, InputManager.movementInput.y * moveSpeed);
        }
        PlayerGravity();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, -transform.up * groundedRayHeight);
        if (tubeGravity)
        {
            Gizmos.DrawRay(transform.position, GetGravityVector());
        }

    }

    public bool IsGrounded()
    {
        return Physics.Raycast(rb.transform.position, -transform.up, groundedRayHeight, LayerMask.GetMask("Ground"));
    }

    void PlayerGravity()
    {
        if (tubeGravity)
        {
            rb.AddForce(GetGravityVector().normalized * gravityScale, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(Vector3.down * gravityScale, ForceMode.Acceleration);
        }
    }

    // TODO: Have this update to ground rotation
    void UpdateBlobShadow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 100f))
        {
            blobShadow.SetActive(true);
            blobShadow.transform.position = hit.point + transform.up * 0.05f;
            //blobShadow.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal );
        }
        else
        {
            blobShadow.SetActive(false);
            blobShadow.transform.position = transform.position + -transform.up * 3f;
        }
    }
    private IEnumerator CoyoteTimer()
    {
        while (coyoteTime < maxCoyoteTime && !IsGrounded())
        {
            yield return null;
            coyoteTime += Time.deltaTime;
        }
    }
    private Vector3 GetGravityVector()
    {
        Vector3 playerToOrigin = transform.position - currentTerrain.transform.position;
        Vector3 lineOfGravity = currentTerrain.GetComponent<MainTerrain>().lineOfGravity;
        Vector3 pointOfGravity = Vector3.Project(playerToOrigin, lineOfGravity) + currentTerrain.transform.position;

        Vector3 forceOfGravity = (pointOfGravity - transform.position);
        return forceOfGravity;
    }
    /* This method rotates the player so that its transform.up faces away from terrain, making it stand upright on the surface */
    private void SurfaceAlignment()
    {
        Vector3 up = -GetGravityVector().normalized;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, GetGravityVector().normalized).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(forward, up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
    private IEnumerator Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        jumped = true;
        recentlyJumped = true;
        yield return new WaitForSecondsRealtime(0.5f);
        recentlyJumped = false;
    }
}