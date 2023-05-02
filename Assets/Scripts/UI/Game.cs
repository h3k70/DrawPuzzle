using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private List<Player> _players = new List<Player>();
    [SerializeField] private GameObject _loosePanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private float _runDuration = 4f;

    private bool _gameEnd = false;

    private void OnEnable()
    {
        for(int i = 0; i < _players.Count; i++)
        {
            _players[i].Dying += Loose;
            _players[i].PathCreated += TryStartRun;
            _players[i].PointHasBeenReached += Win;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].Dying -= Loose;
            _players[i].PathCreated -= TryStartRun;
            _players[i].PointHasBeenReached -= Win;
        }
    }

    private void Loose()
    {
        _loosePanel.SetActive(true);
        _gameEnd = true;
    }

    private void Win()
    {
        if(_gameEnd != true)
            _winPanel.SetActive(true);
    }

    private void TryStartRun()
    {
        for (int i = 0; i < _players.Count; i++)
        {
            if (_players[i].IsReady == false)
                return;
        }
        for (int i = 0; i < _players.Count; i++)
        {
            _players[i].StartMoving(_runDuration);
        }
    }
}
