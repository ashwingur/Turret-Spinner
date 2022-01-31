using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float parallaxEffect;
    private float xLength, xStartpos, yLength, yStartpos;
    


    // Start is called before the first frame update
    void Start()
    {
        xStartpos = transform.position.x;
        yStartpos = transform.position.y;
        xLength = GetComponent<SpriteRenderer>().bounds.size.x;
        yLength = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        float xTemp = camera.transform.position.x * (1 - parallaxEffect);
        float xDist = camera.transform.position.x * parallaxEffect;

        float yTemp = camera.transform.position.y * (1 - parallaxEffect);
        float yDist = camera.transform.position.y * parallaxEffect;

        transform.position = new Vector3(xDist + xStartpos, yDist + yStartpos, transform.position.z);

        // Shifting with the camera
        if (xTemp > xStartpos + xLength)
            xStartpos += xLength;   
        else if (xTemp < xStartpos - xLength)
            xStartpos -= xLength;

        if (yTemp > yStartpos + yLength)
            yStartpos += yLength;
        else if (yTemp < yStartpos - yLength)
            yStartpos -= yLength;
    }
}
