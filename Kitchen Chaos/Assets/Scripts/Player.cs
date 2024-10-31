using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;

    private Vector3 lastInteractDir;
    

    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        // Update inputVector with user input
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputVector = inputVector.normalized;

        // Create move direction based on input
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) { 
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance))
        {
            // Raycast hit, logging the transform of the hit object
            //Debug.Log(raycastHit.transform.name);
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // has a clear counter
                clearCounter.Interact();
            }
        }
        else
        {
            // Optional debug if no interaction detected
            // Debug.Log("No interaction");
        }
    }


    private void HandleMovement()
    {
        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = +1;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        float moveDist = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHieght = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHieght, playerRadius, moveDir, moveDist);
        if (canMove)
        {
            transform.position += moveDir * moveDist;
        }

        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10f);
    }

}
