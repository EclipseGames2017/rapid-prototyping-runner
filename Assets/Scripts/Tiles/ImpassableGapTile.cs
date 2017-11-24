using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassableGapTile : TileBase
{

    public GapComponent gap;
    public FloorComponent floorA, floorB;

    private bool isFloorA;

    public override void Resize(TileResizeArgs bargs)
    {
        IGapResizeArgs args = bargs as IGapResizeArgs;

        base.Resize(bargs);
        isFloorA = args.isFloorA;

        floorA.Resize(args.length);
        floorB.Resize(args.length);
        gap.Resize(args.length);

        floorA.gameObject.SetActive(isFloorA);
        floorB.gameObject.SetActive(!isFloorA);
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

public class IGapResizeArgs : TileResizeArgs
{
    public bool isFloorA;

    public IGapResizeArgs(float length, bool isFloorA) : base(length)
    {
        base.length = length;
        this.isFloorA = isFloorA;
    }

    

}
