using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class FloorTile : TileBase
{
    
    //[Range(1.0f, 20.0f)]
    //private float length = 5.0f;

    public FloorComponent floorComponentA, floorComponentB;

    private void Awake()
    {
        Resize(Length);
    }

    // Use this for initialization
    void Start()
    {

    }

    public override void Resize(float newLength)
    {
        base.Resize(newLength);
        floorComponentA.Resize(newLength);
        floorComponentB.Resize(newLength);
    }

    // Update is called once per frame
    void Update()
    {
        //Resize(length);
    }
}
