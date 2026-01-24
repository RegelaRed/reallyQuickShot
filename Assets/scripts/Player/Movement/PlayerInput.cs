using UnityEngine;
public class PlayerInput
{
    #region Variables
    private readonly PlayerController ctx;
    public PlayerInput(PlayerController ctx)
    {
        this.ctx = ctx;
    }

    //vectors
    public Vector3 MoveDirection { get; private set; }
    public float MoveMagnitude { get; private set; }

    //Key Pressed
    public bool SprintPressed { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool DashPressed { get; private set; }
    public bool IsCrouch { get; private set; }
    public bool AimModeActive { get; private set; }

    //Mouse Inpits
    public float xRotation { get; private set; }
    public float yRotation { get; private set; }

    //Inputs for weapons
    public bool chargeBow { get; private set; }
    public int switchArrows { get; private set; }

    #endregion
    public void Tick()
    {
        //Camera inputs
        float mouseX = Input.GetAxisRaw("Mouse X") * (ctx.PlayerVariables.verticalCameraSensitivity * 10) * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * (ctx.PlayerVariables.horizontalCameraSensitivity * 10) * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, ctx.PlayerVariables.cameraVerticalClampMin, ctx.PlayerVariables.cameraVerticalClampMax);


        //Movement inputs
        Vector3 forward = ctx.Orientation.forward;
        Vector3 right = ctx.Orientation.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        MoveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");
        MoveMagnitude = MoveDirection.magnitude;
        MoveDirection = MoveDirection.normalized;

        //Key Pressed Inputs
        DashPressed = Input.GetKeyDown(KeyCode.LeftShift);
        JumpPressed = Input.GetKeyDown(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.R))
        {
            AimModeActive = !AimModeActive;
        }
        Debug.Log("is aiming " + AimModeActive);

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            IsCrouch = !IsCrouch;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            SprintPressed = !SprintPressed;

        //weapons seaction

        chargeBow = AimModeActive && Input.GetMouseButton(0);

        
    }


}
