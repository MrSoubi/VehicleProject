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
        
        playerInput.actions["Jump"].performed += carSelectionManager.OnSouthButtonPress;
        playerInput.actions["Steer"].performed += carSwitchManager.SwitchCar;
        playerInput.actions["Boost"].performed += carSwitchManager.OnValidateButtonPress;
    }


    private void OnDestroy()
    {
        playerInput.actions["Jump"].performed -= carSelectionManager.OnSouthButtonPress;
        playerInput.actions["Steer"].performed -= carSwitchManager.SwitchCar;
        playerInput.actions["Boost"].performed += carSwitchManager.OnValidateButtonPress;
    }
}
