using UnityEngine;
using System.Collections;

public class RaycastUtility
{
	public static WorldPos GetBlockPos (RaycastHit hit, bool adjacent = false)
	{
		Vector3 pos = hit.point;
		WorldPos blockPos = new WorldPos (Mathf.FloorToInt (pos.x), Mathf.FloorToInt (pos.y), Mathf.FloorToInt (pos.z));

		float norX = hit.normal.x;
		float norY = hit.normal.y;
		float norZ = hit.normal.z;
		if (norX > 0)
			blockPos.x -= 1;
		if (norY > 0)
			blockPos.y -= 1;
		if (norZ > 0)
			blockPos.z -= 1;
		if (adjacent)
			blockPos.AddVecter3 (hit.normal);

		return blockPos;
	}
}
