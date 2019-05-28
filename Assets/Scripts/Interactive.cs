using UnityEngine;
using System;

public class Interactive : MonoBehaviour
{
	[SerializeField]
	Action action;

	public bool active = true;

	public void SetAction(Action action)
	{
		this.action = action;
	}
    public void Interaction()
	{
		action();
	}
}
