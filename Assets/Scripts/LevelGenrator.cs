using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rand = UnityEngine.Random;

public enum ETileType
{
    Unassigned,
    Floor,
    Gap,
    ImpassableGap
}

public class LevelGenrator : MonoBehaviour
{

    public int numInitialTiles = 50;

    [Range(1.0f, 20.0f)]
    public float sectionLength = 8;
    [Range(1.0f, 20.0f)]
    public float jumpLength = 2;

    public PlayerCharacter PlayerCharacterRef;

    public TileBase Floor, Gap, ImpassableGap;

    public Dictionary<ETileType, TileBase> TileTypeMap;
    public Dictionary<ETileType, ETileType[]> TilespawnRules;

    private Queue<TileBase> spawnedTiles;
    private TileBase mLastTile;
    private float offscreenDistance = 4.0f;

    // Use this for initialization
    void Start()
    {
        TileTypeMap = new Dictionary<ETileType, TileBase>
        {
            { ETileType.Floor, Floor },
            { ETileType.Gap, Gap },
            { ETileType.ImpassableGap, ImpassableGap }
        };

        // Specifies what tiles you can spawn after what
        // Key is last tile
        // Value is array of available tiles
        TilespawnRules = new Dictionary<ETileType, ETileType[]>
        {
            {
                ETileType.Floor,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Gap,
                    ETileType.ImpassableGap
                }
            },
            {
                ETileType.Gap,
                new ETileType[]{
                    ETileType.Floor
                }
            },
            {
                ETileType.ImpassableGap,
                new ETileType[]{
                    ETileType.Floor
                }
            },

        };

        InitializeTrack();
    }

    private void InitializeTrack()
    {
        spawnedTiles = new Queue<TileBase>(numInitialTiles);

        // Instantiate the initial track pieces
        // make the first piece
        mLastTile = Instantiate<TileBase>(Floor, transform);
        mLastTile.Init(ETileType.Floor);
        mLastTile.Resize(sectionLength);
        spawnedTiles.Enqueue(mLastTile);

        // loop and make the rest referencing the preveous tile
        for (int i = 1; i < numInitialTiles; i++)
        {
            Debug.Log("Tiles: " + spawnedTiles.Count);
            AddNewTile();
        }

    }

    private void AddNewTile()
    {
        //ETileType tileToUse
        ETileType randomTile = ETileType.Unassigned;

        ETileType[] availableTiles = TilespawnRules[mLastTile.TileType];

        randomTile = availableTiles[Rand.Range(0, availableTiles.Length)];

        TileBase newTile = Instantiate(TileTypeMap[randomTile], new Vector2(mLastTile.transform.position.x + mLastTile.Length, 0.0f), Quaternion.identity, transform);
        newTile.Init(randomTile);

        switch (randomTile)
        {
            case ETileType.Unassigned:
                break;
            case ETileType.Floor:
                newTile.Resize(sectionLength);
                break;
            case ETileType.Gap:
                newTile.Resize(jumpLength);
                break;
            case ETileType.ImpassableGap:
                newTile.Resize(sectionLength);
                break;
            default:
                break;
        }
        spawnedTiles.Enqueue(newTile);
        mLastTile = newTile;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerCharacterRef.transform.position.x > spawnedTiles.Peek().transform.position.x + spawnedTiles.Peek().Length + offscreenDistance)
        {
            //spawnedTiles.Dequeue();
            Destroy(spawnedTiles.Dequeue().gameObject);
            AddNewTile();
        }

        if (PlayerCharacterRef.transform.position.y < -10)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
