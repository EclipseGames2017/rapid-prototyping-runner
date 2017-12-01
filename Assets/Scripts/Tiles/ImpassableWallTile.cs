using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpassableWallTile : WallTile
{
    private bool isWallA;
    public WallComponent dummyWallA, dummyWallB;

    public override void Resize(TileResizeArgs bargs)
    {
        // like casting the tile args to this class's version
        ImpassableTileResizeArgs args = bargs as ImpassableTileResizeArgs;
        base.Resize(bargs);

        isWallA = args.isSideA;

        wallComponentA.gameObject.SetActive(isWallA);
        wallComponentB.gameObject.SetActive(!isWallA);

        dummyWallA.gameObject.SetActive(!isWallA);
        dummyWallB.gameObject.SetActive(isWallA);
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
