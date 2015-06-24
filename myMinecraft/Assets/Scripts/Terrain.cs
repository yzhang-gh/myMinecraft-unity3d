using UnityEngine;
using System.Collections;

public static class Terrain
{
	public static bool SetBlock (RaycastHit hit, Block block, bool adjacent = false)
	{
		Chunk chunk = hit.collider.GetComponent<Chunk> ();
		if (chunk == null)
			return false;

		WorldPos pos = RaycastUtility.GetBlockPos (hit, adjacent);

		chunk.world.SetBlock (pos.x, pos.y, pos.z, block);

		return true;
	}

	public static Block GetBlock (RaycastHit hit)
	{
		Chunk chunk = hit.collider.GetComponent<Chunk> ();
		if (chunk == null)
			return null;

		WorldPos pos = RaycastUtility.GetBlockPos (hit);

		Block block = chunk.world.GetBlock (pos.x, pos.y, pos.z);

		return block;
	}
}