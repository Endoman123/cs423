using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player1, player2;
    private GameObject[] pickups;

    // Init pickups
    void Start()
    {
        pickups = GameObject.FindGameObjectsWithTag("pickup");
    }

    public void Restart(InputAction.CallbackContext context) {
        Debug.Log("Test");
        foreach (GameObject p in pickups) {
            p.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
