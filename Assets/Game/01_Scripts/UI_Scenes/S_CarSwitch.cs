using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.SceneManagement;


public class S_CarSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] _cars;
    [SerializeField] private Camera[] _cameras;
    [SerializeField] private RawImage[] carDisplays;
    [SerializeField] private GameObject[] _textReady;
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationSpeed = 30f;
    public S_CarSelection _carSelection;
    private Dictionary<InputDevice, PlayerInfo> _players => _carSelection.ReturnPlayerInfo();

    [SerializeField] private S_InputEventCarSelection _inputEvent;
    

    private void Update()
    {


        

    }

    private IEnumerator RotateCamera(int playerID, bool rotateRight)
    {
        _players[_players.FirstOrDefault(x => x.Value.playerId == playerID).Key].isRotating = true;

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

        _players[_players.FirstOrDefault(x => x.Value.playerId == playerID).Key].isRotating = false;
    }
    

    public void SwitchCar(InputAction.CallbackContext context)
    {
        int PlayerID = 0;
        InputDevice currentDevice = context.control.device;

        var playerInfo = _players.FirstOrDefault(x => x.Key == currentDevice).Value;
        if (playerInfo != null)
        {
            PlayerID = playerInfo.playerId;
        }

        Debug.Log($"Switch {PlayerID}");

        var matchingEntry = _players.FirstOrDefault(x => x.Key == context.control.device && x.Value.isValidateSelection == false);

        if (context.performed && matchingEntry.Value.isValidateSelection == false)
        {
            float input = context.ReadValue<float>();
            if (input != 0 && !_players[currentDevice].isRotating)
            {
                StartCoroutine(RotateCamera(PlayerID, input > 0));

                _players[currentDevice].carIDSelected += input > 0 ? 1 : -1;

                if (_players[currentDevice].carIDSelected == -1)
                {
                    _players[currentDevice].carIDSelected = _cars.Length - 1;
                }
                else if (_players[currentDevice].carIDSelected == _cars.Length)
                {
                    _players[currentDevice].carIDSelected = 0;
                }

                Debug.Log("car ID selected: " + _players[currentDevice].carIDSelected);
            }
        }
    }

    



    public void OnValidateButtonPress(PlayerInput playerInput, InputAction.CallbackContext context)
    {
        if (context.performed && _players.FirstOrDefault(x => x.Key == context.control.device).Value.isValidateSelection == false)
        {
            InputDevice currentDevice = context.control.device;
            _players.FirstOrDefault(x => x.Key == context.control.device).Value.isValidateSelection = true;
            _inputEvent.DisablePlayerInputEndSelection(playerInput);
            _textReady[_players.FirstOrDefault(x => x.Key == context.control.device).Value.playerId].gameObject.SetActive(true);
            _carSelection.CheckAllPlayersSelection();
        }


    }
    
}
