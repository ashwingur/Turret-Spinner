using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] private Transform playerIcon;
    [SerializeField] private GameObject player;
    [SerializeField] private float minimapPadding;
    private float minimapWidth;
    private float minimapHeight;
    private float xScale;
    private float yScale;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform rt = GetComponent<RectTransform>();
        minimapWidth = rt.rect.width - minimapPadding;
        minimapHeight = rt.rect.height - minimapPadding;

        xScale = 0.5f * minimapWidth / PlayerMovement.horizontalBorder;
        yScale = 0.5f * minimapHeight / PlayerMovement.verticalBorder;
    }

    // Update is called once per frame
    void Update()
    {
        playerIcon.localPosition = new Vector3(
            player.transform.position.x * xScale,
            player.transform.position.y * yScale,
            0);
    }
}
