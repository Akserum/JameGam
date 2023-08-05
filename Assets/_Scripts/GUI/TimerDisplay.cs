using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimerDisplay : MonoBehaviour
{
    #region Variables
    [SerializeField] private Color baseColor = Color.white;
    [SerializeField] private Color lowColor = Color.red;

    private GameManager _gameManager;
    private TextMeshProUGUI _textMesh;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        DisplayGameTime();
    }

    private void FixedUpdate()
    {
        DisplayGameTime();
        HandleTextColor();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Convert and show the game time
    /// </summary>
    private void DisplayGameTime()
    {
        float time = _gameManager.Timer;
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.CeilToInt(time % 60);

        //Check if minute started
        if (seconds == 60)
        {
            minutes += 1;
            seconds = 00;
        }

        _textMesh.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    /// <summary>
    /// Modify the color when the time is low
    /// </summary>
    private void HandleTextColor()
    {
        Color color = _gameManager.Timer < 60 ? lowColor : baseColor;
        _textMesh.color = color;
    }
    #endregion
}
