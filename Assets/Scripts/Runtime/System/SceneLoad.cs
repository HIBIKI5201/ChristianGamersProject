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
            SceneLoader.UnloadScene(activeScene.name);
            SceneLoader.LoadScene(_loadTargetScene.ToString());
        }
    }
}
