using UnityEngine;
using TMPro;

public class ResultsPanel : MonoBehaviour
{
    #region Variables
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
        float totalTime = _gm.GameDuration + _gm.TotalBonusTime - _gm.TotalMalusTime;
        TimeConverter.ConvertTime(totalTime, out int totalMin, out int totalSec);
        TimeConverter.ConvertTime(_gm.Timer, out int remainMin, out int remainSec);
        TimeConverter.ConvertTime(_gm.TotalBonusTime, out int bonusMin, out int bonusSec);
        TimeConverter.ConvertTime(_gm.TotalBonusTime, out int malusMin, out int malusSec);

        //Infos game items
        string results = $"Objets importants : " + "\r\n";
        results += $"Objets récupérés : " + "\r\n";
        //Infos game time
        results += $"Durée totale de la partie : {totalMin}:{totalSec}" + "\r\n";
        results += $"Temps restant : {remainMin}:{remainSec}" + "\r\n";
        //Infos bonus/malus
        results += $"Temps bonus : {bonusMin}:{bonusSec}" + "\r\n";
        results += $"Temps perdu : {malusMin}:{malusSec}" + "\r\n";

        resultsText.SetText(results);
    }
    #endregion
}
