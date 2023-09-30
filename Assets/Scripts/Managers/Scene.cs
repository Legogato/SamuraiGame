using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{


    public void Resume()
    {
        Time.timeScale = 1;
    } 

    public void RestartButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
