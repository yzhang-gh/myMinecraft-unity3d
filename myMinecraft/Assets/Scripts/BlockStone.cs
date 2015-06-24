using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockStone : Block
{
	public string name = "stoneBlock";
	public string Name{get{return name;}}

	public BlockStone (): base()
	{
		
	}

	public override Tile TexturePosition(Direction direction)
	{
		Tile tile = new Tile();
		tile.x = 0;
		tile.y = 0;
		
		return tile;
	}

	public override bool IsSolid(Direction direction)
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
	
	public override string GetName()
	{
		return "stone";
	}
}
