using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton instance
    private static LevelManager _instance = null;

    // Load a scene by index
    public void LoadLevel(int id)
    {
        SceneManager.LoadScene(id);
    }

    #region Singleton

    public static LevelManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
            Destroy(gameObject);

        // Make sure it persits
        DontDestroyOnLoad(gameObject);
    }

    #endregion
}