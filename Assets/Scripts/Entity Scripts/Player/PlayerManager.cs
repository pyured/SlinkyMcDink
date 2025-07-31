using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : Entity
{
    #region Player Ability Stats
    [SerializeField] private float jumpForce;
    #endregion
    #region Miscellaneous
    [SerializeField] private float groundedRayHeight;
    #endregion
    protected override void Start()
    {
        base.Start();
        movementBehavior = new NewPlayerMovement(this);
        stateManager = new StateManager(new string[]{
            "Walking",
            "Rolling"
        });

        Dictionary<string, IAbility> abilityMapping = new(); //making a dictionary with every ability and its button, and then passing it to player abilities
        abilityMapping.Add("Jump", new JumpAbility(this));

        abilitiesBehavior = new PlayerAbilities(abilityMapping, this);
        stateManager.SetState("Walking");
    }
    public float GetJumpForce()
    {
        return jumpForce;
    }
    public bool IsGrounded()
    {
        return Physics.Raycast(rb.transform.position, -transform.up, groundedRayHeight, LayerMask.GetMask("Ground"));
    }
}
