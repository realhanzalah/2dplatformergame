using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    public int moveSpeed = 220;
    // Update is called once per frame
    void Update()
    {
        //Moving objects over time
        transform.Translate (Vector3.right * moveSpeed * Time.deltaTime);
        Destroy(gameObject, 1);

    }
}
