using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class S_CarSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] carSelectionPanels;
    [SerializeField] private GameObject[] textPressATojoin;

    private List<int> availablePanels = new List<int>(); 
    private Dictionary<InputDevice, PlayerInfo> playerPanelMapping = new Dictionary<InputDevice, PlayerInfo>();

    [SerializeField] private S_InputEventCarSelection _inputEvent;

    private int nextPlayerId = 0; 

    private void Start()
    {
        
        for (int i = 0; i < carSelectionPanels.Length; i++)
        {
            carSelectionPanels[i].SetActive(false);
            availablePanels.Add(i); 
        }

    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
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

                    //_inputEvent.UnsubscribeFromOnButtonPress();

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