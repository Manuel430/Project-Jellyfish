using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
  public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }  
}
