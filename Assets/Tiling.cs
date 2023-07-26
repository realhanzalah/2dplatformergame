using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour
{
    // Offset
    public int offsetX = 2;

    public bool hasRightChunk = false;
    public bool hasLeftChunk = false;

    // For objects that aren't tileable
    public bool reverseScale = false;

    // Width of sprites
    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    void Awake()
    {
        cam = Camera.main;
        myTransform = transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    // Checks if has a left chunk OR has a right chunk. If chunks aren't required then do nothing
    void Update()
    {
        if (hasLeftChunk == false || hasRightChunk == false)
        {
            // Calculating the cameras extend of what it can see (in co-ords)
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            // Calculate x position where cam sees edge of sprite
            float edgeVisPosRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisPosLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            // If the edge of the sprite is visible then we call MakeNewChunk
            if (cam.transform.position.x >= edgeVisPosRight - offsetX && hasRightChunk == false)
            {
                MakeNewChunk(1);
                hasRightChunk = true;
            }
            else if (cam.transform.position.x <= edgeVisPosLeft + offsetX && hasLeftChunk == false)
            {
                MakeNewChunk(-1);
                hasLeftChunk = true;
            }
        }
    }

    // Function to create a new chunk of the background sprite on the side that requires more background
    void MakeNewChunk (int rightOrLeft)
    {
        // Calculating position for new chunk to be added
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);

        // Instantiating new chunk, storing it as a variable
        Transform newChunk = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        // Reversing the x size of background sprite if not tiling
        if (reverseScale == true)
        {
            newChunk.localScale = new Vector3(newChunk.localScale.x * -1, newChunk.localScale.y, newChunk.localScale.z);

        }

        newChunk.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newChunk.GetComponent<Tiling>().hasLeftChunk = true;
        }
        else
        {
            newChunk.GetComponent<Tiling>().hasRightChunk = true;
        }
    }
}
