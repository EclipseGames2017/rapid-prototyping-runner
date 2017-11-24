using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapTile : TileBase
{

    //[Range(1.0f, 20.0f)]
    //public float length = 5.0f;

    public GapComponent gapComponent;

    // Use this for initialization
    void Start()
    {

    }

    public override void Resize(TileResizeArgs args)
    {
        base.Resize(args);
        gapComponent.Resize(args.length);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
