using SymphonyFrameWork.System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChristianGamers
{
    public class SceneLoad : MonoBehaviour
    {
        [SerializeField]
        private SceneListEnum _loadTargetScene;

        public void LoadScene()
        {
            Scene activeScene = SceneManager.GetActiveScene();
            SymphonyFrameWork.System.SceneLoader.UnloadScene(activeScene.name);
            SymphonyFrameWork.System.SceneLoader.LoadScene(_loadTargetScene.ToString());
            SceneLoader.RegisterAfterSceneLoad(_loadTargetScene.ToString(),
                () => SceneLoader.SetActiveScene(_loadTargetScene.ToString()));
        }

        public void LoadScene(SceneListEnum scene)
        {
            _loadTargetScene = scene;
            LoadScene();
        }
    }
}
