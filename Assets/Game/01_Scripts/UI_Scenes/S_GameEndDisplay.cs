using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class S_GameEndDisplay : MonoBehaviour
{
    [SerializeField] List<GameObject> _playersPanels = new List<GameObject>();
    [SerializeField] List<GameObject> _playersCarsByID = new List<GameObject>();
    [SerializeField] List<Transform> _cameraPosition = new List<Transform>();
    [SerializeField] List<Transform> _visuelsPosition = new List<Transform>();
    [SerializeField] List<TextMeshProUGUI> _textRank = new List<TextMeshProUGUI>();
    [SerializeField] private EventChannel _gameOverEvent;



    List<GameObject> _playersCars = new List<GameObject>();


    [SerializeField] PlayersData _playersData;
    private Dictionary<InputDevice, PlayerInfo> _players => _playersData.players;

    private void OnEnable()
    {
        _gameOverEvent.onEventTriggered.AddListener(TextRankUpdate);

    }
    void OnDisable()
    {
        _gameOverEvent.onEventTriggered.RemoveListener(TextRankUpdate);
    }

    void Start()
    {
        foreach (var player in _players)
        {
            int carID = player.Value.carIDSelected;
            int playerID = player.Value.playerId;
            var carVisuel = Instantiate(_playersCarsByID[carID], _visuelsPosition[playerID]);
            _playersCars.Add(carVisuel);
            carVisuel.transform.SetPositionAndRotation(_visuelsPosition[playerID].position, _visuelsPosition[playerID].rotation);

            _playersPanels[playerID].SetActive(true);


        }
    }

    void Update()
    {
        
    }

    private void TextRankUpdate()
    {
        foreach(var player in _players)
        {
            _textRank[player.Value.playerId].text = "#" + player.Value.rank;

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
