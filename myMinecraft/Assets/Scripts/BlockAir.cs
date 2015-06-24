using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockAir : Block
{
	public string name = "airBlock";
	public string Name{get{return name;}}

    public BlockAir() : base()
    {

    }

    public override MeshData Blockdata
        (Chunk chunk, int x, int y, int z, MeshData meshData)
    {
        return meshData;
    }

	public override bool IsSolid(Block.Direction direction)
    {
        return false;
    }
	
	public override string GetName()
	{
		return "air";
	}
}