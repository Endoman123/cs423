using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameControllerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject p1Spawn, p2Spawn;
    [SerializeField]
    private GameObject ballPrefab;
    [SerializeField]
    private Text statusText, timeText, p1ScoreText, p2ScoreText;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private float camDist = 10f, smoothSpeed = 0.1f;
    [SerializeField]
    private Color p1Color, p2Color;

    private int p1Score = 0, p2Score = 0;

    private GameObject p1, p2;
    private GameObject[] pickups;
    private PlayerScript p1Script, p2Script;
    private bool gameStart = false;
    private float timer;
    private Vector3 camStart, camTarget, camVelocity = Vector3.zero;

    // Init map and camera
    // This includes tracking who is who and all that jazz
    void Start()
    {
        camStart = transform.position;

        p1 = Instantiate(ballPrefab, p1Spawn.transform.position, Quaternion.identity);
        p2 = Instantiate(ballPrefab, p2Spawn.transform.position, Quaternion.identity);

        p1Script = p1.GetComponent<PlayerScript>();
        p2Script = p2.GetComponent<PlayerScript>();

        p1Script.Index = 1;
        p2Script.Index = 2;

        p1Script.AddListeners(IncP1Score, DecP1Score);
        p2Script.AddListeners(IncP2Score, DecP2Score);

        p1.GetComponent<Renderer>().material.color = p1Color;
        p2.GetComponent<Renderer>().material.color = p2Color;

        pickups = GameObject.FindGameObjectsWithTag("Pickup");
        
        RestartGame();
    }

    // Restarts game
    public void RestartGame() {
        foreach (GameObject p in pickups)
            p.GetComponent<PickupControllerScript>().ResetPickup();

        transform.position = camStart;

        p1.transform.position = p1Spawn.transform.position;
        p2.transform.position = p2Spawn.transform.position;

        p1Script.ResetPlayer();
        p2Script.ResetPlayer();

        timer = 120f;
        p1Score = 0;
        p2Score = 0;

        gameStart = false;

        ResetUI();
        SetFreeze(true);

        UpdateStatus("Press \"Space\" to start");
    }

    // Sets whether or not the game is frozen.
    private void SetFreeze(bool freeze) {
        p1Script.enabled = !freeze;
        p2Script.enabled = !freeze;

        Time.timeScale = freeze ? 0 : 1;
    }

    private void IncP1Score() {
        p1Score++;
        UpdateP1ScoreText();
    }

    private void DecP1Score() {
        p1Score--;
        UpdateP1ScoreText();
    }

    private void IncP2Score() {
        p2Score++;
        UpdateP2ScoreText();
    }

    private void DecP2Score() {
        p2Score--;
        UpdateP2ScoreText();
    }

    private void ResetUI() {
        UpdateStatus("");
        
        UpdateP1ScoreText();
        UpdateP2ScoreText();

        pauseMenu.SetActive(false);
    }

    private void UpdateP1ScoreText() {
        p1ScoreText.text = $"P1 Score: {p1Score}";
    }

    private void UpdateP2ScoreText() {
        p2ScoreText.text = $"P2 Score: {p2Score}";
    }

    // Update time and move position
    private void Update()
    {
        if (!gameStart) {
            if (Input.GetKeyDown("space")) {
                gameStart = true;
                SetFreeze(false);
                UpdateStatus("");
            }
        } else {
            Vector3 
                pos1 = p1.transform.position,
                pos2 = p2.transform.position;

            camTarget = (pos1 + pos2) / 2f - Mathf.Clamp(Mathf.Abs((pos2 - pos1).magnitude), 10f, 20f) * transform.forward;
            
            transform.position = Vector3.SmoothDamp(transform.position, camTarget, ref camVelocity, smoothSpeed);

            if (timer > 0) {
                timer = Mathf.Clamp(timer - Time.deltaTime, 0, timer);
                UpdateTimeDisplay();
            } else if (timer == 0) {
                var winner = Mathf.Sign(p1Score - p2Score);

                switch(winner) {
                    case 1:
                        UpdateStatus("Player 1 won!\n ");
                        break;
                    case -1:
                        UpdateStatus("Player 2 won!");
                        break;
                    default:
                        UpdateStatus("Uhh?");
                        break;
                }
            }

            if (Input.GetButtonDown("Pause")) {
                TogglePause();
            }
        }
    }

    // Toggle pause menu
    public void TogglePause() {
        pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
        SetFreeze(pauseMenu.activeInHierarchy);
    }

    // Format time and print it to the thing
    private void UpdateTimeDisplay() {
        var ts = TimeSpan.FromSeconds(timer);
        var time = string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);

        timeText.text = time;
    }

    // Set the status text
    private void UpdateStatus(String status) {
        statusText.text = status;
    }
}
