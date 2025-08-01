using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            entity.transform.rotation = Quaternion.Euler(0, 0, -90);
        }
    }
}
