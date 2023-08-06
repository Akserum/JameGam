using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class TutorialManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameEvent tutorialEvent;

    private GameObject _tutoriaPanel;
    private GameObject _timerPanel;
    private TextMeshProUGUI _textTimer;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _tutoriaPanel = transform.GetChild(1).gameObject;
        _timerPanel = transform.GetChild(0).gameObject;
        _textTimer = _timerPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        tutorialEvent.Raise();
        EnableTutorialPanel(true);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Start a short timer and start the game
    /// </summary>
    public void StartTimer()
    {
        EnableTutorialPanel(false);
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        int countdown = 3;
        while (countdown >= 0)
        {
            yield return new WaitForSeconds(1f);
            countdown--;
            string text = countdown <= 0 ? "Go !" : countdown.ToString();
            _textTimer.SetText(text);
        }

        //Start game
        GameManager.Instance.StartGame();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Enable or disable tutorial panel
    /// </summary>
    private void EnableTutorialPanel(bool enabled)
    {
        _tutoriaPanel.SetActive(enabled);
        _timerPanel.SetActive(!enabled);
    }
    #endregion
}