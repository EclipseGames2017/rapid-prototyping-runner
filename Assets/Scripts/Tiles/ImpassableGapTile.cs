using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassableGapTile : TileBase
{

    // components for the floor on side a And b
    public GapComponent gap;
    public FloorComponent floorA, floorB;

    // is the floor on side A or B
    private bool isFloorA;

    public override void Resize(TileResizeArgs bargs)
    {
        // like casting the tile args to this class's version
        ImpassableTileResizeArgs args = bargs as ImpassableTileResizeArgs;

        base.Resize(bargs);
        
        // set isFloorA to whatever the generator wanted
        isFloorA = args.isSideA;

        // resize all of the components
        floorA.Resize(args.length);
        floorB.Resize(args.length);
        gap.Resize(args.length);

        // Hide whatever floor tile we dont want
        floorA.gameObject.SetActive(isFloorA);
        floorB.gameObject.SetActive(!isFloorA);
    }
}

/*
 * this tile needs to know whether the bridge is on side A or side B
 * so we extend the base class
 */

public class ImpassableTileResizeArgs : TileResizeArgs
{
    public bool isSideA;

    public ImpassableTileResizeArgs(float length, bool isSideA) : base(length)
    {
        base.length = length;
        this.isSideA = isSideA;
    }

    

}
