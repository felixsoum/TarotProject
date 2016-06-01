using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class GameScriptCondition
{
	// Checks if condition is satisfied from game object.
	public abstract bool Satisfied( GameObject gameObject );
}

// Can probably move each definition to its own file but lazy now.
//---------------------------------------------------------------------------------

// Checks if the area type contains target count time target type object component.
public class ObjectTriggerAreaCondition<TargetType> : GameScriptCondition
{
	public ObjectTriggerArea.Type AreaType { get; private set; }
	public int TargetCount { get; private set; }

	public ObjectTriggerAreaCondition( ObjectTriggerArea.Type areaType, int targetCount = 1 )
	{
		AreaType = areaType;
		TargetCount = targetCount;
	}

	public override bool Satisfied( GameObject gameObject )
	{
		bool satisfied = false;
		var triggerArea = gameObject.GetComponent<ObjectTriggerArea>();
		if( triggerArea != null && triggerArea.type == AreaType )
		{
			int count = 0;
			foreach( var obj in triggerArea.Objects )
			{
				count += obj.GetComponents<TargetType>().Length;
			}
			satisfied = count >= TargetCount;
		}
		return satisfied;
	}
}

// Checks if the card flipped is the picked card
public class CardPickFlippedCondition : GameScriptCondition
{
	public GameObject CardPicked { get; private set; }

	public CardPickFlippedCondition( GameObject cardPicked )
	{
		CardPicked = cardPicked;
	}

	public override bool Satisfied( GameObject gameObject )
	{
		return gameObject == CardPicked;
	}
}

public class InteractionAltUseCondition<InteractableType> : GameScriptCondition
{
	public override bool Satisfied( GameObject gameObject )
	{
		return gameObject.GetComponent<InteractableType>() != null;
	}
}

public class NoCondition : GameScriptCondition
{
	public override bool Satisfied( GameObject gameObject )
	{
		return true;
	}
}