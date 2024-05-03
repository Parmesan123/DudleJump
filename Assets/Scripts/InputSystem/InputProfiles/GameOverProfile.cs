using System;
using UnityEngine;

namespace InputSystem
{
	public class GameOverProfile: InputProfile
	{
		public event Action RestartEvent;
		
		public override void Update()
		{
			Restart();
		}

		private void Restart()
		{
			if (!Input.GetKey(KeyCode.Space))
				return;
			
			RestartEvent.Invoke();
			
			ChangeProfile(ProfileType.GameProfile);
		}
	}
}