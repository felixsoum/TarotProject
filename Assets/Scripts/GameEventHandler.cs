using UnityEngine;
using System.Collections.Generic;

public enum GameEventType
{
	ObjectTrigger,
	InteractionAltUse,
	KnifeStab,
}

public abstract class GameEventHandler : MonoBehaviour
{

	// Handle event type with various parameters
	public virtual void OnEvent( GameEventType type )
	{
	}
	public virtual void OnEvent( GameEventType type, ObjectTriggerArea area )
	{
	}
	public virtual void OnEvent( GameEventType type, InteractableController interactable )
	{
	}

}
