﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // parent
    private GameObject levelParent = null;
    private string levelParentName = "Map";

    // the tile being generated.
    public GameObject tile = null;
    private Vector3 tileOrigin = new Vector3(0.5F, 0.5F, 0.5F);
    private Vector3 tileSize = new Vector3(1.0F, 1.0F, 1.0F);

    // generated tiles
    // private List<GameObject> genTiles;

    // tile area size
    public int tileAreaX = 10;
    public int tileAreaY = 10;
    public int tileAreaZ = 10;

    // chance of changing direction (out of 1.0F (100%))
    public float direcChangeChance = 0.4F; // out of 1.0F

    // chance of spawning room (out of 1.0F (100%))
    public float spreadChance = 0.2F;

    // amount of cycles in one direction.
    public int cycles = 10;

    // copies the rotates the vector 2
    public Vector2 RotateVector2(Vector2 v, float deg)
    {
        Vector2 vx = new Vector2();

        // calculates rotation
        vx.x = v.x * Mathf.Cos(Mathf.Deg2Rad * deg) - v.y * Mathf.Sin(Mathf.Deg2Rad * deg);
        vx.y = v.x * Mathf.Sin(Mathf.Deg2Rad * deg) + v.y * Mathf.Cos(Mathf.Deg2Rad * deg);

        return vx;
    }

    // copies and rotates the vector 2 int
    public Vector2Int RotateVector2Int(Vector2Int v, float deg)
    {
        // calculates the vector rotation
        Vector2 vx = new Vector2();
        Vector2Int vy = new Vector2Int();

        // calculates rotation
        vx.x = v.x * Mathf.Cos(Mathf.Deg2Rad * deg) - v.y * Mathf.Sin(Mathf.Deg2Rad * deg);
        vx.y = v.x * Mathf.Sin(Mathf.Deg2Rad * deg) + v.y * Mathf.Cos(Mathf.Deg2Rad * deg);

        vy.x = Mathf.RoundToInt(vx.x);
        vy.y = Mathf.RoundToInt(vx.y);

        return vy;
    }


    // Start is called before the first frame update
    void Start()
    {
        // tile not set.
        if (tile == null)
            return;


        levelParent = new GameObject(levelParentName);

        // generates the level
        GenerateLevel();
    }

    // generates the tiles
    public void GenerateLevel()
    {
        // gets block size (originally was in Start(), but I did this to refresh
        {
            // gets the renderer
            MeshRenderer renderer = tile.GetComponent<MeshRenderer>();

            // gets the tile origin
            tileOrigin = renderer.bounds.center;

            // gets the size of the object
            tileSize = tile.GetComponent<MeshRenderer>().bounds.size;
        }

        // VARIABLES
        // the value at the given index determines how high the blocks in a given place are
        int[,] grid = new int[tileAreaZ, tileAreaX]; // y-axis is filled by height

        // offset of tiles
        Vector3 offset = tileSize;

        // gets the current cell
        // Vector3Int currCell = new Vector3Int(
        //     Random.Range(0, tileAreaX),
        //     Random.Range(0, tileAreaY),
        //     Random.Range(0, tileAreaZ)
        // );

        // gets the current cell
        Vector2Int currCell = new Vector2Int(Random.Range(0, tileAreaZ), Random.Range(0, tileAreaX));

        // randomizes the direction
        Vector2Int dir = new Vector2Int();

        {
            // randomizes the direction
            int randNum = Random.Range(0, 5);

            switch(randNum)
            {
                case 1: // left first
                    dir = Vector2Int.left;
                    break;

                case 2: // up first
                    dir = Vector2Int.up;
                    break;

                case 3: // right first
                    dir = Vector2Int.right;
                    break;

                case 4: // down first
                default:
                    dir = Vector2Int.down;
                    break;
            }
        }


        // goes in all four directions
        for (int i = 0; i < 4; i++)
        {
            Vector2Int cycleDir = dir; // the direction for the current iteration
            Vector2Int cycleCell = currCell;
            int cycle = 0;

            // while the amount of desired cycles has not been surpassed.
            while(cycle < cycles)
            {
                // gets two random numbers
                float r1 = Random.Range(0.0F, 1.0F);
                float r2 = Random.Range(0.0F, 1.0F);

                // if the direction should change
                if(r1 <= direcChangeChance)
                {
                    cycleDir = RotateVector2Int(cycleDir, 90.0F);
                }

                // a spread shot should be used.
                // fills adjacent tiles
                if (r2 <= spreadChance)
                {
                    // one left
                    if (cycleCell.x - 1 > 0)
                    {
                        grid[cycleCell.x - 1, cycleCell.y]++;
                    }

                    // one right
                    if (cycleCell.x + 1 < tileAreaZ)
                    {
                        grid[cycleCell.x + 1, cycleCell.y]++;
                    }

                    // one up
                    if (cycleCell.y + 1 < tileAreaX)
                    {
                        grid[cycleCell.x, cycleCell.y + 1]++;
                    }

                    // one down
                    if (cycleCell.y - 1 > 0)
                    {
                        grid[cycleCell.x, cycleCell.y - 1]++;
                    }
                }

                // increments current cell
                grid[cycleCell.x, cycleCell.y]++;

                // goes onto new current cell
                cycleCell += cycleDir;

                // if the cell is out of bounds, break.
                if(cycleCell.x < 0 || cycleCell.x >= tileAreaZ || cycleCell.y < 0 || cycleCell.y >= tileAreaX)
                {
                    break;
                }
                else
                {
                    cycle++;
                }
            }

            // rotates to go to the next direction
            dir = RotateVector2Int(dir, 90.0F);
        }

        // for(int i = 0; i < tileAreaX; i++)
        // {
        //     int x = Random.Range(0, 2);
        // 
        //     if(x == 0)
        //     {
        //         Instantiate(tile, new Vector3(offset.x * i, offset.y, offset.z), new Quaternion(0, 0, 0, 1));
        //     }
        // }

        // instatiates objects
        for(int row = 0; row < tileAreaZ; row++)
        {
            for(int col = 0; col < tileAreaX; col++)
            {
                // generates platforms in accordance with tile
                for(int n = 0; n < grid[row, col] && n < tileAreaY; n++)
                {
                    GameObject go = Instantiate(tile, new Vector3(offset.x * col, offset.y * n, offset.z * row), new Quaternion(0, 0, 0, 1));
                    go.transform.parent = levelParent.transform;
                }
            }
        }


        // for the y-axis, randomize the height.
    }

    // references the random maze
    public void Randomize()
    {
        Destroy(levelParent);
        levelParent = new GameObject(levelParentName);
        GenerateLevel();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
