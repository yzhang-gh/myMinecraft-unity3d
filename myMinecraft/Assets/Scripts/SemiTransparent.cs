using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SemiTransparent : MonoBehaviour
{

	public List<Vector3> verts = new List<Vector3> ();
	public List<Vector2> uvs = new List<Vector2> ();
	public List<int> tris = new List<int> ();
	private Mesh mesh;

	public void ClearAll ()
	{
		verts.Clear ();
		uvs.Clear ();
		tris.Clear ();
		
		mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		mesh.vertices = verts.ToArray ();
		mesh.triangles = tris.ToArray ();
		mesh.uv = uvs.ToArray ();
		mesh.RecalculateNormals ();
	}

	public void ShowMarquee (RaycastHit hit)
	{
		verts.Clear ();
		uvs.Clear ();
		tris.Clear ();

		WorldPos pos = RaycastUtility.GetBlockPos (hit);
		Block.Direction direction = Block.Direction.up;
		float norX = hit.normal.x;
		float norY = hit.normal.y;
		float norZ = hit.normal.z;
		if (norX > 0)
			direction = Block.Direction.east;
		if (norX < 0)
			direction = Block.Direction.west;
		if (norY > 0)
			direction = Block.Direction.up;
		if (norY < 0)
			direction = Block.Direction.down;
		if (norZ > 0)
			direction = Block.Direction.north;
		if (norZ < 0)
			direction = Block.Direction.south;

		BuildFace (pos, direction);
	
		mesh = GetComponent<MeshFilter> ().mesh;
		mesh.Clear ();
		mesh.vertices = verts.ToArray ();
		mesh.triangles = tris.ToArray ();
		mesh.uv = uvs.ToArray ();
		mesh.RecalculateNormals ();
	}

	
	//
	private void BuildFace (Vector3 corner, Vector3 v1, Vector3 v2, bool reversed)
	{
		int index = verts.Count;

		verts.Add (corner + v1);
		verts.Add (corner + v1 + v2);
		verts.Add (corner + v2);
		verts.Add (corner);
		
		uvs.Add (new Vector2 (0, 0));
		uvs.Add (new Vector2 (0, 1));
		uvs.Add (new Vector2 (1, 1));
		uvs.Add (new Vector2 (1, 0));
		
		if (reversed)
		{
			tris.Add (index + 0);
			tris.Add (index + 3);
			tris.Add (index + 1);
			tris.Add (index + 1);
			tris.Add (index + 3);
			tris.Add (index + 2);
		} else
		{
			tris.Add (index + 0);
			tris.Add (index + 1);
			tris.Add (index + 3);
			tris.Add (index + 1);
			tris.Add (index + 2);
			tris.Add (index + 3);
		}
	}
	
	private void BuildFace (WorldPos worldCorner, Block.Direction direction)
	{
		Vector3 corner = new Vector3 (worldCorner.x, worldCorner.y, worldCorner.z);
		switch (direction)
		{
		case Block.Direction.up:
			BuildFace (corner + Vector3.up + new Vector3(0, 0.005f, 0), Vector3.forward, Vector3.right, false);
			break;
		case Block.Direction.down:
			BuildFace (corner - new Vector3(0, 0.005f, 0), Vector3.forward, Vector3.right, true);
			break;
		case Block.Direction.west:
			BuildFace (corner - new Vector3(0.005f, 0, 0), Vector3.up, Vector3.forward, true);
			break;
		case Block.Direction.east:
			BuildFace (corner + Vector3.right + new Vector3(0.005f, 0, 0), Vector3.up, Vector3.forward, false);
			break;
		case Block.Direction.north:
			BuildFace (corner + Vector3.forward + new Vector3(0, 0, 0.005f), Vector3.up, Vector3.right, true);
			break;
		case Block.Direction.south:
			BuildFace (corner - new Vector3(0, 0, 0.005f), Vector3.up, Vector3.right, false);
			break;
		}
	}
}
