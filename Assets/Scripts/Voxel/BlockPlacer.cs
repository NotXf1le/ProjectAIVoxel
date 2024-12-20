using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BlockPlacer : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    const int gridSize = 8;
    public Voxel[] voxels;
    public Dictionary<GameObject, Voxel> blocks = new Dictionary<GameObject, Voxel>();
    private Color currentColor = new Color(0.55f, 0.27f, 0.07f);

    [System.Serializable]
    public enum VoxelColor
    {
        White,
        Black,
        DarkGreen,
        Green,
        Blue,
        Yellow,
        Brown,
        Grey
    }
    private static BlockPlacer _instance;
    public static BlockPlacer Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateVoxels();

    }
    public void PlaceBlock(RaycastHit hit)
    {
        Debug.Log("Hit: " + hit.transform.name);
        Vector3 newBlockPosition = hit.transform.position + (hit.normal);
        newBlockPosition = new Vector3((int)newBlockPosition.x, (int)newBlockPosition.y, (int)newBlockPosition.z);

        foreach (Voxel voxel in voxels)
        {
            Vector3 voxelPos = new Vector3(voxel.X, voxel.Y, voxel.Z);
            if (voxelPos == newBlockPosition && !voxel.IsActive)
            {
                voxel.R = currentColor.r;
                voxel.G = currentColor.g;
                voxel.B = currentColor.b;
                voxel.IsActive = true;
                GenertaeBlock(voxel);

            }

        }
    }
    public void RemoveBlock(RaycastHit hit)
    {
        if (hit.transform.gameObject == null)
            return;
        if (blocks.ContainsKey(hit.transform.gameObject))
        {
            GameObject blockForRemoving = hit.transform.gameObject;
            Voxel voxelForRemoving = blocks[blockForRemoving];
            blocks.Remove(blockForRemoving);
            Destroy(blockForRemoving);

            foreach (Voxel voxel in voxels)
            {
                if (voxel == voxelForRemoving)
                {
                    voxel.Reset();
                }

            }
        }
    }
    public void ChangeColor(string colorName)
    {
        if (Enum.TryParse(colorName, true, out VoxelColor result))
        {
            switch (result)
            {
                case VoxelColor.White:
                    currentColor = new Color(0.95f, 1f, 1f);
                    break;

                case VoxelColor.Black:
                    currentColor = new Color(0f, 0f, 0f);
                    break;

                case VoxelColor.DarkGreen:
                    currentColor = new Color(0.05f, 0.5f, 0.1f);
                    break;

                case VoxelColor.Green:
                    currentColor = new Color(0.18f, 0.8f, 0.25f);
                    break;

                case VoxelColor.Blue:
                    currentColor = new Color(0f, 0.45f, 0.85f);
                    break;

                case VoxelColor.Yellow:
                    currentColor = new Color(1f, 0.86f, 0f);
                    break;

                case VoxelColor.Brown:
                    currentColor = new Color(0.55f, 0.27f, 0.07f);
                    break;

                case VoxelColor.Grey:
                    currentColor = new Color(0.67f, 0.67f, 0.67f);
                    break;

            }
        }
    }
    private void GenerateVoxels()
    {
        List<Voxel> voxelsList = new List<Voxel>();
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {

                for (int z = 0; z < gridSize; z++)
                {
                    voxelsList.Add(new Voxel(new Vector3(x, y, z), Color.black, false));

                }
            }
        }
        voxels = voxelsList.ToArray();
        GenerateBlocks();

    }
    public void GenerateBlocks()
    {
        foreach(Voxel voxel in voxels)
        {
            if(voxel.IsActive)
                GenertaeBlock(voxel);

        }
    }

    private void GenertaeBlock(Voxel voxel)
    {
        GameObject newBlock = Instantiate(blockPrefab);

        newBlock.transform.position = new Vector3(voxel.X, voxel.Y, voxel.Z);
        newBlock.GetComponent<MeshRenderer>().material.color = new Color(voxel.R, voxel.G, voxel.B, 1);
        blocks.Add(newBlock, voxel);

    }
    public void ChangeGreenColor()
    {
        foreach (Voxel voxel in voxels)
        {
            if (voxel.IsActive)
            {
                Color voxelColor = new Color(voxel.R, voxel.G, voxel.B);
                if(voxelColor == new Color (0.05f, 0.5f, 0.1f))
                {
                    if(UnityEngine.Random.Range(0f,1f) > 0.5f)
                    {
                        voxel.R = 0.18f;
                        voxel.G = 0.8f;
                        voxel.B = 0.25f;

                    }
                    

                }
                else if(voxelColor == new Color(0.18f, 0.8f, 0.25f))
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
                    { 
                        voxel.R = 0.05f;
                        voxel.G = 0.5f;
                        voxel.B = 0.1f;

                    }
                }
            }
            List<GameObject> keys = new List<GameObject>(blocks.Keys);
            foreach (GameObject block in keys)
            {
                Destroy(block);
            }
            blocks.Clear();
            GenerateBlocks();

        }
    }
    public void RemoveAllVoxels()
    {
        foreach (Voxel voxel in voxels)
        {
            voxel.Reset();
            if(voxel.IsActive)
                Debug.Log(voxel.IsActive);
        }
        List<GameObject> keys = new List<GameObject>(blocks.Keys);
        foreach (GameObject block in keys)
        {
            Destroy(block);
        }
        blocks.Clear();
        GenerateBlocks();

    }
    public void Rotate90()
    {
        voxels = RotateVoxels(voxels, 90);
        List<GameObject> keys = new List<GameObject>(blocks.Keys);
        foreach (GameObject block in keys)
        {
            Destroy(block);
        }
        blocks.Clear();
        GenerateBlocks();
    }

    private Voxel[] RotateVoxels(Voxel[] voxels, int degrees)
    {
        List<Voxel> rotatedVoxels = new List<Voxel>();

        foreach (Voxel voxel in voxels)
        {
            Vector3 pos = new Vector3(voxel.X, voxel.Y, voxel.Z);
            Vector3 newPos;

            switch (degrees)
            {
                case 90:
                    newPos = new Vector3(pos.z, pos.y, gridSize - 1 - pos.x);
                    break;

                case 180:
                    newPos = new Vector3(gridSize - 1 - pos.x, pos.y, gridSize - 1 - pos.z);
                    break;

                case 270:
                    newPos = new Vector3(gridSize - 1 - pos.z, pos.y, pos.x);
                    break;

                default:
                    throw new ArgumentException("Pogresan unos.");
            }

            newPos = ClampToGrid(newPos, gridSize);

            rotatedVoxels.Add(new Voxel(newPos, new Color(voxel.R, voxel.G, voxel.B), voxel.IsActive));
        }

        return rotatedVoxels.ToArray();
    }
    public void Flip(string axis)
    {
        voxels = FlipVoxels(voxels, 8, axis);
        List<GameObject> keys = new List<GameObject>(blocks.Keys);
        foreach (GameObject block in keys)
        {
            Destroy(block);
        }
        blocks.Clear();
        GenerateBlocks();

    }
    private Voxel[] FlipVoxels(Voxel[] voxels, int gridSize, string axis)
    {
        List<Voxel> flippedVoxels = new List<Voxel>();

        foreach (Voxel voxel in voxels)
        {
            Vector3 pos = new Vector3(voxel.X, voxel.Y, voxel.Z);
            Vector3 newPos;

            switch (axis.ToLower())
            {
                case "z":
                    newPos = new Vector3(pos.x, pos.y, gridSize - 1 - pos.z);
                    break;

                case "x":
                    newPos = new Vector3(gridSize - 1 - pos.x, pos.y, pos.z);
                    break;

                default:
                    throw new ArgumentException("Pogresan unos.");

            }

            flippedVoxels.Add(new Voxel(newPos, new Color(voxel.R, voxel.G, voxel.B), voxel.IsActive));
        }

        return flippedVoxels.ToArray();
    }

    private Vector3 ClampToGrid(Vector3 pos, int gridSize)
    {
        return new Vector3(
            Mathf.Clamp(pos.x, 0, gridSize - 1),
            Mathf.Clamp(pos.y, 0, gridSize - 1),
            Mathf.Clamp(pos.z, 0, gridSize - 1)
        );
    }

}


