using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassableGapTile : TileBase
{

    public GapComponent gap;
    public FloorComponent floor;

    public override void Resize(float newLength)
    {
        base.Resize(newLength);
        floor.Resize(newLength);
        gap.Resize(newLength);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
