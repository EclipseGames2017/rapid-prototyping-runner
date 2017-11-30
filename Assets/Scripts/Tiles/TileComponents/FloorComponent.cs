using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FloorComponent : MonoBehaviour
{

    [Range(1.0f, 20.0f)]
    private float length;

    public Transform floorScaler;

    // Use this for initialization
    void Start()
    {
        
    }

    public void Resize(float newLength)
    {
        length = newLength;
        floorScaler.localPosition = new Vector2(length / 2, floorScaler.localPosition.y);
        floorScaler.localScale = new Vector2(length, floorScaler.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
