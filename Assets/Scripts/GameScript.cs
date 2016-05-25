using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameScript : GameEventHandler
{
	public DialogController dialogController = null;

	// Set the card object the subject will pick during the script scenario.
	public GameObject cardPicked = null;
	public Vector3 cardPickImpulse = Vector3.back;

	private List<GameScriptAct> acts = new List<GameScriptAct>();
	private int actIndex = 0;
	
	void Start()
	{
		SetupActs();
	}

	private void SetupActs()
	{
		acts = new List<GameScriptAct>();
		acts.Add(new GameScriptAct(
			new ObjectTriggerAreaCondition<KnifeController>(ObjectTriggerArea.Type.Place),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>T</b>: Did you kill this person...?", 2.2f));
				dialogController.Queue(new DialogLine("<b>S</b>: No, no I swear it wasn't me!", 3.0f));
			}
		));
		acts.Add(new GameScriptAct(
			new ObjectTriggerAreaCondition<CardController>(ObjectTriggerArea.Type.Place, 4),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>T</b>: Pick a card.", 1.5f));
				dialogController.Queue(new DialogLine("<b>S</b>: I don't want to play your games.", 3.8f));
			}
		));
		acts.Add(new GameScriptAct(
			new ObjectTriggerAreaCondition<KnifeController>(ObjectTriggerArea.Type.Hover),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>S</b>: Okay, okay! Put the knife away!", 3.8f));
				StartCoroutine(DelayCardPick(2.0f));
			}
		));
	}

	private void AdvanceAct( GameObject source = null )
	{
		var gameObj = source != null ? source : this.gameObject;
		var nextAct = actIndex < acts.Count ? acts[actIndex] : null;
		if( nextAct != null && nextAct.Condition.Satisfied(gameObj) )
		{
			nextAct.Action();
			++actIndex;
		}
	}

	public override void OnObjectTrigger( ObjectTriggerArea area )
	{
		AdvanceAct(area.gameObject);
	}

	private IEnumerator DelayCardPick( float delay )
	{
		yield return new WaitForSeconds(delay);
		PickCard(cardPicked);
	}

	private void PickCard( GameObject cardObj )
	{
		var rigidbody = cardObj.GetComponent<Rigidbody>();
		if( rigidbody != null )
		{
			rigidbody.AddForce(cardPickImpulse, ForceMode.Impulse);
		}
	}
}
