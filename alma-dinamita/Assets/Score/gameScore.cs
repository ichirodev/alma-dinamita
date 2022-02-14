using UnityEngine;

public class gameScore : MonoBehaviour
{
    [SerializeField] int _gameScore;
    private int commonScore = 15;
    private int specialScore = 45;
    private int score15Seconds = 5;
    
    private void Start()
    {
        _gameScore = 0;
    }

    public int ScoreUpCommonKilled()
    {
        _gameScore += commonScore;
        return _gameScore;
    }

    public int ScoreUpSpecialKilled()
    {
        _gameScore += specialScore;
        return _gameScore;
    }

    public int FinalScore(int gameTimeInSeconds)
    {
        var timeScore = (int)(gameTimeInSeconds / 15) * score15Seconds;
        _gameScore = _gameScore + timeScore + 100;
        return _gameScore;
    }

    public int GetGameScore()
    {
        return _gameScore;
    }
}
