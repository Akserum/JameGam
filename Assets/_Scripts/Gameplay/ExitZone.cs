using UnityEngine;

public class ExitZone : MonoBehaviour
{
    #region Variables
    private GameManager _gm;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _gm = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" )
            return;

        _gm.CanEscape = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;

        _gm.CanEscape = false;
    }
    #endregion
}
