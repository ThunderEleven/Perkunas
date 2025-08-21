using UnityEngine.SceneManagement;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
