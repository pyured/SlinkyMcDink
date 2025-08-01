using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// will comment later
public class RollAbility : IAbility
{
    private PlayerManager playerManager;
    private float rollThreshold = 3f;

    public RollAbility(PlayerManager pm)
    {
        playerManager = pm;
    }

    public void Execute(Entity entity)
    {
        Rigidbody rb = entity.rb;
        float directionalSpeed = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.z * rb.velocity.z);

        if (playerManager.stateManager.GetCurrentState() == "Walking" && directionalSpeed >= rollThreshold)
        {
            playerManager.stateManager.SetState("Rolling");
            playerManager.rollSpeed = directionalSpeed;
            float angle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
            playerManager.model.transform.rotation = Quaternion.Euler(0, 0, -90);
            entity.transform.rotation = Quaternion.Euler(0, angle, 0);
            entity.GetComponent<CapsuleCollider>().direction = 0;
        }
        else if (playerManager.stateManager.GetCurrentState() == "Rolling")
        {
            playerManager.StopRolling();
        }
    }
}
