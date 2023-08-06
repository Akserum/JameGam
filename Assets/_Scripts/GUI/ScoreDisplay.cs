using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreDisplay : MonoBehaviour
{
    #region Variables
    private TextMeshProUGUI _textMesh;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnScoreChanged += ShowScore;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScoreChanged -= ShowScore;
    }
    #endregion

    #region Methods
    private void ShowScore()
    {
        _textMesh.text = GameManager.Instance.Score.ToString() + "$";
    }
    #endregion
}