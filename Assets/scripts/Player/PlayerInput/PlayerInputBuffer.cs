
public class PlayerInputBuffer
{
    private float _jumpBufferTimer;
    private float _dashBufferTimer;


    private float _jumpBufferTime;
    private float _dashBufferTime;

    public bool JumpBufferActive => _jumpBufferTimer > 0f;
    public bool DashBufferActive => _dashBufferTimer > 0f;


    public PlayerInputBuffer(float jumpTime, float dashTime)
    {
        _jumpBufferTime = jumpTime;
        _dashBufferTime = dashTime;
    }

    public void Register(PlayerInputSnapshot input)
    {
        if (input.JumpPressed)
            _jumpBufferTimer = _jumpBufferTime;

        if (input.DashPressed)
            _dashBufferTimer = _dashBufferTime;
    }

    public void Tick(float deltaTime)
    {
        if (_jumpBufferTimer > 0f)
            _jumpBufferTimer -= deltaTime;

        if (_dashBufferTimer > 0f)
            _dashBufferTimer -= deltaTime;


    }
    // ------------ Jump ------------
    public void ConsumeJump()
    {
        _jumpBufferTimer = 0f;
    }
    public void SetCyoteTime(float jumpBufferTime)
    {
        _jumpBufferTimer = jumpBufferTime;
    }

    // ------------ Dash ------------
    public void ConsumeDash()
    {
        _dashBufferTimer = 0f;
    }

}
