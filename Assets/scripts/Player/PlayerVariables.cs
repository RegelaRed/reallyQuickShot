using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Player Stats")]
public class PlayerVariables : ScriptableObject
{
    [Header(" Walk/Sprint")]
    public float walkSpeed = 5;
    public float sprintSpeed = 8f;
    [Tooltip("values 1-30 for smooth falloff, 50+ for snappy stop")]
    public float groundDeceleration = 14;

    [Header("Jump")]
    public float maxJumpHeight = 1.4f;
    public float maxJumpDuration = 0.5f;
    public float jumpBufferTime = 0.4f;
    public float jumpInterval = 0.1f;

    [Header("Dash")]
    public int maxDashCharges = 2;
    public float dashInterval = 0.2f;
    public float dashRegenTime = 0.6f;
    public float dashDuration = 0.8f;
    public float dashDistance = 2f;
    public float dashApexHeight = 0.5f;
    public float dashBufferTimer = 0.2f;

    [Space]
    [Header("Air")]
    public float airMoveSpeed = 0.6f;
    public float maxAirSpeed = 2f;
    public float airControl = 2f;

    [Header("Physics")]
    public float pushForce = 1f;

    [Header("Camera and Rotation")]
    [Tooltip(" lower value -> slower turn")]
    public float playerBodyRotationSpeed = 5f;
    public float horizontalCameraSensitivity = 50f;
    public float verticalCameraSensitivity = 50f;

    [Header("Main Camera")]
    [Tooltip("minimum value to look down, place between 0 to (-90)")]
    public float mainCameraPitchMin = -60f;
    [Tooltip("maximum value to look up, place between 0 to 90")]
    public float mainCameraPitchMax = 30f;

    [Header("Aim Camera")]
    [Tooltip("minimum value to look down, place between 0 to (-90)")]
    public float aimCameraPitchMin = -80f;
    [Tooltip("maximum value to look up, place between 0 to 90")]
    public float aimCameraPitchMax = 70f;

    [Header("Aim Mode")]
    public float aimModeSpeed = 4f;
}
