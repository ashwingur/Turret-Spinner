using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Update()
    {
        SetPosition(player.transform);
    }

    public void SetPosition(Transform t)
    {
        transform.position = new Vector3(t.position.x, t.position.y, transform.position.z);   
    }
}
