using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameScript : GameEventHandler
{
	public DialogController dialogController = null;

	// Screen overlay effect
	public Animator overlayAnimator = null;

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
				dialogController.Queue(new DialogLine("<b>T</b>: Did you kill this person...?", 2.0f));
				dialogController.Queue(new DialogLine("<b>S</b>: No, no I swear it wasn't me!", 3.0f));
			}
		));
		acts.Add(new GameScriptAct(
			new ObjectTriggerAreaCondition<CardController>(ObjectTriggerArea.Type.Place, 4),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>T</b>: Pick a card.", 1.5f));
				dialogController.Queue(new DialogLine("<b>S</b>: I don't want to play your games.", 2.5f));
			}
		));
		acts.Add(new GameScriptAct(
			new ObjectTriggerAreaCondition<KnifeController>(ObjectTriggerArea.Type.Hover),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>S</b>: Okay, okay! Put the knife away!", 3.5f));
				StartCoroutine(DelayCardPick(2.0f));
			}
		));
		acts.Add(new GameScriptAct(
			new CardPickFlippedCondition(cardPicked),
			() =>
			{
				dialogController.Queue(new DialogLine("", 0.5f));
				dialogController.Queue(new DialogLine("<b>S</b>: What is the meaning of this?", 2.5f));
				dialogController.Queue(new DialogLine("<b>T</b>: Death...!", 1.5f));
			}
		));
		acts.Add(new GameScriptAct(
			new ObjectTriggerAreaCondition<KnifeController>(ObjectTriggerArea.Type.Hover),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>S</b>: No! It is not like that!", 2.5f));
			}
		));
		acts.Add(new GameScriptAct(
			new InteractionAltUseCondition<KnifeController>(),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>S</b>: Please!", 0.2f));
				dialogController.Queue(new DialogLine("<b>S</b>: Gargh!", 1.5f));
				StartCoroutine(DelayFadeReload(1.0f));
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
	public override void OnInteractionAltUse( InteractableController interactable )
	{
		AdvanceAct(interactable.gameObject);
	}

	private void PickCard( GameObject cardObj )
	{
		var rigidbody = cardObj.GetComponent<Rigidbody>();
		if( rigidbody != null )
		{
			rigidbody.AddForce(cardPickImpulse, ForceMode.Impulse);
		}
	}

	private IEnumerator DelayCardPick( float delay )
	{
		yield return new WaitForSeconds(delay);
		PickCard(cardPicked);
	}

	private IEnumerator DelayFadeReload( float delay )
	{
		// Ghetto fade reload
		yield return new WaitForSeconds(delay);
		overlayAnimator.Play("FadeOut");
		yield return new WaitForSeconds(overlayAnimator.GetCurrentAnimatorStateInfo(0).length);
		DontDestroyOnLoad(overlayAnimator.gameObject);
		DontDestroyOnLoad(this.gameObject);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		overlayAnimator.Play("FadeIn");
		yield return new WaitForSeconds(overlayAnimator.GetCurrentAnimatorStateInfo(0).length);
		Destroy(overlayAnimator.gameObject);
		Destroy(this.gameObject);
	}
}
