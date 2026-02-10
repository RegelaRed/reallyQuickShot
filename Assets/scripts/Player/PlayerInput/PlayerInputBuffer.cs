public sealed class PlayerInputBuffer
{
    private float _jumpBuffer;
    private float _dashBuffer;

    private readonly float _jumpBufferTime;
    private readonly float _dashBufferTime;

    public PlayerInputBuffer(float jumpBufferTime, float dashBufferTime)
    {
        _jumpBufferTime = jumpBufferTime;
        _dashBufferTime = dashBufferTime;
    }

    public void Tick(float deltaTime)
    {
        if (_jumpBuffer > 0f) _jumpBuffer -= deltaTime;
        if (_dashBuffer > 0f) _dashBuffer -= deltaTime;
    }

    public void Register(PlayerInputSnapshot input)
    {
        if (input.JumpPressed)
            _jumpBuffer = _jumpBufferTime;

        if (input.DashPressed)
            _dashBuffer = _dashBufferTime;
    }

    public bool ConsumeJump()
    {
        if (_jumpBuffer <= 0f) return false;
        _jumpBuffer = 0f;
        return true;
    }

    public bool ConsumeDash()
    {
        if (_dashBuffer <= 0f) return false;
        _dashBuffer = 0f;
        return true;
    }
}
