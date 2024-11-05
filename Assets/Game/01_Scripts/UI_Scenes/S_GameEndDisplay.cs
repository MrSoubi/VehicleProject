using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class S_GameEndDisplay : MonoBehaviour
{
    [SerializeField] List<GameObject> _playersPanels = new List<GameObject>();
    [SerializeField] List<GameObject> _playersCarsByID = new List<GameObject>();
    [SerializeField] List<Transform> _cameraPosition = new List<Transform>();
    [SerializeField] List<Transform> _visuelsPosition = new List<Transform>();
    [SerializeField] List<TextMeshProUGUI> _textRank = new List<TextMeshProUGUI>();



    List<GameObject> _playersCars = new List<GameObject>();


    [SerializeField] PlayersData _playersData;
    private Dictionary<InputDevice, PlayerInfo> _players => _playersData.players;

    private void OnEnable()
    {

    }
    void OnDisable()
    {
    }

    void Start()
    {
        foreach (var player in _players)
        {
            int carID = player.Value.carIDSelected;
            int playerID = player.Value.playerId;
            var carVisuel = Instantiate(_playersCarsByID[carID], _visuelsPosition[playerID]);
            var carColorManager = carVisuel.GetComponent<CarColorManager>();
            carColorManager.SetColor(playerID);

            _playersCars.Add(carVisuel);
            carVisuel.transform.SetPositionAndRotation(_visuelsPosition[playerID].position, _visuelsPosition[playerID].rotation);

            _playersPanels[playerID].SetActive(true);


        }
    }

    void Update()
    {
        
    }

    public void TextRankUpdate()
    {
        AssignRanks();
        foreach (var player in _players)
        {
            _textRank[player.Value.playerId].text = "#" + player.Value.rank;

        }
    }

    private void AssignRanks()
    {
        var alivePlayers = _players.Values
            .Where(x => x.isAlive == true)
            .OrderBy(x => x.playerLife)
            .ThenByDescending(x => x.bumpPourcentage)
            .ToList();

        int playerAliveCount = _playersData.players.Count;
        foreach (var players in _playersData.players)
        {
            if (players.Value.isAlive == false)
            {
                playerAliveCount--;
            }

        }

        int nextRank = playerAliveCount;

        foreach (var player in alivePlayers)
        {
            player.rank = nextRank;
            nextRank--;
        }
    }


    void FixedUpdate()
    {
        //foreach (GameObject car in _playersCars)
        //{
        //    car.transform.Rotate(Vector3.up, 50 * Time.fixedDeltaTime);
        //}
    }
}
