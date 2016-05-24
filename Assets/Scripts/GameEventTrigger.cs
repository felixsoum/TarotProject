using UnityEngine;
using System.Collections.Generic;

public class GameEventTrigger : GameEventHandler
{
	public List<GameEventHandler> handlers = new List<GameEventHandler>();

	public override void OnObjectTrigger( ObjectTriggerArea area )
	{
		foreach( var handler in handlers )
		{
			handler.OnObjectTrigger(area);
		}
	}
}
