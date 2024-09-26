using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class MapSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] maps;
    //[SerializeField] private float slideSpeed = 5f;
    public PlayerData playerData;

    private int currentMapIndex = 0;

    private void Start()
    {
        
        DisableOtherPlayersControls();

        UpdateMapDisplay();
    }

    private void Update()
    {
        
            //var playerInput = playerData.playerPanelMapping[playerData.playerPanelMapping.FirstOrDefault(x => x.Value.playerId == 0)]._playerInput;
            //Vector2 moveInput = playerInput.actions["Steer"].ReadValue<Vector2>();

            
            //if (moveInput.x > 0)
            //{
            //    NextMap();
            //}
            //else if (moveInput.x < 0)
            //{
            //    PreviousMap();
            //}
            //Debug.Log(currentMapIndex);
            
        
    }

    private void NextMap()
    {
        currentMapIndex++;
        if (currentMapIndex >= maps.Length)
        {
            currentMapIndex = 0;
        }
        UpdateMapDisplay();
    }

    private void PreviousMap()
    {
        currentMapIndex--;
        if (currentMapIndex < 0)
        {
            currentMapIndex = maps.Length - 1;
        }
        UpdateMapDisplay();
    }

    private void UpdateMapDisplay()
    {
        
        foreach (var map in maps)
        {
            map.SetActive(false);
        }

        
        maps[currentMapIndex].SetActive(true);
    }

    private void DisableOtherPlayersControls()
    {
        foreach (var playerInfo in playerData.playerPanelMapping.Values)
        {
            if (playerInfo.playerId != 0)
            {
                var input = playerInfo._playerInput;
                input.actions.Disable();
            }
        }
    }
}
