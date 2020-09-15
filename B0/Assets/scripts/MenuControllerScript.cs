using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour
{
    [SerializeField]
    private string map;

    // Function to load scenes
    public void LoadScene() {
        SceneManager.LoadScene(map, LoadSceneMode.Single);
    } 
}
