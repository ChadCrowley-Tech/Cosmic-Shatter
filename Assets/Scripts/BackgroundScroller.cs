using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // Sets the scrolling speed
    public float scrollSpeed = 0.03f;

    // Holds the image material
    private Material mat;

    void Start()
    {
        // Gets the material component attached to the object
        mat = GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        // Calculate the movement over time
        Vector2 offset = new Vector2(0, Time.time * scrollSpeed);
        
        // Shifts the image to create the scrolling effect
        mat.mainTextureOffset = offset;

    }
}
