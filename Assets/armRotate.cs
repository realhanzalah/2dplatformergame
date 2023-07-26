using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armRotate : MonoBehaviour
{
    public int rotationOffset = 90;
    // Update is called once per frame
    void Update()
    {
        // Subtracting the position of the player from the mouse
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize(); // Normalising the vector to make the sum of the vector equal to 1.

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg; // Finding the angle between x axis and a vector and the size of that vector. Then converting from radians to degrees
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);
    }
}
