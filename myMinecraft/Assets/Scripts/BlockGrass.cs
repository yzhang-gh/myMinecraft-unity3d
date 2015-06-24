using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BlockGrass : Block
{
	public string name = "grassBlock";
	public string Name{get{return name;}}

	public BlockGrass (): base()
	{

	}

	public override Tile TexturePosition (Direction direction)
	{
		Tile tile = new Tile ();

		switch (direction)
		{
		case Direction.up:
			tile.x = 1;
			tile.y = 1;
			return tile;
		case Direction.down:
			tile.x = 0;
			tile.y = 1;
			return tile;
		}

		tile.x = 1;
		tile.y = 0;

		return tile;
	}
	
	public override string GetName()
	{
		return "grass";
	}
}