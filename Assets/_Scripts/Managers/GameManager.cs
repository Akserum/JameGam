using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : SingletonClass<GameManager>
{
    #region Variables
    [Header("Game Properties")]
    [SerializeField] private float gameDuration = 180f;
    [SerializeField] private float bonusTime = 5f;

    [Header("Game Items options")]
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private int requiredItemsAmount = 2;

    public event System.Action OnScoreChanged;
    #endregion

    #region Properties
    public float Timer { get; private set; }
    public int Score { get; private set; }
    public ItemSO[] RequiredItems { get; private set; }
    #endregion

    #region Builts_In
    protected override void Awake()
    {
        base.Awake();
        Timer = gameDuration;
        SelectRandomItems();
    }

    private void Start()
    {
        SetScore(0);
        StartCoroutine(GameTimerRoutine());
    }

    private void OnEnable()
    {
        ScoreZone.OnItemScored += AddScore;
        ScoreZone.OnItemScored += AddBonusTime;
    }

    private void OnDisable()
    {
        ScoreZone.OnItemScored -= AddScore;
        ScoreZone.OnItemScored -= AddBonusTime;
    }
    #endregion

    #region Score Methods
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

    #region Game Item Methods
    /// <summary>
    /// Select randomly a given amount of items in the database
    /// </summary>
    private void SelectRandomItems()
    {
        List<ItemSO> datas = itemDataBase.Items.ToList();
        RequiredItems = new ItemSO[requiredItemsAmount];

        for (int i = 0; i < RequiredItems.Length; i++)
        {
            int index = Random.Range(0, datas.Count);
            ItemSO item = datas.ElementAt(index);

            if (!item)
                return;

            RequiredItems[i] = item;
            datas.RemoveAt(index);
        }

        datas.Clear();
    }

    /// <summary>
    /// Indicates if the given item is required
    /// </summary>
    public bool HasItemInList(ItemSO Item)
    {
        foreach (ItemSO item in RequiredItems)
        {
            if (item == Item)
                return true;
            else
                continue;
        }

        return false;
    }
    #endregion
}
