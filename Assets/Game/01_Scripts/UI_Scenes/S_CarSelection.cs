using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class S_CarSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] carSelectionPanels;
    [SerializeField] private GameObject[] textPressATojoin;
    [SerializeField] private TextMeshProUGUI timerText;

    private List<int> availablePanels = new List<int>();
    public PlayerData playerData;
    public Dictionary<InputDevice, PlayerInfo> playerPanelMapping => playerData.playerPanelMapping;

    [SerializeField] private S_InputEventCarSelection _inputEvent;

    private int nextPlayerId = 0;

    private Coroutine loadSceneCoroutine;
    private bool isLoadingScene = false;
    [SerializeField] private float countdownTimer = 3f;

    private void Start()
    {
        
        for (int i = 0; i < carSelectionPanels.Length; i++)
        {
            carSelectionPanels[i].SetActive(false);
            availablePanels.Add(i); 
        }

    }

    private void Update()
    {
        
    }
    private void Awake()
    {

    }

    public void OnSouthButtonPress(InputAction.CallbackContext context)
    {

        if (context.performed)
        {
            InputDevice playerDevice = context.control.device;

            var playerInput = PlayerInput.all.FirstOrDefault(pi => pi.devices.Contains(playerDevice));

            if (playerInput != null && !playerPanelMapping.ContainsKey(playerDevice))
            {
                if (availablePanels.Count > 0)
                {
                    int assignedPanel = availablePanels[0];
                    availablePanels.RemoveAt(0);

                    PlayerInfo newPlayer = new PlayerInfo
                    {
                        playerId = nextPlayerId++,
                        panelIndex = assignedPanel,
                        _playerInput = playerInput, 
                        jumpAction = playerInput.actions["Jump"]
                    };

                    playerPanelMapping.Add(playerDevice, newPlayer);
                    Debug.Log("yes "+ playerInput.ToString());
                    carSelectionPanels[assignedPanel].SetActive(true);
                    textPressATojoin[assignedPanel].SetActive(false);

                    //_inputEvent.DisablePlayerInputEnterParty(playerDevice);

                    Debug.Log($"Joueur {newPlayer.playerId} avec {playerDevice.name} a été assigné au cadrant {assignedPanel + 1}");

                    OnPlayerJoined();
                }
            }
        }
    }

    public Dictionary<InputDevice, PlayerInfo> ReturnPlayerInfo()
    {
        return playerPanelMapping;
    }

    public void OnPlayerJoined()
    {
        
        if (isLoadingScene)
        {
            StopCoroutine(loadSceneCoroutine);
            isLoadingScene = false;
            timerText.gameObject.SetActive(false);
        }
    }

    public void CheckAllPlayersSelection()
    {      
        foreach (var playerInfo in playerPanelMapping.Values)
        {
            if (!playerInfo.isValidateSelection)
            {
                return;
            }
        }

        loadSceneCoroutine = StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        isLoadingScene = true;
        timerText.gameObject.SetActive(true);
        countdownTimer = 3f;
        while (countdownTimer > 0)
        {
            timerText.text = Mathf.CeilToInt(countdownTimer).ToString();
            Debug.Log($"Chargement dans {countdownTimer:F1} secondes...");
            yield return new WaitForSeconds(0.1f);
            countdownTimer -= 0.1f;
        }


        SceneManager.LoadScene("ArenaSelection");
    }

   
}
    
