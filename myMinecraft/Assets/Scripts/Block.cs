using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Block
{

    public enum Direction { north, east, south, west, up, down };

    public struct Tile { public int x; public int y;}
    const float tileSize = 0.25f;
    public bool changed = true;

    //Base block constructor
    public Block()
    {

    }

    public virtual MeshData Blockdata(Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        meshData.useRenderDataForCol = true;

		if (!chunk.GetBlock (x, y + 1, z).IsSolid (Direction.down))
		{
			meshData = FaceData (chunk, new Vector3 (x, y + 1, z), Vector3.forward, Vector3.right, Direction.up, meshData);
		}
		
		if (!chunk.GetBlock (x, y - 1, z).IsSolid (Direction.up))
		{
			meshData = FaceData (chunk, new Vector3 (x, y, z), Vector3.right, Vector3.forward, Direction.down, meshData);
		}
		
		if (!chunk.GetBlock (x, y, z + 1).IsSolid (Direction.south))
		{
			meshData = FaceData (chunk, new Vector3 (x, y, z + 1), Vector3.right, Vector3.up, Direction.north, meshData);
		}
		
		if (!chunk.GetBlock (x, y, z - 1).IsSolid (Direction.north))
		{
			meshData = FaceData (chunk, new Vector3 (x + 1, y, z), Vector3.left, Vector3.up, Direction.south, meshData);
		}
		
		if (!chunk.GetBlock (x + 1, y, z).IsSolid (Direction.west))
		{
			meshData = FaceData (chunk, new Vector3 (x + 1, y, z + 1), Vector3.back, Vector3.up, Direction.east, meshData);
		}
		
		if (!chunk.GetBlock (x - 1, y, z).IsSolid (Direction.east))
		{
			meshData = FaceData (chunk, new Vector3 (x, y, z), Vector3.forward, Vector3.up, Direction.west, meshData);
		}

        return meshData;
    }
	
	protected virtual MeshData FaceData (Chunk chunk, Vector3 corner, Vector3 v1, Vector3 v2, Direction direction, MeshData meshData)
	{
		meshData.AddVertex (corner + v1);
		meshData.AddVertex (corner + v1 + v2);
		meshData.AddVertex (corner + v2);
		meshData.AddVertex (corner);
		
		meshData.AddQuadTriangles ();
		
		meshData.uv.AddRange(FaceUVs(direction));
		
		return meshData;
	}

    public virtual Vector2[] FaceUVs(Direction direction)
    {
        Vector2[] UVs = new Vector2[4];
        Tile tilePos = TexturePosition(direction);

        UVs[0] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y);
        UVs[1] = new Vector2(tileSize * tilePos.x + tileSize,
            tileSize * tilePos.y + tileSize);
        UVs[2] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y + tileSize);
        UVs[3] = new Vector2(tileSize * tilePos.x,
            tileSize * tilePos.y);

        return UVs;
    }

	public virtual Tile TexturePosition(Direction direction)
	{
		Tile tile = new Tile();
		tile.x = 0;
		tile.y = 0;
		
		return tile;
	}
	
	public virtual bool IsSolid(Direction direction)
	{
		switch (direction)
		{
		case Direction.north:
			return true;
		case Direction.east:
			return true;
		case Direction.south:
			return true;
		case Direction.west:
			return true;
		case Direction.up:
			return true;
		case Direction.down:
			return true;
		}
		
		return false;
	}

	public virtual string GetName()
	{
		return "baseBlock";
	}
}