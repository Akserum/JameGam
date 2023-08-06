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
        TimeConverter.ConvertTime(time, out int minutes, out int seconds);
        _textMesh.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    /// <summary>
    /// Modify the color when the time is low
    /// </summary>
    private void HandleTextColor()
    {
        Color color = _gameManager.Timer <= 59 ? lowColor : baseColor;
        _textMesh.color = color;
    }
    #endregion
}

#region TimeConverter Class
public abstract class TimeConverter
{
    /// <summary>
    /// Convert a time in minuts, seconds format and return both parameters
    /// </summary>
    /// <param name="time"> Duration</param>
    /// <param name="minutes"> Minutes in time </param>
    /// <param name="seconds"> Seconds in time </param>
    public static void ConvertTime(float time, out int minutes, out int seconds)
    {
        int m_minutes = Mathf.FloorToInt(time / 60);
        int m_seconds = Mathf.CeilToInt(time % 60);

        if (m_seconds == 60)
        {
            m_minutes += 1;
            m_seconds = 00;
        }

        minutes = m_minutes;
        seconds = m_seconds;
    }
}
#endregion