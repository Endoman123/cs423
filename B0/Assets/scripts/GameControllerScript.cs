using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    private float camDist = 10f, smoothSpeed = 0.1f;
    [SerializeField]
    private Color p1Color, p2Color;

    private int p1Score = 0, p2Score = 0;

    private GameObject p1, p2;
    private GameObject[] pickups;
    private bool gameStart = false;
    private float timer;
    private Vector3 camTarget , camVelocity = Vector3.zero;

    // Init map
    // This includes tracking who is who and all that jazz
    void Start()
    {
        p1 = Instantiate(ballPrefab, p1Spawn.transform.position, Quaternion.identity);
        p2 = Instantiate(ballPrefab, p2Spawn.transform.position, Quaternion.identity);

        p1.GetComponent<PlayerScript>().Index = 1;
        p2.GetComponent<PlayerScript>().Index = 2;

        p1.GetComponent<PlayerScript>().AddListeners(IncP1Score, DecP1Score);
        p2.GetComponent<PlayerScript>().AddListeners(IncP2Score, DecP2Score);

        p1.GetComponent<Renderer>().material.SetColor("Albedo", p1Color);
        p2.GetComponent<Renderer>().material.SetColor("Albedo", p2Color);

        pickups = GameObject.FindGameObjectsWithTag("Pickup");
        
        RestartGame();
    }

    // Restarts game
    private void RestartGame() {
        foreach (GameObject p in pickups) {
            p.SetActive(true);
        }

        p1.transform.position = p1Spawn.transform.position;
        p2.transform.position = p2Spawn.transform.position;

        timer = 120f;
        p1Score = 0;
        p2Score = 0;

        gameStart = false;

        ResetUI();
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
        statusText.text = "";
        
        UpdateP1ScoreText();
        UpdateP2ScoreText();
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
        Vector3 
            pos1 = p1.transform.position,
            pos2 = p2.transform.position;

        camTarget = (pos1 + pos2) / 2f - Mathf.Clamp(Mathf.Abs((pos2 - pos1).magnitude), 10f, 20f) * transform.forward;
        
        transform.position = Vector3.SmoothDamp(transform.position, camTarget, ref camVelocity, smoothSpeed);

        if (timer > 0) {
            timer = Mathf.Clamp(timer - Time.deltaTime, 0, timer);
            UpdateTimeDisplay();
        }
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
