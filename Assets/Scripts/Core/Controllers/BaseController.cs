using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class BaseController : MonoBehaviour
{
    [Header("General")]
    public Transform rootTransform;
    public Transform headTransform;

    [Header("Rotation")]
    public float cameraRotationSpeed = 2.0f;
    private bool canRotate = false;
    private float yaw = 0.0f;
    public Vector2 rotAngle;

    private void OnEnable()
    {
        if (InputManager.instance.targetControlScheme == ControlSchemeEnum.VR)
        {
            InputManager.instance.EnableAllRays();
        }
        else
        {
            InputManager.instance.DisableAllRays();
        }

        InputManager.instance.playerActions.DefaultControls.CharacterRotation.canceled += OnRotation;
        InputManager.instance.playerActions.DefaultControls.CharacterRotation.performed += OnRotation;
    }

    private void OnDisable()
    {
        canRotate = false;

        InputManager.instance.playerActions.DefaultControls.CharacterRotation.canceled -= OnRotation;
        InputManager.instance.playerActions.DefaultControls.CharacterRotation.performed -= OnRotation;
    }

    public void OnDestroy()
    {
        if (isActiveAndEnabled)
        {
            canRotate = false;

            InputManager.instance.playerActions.DefaultControls.CharacterRotation.canceled -= OnRotation;
            InputManager.instance.playerActions.DefaultControls.CharacterRotation.performed -= OnRotation;
        }
    }

    private void Update()
    {
        if (canRotate)
        {
            headTransform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        }
    }

    private void OnRotation(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            rotAngle = value.ReadValue<Vector2>();
            yaw += cameraRotationSpeed * rotAngle.x;
            canRotate = true;
        }
        else if (value.canceled)
        {
            rotAngle = Vector2.zero;
            canRotate = false;
        }
    }
}
