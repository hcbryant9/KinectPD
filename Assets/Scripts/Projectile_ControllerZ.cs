using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_ControllerZ : MonoBehaviour
{
    public GameObject cubePrefab;
    public KeyCode button;
    private Vector3 originalPosition;
    private bool isMoving = false;
    private float distanceToMove = 80f;
    private float moveSpeed = 30f;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(button) && !isMoving)
        {
            originalPosition = transform.position;
            isMoving = true;
            StartCoroutine(MoveCube());
        }
    }

    IEnumerator MoveCube()
    {
        // Create a new instance of the cube prefab and position it at the original position
        GameObject newCube = Instantiate(cubePrefab, originalPosition, Quaternion.identity);
        newCube.tag = "Bullet";
        // Calculate the target position for the new cube
        Vector3 targetPosition = newCube.transform.position + new Vector3(0, 0, distanceToMove);

        // Move the new cube to the target position
        while (newCube.transform.position != targetPosition)
        {
            newCube.transform.position = Vector3.MoveTowards(newCube.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        // Move the new cube back to its original position and then destroy it
        while (newCube.transform.position != originalPosition)
        {
            newCube.transform.position = Vector3.MoveTowards(newCube.transform.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(newCube);

        isMoving = false;
    }
}
