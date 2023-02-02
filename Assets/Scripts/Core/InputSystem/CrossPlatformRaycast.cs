using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossPlatformRaycast : MonoBehaviour
{
    private GameObject leftHandController = null;
    private GameObject rightHandController = null;
    private GameObject currentHandController = null;
    private Vector2 mousePosition = Vector2.zero;

    public void OnEnable()
    {
        leftHandController = GameObject.FindGameObjectWithTag("leftControllerXR");
        rightHandController = GameObject.FindGameObjectWithTag("rightControllerXR");

        InputManager.instance.playerActions.DefaultControls.MousePoint.performed += value => mousePosition = value.ReadValue<Vector2>();
        InputManager.instance.playerActions.DefaultControls.MousePoint.canceled += value => mousePosition = value.ReadValue<Vector2>();
        InputManager.instance.playerActions.DefaultControls.MouseClick.canceled += RaycastReturnObject;
        InputManager.instance.playerActions.XRILeftHand.MouseClick.canceled += RaycastReturnObject;
        InputManager.instance.playerActions.XRIRightHand.MouseClick.canceled += RaycastReturnObject;
    }

    public void OnDisable()
    {
        InputManager.instance.playerActions.DefaultControls.MousePoint.performed -= value => mousePosition = value.ReadValue<Vector2>();
        InputManager.instance.playerActions.DefaultControls.MousePoint.canceled -= value => mousePosition = value.ReadValue<Vector2>();
        InputManager.instance.playerActions.DefaultControls.MouseClick.canceled -= RaycastReturnObject;
        InputManager.instance.playerActions.XRILeftHand.MouseClick.canceled -= RaycastReturnObject;
        InputManager.instance.playerActions.XRIRightHand.MouseClick.canceled -= RaycastReturnObject;
    }

    public void OnDestroy()
    {
        if (isActiveAndEnabled)
        {
            InputManager.instance.playerActions.DefaultControls.MousePoint.performed -= value => mousePosition = value.ReadValue<Vector2>();
            InputManager.instance.playerActions.DefaultControls.MousePoint.canceled -= value => mousePosition = value.ReadValue<Vector2>();
            InputManager.instance.playerActions.DefaultControls.MouseClick.canceled -= RaycastReturnObject;
            InputManager.instance.playerActions.XRILeftHand.MouseClick.canceled -= RaycastReturnObject;
            InputManager.instance.playerActions.XRIRightHand.MouseClick.canceled -= RaycastReturnObject;
        }
    }

    private void RaycastReturnObject(UnityEngine.InputSystem.InputAction.CallbackContext value)
    {
        if (value.canceled)
        {
            Ray ray = new Ray();
            RaycastHit hit;

            if (InputManager.instance.targetControlScheme == ControlSchemeEnum.VR)
            {
                if (value.action.actionMap.name.ToLower().Contains("left"))
                {
                    currentHandController = leftHandController;
                }
                else
                {
                    currentHandController = rightHandController;
                }

                ray.origin = currentHandController.transform.position;
                ray.direction = currentHandController.transform.forward;
            }
            else
            {
                ray = Camera.main.ScreenPointToRay(mousePosition);
            }

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.name);
            }
        }
    }
}