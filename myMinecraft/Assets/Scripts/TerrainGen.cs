using UnityEngine;
using System.Collections;
using SimplexNoise;

public class TerrainGen
{
	/*float stoneBaseHeight = -24;
	float stoneBaseNoise = 0.05f;
	float stoneBaseNoiseHeight = 4;
	float stoneMountainHeight = 48;
	float stoneMountainFrequency = 0.008f;
	float stoneMinHeight = -12;
	float dirtBaseHeight = 1;
	float dirtNoise = 0.04f;
	float dirtNoiseHeight = 3;*/

	public Chunk ChunkGen (Chunk chunk)
	{
		for (int x = chunk.pos.x; x < chunk.pos.x + Chunk.chunkWidth; x++)
		{
			for (int z = chunk.pos.z; z < chunk.pos.z + Chunk.chunkWidth; z++)
			{
				chunk = ChunkColumnGen (chunk, x, z);
			}
		}
		return chunk;
	}

	public Chunk ChunkColumnGen (Chunk chunk, int x, int z)
	{
		/*int stoneHeight = Mathf.FloorToInt (stoneBaseHeight);
		stoneHeight += GetNoise (x, 0, z, stoneMountainFrequency, Mathf.FloorToInt (stoneMountainHeight));

		if (stoneHeight < stoneMinHeight)
			stoneHeight = Mathf.FloorToInt (stoneMinHeight);

		stoneHeight += GetNoise (x, 0, z, stoneBaseNoise, Mathf.FloorToInt (stoneBaseNoiseHeight));

		int dirtHeight = stoneHeight + Mathf.FloorToInt (dirtBaseHeight);
		dirtHeight += GetNoise (x, 100, z, dirtNoise, Mathf.FloorToInt (dirtNoiseHeight));

		for (int y = 0; y < Chunk.chunkHeight; y++)
		{
			if (y <= stoneHeight)
			{
				chunk.SetBlock (x - chunk.pos.x, y, z - chunk.pos.z, new Block ());
			} else if (y <= dirtHeight)
			{
				chunk.SetBlock (x - chunk.pos.x, y, z - chunk.pos.z, new BlockGrass ());
			} else
			{
				chunk.SetBlock (x - chunk.pos.x, y, z - chunk.pos.z, new BlockAir ());
			}
		}*/

		//*********************
		Random.seed = chunk.world.seed;
		Vector3 offSet0 = new Vector3 (Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 offSet1 = new Vector3 (Random.value * 10000, Random.value * 10000, Random.value * 10000);
		Vector3 offSet2 = new Vector3 (Random.value * 10000, Random.value * 10000, Random.value * 10000);

		for (int y = 0; y < Chunk.chunkHeight; y++)
		{
			WorldPos pos = new WorldPos (x, y, z);
					
			float noiseValue = CalculateNoiseValue (pos, offSet0, 0.03f);
			noiseValue += CalculateNoiseValue (pos, offSet1, 0.02f);
			noiseValue += CalculateNoiseValue (pos, offSet2, 0.005f);
			noiseValue += (20 - y) / 10f;
			if (noiseValue < 0.08f)
			{
				chunk.SetBlock (x - chunk.pos.x, y, z - chunk.pos.z, new BlockAir ());
			} else if (noiseValue < 0.3f)
			{
				chunk.SetBlock (x - chunk.pos.x, y, z - chunk.pos.z, new BlockGrass ());
			} else
			{
				chunk.SetBlock (x - chunk.pos.x, y, z - chunk.pos.z, new BlockStone ());
			}
		}
		
		return chunk;
	}
	
	private float CalculateNoiseValue (WorldPos pos, Vector3 offSet, float scale)
	{
		float noiseX = Mathf.Abs ((pos.x + offSet.x) * scale);
		float noiseY = Mathf.Abs ((pos.y + offSet.y) * scale);
		float noiseZ = Mathf.Abs ((pos.z + offSet.z) * scale);
		
		return SimplexNoise.Noise.Generate (noiseX, noiseY, noiseZ);
	}

	/*public static int GetNoise (int x, int y, int z, float scale, int max)
	{
		return Mathf.FloorToInt ((Noise.Generate (x * scale, y * scale, z * scale) + 1f) * (max / 2f));
	}*/
}