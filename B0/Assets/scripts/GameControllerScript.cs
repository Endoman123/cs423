using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject p1spawn, p2spawn;
    [SerializeField]
    private GameObject ballPrefab;
    private PlayerInputManager playerInputManager;
    private GameObject p1, p2;
    private GameObject[] pickups;
    private float timer = 120f;

    // Init map
    // This includes tracking who is who and all that jazz
    void Start()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        p1 = Instantiate(ballPrefab, p1spawn.transform.position, Quaternion.identity);
        p2 = Instantiate(ballPrefab, p2spawn.transform.position, Quaternion.identity);

        p1.GetComponent<PlayerScript>().Index = 1;
        p2.GetComponent<PlayerScript>().Index = 2;

        pickups = GameObject.FindGameObjectsWithTag("pickup");
    }

    public void RestartMap() {
        foreach (GameObject p in pickups) {
            p.SetActive(true);
        }

        p1.transform.position = p1spawn.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
    }
}
