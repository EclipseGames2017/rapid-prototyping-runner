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

    }

    // Use this for initialization
    void Start()
    {

    }

    public override void Resize(TileResizeArgs args)
    {
        base.Resize(args);
        floorComponentA.Resize(args.length);
        floorComponentB.Resize(args.length);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
