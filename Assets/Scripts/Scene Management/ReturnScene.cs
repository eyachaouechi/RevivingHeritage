using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnScene : MonoBehaviour
{
    public int sceneNumberToReturn = 1; 

    public void ReturnToMainScene()
    {
        SceneManager.LoadScene(sceneNumberToReturn);
    }
}
