using System;

namespace InputSystem
{
	public abstract class InputProfile
	{
		public event Action<ProfileType> ChangeProfileEvent;
		
		public abstract void Update();

		protected void ChangeProfile(ProfileType profileType)
		{
			ChangeProfileEvent.Invoke(profileType);
		}
	}
}