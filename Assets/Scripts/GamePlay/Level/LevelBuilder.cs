using GamePlay;
using System.Collections.Generic;
using UnityEngine;


public class LevelBuilder
{
	private readonly PlatformsHandler _platformHandler;

	private readonly BuildTemplate _currentTemplate;

	private readonly List<Platform> _lastTemplatesPlatform;
	private Platform _lastPlatform;

	public LevelBuilder(PlatformsHandler platformsHandler, BuildTemplate startTemplate)
	{
		_platformHandler = platformsHandler;
		_currentTemplate = startTemplate;

		_lastTemplatesPlatform = new List<Platform>();
		
		_currentTemplate.SetStartValue(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0)));
		
		BuildLevel();
	}
	
	private void BuildLevel()
	{	for(; _lastTemplatesPlatform.Count < 3;)
		{
			for (; _currentTemplate.CurrentPlatformAmount >= 0;)
			{
				Platform platform = _platformHandler.GetFreePlatform(_currentTemplate.GetRandomPlatformType());
				
				if (_currentTemplate.TrySetPlatform(platform))
				{
					_lastPlatform = platform;
					continue;
				}

				_lastTemplatesPlatform.Add(_lastPlatform);

				_lastPlatform.DisablePlatformEvent += TemplateIsOver;
				
				break;
			}
			
			_currentTemplate.SetStartValue(_lastPlatform.Position);
		}
	}

	private void TemplateIsOver(Platform platform)
	{
		platform.DisablePlatformEvent -= TemplateIsOver;
		_lastTemplatesPlatform.Remove(platform);
		
		_currentTemplate.SetStartValue(_lastPlatform.Position);
		
		BuildLevel();
	}
}
