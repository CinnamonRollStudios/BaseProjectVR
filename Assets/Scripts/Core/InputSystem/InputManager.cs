using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance { get; private set; }

    [Header("Input modules")]
    public InputSystemUIInputModule baseInputModule;
    public XRUIInputModule xrInputModule;
    //public CrossPlatformCursor cursorController;
    public Gradient clearGradient;
    public Gradient defaultGradient;

    [Header("Movement types")]
    public MovementTypeEnum movementType = MovementTypeEnum.None;
    public RotationTypeEnum rotationType = RotationTypeEnum.None;

    [Header("Control scheme type")]
    public ControlSchemeEnum targetControlScheme = ControlSchemeEnum.None;

    [Header("Action maps controls")]
    public PlayerControls playerActions;
    public InputActionMap currentActionMap;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject); //If not DoNotDestroy, null instance on OnDestroy

        playerActions = new PlayerControls();

        playerActions.Disable();
        playerActions.XRIHead.Enable();
        playerActions.XRILeftHand.Enable();
        playerActions.XRIRightHand.Enable();
        playerActions.DefaultControls.Enable();

        currentActionMap = playerActions.DefaultControls;

        //set up which character controller to use
        if (!(string.IsNullOrEmpty(XRSettings.loadedDeviceName))) //old: XRSettings.isDeviceActive
        {
            Debug.Log("Using " + XRSettings.loadedDeviceName + " headset");
            movementType = MovementTypeEnum.SmoothMovement;
            rotationType = RotationTypeEnum.SnapRotation;
            SetControlType(ControlSchemeEnum.VR);
        }
        else
        {
            Debug.Log("Using PC or Gamepad");
            movementType = MovementTypeEnum.SmoothMovement;
            rotationType = RotationTypeEnum.SmoothRotation;
            SetControlType(ControlSchemeEnum.Gamepad); //Start with gamepad because if you are on console then there will be no mouse input to switch inputs (pc can have gamepad and mouse inputs)
            //cursorController.CreateVirtualMouse();
        }

        //DIFFERENT WAYS TO DO INPUTS (BELOW)
        ////Can place in update loop
        ////IsPressed(), WasReleasedThisFrame(), ReadValueAsButton(), WasPerformedThisFrame()
        //var jumpAction = playerActions.DefaultControls.Jump.actionMap.actions["Jump"];
        //if (jumpAction.IsPressed())
        //{
        //    // Do Something
        //}
        ////Can place in update loop
        //if (playerActions.UIControls.Jump.WasReleasedThisFrame())
        //{
        //    // Do Something
        //}
        ////Can place in update loop
        //if (playerActions.DefaultControls.Jump.phase == InputActionPhase.Started)
        //{
        //    // Do Something
        //}
        ////Can place in update loop
        //if (playerActions.DefaultControls.Jump.ReadValue<float>() > 0.4f)
        //{
        //    // Do Something
        //}
        ////Can place in update loop
        //if (playerActions.DefaultControls.Jump.triggered)
        //{
        //    // Do Something
        //}
        //playerActions.PlayerControls.Jump.started += value => { 
        //    //Code goes here
        //    Debug.Log("Jump"); 
        //};
        //playerActions.DefaultControls.Jump.started += ChangeGrabState; //this is recommended over using the lamba (value) because its easier to unsubscribe to
        //playerActions.DefaultControls.Jump.started += value => ChangeGrabState(value);
        //playerActions.DefaultControls.Jump.performed += value => ChangeGrabState(value.ReadValue<float>());
        //playerActions.DefaultControls.Jump.started += value => left = value.ReadValue<bool>();
    }

    private void ChangeGrabState(UnityEngine.InputSystem.InputAction.CallbackContext value)
    {
        if (value.started)
        {
            Debug.Log("Started");
        }
        else if (value.performed)
        {
            Debug.Log("Performed");
        }
        else if (value.canceled)
        {
            Debug.Log("Canceled");
        }
    }

    private void ChangeGrabState(Vector2 value)
    {
        Debug.Log(value);
    }

    [ContextMenu("Enable Player Controls")]
    public void EnablePlayerControls()
    {
        DisableControls();
        playerActions.Enable();
        playerActions.XRIHead.Enable();
        playerActions.XRILeftHand.Enable();
        playerActions.XRIRightHand.Enable();
        playerActions.DefaultControls.Enable();
    }

    [ContextMenu("Disable Controls")]
    public void DisableControls()
    {
        playerActions.Disable();
    }

    public void EnableAllRays()
    {
        if(targetControlScheme != ControlSchemeEnum.VR)
        {
            return;
        }

        GameObject leftHandController = GameObject.FindGameObjectWithTag("leftControllerXR");
        GameObject rightHandController = GameObject.FindGameObjectWithTag("rightControllerXR");
        
        if (((leftHandController != null) && (rightHandController != null)))
        {
            leftHandController.GetComponent<XRInteractorLineVisual>().validColorGradient = defaultGradient;
            rightHandController.GetComponent<XRInteractorLineVisual>().validColorGradient = defaultGradient;
            leftHandController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = defaultGradient;
            rightHandController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = defaultGradient;
        }
    }

    public void EnableUiRays()
    {
        if (targetControlScheme != ControlSchemeEnum.VR)
        {
            return;
        }

        GameObject leftHandController = GameObject.FindGameObjectWithTag("leftControllerXR");
        GameObject rightHandController = GameObject.FindGameObjectWithTag("rightControllerXR");
        if (((leftHandController != null) && (rightHandController != null)))
        {
            leftHandController.GetComponent<XRInteractorLineVisual>().validColorGradient = defaultGradient;
            rightHandController.GetComponent<XRInteractorLineVisual>().validColorGradient = defaultGradient;
            leftHandController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = clearGradient;
            rightHandController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = clearGradient;
        }
    }

    public void DisableAllRays()
    {
        GameObject leftHandController = GameObject.FindGameObjectWithTag("leftControllerXR");
        GameObject rightHandController = GameObject.FindGameObjectWithTag("rightControllerXR");
        if (((leftHandController != null) && (rightHandController != null)))
        {
            leftHandController.GetComponent<XRInteractorLineVisual>().validColorGradient = clearGradient;
            rightHandController.GetComponent<XRInteractorLineVisual>().validColorGradient = clearGradient;
            leftHandController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = clearGradient;
            rightHandController.GetComponent<XRInteractorLineVisual>().invalidColorGradient = clearGradient;
        }
    }

    #region Action Maps

    public void ChangeActionMap(InputActionMap newActionMap)
    {
        Debug.Log("Switching action map to: " + newActionMap.name);
        currentActionMap.Disable();
        currentActionMap = newActionMap;
        currentActionMap.Enable();
    }

    #endregion

    #region Control Schemes

    [ContextMenu("PC Controls Only")]
    public void SwitchControlTypeToPC()
    {
        SetControlType(ControlSchemeEnum.PC);
    }

    [ContextMenu("VR Controls Only")]
    public void SwitchControlTypeToVR()
    {
        SetControlType(ControlSchemeEnum.VR);
    }

    [ContextMenu("Controller Controls Only")]
    public void SwitchControlTypeToGamePad()
    {
        SetControlType(ControlSchemeEnum.Gamepad);
    }

    public void SetControlType(ControlSchemeEnum controlType)
    {
        targetControlScheme = controlType;

        switch (controlType)
        {
            case ControlSchemeEnum.PC:
                {
                    playerActions.asset.bindingMask = new InputBinding { groups = "PC" };
                    baseInputModule.enabled = true;
                    xrInputModule.enabled = false;
                    break;
                }
            case ControlSchemeEnum.Gamepad:
                {
                    playerActions.asset.bindingMask = new InputBinding { groups = "Gamepad" };
                    baseInputModule.enabled = true;
                    xrInputModule.enabled = false;
                    break;
                }
            case ControlSchemeEnum.VR:
                {
                    playerActions.asset.bindingMask = new InputBinding { groups = "Generic XR Controller" };
                    baseInputModule.enabled = false;
                    xrInputModule.enabled = true;
                    break;
                }
            case ControlSchemeEnum.None:
                {
                    baseInputModule.enabled = false;
                    xrInputModule.enabled = false;
                    break;
                }
        }
    }

    #endregion
}