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
    ImpassableGap,
    Wall,
    ImpassableWall
}

public class LevelGenrator : MonoBehaviour
{

    public int numInitialTiles = 50;

    [Range(1.0f, 20.0f)]
    public float sectionLength = 8;
    [Range(1.0f, 20.0f)]
    public float jumpLength = 2;

    [Range(50, 500)]
    public float medSpawnerThreshhold = 100.0f;

    [Range(50, 500)]
    public float hardSpawnerThreshhold = 400.0f;

    public PlayerCharacter PlayerCharacterRef;

    // this is because you cant edit a dictionary in the inspector
    public TileBase Floor, Gap, ImpassableGap, Wall, ImpassableWall;

    public Dictionary<ETileType, TileBase> TileTypeMap;
    public Dictionary<ETileType, ETileType[]> spawnRulesEasy;
    public Dictionary<ETileType, ETileType[]> spawnRulesMedeum;
    public Dictionary<ETileType, ETileType[]> spawnRulesHard;

    // queue of tiles i've spawned
    private Queue<TileBase> spawnedTiles;
    // what was the last tile
    private TileBase mLastTile;
    // how far off the screen should a tile be before i delete it
    private float offscreenDistance = 8.0f;

    // Use this for initialization
    void Start()
    {
        // map the available tiles to the right Enums
        TileTypeMap = new Dictionary<ETileType, TileBase>
        {
            { ETileType.Floor, Floor },
            { ETileType.Gap, Gap },
            { ETileType.ImpassableGap, ImpassableGap },
            { ETileType.Wall, Wall },
            { ETileType.ImpassableWall, ImpassableWall }
        };

        // Specifies what tiles you can spawn after what
        // Key is last tile
        // Value is array of available tiles
        // add a new tile to this when you make one

        spawnRulesEasy = new Dictionary<ETileType, ETileType[]>
        {
            {
                ETileType.Floor,
                new ETileType[]{
                    ETileType.Gap,
                    ETileType.Wall
                }
            },
            {
                ETileType.Gap,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Wall,
                }
            },
            {
                ETileType.Wall,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Gap,
                    ETileType.Wall
                }
            }

        };

        spawnRulesMedeum = new Dictionary<ETileType, ETileType[]>
        {
            {
                ETileType.Floor,
                new ETileType[]{
                    ETileType.Gap,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                    ETileType.ImpassableWall
                }
            },
            {
                ETileType.Gap,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Wall,
                }
            },
            {
                ETileType.ImpassableGap,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Wall,
                }
            },
            {
                ETileType.Wall,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Gap,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                    ETileType.ImpassableWall
                }
            },
            {
                ETileType.ImpassableWall,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Gap,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                }
            }

        };

        spawnRulesHard = new Dictionary<ETileType, ETileType[]>
        {
            {
                ETileType.Floor,
                new ETileType[]{
                    ETileType.Gap,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                    ETileType.ImpassableWall
                }
            },
            {
                ETileType.Gap,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                    ETileType.ImpassableWall
                }
            },
            {
                ETileType.ImpassableGap,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Gap,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                    ETileType.ImpassableWall
                }
            },
            {
                ETileType.Wall,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Gap,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                    ETileType.ImpassableWall
                }
            },
            {
                ETileType.ImpassableWall,
                new ETileType[]{
                    ETileType.Floor,
                    ETileType.Gap,
                    ETileType.ImpassableGap,
                    ETileType.Wall,
                    ETileType.ImpassableWall
                }
            }

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
        mLastTile.Resize(new TileResizeArgs(sectionLength));
        spawnedTiles.Enqueue(mLastTile);

        // loop and make the rest referencing the preveous tile
        for (int i = 1; i < numInitialTiles; i++)
        {
            AddNewTile();
        }

    }

    private void AddNewTile()
    {
        //ETileType tileToUse
        ETileType randomTile = ETileType.Unassigned;

        // list of tiles i'm allowed to spawn

        ETileType[] availableTiles;
        if (PlayerCharacterRef.distanceTravelled < medSpawnerThreshhold)
        {
            availableTiles = spawnRulesEasy[mLastTile.TileType];
        }
        else if (PlayerCharacterRef.distanceTravelled >= medSpawnerThreshhold)
        {
            availableTiles = spawnRulesMedeum[mLastTile.TileType];
        }
        else
        {
            availableTiles = spawnRulesHard[mLastTile.TileType];
        }

        // randomly pick the next tile
        randomTile = availableTiles[Rand.Range(0, availableTiles.Length)];

        // instantiate the next tile using the selected ETileType
        TileBase newTile = Instantiate(TileTypeMap[randomTile], new Vector2(mLastTile.transform.position.x + mLastTile.Length, 0.0f), Quaternion.identity, transform);
        newTile.Init(randomTile);

        // initialize the new tile with the right arguments
        switch (randomTile)
        {
            case ETileType.Unassigned:
                break;
            case ETileType.Floor:
                newTile.Resize(new TileResizeArgs(sectionLength * PlayerCharacterRef.moveSpeed/10));
                break;
            case ETileType.Gap:
                newTile.Resize(new TileResizeArgs(jumpLength * PlayerCharacterRef.moveSpeed / 10));
                break;
            case ETileType.Wall:
                newTile.Resize(new TileResizeArgs(sectionLength * PlayerCharacterRef.moveSpeed / 10));
                break;
            case ETileType.ImpassableGap:
                newTile.Resize(new ImpassableTileResizeArgs(sectionLength * PlayerCharacterRef.moveSpeed / 10, !PlayerCharacterRef.IsLayerA));
                break;
            case ETileType.ImpassableWall:
                newTile.Resize(new ImpassableTileResizeArgs(sectionLength * PlayerCharacterRef.moveSpeed / 10, PlayerCharacterRef.IsLayerA));
                break;
            default:
                break;
        }
        // add this new tile to the queue
        spawnedTiles.Enqueue(newTile);
        // set the last tile to the one we just spawned
        mLastTile = newTile;
    }

    // Update is called once per frame
    void Update()
    {
        // check to see of the next tile in the queue is off the screen (queue means that the first one we pit in is the first one we take out
        if (PlayerCharacterRef.transform.position.x > spawnedTiles.Peek().transform.position.x + spawnedTiles.Peek().Length + offscreenDistance)
        {
            //spawnedTiles.Dequeue();
            // destroy the tile and remove it from the queue
            Destroy(spawnedTiles.Dequeue().gameObject);
            // now add a new tile
            AddNewTile();
        }

        if (PlayerCharacterRef.transform.position.y < -10)
        {
            //Application.LoadLevel(Application.loadedLevel);
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                PlayerCharacterRef.FailScreen.SetActive(true);
            }  
        }

        if(PlayerCharacterRef.transform.position.y > -10)
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                PlayerCharacterRef.FailScreen.SetActive(false);
            }
        }
    }
}
