using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerMovement : IMovementBehavior
{
    private PlayerManager playerManager;
    public NewPlayerMovement(PlayerManager p)
    {
        playerManager = p;
    }
    public void Move(Entity entity)
    {
        WalkingMovement(entity);
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
                rb.velocity = new Vector3(InputManager.movementInput.x * moveSpeed, rb.velocity.y, InputManager.movementInput.y * moveSpeed);
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
}
