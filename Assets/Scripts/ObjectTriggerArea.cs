using UnityEngine;
using System.Collections.Generic;

public class ObjectTriggerArea : MonoBehaviour
{
	// Type of defined area
	public enum Type
	{
		Place,
		Hover,
	}

	// Specify the event handler to notify
	public GameEventHandler gameEventHandler = null;

	// Specify which area type the attached collider trigger defines.
	public Type type = Type.Place;

	// Track objects currently inside the trigger area.
	public IList<GameObject> Objects { get { return objects.AsReadOnly(); } }
	private List<GameObject> objects = new List<GameObject>();

	void OnTriggerEnter( Collider other )
	{
		objects.Add(other.gameObject);
		gameEventHandler.OnObjectTrigger(this);
	}

	void OnTriggerExit( Collider other )
	{
		objects.Remove(other.gameObject);
	}
}
