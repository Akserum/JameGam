using UnityEngine;
using System.Collections;

public class GameManager : SingletonClass<GameManager>
{
    #region Variables
    [Header("Game Properties")]
    [SerializeField] private float gameDuration = 180;
    #endregion

    #region Properties
    public float Timer { get; private set; }
    #endregion

    #region Buitls_In
    protected override void Awake()
    {
        base.Awake();
        Timer = gameDuration;
    }

    private void Start()
    {
        StartCoroutine(GameTimerRoutine());
    }
    #endregion

    #region Methods
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
    #endregion
}
