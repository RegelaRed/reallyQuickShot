using JetBrains.Annotations;
using UnityEditor.EditorTools;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Data/Player Stats")]
public class PlayerVariables : ScriptableObject
{
    //Walk
    public float walkSpeed = 5;

    //Sprint
    public float sprintSpeed = 8f;

    //Jump
    public float jumpForce = 4;
    public float maxJumpHeight = 1.4f;
    public float maxJumpTime = 0.5f;

    //Dash
    public float dashForce = 4;
    public int maxDashCharges = 2;
    public float dashRegenTime = 1f;
    public float dashDuration = 0.8f;

    //Gravity
    public float gravity = 10f;

    //LayerMasks
    public LayerMask groundLayer;

    //Camera
    public float horizontalCameraSensitivity = 50f;
    public float verticalCameraSensitivity = 50f;

    [Tooltip("minimum value to look down, place between 0 to (-90)")]
    public float cameraVerticalClampMin = -80f;
    [Tooltip("maximum value to look up, place between 0 to 90")]
    public float cameraVerticalClampMax = 90f;
}
