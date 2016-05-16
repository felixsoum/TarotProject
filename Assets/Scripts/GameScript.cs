using UnityEngine;
using System.Collections;

public class GameScript : MonoBehaviour
{
	public DialogController dialogController = null;

	// Use this for initialization
	void Start()
	{
		// Testing some dialogs
		dialogController.Queue(new DialogLine("", 3.0f));
		dialogController.Queue(new DialogLine("<b>System</b>: Testing dialog display...", 2.0f));
		dialogController.Queue(new DialogLine("<b>System</b>: Dialog test 1", 1.0f));
		dialogController.Queue(new DialogLine("<b>System</b>: Dialog test 2", 1.0f));
		dialogController.Queue(new DialogLine("<b>System</b>: Dialog test end", 1.5f));
	}
}
