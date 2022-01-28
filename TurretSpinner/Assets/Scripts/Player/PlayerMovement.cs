using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerSpeedUpgrade;
    [SerializeField] private CameraFollow cameraFollow;
    public static float horizontalBorder = 10f;
    public static float verticalBorder = 7.6f;


    // Update is called once per frame
    void Update()
    {
        if (!UpgradeMenu.UpgradeMenuOpen)
        {
            Translate();
            Rotate();
        }
        cameraFollow.SetPosition(transform);
    }

    void Rotate()
    {
        // Make the player face the direction of the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        transform.up = mousePosition - transform.position;
    }

    void Translate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float vertSpeed = verticalInput * playerSpeed * Time.deltaTime;
        float horiSpeed = horizontalInput * playerSpeed * Time.deltaTime;

        float xPos = transform.position.x;
        float yPos = transform.position.y;

        // Prevent the player from going out of bounds
        if ((xPos >= horizontalBorder && horiSpeed > 0) || (xPos <= -horizontalBorder && horiSpeed < 0))
        {
            horiSpeed = 0;
        }
        if ((yPos >= verticalBorder && vertSpeed > 0) || (yPos <= -verticalBorder && vertSpeed < 0))
        {
            vertSpeed = 0;
        }

        transform.position += new Vector3(horiSpeed, vertSpeed, 0);
    }

    public void UpgradeMovementSpeed()
    {
        playerSpeed += playerSpeedUpgrade;
    }
}
