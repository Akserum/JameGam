using UnityEngine;
using System.Collections;
using System;

public class GameManager : SingletonClass<GameManager>
{
    #region Variables
    [Header("Game Properties")]
    [SerializeField] private float gameDuration = 180f;
    [SerializeField] private float bonusTime = 5f;

    public event Action OnScoreChanged;
    #endregion

    #region Properties
    public float Timer { get; private set; }
    public int Score { get; private set; }
    #endregion

    #region Buitls_In
    protected override void Awake()
    {
        base.Awake();
        Timer = gameDuration;
    }

    private void Start()
    {
        SetScore(0);
        StartCoroutine(GameTimerRoutine());
    }

    private void OnEnable()
    {
        ScoreZone.OnItemScored += AddBonusTime;
    }

    private void OnDisable()
    {
        ScoreZone.OnItemScored -= AddBonusTime;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Set score value
    /// </summary>
    private void SetScore(int value)
    {
        Score = value;
        OnScoreChanged?.Invoke();
    }
    #endregion

    #region Time Methods
    /// <summary>
    /// Handle game time
    /// </summary>
    private IEnumerator GameTimerRoutine()
    {
        while(Timer > 0)
        {
            Timer -= Time.deltaTime;
            yield return null;
        }

        //Raise end game
    }

    /// <summary>
    /// Add a given amount of time to the current timer
    /// </summary>
    private void AddBonusTime(ItemSO item)
    {
        Timer += bonusTime;
    }
    #endregion
}
