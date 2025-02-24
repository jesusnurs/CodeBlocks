using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OfficeGameManager : NetworkBehaviour
{
    public static OfficeGameManager Instance;
    
    private event Action OnGameStarted;
    private event Action OnGameOver;
    private event Action OnGamePaused;
    private event Action OnStateChanged;
    private event Action OnLocalPlayerReadyChanged;

    private NetworkVariable<State> _state = new NetworkVariable<State>(State.WaitingToStart);
    
    private bool _isLocalPlayerReady;
    private NetworkVariable<bool> _isGamePaused = new NetworkVariable<bool>(false);
    
    private NetworkVariable<float> _countdownToStartTimer =  new NetworkVariable<float>(3f);
    private NetworkVariable<float> _gamePlayingTimer =  new NetworkVariable<float>(0f);
    private float _gamePlayingTimerMax;
    
    private Dictionary<ulong,bool> _playersReadyDictionary;
    
    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
        
        _playersReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        _state.OnValueChanged += OnStateValueChanged;
    }

    private void Update()
    {
        if(!IsServer)
            return;

        switch (_state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountdownToStart:
                _countdownToStartTimer.Value -= Time.deltaTime;
                if (_countdownToStartTimer.Value <= 0)
                {
                    _state.Value = State.GamePlaying;
                    _gamePlayingTimer.Value = _gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                _gamePlayingTimer.Value -= Time.deltaTime;
                if (_gamePlayingTimer.Value <= 0)
                {
                    _state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void OnInteractAction()
    {
        if (_state.Value == State.WaitingToStart)
        {
            _isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke();
            
            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        
        bool allPlayersReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (_playersReadyDictionary.ContainsKey(clientId) || !_playersReadyDictionary[clientId])
            {
                allPlayersReady = false;
                break;
            }
        }
        
        if(allPlayersReady)
            _state.Value = State.CountdownToStart;
    }
    
    public bool IsGamePlaying() => _state.Value == State.GamePlaying;
    
    public bool IsCountdownToStartActive() => _state.Value == State.CountdownToStart;
    
    public bool IsGameOver() => _state.Value == State.GameOver;
    
    public bool IsLocalPlayerReady() => _isLocalPlayerReady;
    
    public float GetCountdownToStartTimer => _countdownToStartTimer.Value;

    private void OnStateValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke();
    }
}
