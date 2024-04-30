using Properties;
using System;
using UnityEngine;

public class Platform : MonoBehaviour, IDestroyed
{
	public event Action<Platform> DisablePlatformEvent;
	
	public Vector2 Position => transform.position;
	
	public void Move(Vector2 moveVector, float moveSpeed)
	{
		transform.position = Position + moveVector * (Time.fixedDeltaTime * moveSpeed);

		if (Camera.main.WorldToScreenPoint(Position).y <= 0)
		{
			SetPlatformActive(false);
		}
	}

	public virtual void SetPlatformActive(bool value)
	{
		gameObject.SetActive(value);
		
		if (!value)
			DisablePlatformEvent.Invoke(this);
	}
	
	public void Destroy()
	{
		Destroy(gameObject);
	}
}
