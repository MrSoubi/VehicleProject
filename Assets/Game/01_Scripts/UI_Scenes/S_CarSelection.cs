using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_CarSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] carSelectionPanels;
    [SerializeField] private GameObject[] textPressATojoin;

    private List<int> availablePanels = new List<int>(); 
    private Dictionary<InputDevice, PlayerInfo> playerPanelMapping = new Dictionary<InputDevice, PlayerInfo>(); 

    private int nextPlayerId = 0; 

    private void Start()
    {
        // Initialiser les panneaux comme inactifs
        for (int i = 0; i < carSelectionPanels.Length; i++)
        {
            carSelectionPanels[i].SetActive(false);
            availablePanels.Add(i); 
        }

    }

    public void OnSouthButtonPress(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            var playerInput = context.control.device;

            if (!playerPanelMapping.ContainsKey(playerInput))
            {
                if (availablePanels.Count > 0)
                {
                    int assignedPanel = availablePanels[0];
                    availablePanels.RemoveAt(0);
              
                    PlayerInfo newPlayer = new PlayerInfo
                    {
                        playerId = nextPlayerId++,
                        panelIndex = assignedPanel,

                    };

                    playerPanelMapping.Add(playerInput, newPlayer);

                    carSelectionPanels[assignedPanel].SetActive(true);
                    textPressATojoin[assignedPanel].SetActive(false);

                    

                    Debug.Log($"Joueur {newPlayer.playerId} avec {playerInput.name} a été assigné au cadrant {assignedPanel + 1}");
                }
            }
        }
    }

    public Dictionary<InputDevice, PlayerInfo> ReturnPlayerInfo()
    {
        return playerPanelMapping;
    }
}