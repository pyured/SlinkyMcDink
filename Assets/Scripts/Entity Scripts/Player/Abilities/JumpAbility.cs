using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAbility : IAbility
{
    private PlayerManager playerManager;
    public JumpAbility(PlayerManager pm)
    {
        playerManager = pm;
    }
    public void Execute(Entity entity)
    {
        if (playerManager.IsGrounded() || (playerManager.coyoteTime <= playerManager.maxCoyoteTime && !playerManager.jumped))
        {
            Rigidbody rb = entity.rb;
            Transform transform = entity.transform;
            playerManager.jumped = true;
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * playerManager.GetJumpForce(), ForceMode.Impulse);
        }
    }
}
