using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using static UnityEditor.Experimental.GraphView.GraphView;
#endif

public class S_GameEndDisplay : MonoBehaviour
{
    [SerializeField] List<GameObject> _playersPanels = new List<GameObject>();
    [SerializeField] List<GameObject> _playersCarsByID = new List<GameObject>();
    [SerializeField] List<Transform> _cameraPosition = new List<Transform>();
    [SerializeField] List<Transform> _visuelsPosition = new List<Transform>();
    [SerializeField] List<TextMeshProUGUI> _textRank = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> _textGetKilled = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> _textPeopleKilled = new List<TextMeshProUGUI>();
    [SerializeField] List<TextMeshProUGUI> _textPlayersNames = new List<TextMeshProUGUI>();






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

            _textPlayersNames[playerID].text += (player.Value.playerId + 1);

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

            //_textGetKilled[player.Value.playerId].text += "\n";
            _textPeopleKilled[player.Value.playerId].text += "\n";

            for (int i = 0; i < player.Value.listKilledBy.Count; i++)
            {
                Debug.Log(player.Value.listKilledBy[i]);
                //TextMeshProUGUI text = new TextMeshProUGUI();
                //string textes = player.Value.listKilledBy[i];
                //text.text = textes;
                //text.color = Color.green;
                ////SetTextColor(text, player.Value.playerId);
                //_textGetKilled[player.Value.playerId].text += "\n" + text;
                //_textGetKilled[player.Value.playerId].color = Color.red;
                _textGetKilled[player.Value.playerId].text += "\n" + player.Value.listKilledBy[i];

            }
            for (int i = 0; i < player.Value.listYouKilled.Count; i++)
            {
                _textPeopleKilled[player.Value.playerId].text +=  player.Value.listYouKilled[i] + " ";
            }
            

        }
    }

    public TextMeshProUGUI SetTextColor(TextMeshProUGUI text, int playerID)
    {
        switch (playerID)
        {
            case 0:
                text.color = Color.green;
                break;
            case 1:
                text.color = Color.red;
                break;
            case 2:
                text.color = Color.yellow;
                break;
            case 3:
                text.color = Color.blue;
                break;
            case 4:
                text.color = Color.grey;
                break;
            default:
                text.color = Color.green;
                break;
        }
        return text;
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
