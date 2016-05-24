using System;
using System.Collections.Generic;

public class GameScriptAct
{
	public GameScriptCondition Condition { get; private set; }
	public Action Action { get; private set; }

	public GameScriptAct( GameScriptCondition condition, Action action )
	{
		Condition = condition;
		Action = action;
	}
}
