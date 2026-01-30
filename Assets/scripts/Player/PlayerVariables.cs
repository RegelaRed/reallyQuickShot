using JetBrains.Annotations;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Player Stats")]
public class PlayerVariables : ScriptableObject
{
    [Header(" Walk/Sprint")]
    public float walkSpeed = 5;
    public float sprintSpeed = 8f;

    [Header("Jump")]
    public float maxJumpHeight = 1.4f;
    public float maxJumpTime = 0.5f;
    public float cyoteTime = 0.4f;

    [Header("Dash")]
    public int maxDashCharges = 2;
    public float dashInterval = 0.2f;
    public float dashRegenTime = 0.6f;
    public float dashDuration = 0.8f;
    public float dashDistance = 2f;

    [Header("Gravity")]
    public float gravity = 10f;
    [Space]
    [Header("Air")]
    public float airMoveSpeed = 0.6f;
    public float maxAirSpeed = 2f;
    public float airControll = 2f;

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
}
