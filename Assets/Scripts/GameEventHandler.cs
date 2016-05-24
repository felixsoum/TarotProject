using UnityEngine;
using System.Collections.Generic;

public abstract class GameEventHandler : MonoBehaviour
{
	// When an object enters a trigger area
	public virtual void OnObjectTrigger( ObjectTriggerArea area )
	{
	}

}
