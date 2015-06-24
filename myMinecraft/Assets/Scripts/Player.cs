using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public int viewRange = 50;
	int blockID = 0;
	int fpsCtrl = 0;

	public int chunkWidth{ get { return Chunk.chunkWidth; } }

	public World world{ get { return World.currentWorld; } }
	
	public SemiTransparent semiTransparentFab;
	private SemiTransparent semiTransparent;

	void Awake()
	{
		Screen.SetResolution(1000, 618, false);
	}

	// Use this for initialization
	void Start ()
	{
		semiTransparent = (SemiTransparent)Instantiate (semiTransparentFab, new Vector3 (0, 0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update ()
	{
		KeyEvent ();

		LoadChunk ();
		if (fpsCtrl > 600)
		{
			fpsCtrl = 0;
			SaveChunk ();
		} else
		{
			fpsCtrl++;
		}
		
		if (Screen.lockCursor == true)
			PlaceAndDestroy ();
		
		CursorCtrl ();
	}

	void KeyEvent ()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1))
		{
			blockID = 0;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2))
		{
			blockID = 1;
		}
		OnGui.showBlock(blockID);
	}

	void LoadChunk ()
	{
		Vector3 cameraPos = transform.position;
		for (float x = cameraPos.x - viewRange; x < cameraPos.x + viewRange; x += chunkWidth)
		{
			for (float z = cameraPos.z - viewRange; z < cameraPos.z + viewRange; z += chunkWidth)
			{
				int intX = Mathf.FloorToInt (x / (float)chunkWidth) * chunkWidth;
				int intZ = Mathf.FloorToInt (z / (float)chunkWidth) * chunkWidth;
				Chunk chunk = world.GetChunk (intX, 0, intZ);
				if (chunk != null)
					continue;
				world.CreateChunk (intX, 0, intZ);
			}
		}
	}

	void SaveChunk ()
	{
		List<WorldPos> saveKeys = new List<WorldPos> ();
		List<WorldPos> destroyKeys = new List<WorldPos> ();
		foreach (KeyValuePair<WorldPos, Chunk> pair in world.chunks)
		{
			Chunk chunk = pair.Value;
			foreach(Block block in chunk.blocks)
			{
				if(block.changed)
				{
					saveKeys.Add(pair.Key);
					break;
				}
			}
			if (Mathf.Abs (chunk.pos.x - transform.position.x) > viewRange + chunkWidth|| Mathf.Abs (chunk.pos.z - transform.position.z) > viewRange + chunkWidth)
			{
				destroyKeys.Add(pair.Key);
			}
		}
		foreach (WorldPos key in saveKeys)
		{
			Chunk chunk = null;
			world.chunks.TryGetValue(key, out chunk);
			Serialization.SaveChunk(chunk);
		}
		foreach (WorldPos key in destroyKeys)
		{
			world.DestroyChunk(key.x, key.y, key.z);
		}
	}

	void PlaceAndDestroy ()
	{
		RaycastHit hit;
		Ray ray = camera.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 5))
		{
			if (IsValid (hit))
			{
				semiTransparent.ShowMarquee (hit);
				if (Input.GetMouseButtonDown (0))
				{
					Terrain.SetBlock (hit, new BlockAir ());
				}
				if (Input.GetMouseButtonDown (1))
				{
					Terrain.SetBlock (hit, GetCurrentBlock(), true);
				}
			}
		} else
		{
			semiTransparent.ClearAll ();
		}
	}

	Block GetCurrentBlock ()
	{
		Block block = null;
		switch (blockID)
		{
		case 0:block = new BlockStone();
			break;
		case 1:block = new BlockGrass();
			break;
		}
		return block;
	}

	bool IsValid (RaycastHit hit)
	{
		Vector3 pos = hit.point;
		if (Mathf.Abs (pos.x - Mathf.FloorToInt (pos.x)) < 0.025f)
		{
			if (Mathf.Abs (pos.y - Mathf.FloorToInt (pos.y)) < 0.025f)
				return false;
			if (Mathf.Abs (pos.z - Mathf.FloorToInt (pos.z)) < 0.025f)
				return false;
		}
		if (Mathf.Abs (pos.y - Mathf.FloorToInt (pos.y)) < 0.025f)
		if (Mathf.Abs (pos.z - Mathf.FloorToInt (pos.z)) < 0.025f)
			return false;

		return true;
	}

	void CursorCtrl ()
	{
		if (Screen.lockCursor == false)
		if (Input.GetMouseButtonDown (0))
		{
			Screen.lockCursor = true;
		}
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			semiTransparent.ClearAll ();
			Screen.lockCursor = false;
		}
	}
}
