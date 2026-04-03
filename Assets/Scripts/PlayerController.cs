using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using System.Net;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public float jumpHeight = 0f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private bool jumped = false;
    private bool doubleAvailable = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winTextObject.SetActive(false);
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        jumped = false;
    }
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    public void OnJump()
    {
        if (IsGrounded())
        {
            jumped = true;
        } else if (doubleAvailable) {
            jumped = true;
            doubleAvailable = false;
        }
    }
    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 6)
        {
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        
        if (IsGrounded())
        {
            doubleAvailable = true;
        }

        if (jumped)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpHeight, rb.linearVelocity.z);
            jumped = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            count = count + 1;
            SetCountText();
            other.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
        }
    }

    private bool IsGrounded()
    {
        return rb.linearVelocity.y == 0;
    }
}