using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader: MonoBehaviour
{
    #region Methods
    public void LoadScene(string name) => SceneManager.LoadScene(name);
    #endregion
}
