using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//converts block data into bitstream for storage in a matrix
[Serializable]
class BlockData
{
    public Block.BlockType[,,] matrix;

    public BlockData() { }

    public BlockData(Block[,,] b)
    {
        matrix = new Block.BlockType[World.chunkSize, World.chunkSize, World.chunkSize];
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    matrix[x, y, z] = b[x, y, z].bType;
                }
    }
}


public class Chunk
{

    public Material cubeMaterial;
    public Material fluidMaterial;
    public Block[,,] chunkData;
    public GameObject chunk;
    public GameObject fluid;
    public enum ChunkStatus { DRAW, DONE, KEEP };
    public ChunkStatus status;
    public ChunkMB mb;
    BlockData bd;
    public bool changed = false;
    bool treesCreated = false;

    //names chunkdata file
    string BuildChunkFileName(Vector3 v)
    {
        return Application.persistentDataPath + "/savedata/Chunk_" +
                                (int)v.x + "_" +
                                    (int)v.y + "_" +
                                        (int)v.z +
                                        "_" + World.chunkSize +
                                        "_" + World.radius +
                                        ".dat";
    }

    //read data from file
    bool Load()
    {
        /*string chunkFile = BuildChunkFileName(chunk.transform.position);
		if(File.Exists(chunkFile))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(chunkFile, FileMode.Open);
			bd = new BlockData();
			bd = (BlockData) bf.Deserialize(file);
			file.Close();
			//Debug.Log("Loading chunk from file: " + chunkFile);
			return true;
		}*/
        return false;
    }

    //write data to file
    public void Save()
    {
        /*string chunkFile = BuildChunkFileName(chunk.transform.position);
		
		if(!File.Exists(chunkFile))
		{
			Directory.CreateDirectory(Path.GetDirectoryName(chunkFile));
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(chunkFile, FileMode.OpenOrCreate);
		bd = new BlockData(chunkData);
		bf.Serialize(file, bd);
		file.Close();*/
        //Debug.Log("Saving chunk from file: " + chunkFile);
    }

