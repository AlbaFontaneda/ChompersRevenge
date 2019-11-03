using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string m_SceneToLoad;

    public void StartGame()
    {
        SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
    }
}
