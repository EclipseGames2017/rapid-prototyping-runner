using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTile : FloorTile {

    public WallComponent jumpComponentA, jumpComponentB;

    public override void Resize(TileResizeArgs args)
    {
        base.Resize(args);

        jumpComponentA.Resize(args.length / 2);
        jumpComponentB.Resize(args.length / 2);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
