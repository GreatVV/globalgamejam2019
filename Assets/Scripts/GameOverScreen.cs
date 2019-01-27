using UnityEngine;
using UnityEngine.SceneManagement;

namespace Client
{
    public class GameOverScreen : MonoBehaviour
    {
        public void Show (bool state)
        {
            if (state)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            gameObject.SetActive (state);
        }

        public void RestartGame ()
        {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
        }
    }
}
