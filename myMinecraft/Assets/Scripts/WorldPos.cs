using UnityEngine;
using System.Collections;
using System;

[Serializable]
public struct WorldPos
{
	public int x, y, z;

	public WorldPos (int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public void AddVecter3 (Vector3 vec)
	{
		x += (int)vec.x;
		y += (int)vec.y;
		z += (int)vec.z;
	}

	public override bool Equals (object obj)
	{
		if (!(obj is WorldPos))
			return false;

		WorldPos pos = (WorldPos)obj;
		if (pos.x != x || pos.y != y || pos.z != z)
		{
			return false;
		} else
		{
			return true;
		}
	}

	public override int GetHashCode ()
	{
		return (x + "," + y + "," + z).GetHashCode ();
	}

	public override string ToString ()
	{
		return string.Format ("[WorldPos] (" + x + ", " + y + ", " + z + ")");
	}
}