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
        foreach (var playerInfo in playerData.players.Values)
        {
            if (playerInfo.playerId != 0)
            {
                var input = playerInfo._playerInput;
                input.actions.Disable();
            }
        }
    }
}
