using System;
using Unity.Netcode;

public class ScoreSystem : NetworkBehaviour
{
    public static ScoreSystem Instance;
    
    public event Action OnScoreChanged;

    public int Score => _score;
    
    private int _score;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else 
            Destroy(gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    public void AddScoreServerRpc(int score)
    {
        AddScoreClientRpc(score);
    }

    [ClientRpc]
    private void AddScoreClientRpc(int score)
    {
        _score += score;
        if (_score < 0)
            _score = 0;
        OnScoreChanged?.Invoke();
    }
}
