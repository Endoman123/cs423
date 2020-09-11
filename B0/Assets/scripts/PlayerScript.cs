﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerScript : MonoBehaviour
{
    [SerializeField]
    private float rollForce = 3f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private Text scoreText;
    private Rigidbody rb;
    private bool grounded = true;
    private bool jump = false;

    private int score = 0;

    Vector3 vec = new Vector3();

    // Use start to get the needed components
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        UpdateScore();
    }

    // Use this for all input
    private void Update() {
        // Rather than deallocate and reallocate,
        // just use the same vector
        vec = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        
        // Only handle the intent of jumping here 
        if (!jump)
            jump = grounded && Input.GetButtonDown("Jump");
        
        if (jump) 
            Debug.Log("Jump");

        // DEbug
        Debug.DrawLine(transform.position + Vector3.down * 0.50f, transform.position + Vector3.down * 0.52f, Color.blue);
    }

    // Pickup pickups
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("pickup")) {
            score++;
            other.gameObject.SetActive(false);
            UpdateScore();
        }
    }

    // Use this for physics-based calculations
    // I guess ground checking belongs here?
    private void FixedUpdate() {
        // Basic rolling movement
        rb.AddForce(vec * rollForce);

        // Jump force will be applied separately
        // This should also flip the grounded and jump bools to false immediately after
        if (jump) {
            jump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Figure out if we are touching the ground;
        // Only consider ourselves grounded if we are touching anything tagged as such.
        // Raycast needs to start inside the player, helps the raycast detect the ground underneath if it slightly coincides
        // with the ground.
        grounded = Physics.Raycast(transform.position + Vector3.down * 0.45f, Vector3.down, 0.055f, 1 << 8);
        
        // Keep us slightly off ground to make sure the raycast be working
        if (grounded) {
            rb.AddForce(Vector3.up * 0.01f);
        }
    }

    private void UpdateScore() {
        scoreText.text = "Score: " + score.ToString();
    }
}