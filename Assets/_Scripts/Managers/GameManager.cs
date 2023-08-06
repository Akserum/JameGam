using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : SingletonClass<GameManager>
{
    #region Variables
    [Header("GameEvents")]
    [SerializeField] private GameEvent startGameEvent;
    [SerializeField] private GameEvent endGameEvent;
    [SerializeField] private bool startAtLaunch = false;

    [Header("Game Properties")]
    [SerializeField] private float gameDuration = 180f;
    [SerializeField] private float bonusTime = 5f;
    [SerializeField] private float malusTime = 10f;

    [Header("Game Items options")]
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private int requiredItemsAmount = 2;

    public event System.Action OnScoreChanged;
    #endregion

    #region Properties
    public float Timer { get; private set; } = 0;
    public float GameDuration => gameDuration;
    public float TotalBonusTime { get; private set; } = 0;
    public float TotalMalusTime { get; private set; } = 0;
    public int Score { get; private set; }
    public ItemSO[] RequiredItems { get; private set; }
    public List<PickableItem> ScoredItems { get; set; } = new List<PickableItem>();
    public PickableItem[] AllItems { get; private set; }
    #endregion

    #region Builts_In
    protected override void Awake()
    {
        base.Awake();
        Timer = gameDuration;

        GetAllItems();
        SelectRandomItems();
    }

    private void Start()
    {
        SetScore(0);

        if (startAtLaunch)
            StartGame();
    }

    private void OnEnable()
    {
        ScoreZone.OnItemScored += HandleScore;
    }

    private void OnDisable()
    {
        ScoreZone.OnItemScored -= HandleScore;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Method to start the game timer
    /// </summary>
    public void StartGame()
    {
        startGameEvent.Raise();
        StartCoroutine("GameTimerRoutine");
    }

    /// <summary>
    /// End game method
    /// </summary>
    private void EndGame()
    {
        Debug.Log("Game should end.");

        if (Timer > 0)
            StopCoroutine(nameof(GameTimerRoutine));

        endGameEvent.Raise();
        PointerManager.Instance.EnableCursor(true);
    }
    #endregion

    #region Score Methods
    /// <summary>
    /// Check if all objects are collected, else add score
    /// </summary>
    private void HandleScore(ItemSO item)
    {
        //All objects collected
        if (ScoredItems.Count == AllItems.Length)
        {
            EndGame();
            return;
        }

        AddScore(item);
        AddBonusTime(item);
    }

    /// <summary>
    /// Set score value
    /// </summary>
    private void SetScore(int value)
    {
        Score = value;
        OnScoreChanged?.Invoke();
    }

    /// <summary>
    /// Add score when scoring an item
    /// </summary>
    private void AddScore(ItemSO item)
    {
        int score = Score + item.Score;
        SetScore(score);
    }
    #endregion

    #region Time Methods
    /// <summary>
    /// Handle game time
    /// </summary>
    private IEnumerator GameTimerRoutine()
    {
        while (Timer > 0)
        {
            Timer -= Time.deltaTime;
            yield return null;
        }

        //Raise end game
        EndGame();
    }

    /// <summary>
    /// Add a given amount of time to the current timer
    /// </summary>
    private void AddBonusTime(ItemSO item)
    {
        Timer += bonusTime;
        TotalBonusTime += bonusTime;
    }

    /// <summary>
    /// Remove a certain amount of seconds to the game timer
    /// </summary>
    private void RemoveTime()
    {
        Timer -= malusTime;
        TotalMalusTime += malusTime;
    }
    #endregion

    #region Game Item Methods
    /// <summary>
    /// Get all the items in the level
    /// </summary>
    private void GetAllItems()
    {
        PickableItem[] items = FindObjectsOfType<PickableItem>();
        AllItems = items;
    }

    /// <summary>
    /// Select randomly a given amount of items in the database
    /// </summary>
    private void SelectRandomItems()
    {
        List<PickableItem> datas = AllItems.ToList();
        int length = datas.Count >= requiredItemsAmount ? requiredItemsAmount : datas.Count;
        RequiredItems = new ItemSO[length];

        //Get random items from the array that contains items level
        for (int i = 0; i < RequiredItems.Length; i++)
        {
            int index = Random.Range(0, datas.Count);
            ItemSO item = datas.ElementAt(index).ItemInfos;

            if (!item)
                return;

            RequiredItems[i] = item;
            datas.RemoveAt(index);
        }
    }

    /// <summary>
    /// Indicates how many required items are collected
    /// </summary>
    public int GetRequiredItemCollected()
    {
        int amount = 0;
        foreach (ItemSO item in RequiredItems)
        {
            PickableItem element = ScoredItems.Find(x => x.ItemInfos == item);

            if (!element)
                continue;
            else
                amount++;
        }

        return amount;
    }
    #endregion
}
