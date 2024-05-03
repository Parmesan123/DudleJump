using InputSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace GamePlay
{
	public class PlatformsHandler
	{
		private const string PLATFORM_PATH = "GamePlay/Level/Platform";
		private const string MOVE_PLATFORM_PATH = "GamePlay/Level/MovePlatform";

		private readonly Pool<Platform> _platforms;
		private readonly Pool<Platform> _movePlatforms;

		private readonly List<Platform> _markedOfRemove;
		private readonly List<Platform> _markedOfAdd;
		
		public readonly List<Platform> ActivePlatforms;
			
		public PlatformsHandler(Transform platformContainer, InputProvider inputProvider)
		{
			ActivePlatforms = new List<Platform>();
			_markedOfRemove = new List<Platform>();
			_markedOfAdd = new List<Platform>();

			GameOverProfile profile = inputProvider.GetProfile(ProfileType.GameOverProfile) as GameOverProfile;
			profile.RestartEvent += Restart;
			
			Platform platformPrefab = Resources.Load<Platform>(PLATFORM_PATH);
			MovePlatform movePlatformPrefab = Resources.Load<MovePlatform>(MOVE_PLATFORM_PATH);

			_platforms = new Pool<Platform>(platformPrefab, platformContainer, true);
			_movePlatforms = new Pool<Platform>(movePlatformPrefab, platformContainer, true);
			
			_platforms.CreatePool(10);
			_movePlatforms.CreatePool(10);
		}

		public Platform GetFreePlatform(PlatformType platformType)
		{
			Platform freePlatform;
			
			switch (platformType)
			{
				case PlatformType.Platform:
					_platforms.GetFreeElement(out freePlatform);
					break;
				case PlatformType.MovePlatform:
					_movePlatforms.GetFreeElement(out freePlatform);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(platformType), platformType, null);
			}

			freePlatform.SetPlatformActive(true);
			
			freePlatform.DisablePlatformEvent += MarkOfRemove;
			_markedOfAdd.Add(freePlatform);
			
			return freePlatform;
		}

		public void RemoveMarkedPlatform()
		{
			foreach (Platform platform in _markedOfRemove)
			{
				ActivePlatforms.Remove(platform);
			}
			
			_markedOfRemove.Clear();
		}

		public void AddMarkedPlatform()
		{
			foreach (Platform platform in _markedOfAdd)
			{
				ActivePlatforms.Add(platform);
			}
			
			_markedOfAdd.Clear();
		}
		
		private void MarkOfRemove(Platform platform)
		{
			platform.DisablePlatformEvent -= MarkOfRemove;

			_markedOfRemove.Add(platform);
		}

		private void Restart()
		{
			ActivePlatforms.Clear();
			
			_platforms.DestroyPool();
			
			_platforms.CreatePool(10);
		}
	}
}