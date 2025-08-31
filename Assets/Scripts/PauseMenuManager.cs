using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuHolder;

    bool isActive = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (!isActive) ActivatePauseMenu();
            else TurnOffPouseMenu();
        } 
    }

    private void ActivatePauseMenu()
    {
        isActive = true;
        pauseMenuHolder.SetActive(true);
        Time.timeScale = 0f;
    }

    public void TurnOffPouseMenu()
    {
        isActive = false;
        pauseMenuHolder.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }
}
