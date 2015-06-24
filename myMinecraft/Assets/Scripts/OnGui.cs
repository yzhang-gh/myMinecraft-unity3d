using UnityEngine;
using System.Collections;

public class OnGui : MonoBehaviour
{

	public Texture crosshair;

	void OnGUI ()
	{
		GUI.DrawTexture (new Rect (Input.mousePosition.x - 16, Screen.height - Input.mousePosition.y - 16, 32, 32), crosshair);
	}

	public static void showBlock (int id)
	{
		string blockName = "null";
		switch (id)
		{
		case 0:blockName = new BlockStone().GetName();
			break;
		case 1:blockName = new BlockGrass().GetName();
			break;
		}
		GUI.Label (new Rect(100, 100, 100, 40), "currentBlock: " + blockName);
	}
}
