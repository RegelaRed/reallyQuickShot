using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    #region Variables
    public float MoveDirection;


    private PlayerInputActions Input;
    private PlayerController _ctx;
    private PlayerInput(PlayerController ctx)
    {
        _ctx = ctx;
        Input = ctx.Actions;
    }

    #endregion
    public void Tick()
    {

    }
}
