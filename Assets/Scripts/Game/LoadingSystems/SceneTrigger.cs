using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public static UnityEvent OnSceneLoad;
    
    public void ChangeScene(string sceneName)
    {
        LoadingData.sceneToLoad = sceneName;

        SceneManager.LoadScene("Loading");
    }
}
