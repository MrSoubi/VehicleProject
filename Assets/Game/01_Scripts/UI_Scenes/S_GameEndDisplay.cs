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
            _textPlayersNames[playerID].text = SetTextColor(_textPlayersNames[playerID].text, player.Value.playerId);

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
            _textRank[player.Value.playerId].text = SetTextRankColor("#" + player.Value.rank, player.Value.rank);

            //_textGetKilled[player.Value.playerId].text += "\n";
            _textPeopleKilled[player.Value.playerId].text += "\n";

            for (int i = 0; i < player.Value.listKilledBy.Count; i++)
            {
                _textGetKilled[player.Value.playerId].text += "\n" + SetTextColor(player.Value.listKilledBy[i].ToString(), (int)player.Value.listKilledBy[i]);

            }
            for (int i = 0; i < player.Value.listYouKilled.Count; i++)
            {
                _textPeopleKilled[player.Value.playerId].text += SetTextColor(player.Value.listYouKilled[i].ToString(), (int)player.Value.listYouKilled[i]) + " ";
            }
        }
    }

    public string SetTextColor(string text, int playerID)
    {
        switch (playerID)
        {
            case 0:
                text = $"<color=green>{text}</color>";

                break;
            case 1:
                text = $"<color=red>{text}</color>";
                break;
            case 2:
                text = $"<color=yellow>{text}</color>";
                break;
            case 3:
                text = $"<color=blue>{text}</color>";
                break;
            case 4:
                text = $"<color=black>{text}</color>";
                break;
            default:
                text = $"<color=red>{text}</color>";
                break;
        }
        return text;
    }

    public string SetTextRankColor(string text, int playerRank)
    {
        switch (playerRank)
        {
            case 1:
                text = $"<color=#FFD700>{text}</color>";
                break;
            case 2:
                text = $"<color=#C0C0C0>{text}</color>";
                break;
            case 3:
                text = $"<color=#CD7F32>{text}</color>";
                break;
            case 4:
                text = $"<color=#B87333>{text}</color>";
                break;
            default:
                text = $"<color=#B87333>{text}</color>";
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
