using UnityEngine;

public class CarCollision : MonoBehaviour
{ 
public Transform checkPoint; // A child object slightly behind the car
public LayerMask roadLayer;  // Assign this to only the "Road" layer

void Update()
{
    // Check if the point is still over the road layer
    if (Physics2D.OverlapPoint(checkPoint.position, roadLayer))
    {
        Debug.Log("Game Over");
        // Insert game over logic here
    }
}
 
 }