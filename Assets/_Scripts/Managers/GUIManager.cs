using UnityEngine;
using TMPro;

public class GUIManager : MonoBehaviour
{
    #region Variables
    private GameManager _gameManager;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    #endregion

    #region Methods
    #endregion
}
