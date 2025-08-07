using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : Entity
{
    #region Player Ability Stats
    [SerializeField] private float jumpForce;
    [HideInInspector] public bool jumped;
    [HideInInspector] public float rollSpeed;
    public float turnSpeed;
    public float rollDampener;
    public float rollAcceleration;
    #endregion

    #region Player Utilities
    [SerializeField] private float groundedRayHeight;
    public float maxCoyoteTime;
    [HideInInspector] public float coyoteTime;
    #endregion

    #region Miscellaneous
    [SerializeField] private GameObject blobShadowReference;
    [SerializeField] private CinemachineBrain cameraBrain;
    public GameObject model;
    #endregion

    protected override void Start()
    {
        base.Start();
        movementBehavior = new NewPlayerMovement(this);
        stateManager = new StateManager(new string[]{
            "Walking",
            "Rolling"
        });

        Dictionary<string, IAbility> abilityMapping = new()
        {
            { "Jump", new JumpAbility(this) },
            { "Roll", new RollAbility(this) }
        }; //making a dictionary with every ability and its button, and then passing it to player abilities

        model.transform.rotation = Quaternion.Euler(0, 0, 0);

        abilitiesBehavior = new PlayerAbilities(abilityMapping, this);
        stateManager.SetState("Walking");
    }

    protected override void Update() //uwuwuu update
    {
        base.Update();

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

        //Debug.Log(stateManager.GetCurrentState());

        UpdateBlobShadow();
        Debug.DrawRay(transform.position, GetCameraVector()[0]);
    }

    public float GetJumpForce()
    {
        return jumpForce;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(rb.transform.position, -transform.up, groundedRayHeight, LayerMask.GetMask("Ground"));
    }

    // TODO: Have this update to ground rotation
    void UpdateBlobShadow()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 100f))
        {
            blobShadowReference.SetActive(true);
            blobShadowReference.transform.position = hit.point + transform.up * 0.05f;
            //blobShadowReference.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal );
        }
        else
        {
            blobShadowReference.SetActive(false);
            blobShadowReference.transform.position = transform.position + -transform.up * 3f;
        }
    }
    public Vector3[] GetCameraVector() //what direction our camera for player is facing, 0 is the forward vector of camera and 1 is the vector facing the right
    {
        return new Vector3[] {
            Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized,
            Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized,
        };
    }

    public void StopRolling()
    {
        Debug.Log("they see me walkin");
        stateManager.SetState("Walking");
        model.transform.rotation = Quaternion.Euler(0, 0, 0);
        //model.GetComponent<CapsuleCollider>().direction = 1;
    }
    public void UpdateFacingRotation(Vector3 direction) //this really only works with regular vector3.down gravity
    {
        Quaternion target;
        float rotationSpeed = 5f;
        switch (GetGravityState())
        {
            case GravityState.Regular:
                target = Quaternion.LookRotation(direction, transform.up);
                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, target, Time.deltaTime * rotationSpeed);
                break;
            case GravityState.Tube:
                Vector3 moveDir = Vector3.ProjectOnPlane(transform.forward, transform.up).normalized * InputManager.movementInput.y
                + Vector3.ProjectOnPlane(transform.right, transform.up).normalized * InputManager.movementInput.x;
                target = Quaternion.LookRotation(moveDir, transform.up);
                model.transform.rotation = Quaternion.Slerp(model.transform.rotation, target, Time.deltaTime * rotationSpeed);
                break;
        }
    }
}
