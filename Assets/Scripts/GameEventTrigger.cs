using UnityEngine;
using System.Collections.Generic;

public class GameEventTrigger : GameEventHandler
{
	public List<GameEventHandler> handlers = new List<GameEventHandler>();

	// Just redirect to every handler.
	public override void OnEvent( GameEventType type )
	{
		foreach( var handler in handlers )
		{
			handler.OnEvent(type);
		}
	}
	public override void OnEvent( GameEventType type, ObjectTriggerArea area )
	{
		foreach( var handler in handlers )
		{
			handler.OnEvent(type, area);
		}
	}
	public override void OnEvent( GameEventType type, InteractableController interactable )
	{
		foreach( var handler in handlers )
		{
			handler.OnEvent(type, interactable);
		}
	}
}
