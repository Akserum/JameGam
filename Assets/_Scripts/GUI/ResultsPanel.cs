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
        string title = _gm.Timer <= 0 ? "Temps écoulé!" : "Perfect!";

        //Convert times
        TimeConverter.ConvertTime(_gm.Timer, out int remainMin, out int remainSec);
        TimeConverter.ConvertTime(_gm.TotalBonusTime, out int bonusMin, out int bonusSec);
        TimeConverter.ConvertTime(_gm.TotalMalusTime, out int malusMin, out int malusSec);

        //Infos game items
        string results = $"Objets importants récupérés : {requiredItemsAmount}/{_gm.RequiredItems.Length}." + "\r\n";
        results += $"Objets récupérés : {_gm.ScoredItems.Count}/{_gm.AllItems.Length}." + "\r\n";
        //Infos game time
        results += $"Temps restant : {remainMin}m {remainSec}s." + "\r\n";
        //Infos bonus/malus
        string bonus = bonusMin <= 0 ? $"{bonusSec}s" : $"{bonusMin}m {bonusSec}s"; 
        string malus = malusMin <= 0 ? $"{malusSec}s" : $"{malusMin}m {malusSec}s";
        results += $"Temps bonus : {bonus}." + "\r\n";
        results += $"Temps perdu : {malus}.";

        resultsText.SetText(results);
        titleText.SetText(title);
    }
    #endregion
}