    //checks if there is a sandblock that needs to move when a block is destroyed
    public void UpdateChunk()
    {
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    if (chunkData[x, y, z].bType == Block.BlockType.SAND)
                    {
                        mb.StartCoroutine(mb.Drop(chunkData[x, y, z],
                                        Block.BlockType.SAND,
                                        20));
                    }
                    if (y == (World.chunkSize - 1))
                    {
                        Block blockAbove = chunkData[x, y, z].GetBlock(x, y + 1, z);
                        if (blockAbove.bType == Block.BlockType.SAND)
                            blockAbove.owner.UpdateChunk();
                    }
                    if (chunkData[x, y, z].bType == Block.BlockType.SNOW)
                    {
                        mb.StartCoroutine(mb.Drop(chunkData[x, y, z],
                                        Block.BlockType.SNOW,
                                        20));
                    }
                    if (y == (World.chunkSize - 1))
                    {
                        Block blockAbove = chunkData[x, y, z].GetBlock(x, y + 1, z);
                        if (blockAbove.bType == Block.BlockType.SNOW)
                            blockAbove.owner.UpdateChunk();
                    }
                }
    }

    void BuildChunk()
    {
        //loads previously generated fbm data rather than generating it again
        bool dataFromFile = false;
        //dataFromFile = Load();

        //loops around building chunks using nested for loop
        chunkData = new Block[World.chunkSize, World.chunkSize, World.chunkSize];
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    //creates a position for the chunk based on the XYZ loop and the perlin height
                    Vector3 pos = new Vector3(x, y, z);
                    int worldX = (int)(x + chunk.transform.position.x);
                    int worldY = (int)(y + chunk.transform.position.y);
                    int worldZ = (int)(z + chunk.transform.position.z);

                    //if saved data exists, use that and skip to the next for loop
                    if (dataFromFile)
                    {
                        chunkData[x, y, z] = new Block(bd.matrix[x, y, z], pos,
                                        chunk.gameObject, this);
                        continue;
                    }

                    //finds the surface height from utils
                    int surfaceHeight = Utils.GenerateHeight(worldX, worldZ);

                    //spawns BEDROCK
                    if (worldY == 0)
                        chunkData[x, y, z] = new Block(Block.BlockType.BEDROCK, pos,
                                        chunk.gameObject, this);
                    //spawns DIAMOND, REDSTONE, SNOW and STONE
                    else if (worldY <= Utils.GenerateStoneHeight(worldX, worldZ))
                    {
                        if (Utils.fBM3D(worldX, worldY, worldZ, 0.01f, 2) < 0.4f && worldY < 40)
                            chunkData[x, y, z] = new Block(Block.BlockType.DIAMOND, pos,
                                chunk.gameObject, this);
                        else if (Utils.fBM3D(worldX, worldY, worldZ, 0.03f, 3) < 0.41f && worldY < 20)
                            chunkData[x, y, z] = new Block(Block.BlockType.REDSTONE, pos,
                                chunk.gameObject, this);
                        else if (worldY > 85)
                            chunkData[x, y, z] = new Block(Block.BlockType.SNOW, pos,
                                chunk.gameObject, this);
                        else
                            chunkData[x, y, z] = new Block(Block.BlockType.STONE, pos,
                                chunk.gameObject, this);
                    }
                    //spawns GRASS, SAND and TREEBASE's
                    else if (worldY == surfaceHeight)
                    {
                        if (Utils.fBM3D(worldX, worldY, worldZ, 0.4f, 2) < 0.35f)
                        {
                            if (worldY > 49)
                                chunkData[x, y, z] = new Block(Block.BlockType.PINEBASE, pos,
                                        chunk.gameObject, this);
                            else
                                chunkData[x, y, z] = new Block(Block.BlockType.WOODBASE, pos,
                                        chunk.gameObject, this);
                        }
                        else if (worldY < 55)
                            chunkData[x, y, z] = new Block(Block.BlockType.SAND, pos,
                                        chunk.gameObject, this);
                        else
                            chunkData[x, y, z] = new Block(Block.BlockType.GRASS, pos,
                                        chunk.gameObject, this);
                    }
                    //spawns DIRT and WATER
                    else if (worldY < surfaceHeight)
                        chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos,
                            chunk.gameObject, this);
                    else if (worldY < 50)
                        chunkData[x, y, z] = new Block(Block.BlockType.WATER, pos,
                                        fluid.gameObject, this);
                    //otherwise, AIR
                    else
                    {
                        chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos,
                            chunk.gameObject, this);
                    }

                    //works out caves and water for cave generation
                    if (Utils.fBM3D(worldX, worldY, worldZ, 0.1f, 3) < 0.42f)
                    {
                        if (worldY < 50)
                            chunkData[x, y, z] = new Block(Block.BlockType.WATER, pos,
                                        chunk.gameObject, this);
                        else
                            chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos,
                                       chunk.gameObject, this);
                    }

                    //block is ready to be drawn
                    status = ChunkStatus.DRAW;

                }
    }

    //cleans up and redraws when the block calls for a redraw
    public void Redraw()
    {
        GameObject.DestroyImmediate(chunk.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(chunk.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(chunk.GetComponent<Collider>());
        GameObject.DestroyImmediate(fluid.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(fluid.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(fluid.GetComponent<Collider>());
        DrawChunk();
    }

    public void DrawChunk()
    {
        if (!treesCreated)
        {
            for (int z = 0; z < World.chunkSize; z++)
                for (int y = 0; y < World.chunkSize; y++)
                    for (int x = 0; x < World.chunkSize; x++)
                    {
                        BuildTrees(chunkData[x, y, z], x, y, z);
                    }
            treesCreated = true;
        }
        //loops around drawing chunks using nested for loop
        for (int z = 0; z < World.chunkSize; z++)
            for (int y = 0; y < World.chunkSize; y++)
                for (int x = 0; x < World.chunkSize; x++)
                {
                    chunkData[x, y, z].Draw();
                }
        CombineQuads(chunk.gameObject, cubeMaterial);
        MeshCollider collider = chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = chunk.transform.GetComponent<MeshFilter>().mesh;

        CombineQuads(fluid.gameObject, fluidMaterial);
        status = ChunkStatus.DONE;
    }

    //builds pine trees
    void BuildPineTree(Block trunk, int x, int y, int z)
    {
        Block t = trunk.GetBlock(x, y + 1, z);
        if (t != null)
        {
            t.SetType(Block.BlockType.PINE);
            Block t1 = t.GetBlock(x, y + 2, z);
            if (t1 != null)
            {
                t1.SetType(Block.BlockType.PINE);

                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        for (int k = 3; k <= 4; k++)
                        {
                            Block t2 = trunk.GetBlock(x + i, y + k, z + j);

                            if (t2 != null)
                            {
                                t2.SetType(Block.BlockType.PINELEAVES);
                            }
                            else return;
                        }
                Block t3 = t1.GetBlock(x, y + 5, z);
                if (t3 != null)
                {
                    t3.SetType(Block.BlockType.PINELEAVES);
                }
            }
        }
    }

    //builds a regular tree
    void BuildTree(Block trunk, int x, int y, int z)
    {
        Block t = trunk.GetBlock(x, y + 1, z);
        if (t != null)
        {
            t.SetType(Block.BlockType.WOOD);
            Block t1 = t.GetBlock(x, y + 2, z);
            if (t1 != null)
            {
                t1.SetType(Block.BlockType.WOOD);

                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                        for (int k = 3; k <= 4; k++)
                        {
                            Block t2 = trunk.GetBlock(x + i, y + k, z + j);

                            if (t2 != null)
                            {
                                t2.SetType(Block.BlockType.LEAVES);
                            }
                            else return;
                        }
                Block t3 = t1.GetBlock(x, y + 5, z);
                if (t3 != null)
                {
                    t3.SetType(Block.BlockType.LEAVES);
                    Block t4 = t3.GetBlock(x, y + 1, z);
                    if (t4 != null)
                        t4.SetType(Block.BlockType.LEAVES);
                }
            }
        }
    }

    //looks for treebases in the world and builds trees there
    void BuildTrees(Block trunk, int x, int y, int z)
    {
        if (trunk.bType == Block.BlockType.WOODBASE)
            BuildTree(trunk, x, y, z);

        else if (trunk.bType == Block.BlockType.PINEBASE)
            BuildPineTree(trunk, x, y, z);
    }

    public Chunk() { }
    // Use this for initialization, chunk creates itself and its own game object
    public Chunk(Vector3 position, Material c, Material t)
    {

        chunk = new GameObject(World.BuildChunkName(position));
        chunk.transform.position = position;
        fluid = new GameObject(World.BuildChunkName(position) + "_F");
        fluid.transform.position = position;

        mb = chunk.AddComponent<ChunkMB>();
        mb.SetOwner(this);
        cubeMaterial = c;
        fluidMaterial = t;
        BuildChunk();
    }


    public void CombineQuads(GameObject o, Material m)
    {
        //1. Combine all children meshes
        MeshFilter[] meshFilters = o.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }

        //2. Create a new mesh on the parent object
        MeshFilter mf = (MeshFilter)o.gameObject.AddComponent(typeof(MeshFilter));
        mf.mesh = new Mesh();

        //3. Add combined meshes on children as the parent's mesh
        mf.mesh.CombineMeshes(combine);

        //4. Create a renderer for the parent
        MeshRenderer renderer = o.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = m;

        //5. Delete all uncombined children
        foreach (Transform quad in o.transform)
        {
            GameObject.Destroy(quad.gameObject);
        }

    }

}
