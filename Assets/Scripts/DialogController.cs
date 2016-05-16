using UnityEngine;
using System.Collections.Generic;

// Defines a single dialog line to play.
public struct DialogLine
{
	// Text string that supports rich text tags like <b>bold</b>
	public string RichText { get; set; }
	// Duration of text display in seconds.
	public float Duration { get; set; }

	public DialogLine( string richText, float duration )
	{
		RichText = richText;
		Duration = duration;
	}
}

// Controls dialog displaying.
public class DialogController : MonoBehaviour
{
	// Specify which text UI object to control
	public UnityEngine.UI.Text textObject = null;

	// Track dialogs to display.
	private List<DialogLine> dialogs = new List<DialogLine>();

	// Track when is the next dialog to be displayed.
	private float timeNext = 0.0f;

	// Set a dialog line to be played next in queue.
	public void Queue( DialogLine line )
	{
		dialogs.Add(line);
	}

	void Update()
	{
		// When a dialog is playing
		if( timeNext > 0.0f || dialogs.Count > 0 )
		{
			timeNext -= Time.deltaTime;
			if( timeNext <= 0.0f )
			{
				ClearText();
				StartNext();
			}
		}
	}

	private void StartNext()
	{
		if( dialogs.Count > 0 && textObject != null )
		{
			var dialog = dialogs[0];
			dialogs.RemoveAt(0);

			textObject.text = dialog.RichText;
			timeNext = dialog.Duration;
		}
	}

	private void ClearText()
	{
		if( textObject != null )
		{
			textObject.text = string.Empty;
		}
	}
}
