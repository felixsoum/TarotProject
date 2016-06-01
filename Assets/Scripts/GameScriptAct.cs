using System;
using System.Collections.Generic;

public class GameScriptAct
{
	public GameEventType EventType { get; private set; }
	public GameScriptCondition Condition { get; private set; }
	public Action Action { get; private set; }

	public GameScriptAct( GameEventType eventType, GameScriptCondition condition, Action action )
	{
		EventType = eventType;
		Condition = condition;
		Action = action;
	}
}
