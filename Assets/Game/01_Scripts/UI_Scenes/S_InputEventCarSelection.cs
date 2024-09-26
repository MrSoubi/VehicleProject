using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_InputEventCarSelection : MonoBehaviour
{
    public S_CarSelection carSelectionManager;
    public S_CarSwitch carSwitchManager;
    public PlayerInput playerInput;


    private void Awake()
    {
        SubscribeToCarSwitching();
    }

    public void OnDestroy()
    {
        UnsubscribeFromAll();
    }

    public void SubscribeToCarSwitching()
    {
        playerInput.actions["Steer"].performed += carSwitchManager.SwitchCar;
        playerInput.actions["Jump"].performed += carSelectionManager.OnSouthButtonPress;
    }

    public void UnsubscribeFromCarSwitching()
    {
        playerInput.actions["Steer"].performed -= carSwitchManager.SwitchCar;
    }

    public void SubscribeToCarValidation()
    {
        playerInput.actions["Jump"].performed += carSwitchManager.OnValidateButtonPress;
    }

    public void UnsubscribeFromOnButtonPress()
    {
        playerInput.actions["Jump"].performed -= carSelectionManager.OnSouthButtonPress;
        playerInput.actions["Jump"].performed += carSwitchManager.OnValidateButtonPress;

    }

    public void UnsubscribeFromAll()
    {
        playerInput.actions["Steer"].performed -= carSwitchManager.SwitchCar;
        playerInput.actions["Jump"].performed -= carSelectionManager.OnSouthButtonPress;
        playerInput.actions["Jump"].performed -= carSwitchManager.OnValidateButtonPress;
    }
}

