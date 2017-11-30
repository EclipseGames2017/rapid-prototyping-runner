using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class WallTile : FloorTile
{
    
    public WallComponent wallComponentA, wallComponentB;

    public override void Resize(TileResizeArgs args)
    {
        base.Resize(args);
        floorComponentA.Resize(args.length);
        floorComponentA.Resize(args.length);

        wallComponentA.Resize(args.length/2);
        wallComponentB.Resize(args.length/2);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //Resize(new TileResizeArgs(length));
	}
}
