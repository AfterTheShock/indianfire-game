using UnityEngine;
using UnityEngine.SceneManagement;

public class WinconditionManager : MonoBehaviour
{
    private GameObject[] allHutsInScene;
    private void Start()
    {
        allHutsInScene = GameObject.FindGameObjectsWithTag("Hut");
    }

    private void Update()
    {
        bool weWin = true;

        foreach (GameObject go in allHutsInScene)
        {
            if(!go.GetComponent<HutController>().wasTuredOff) weWin = false;
        }

        if (weWin) 
        {
            GoToNextScene();
        }
    }

    private void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
