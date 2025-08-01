using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : Entity
{
    #region Player Ability Stats
    [SerializeField] private float jumpForce;
    [HideInInspector] public bool jumped;
    #endregion

    #region Player Utilities
    [SerializeField] private float groundedRayHeight;
    public float maxCoyoteTime;
    [HideInInspector] public float coyoteTime;
    #endregion

    #region Miscellaneous
    public GameObject blobShadowReference;
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

        transform.rotation = Quaternion.Euler(0, 0, 0);

        abilitiesBehavior = new PlayerAbilities(abilityMapping, this);
        stateManager.SetState("Walking");
    }

    protected override void Update()
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

        Debug.Log(stateManager.GetCurrentState());

        UpdateBlobShadow();
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
}
