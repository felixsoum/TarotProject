using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameScript : GameEventHandler
{
	public DialogController dialogController = null;

	// Screen overlay effect
	public Animator overlayAnimator = null;
	public Canvas overlayFadeCanvas = null;

	// Game over overlay parent canvas and text object
	public Canvas overlayGameOverCanvas = null;
	public Text overlayGameOverText = null;

	// Set the card object the subject will pick during the script scenario.
	public GameObject cardPicked = null;
	public Vector3 cardPickImpulse = Vector3.back;

	// Set the photo object the subject will push away
	public GameObject photoObject = null;
	public Vector3 photoPushImpulse = Vector3.back;

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
			GameEventType.ObjectTrigger,
			new ObjectTriggerAreaCondition<PhotographController>(ObjectTriggerArea.Type.Place),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>Player</b>: Did you kill this person...?", 2.0f));
				dialogController.Queue(new DialogLine("<b>Suspect 0</b>: No, no I swear it wasn't me!", 3.0f));
				StartCoroutine(DelayPushObject(2.0f, photoObject, photoPushImpulse));
			}
		));
		acts.Add(new GameScriptAct(
			GameEventType.ObjectTrigger,
			new ObjectTriggerAreaCondition<CardController>(ObjectTriggerArea.Type.Place, 4),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>Player</b>: Pick a card.", 1.5f));
				dialogController.Queue(new DialogLine("<b>Suspect 0</b>: I don't want to play your games.", 2.5f));
			}
		));
		acts.Add(new GameScriptAct(
			GameEventType.ObjectTrigger,
			new ObjectTriggerAreaCondition<KnifeController>(ObjectTriggerArea.Type.Hover),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>Suspect 0</b>: Okay, okay! Put the knife away!", 3.5f));
				StartCoroutine(DelayCardPick(2.0f));
			}
		));
		acts.Add(new GameScriptAct(
			GameEventType.InteractionAltUse,
			new CardPickFlippedCondition(cardPicked),
			() =>
			{
				dialogController.Queue(new DialogLine("", 0.5f));
				dialogController.Queue(new DialogLine("<b>Suspect 0</b>: What is the meaning of this?", 2.5f));
				dialogController.Queue(new DialogLine("<b>Player</b>: Death...!", 1.5f));
			}
		));
		acts.Add(new GameScriptAct(
			GameEventType.ObjectTrigger,
			new ObjectTriggerAreaCondition<KnifeController>(ObjectTriggerArea.Type.Hover),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>Suspect 0</b>: No! It is not like that!", 2.5f));
			}
		));
		acts.Add(new GameScriptAct(
			GameEventType.KnifeStab,
			new NoCondition(),
			() =>
			{
				dialogController.Queue(new DialogLine("<b>Suspect 0</b>: Please!", 0.2f));
				dialogController.Queue(new DialogLine("<b>Suspect 0</b>: Gargh!", 1.5f));
				StartCoroutine(DelayFadeGameOver(1.0f, "You have carried out the death penalty successfully".ToUpper()));
			}
		));
	}

	private bool AdvanceAct( GameEventType gameEvent, GameObject source = null )
	{
		var gameObj = source != null ? source : this.gameObject;
		var nextAct = actIndex < acts.Count ? acts[actIndex] : null;
		if( nextAct != null && nextAct.EventType == gameEvent && nextAct.Condition.Satisfied(gameObj) )
		{
			nextAct.Action();
			++actIndex;
			return true;
		}
		return false;
	}

	public override void OnEvent( GameEventType type, ObjectTriggerArea area )
	{
		AdvanceAct(type, area.gameObject);
	}
	public override void OnEvent( GameEventType type, InteractableController interactable )
	{
		AdvanceAct(type, interactable.gameObject);
	}
	public override void OnEvent( GameEventType type )
	{
		if( !AdvanceAct(type) )
		{
			// Game over when killing at the wrong script order.
			StartCoroutine(DelayFadeGameOver(1.0f, "Your cards has been revoked for killing without justification".ToUpper()));
		}
	}

	public void LoadScene( string sceneName )
	{
		StartCoroutine(DelayFadeLoadScene(0.0f, sceneName));
	}

	private void PickCard( GameObject cardObj )
	{
		PushObject(cardObj, cardPickImpulse);
	}

	private void PushObject( GameObject obj, Vector3 impulse )
	{
		var rigidbody = obj.GetComponent<Rigidbody>();
		if( rigidbody != null )
		{
			rigidbody.AddForce(impulse, ForceMode.Impulse);
		}
	}

	private IEnumerator DelayPushObject( float delay, GameObject obj, Vector3 impulse )
	{
		yield return new WaitForSeconds(delay);
		PushObject(obj, impulse);
	}

	private IEnumerator DelayCardPick( float delay )
	{
		yield return new WaitForSeconds(delay);
		PickCard(cardPicked);
	}

	private IEnumerator DelayFadeLoadScene( float delay, string sceneName )
	{
		// Ghetto fade reload
		overlayAnimator.Play("Idle");
		yield return new WaitForSeconds(delay);
		overlayFadeCanvas.sortingOrder = overlayGameOverCanvas.sortingOrder + 1;
		overlayAnimator.Play("FadeOut");
		float fadeTime = overlayAnimator.GetCurrentAnimatorStateInfo(0).length;
		yield return new WaitForSeconds(fadeTime);
		overlayGameOverCanvas.gameObject.SetActive(false);
		StartCoroutine(DelayDestroyObject(fadeTime, overlayAnimator.gameObject));
		StartCoroutine(DelayDestroyObject(fadeTime, this.gameObject));
		SceneManager.LoadScene(sceneName);
		overlayAnimator.Play("FadeIn");
	}

	private IEnumerator DelayFadeGameOver( float delay, string gameOverText )
	{
		// Ghetto fade show
		yield return new WaitForSeconds(delay);
		overlayAnimator.Play("FadeOut");
		float fadeTime = overlayAnimator.GetCurrentAnimatorStateInfo(0).length;
		yield return new WaitForSeconds(fadeTime);
		overlayGameOverCanvas.gameObject.SetActive(true);
		overlayGameOverCanvas.sortingOrder = overlayFadeCanvas.sortingOrder + 1;
		overlayGameOverText.text = gameOverText;
	}

	private IEnumerator DelayDestroyObject( float delay, GameObject gameObject )
	{
		DontDestroyOnLoad(gameObject);
		yield return new WaitForSeconds(delay);
		Destroy(gameObject);
	}
}