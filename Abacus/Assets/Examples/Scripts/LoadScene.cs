using UnityEngine;
using UnityEngine.SceneManagement;

namespace Abacus.Examples
{
    /// <summary>
    /// Reloads the current scene
    /// Notice how Abacus will dump the values from the
    /// previous scene and start new ones on scene change
    /// </summary>
    public class LoadScene : MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("Scene Loaded!");
        }

        /// <summary>
        /// Reloads the active scene
        /// </summary>
        public void ReloadScene()
        {
            var active = SceneManager.GetActiveScene();
            SceneManager.LoadScene(active.name);
        }
    }
}