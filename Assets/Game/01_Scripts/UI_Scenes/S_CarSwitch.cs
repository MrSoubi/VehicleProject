using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;


public class S_CarSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _cars;
    [SerializeField] private Camera[] _cameras;
    [SerializeField] private RawImage[] carDisplays;
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationSpeed = 30f;
    public S_CarSelection S_CarSelection;
    private Dictionary<InputDevice, PlayerInfo> players => S_CarSelection.ReturnPlayerInfo();

    [SerializeField] private S_InputEventCarSelection _inputEvent;
    

    private void Update()
    {


        

    }

    private IEnumerator RotateCamera(int playerID, bool rotateRight)
    {
        players[players.FirstOrDefault(x => x.Value.playerId == playerID).Key].isRotating = true;

        float totalRotation = 0f;

        while (totalRotation < 90f)
        {
            float rotationThisFrame = _rotationSpeed * Time.deltaTime;

            if (totalRotation + rotationThisFrame >= 90f)
            {
                rotationThisFrame = 90f - totalRotation;
            }

            Vector3 rotationDirection = rotateRight ? Vector3.down : Vector3.up;
            _cameras[playerID].transform.RotateAround(_target.position, rotationDirection, rotationThisFrame);
            totalRotation += rotationThisFrame;


            yield return null;
        }

        players[players.FirstOrDefault(x => x.Value.playerId == playerID).Key].isRotating = false;
    }
    

    public void SwitchCar(InputAction.CallbackContext context)
    {
        int PlayerID = 0;
        InputDevice currentDevice = context.control.device;

        var playerInfo = players.FirstOrDefault(x => x.Key == currentDevice).Value;
        if (playerInfo != null)
        {
            PlayerID = playerInfo.playerId;
        }

        //foreach (var value in players)
        //{
        //    if (value.Key == currentDevice)
        //    {
        //        PlayerID = value.Value.playerId;
        //        break;
        //    }
        //}

        Debug.Log($"Switch {PlayerID}");

        var matchingEntry = players.FirstOrDefault(x => x.Key == context.control.device && x.Value.isValidateSelection == false);

        if (context.performed && matchingEntry.Value.isValidateSelection == false)
        {
            float input = context.ReadValue<float>();
            if (input != 0 && !players[currentDevice].isRotating)
            {
                StartCoroutine(RotateCamera(PlayerID, input > 0));

                players[currentDevice].carIDSelected += input > 0 ? 1 : -1;

                if (players[currentDevice].carIDSelected == -1)
                {
                    players[currentDevice].carIDSelected = _cars.Length - 1;
                }
                else if (players[currentDevice].carIDSelected == _cars.Length)
                {
                    players[currentDevice].carIDSelected = 0;
                }

                Debug.Log("car ID selected: " + players[currentDevice].carIDSelected);
            }
        }
    }

    



    public void OnValidateButtonPress(InputAction.CallbackContext context)
    {
        //var matchingEntry = players.FirstOrDefault(x => x.Key == context.control.device && x.Value.isValidateSelection == false);

        if (context.performed && players.FirstOrDefault(x => x.Key == context.control.device).Value.isValidateSelection == false)
        {
            InputDevice currentDevice = context.control.device;
            players.FirstOrDefault(x => x.Key == context.control.device).Value.isValidateSelection = true;
        }


    }
}
