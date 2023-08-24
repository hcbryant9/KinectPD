using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    public float moveDistance = 5f; // The distance the object should move
    public float moveSpeed = 2f; // The speed at which the object moves
    public Vector3 movementAxis = Vector3.right; // The axis along which the object should move

    private Vector3 startPosition; // The starting position of the object
    private Vector3 endPosition; // The ending position of the object
    private bool movingToEndPosition = true; // Whether the object is moving towards the end position or not

    void Start()
    {
        // Set the starting and ending positions based on the initial position of the object
        startPosition = transform.position;
        endPosition = transform.position + (movementAxis * moveDistance);
    }

    void Update()
    {
        // Calculate the new position based on the current direction of movement
        Vector3 newPosition = movingToEndPosition ? endPosition : startPosition;

        // Move the object towards the new position at the specified speed
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);

        // If the object reaches the end position, switch the direction of movement
        if (transform.position == endPosition)
        {
            movingToEndPosition = false;
        }
        // If the object reaches the starting position, switch the direction of movement
        else if (transform.position == startPosition)
        {
            movingToEndPosition = true;
        }
    }
}
