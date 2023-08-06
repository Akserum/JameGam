using UnityEngine;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    #region Variables
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI resultsText;
    private GameManager _gm;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _gm = GameManager.Instance;
    }

    private void OnEnable()
    {
        ShowResults();
    }
    #endregion

    #region Methods
    /// <summary>
    /// Show game results
    /// </summary>
    private void ShowResults()
    {
        int requiredItemsAmount = _gm.GetRequiredItemCollected();

        //Convert times
        TimeConverter.ConvertTime(_gm.Timer, out int remainMin, out int remainSec);
        TimeConverter.ConvertTime(_gm.TotalBonusTime, out int bonusMin, out int bonusSec);
        TimeConverter.ConvertTime(_gm.TotalMalusTime, out int malusMin, out int malusSec);

        //Infos game items
        string results = $"Score: {_gm.Score}$" + "\r\n";
        results += $"Required items collected: {requiredItemsAmount}/{_gm.RequiredItems.Length}." + "\r\n";
        results += $"All collected items: {_gm.ScoredItems.Count}/{_gm.AllItems.Length}." + "\r\n";
        //Infos game time
        results += $"Remaining time: {remainMin}m {remainSec}s." + "\r\n";
        //Infos bonus/malus
        string bonus = bonusMin <= 0 ? $"{bonusSec}s": $"{bonusMin}m {bonusSec}s"; 
        string malus = malusMin <= 0 ? $"{malusSec}s": $"{malusMin}m {malusSec}s";
        results += $"Bonus time: {bonus}." + "\r\n";
        results += $"Penalty time: {malus}.";

        resultsText.SetText(results);
        titleText.SetText(GetTitle());
    }

    /// <summary>
    /// Get title for the end game panel
    /// </summary>
    /// <returns></returns>
    private string GetTitle()
    {
        if (_gm.Timer > 0)
            return "<b>VICTORY</b> \r\n Perfect! You did a great job!";

        return _gm.Timer <= 0 && _gm.CanEscape ? "<b>VICTORY</b> \r\n You escaped!" :
                                                 "<b>LOSE</b> \r\n You couldn't escape at time!";
    }
    #endregion
}
