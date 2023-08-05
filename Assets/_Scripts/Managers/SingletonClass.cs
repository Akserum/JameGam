using UnityEngine;

public class SingletonClass<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Properties
    public static T Instance;
    #endregion

    #region Methods
    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }
    #endregion
}
