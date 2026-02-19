using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Manegement.Scene
{
    public class SceneChanging : MonoBehaviour
    {
        void Start()
        {
            var _loading = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad);

            if (_loading.isDone)
                SceneTrigger.OnSceneLoad?.Invoke();
        }
    }
}
