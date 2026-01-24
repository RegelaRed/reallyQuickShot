using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private Transform _Orientation;
    [SerializeField] private PlayerVariables playerVariables;

    private float xRot;
    private float yRot;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * (playerVariables.verticalCameraSensitivity * 10) * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * (playerVariables.horizontalCameraSensitivity * 10) * Time.deltaTime;

        yRot += mouseX;
        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -30f, 90f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
        _Orientation.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
