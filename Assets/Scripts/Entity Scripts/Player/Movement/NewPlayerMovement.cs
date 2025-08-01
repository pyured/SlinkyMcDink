using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : IMovementBehavior
{
    private PlayerManager playerManager;
    private float walkThreshold = 0.3f;
    public NewPlayerMovement(PlayerManager p)
    {
        playerManager = p;
    }
    public void Move(Entity entity)
    {
        if (playerManager.stateManager.GetCurrentState() == "Walking")
        {
            WalkingMovement(entity);
        }
        else
        {
            RollingMovement(entity);
        }
    }
    void WalkingMovement(Entity entity)
    {
        //Debug.Log("running walking movement");
        Rigidbody rb = entity.rb;
        float moveSpeed = entity.MoveSpeed();

        switch (playerManager.GetGravityState()) //lowkey having to make movement modes for tube and regular for both walking and rolling is gonna be alot...
        //how can i simplify tube movement to translate regular movement?
        {
            case Entity.GravityState.Regular:
                var forwardVelocity = InputManager.movementInput.y * playerManager.GetCameraVector()[0]; //0 is camera foreward
                var tangentVelocity = InputManager.movementInput.x * playerManager.GetCameraVector()[1]; //1 is camera right
                //rb.velocity = new Vector3(InputManager.movementInput.x * moveSpeed, rb.velocity.y, InputManager.movementInput.y * moveSpeed);
                //add forward and side velocitys, and preserve vertical velocity with the project method
                rb.velocity = (forwardVelocity + tangentVelocity).normalized * moveSpeed + Vector3.Project(rb.velocity, Vector3.down);
                break;
            case Entity.GravityState.Tube:
                MainTerrain mainTerrain = playerManager.GetCurrentTerrain().GetComponent<MainTerrain>();

                Vector3 forwardMovement = mainTerrain.lineOfGravity.normalized;
                Vector3 tangentMovement = Vector3.Cross(-playerManager.GetGravityDirection().normalized, forwardMovement).normalized;
                Vector3 moveDir = (forwardMovement * InputManager.movementInput.y + tangentMovement * InputManager.movementInput.x).normalized;
                rb.velocity = moveDir * moveSpeed + Vector3.Project(rb.velocity, playerManager.GetGravityDirection());
                break;
        }
    }

    void RollingMovement(Entity entity)
    {
        entity.transform.rotation = Quaternion.Euler(0, entity.transform.eulerAngles.y + playerManager.turnSpeed * InputManager.movementInput.x, 0);
        playerManager.rollSpeed += InputManager.movementInput.y * playerManager.rollAcceleration;
        if (playerManager.rollSpeed < walkThreshold)
        {
            playerManager.StopRolling();
        }
        entity.rb.velocity = entity.transform.forward * playerManager.rollSpeed + Vector3.Project(entity.rb.velocity, Vector3.down);
    }
}
