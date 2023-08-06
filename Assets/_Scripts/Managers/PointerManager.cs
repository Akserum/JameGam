using UnityEngine;

public class PointerManager : SingletonClass<PointerManager>
{
    #region Variables
    [SerializeField] private bool enabledAtStart = false;
    #endregion

    #region Builts_In
    private void Start()
    {
        EnableCursor(enabledAtStart);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Enable or disable the cursor
    /// </summary>
    public void EnableCursor(bool enabled)
    {
        Cursor.visible = enabled;
    }
    #endregion
}
