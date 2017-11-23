using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETileType
{
    Floor,
    Gap
}

public class LevelGenrator : MonoBehaviour
{

    public int numInitialTiles = 5;

    [Range(1.0f, 20.0f)]
    public float sectionLength = 8;
    [Range(1.0f, 20.0f)]
    public float jumpLength = 2;

    public PlayerCharacter PlayerCharacterRef;

    public TileBase Floor, Gap;

    public Dictionary<ETileType, TileBase> TileTypeMap;

    private Queue<TileBase> spawnedTiles;
    private TileBase mLastTile;
    private float offscreenDistance = 4.0f;

    // Use this for initialization
    void Start()
    {
        TileTypeMap = new Dictionary<ETileType, TileBase>
        {
            { ETileType.Floor, Floor },
            { ETileType.Gap, Gap }
        };

        InitializeTrack();
    }

    private void InitializeTrack()
    {
        spawnedTiles = new Queue<TileBase>(numInitialTiles);

        // Instantiate the initial track pieces
        // make the first piece
        mLastTile = Instantiate<TileBase>(Floor, transform);
        mLastTile.Resize(sectionLength);
        spawnedTiles.Enqueue(mLastTile);

        // loop and make the rest referencing the preveous tile
        for (int i = 1; i < numInitialTiles; i++)
        {
            AddNewTile();
        }

    }

    private void AddNewTile()
    {
        TileBase newTile = Instantiate(Floor, new Vector2(mLastTile.transform.position.x + mLastTile.Length, 0.0f), Quaternion.identity, transform);
        newTile.Resize(sectionLength);
        spawnedTiles.Enqueue(newTile);
        mLastTile = newTile;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCharacterRef.transform.position.x > spawnedTiles.Peek().transform.position.x + spawnedTiles.Peek().Length + offscreenDistance)
        {
            Destroy(spawnedTiles.Dequeue().gameObject);
            AddNewTile();
        }
    }
}
