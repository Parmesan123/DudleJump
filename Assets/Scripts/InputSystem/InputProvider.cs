using System;
using System.Collections.Generic;
using System.Linq;

namespace InputSystem
{
	public class InputProvider
	{
		public readonly List<InputProfile> InputProfiles;

		public InputProvider(List<InputProfile> inputProfiles)
		{
			InputProfiles = inputProfiles;
		}

		public InputProfile GetProfile(ProfileType profileType)
		{
			InputProfile inputProfile = profileType switch
			{
				ProfileType.GameProfile => InputProfiles.First(p => p.GetType() == typeof(GameProfile)),
				ProfileType.GameOverProfile => InputProfiles.First(p => p.GetType() == typeof(GameOverProfile)),
				_ => null
			};

			return inputProfile;
		}
	}
}