using UnityEngine;

public class PlayerPhysics
{
    private PlayerController _ctx;
    private PlayerStateFactory _factory;
    public PlayerPhysics(PlayerController currentContext, PlayerStateFactory stateFactory)
    {
        _ctx = currentContext;
        _factory = stateFactory;
    }
    public void Tick()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        Vector3 currentMovement = _ctx.Input.CurrentMovement;
        Vector3 currentSprintMovement = _ctx.Input.CurrentSprintMoveemnt;

        if (_ctx.CController.isGrounded)
        {
            float groundGravity = -0.05f;
            currentMovement.y = groundGravity;
            currentSprintMovement.y = groundGravity;
        }
        else
        {
            float airGravity = -10f;
            currentMovement.y += airGravity;
            currentSprintMovement.y += airGravity;
        }
    }
}
