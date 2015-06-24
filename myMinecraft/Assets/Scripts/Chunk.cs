using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class Chunk : MonoBehaviour
{

	public Block[,,] blocks = new Block[chunkWidth, chunkHeight, chunkWidth];
	public static int chunkWidth = 16;
	public static int chunkHeight = 128;
	public bool update = true;
	MeshFilter filter;
	MeshCollider coll;
	public World world;
	public WorldPos pos;

	void Start ()
	{
		filter = gameObject.GetComponent<MeshFilter> ();
		coll = gameObject.GetComponent<MeshCollider> ();
	}

	//Update is called once per frame
	void Update ()
	{
		if (update)
		{
			update = false;
			UpdateChunk ();
		}
	}

	public Block GetBlock (int x, int y, int z)
	{
		if (y < 0 || y >= chunkHeight)
		{
			return new Block();
		}
		if (InRange (x) && InRange (z))
			return blocks [x, y, z];
		return world.GetBlock (pos.x + x, y, pos.z + z);
	}

	public static bool InRange (int index)
	{
		if (index < 0 || index >= chunkWidth)
			return false;
		return true;
	}

	public void SetBlock (int x, int y, int z, Block block)
	{
		if (y < 0 || y > chunkHeight)
		{
			return;
		}
		if (InRange (x) && InRange (z))
		{
			blocks [x, y, z] = block;
			blocks [x, y, z].changed = true;
		} else
		{
			world.SetBlock (pos.x + x, y, pos.z + z, block);
		}
	}

	public void SetBlocksUnmodified ()
	{
		for(int x = 0; x < chunkWidth; x++)
		{
			for(int y = 0; y < chunkHeight; y++)
			{
				for(int z = 0; z < chunkWidth; z++)
				{
					blocks[x, y, z].changed = false;
				}
			}
		}
	}

	// Updates the chunk based on its contents
	void UpdateChunk ()
	{
		MeshData meshData = new MeshData ();

		for (int x = 0; x < chunkWidth; x++)
		{
			for (int y = 0; y < chunkHeight; y++)
			{
				for (int z = 0; z < chunkWidth; z++)
				{
					meshData = blocks [x, y, z].Blockdata (this, x, y, z, meshData);
				}
			}
		}

		RenderMesh (meshData);
	}

	// Sends the calculated mesh information
	// to the mesh and collision components
	void RenderMesh (MeshData meshData)
	{
		filter.mesh.Clear ();
		filter.mesh.vertices = meshData.vertices.ToArray ();
		filter.mesh.triangles = meshData.triangles.ToArray ();

		filter.mesh.uv = meshData.uv.ToArray ();
		filter.mesh.RecalculateNormals ();

		coll.sharedMesh = null;
		Mesh mesh = new Mesh ();
		mesh.vertices = meshData.colVertices.ToArray ();
		mesh.triangles = meshData.colTriangles.ToArray ();
		mesh.RecalculateNormals ();
		coll.sharedMesh = mesh;
	}
}