using GamePlay;
using InputSystem;
using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EntryPoint : MonoBehaviour
{
	private const string PLAYER_PREFAB_PATH = "GamePlay/Player/PlayerPrefab";
	
	[SerializeField] private Transform _playerSpawnPoint;
	[SerializeField] private InputHandler _inputHandler;
	[SerializeField] private MoveHandler _moveHandler;
	[SerializeField] private Transform _levelContainer;
	[SerializeField] private BuildTemplate _startLevelTemplate;
    
	private Player _playerPrefab;
	private InputProvider _inputProvider;
	private PlatformsHandler _platformsHandler;
	private Player _player;
	private LevelBuilder _levelBuilder;
	
    private void Awake()
	{
		InitInputSystem();
		InitPlayer();
		InitLevelBuilder();
		InitMoveHandler();
	}

	private void InitInputSystem()
	{
		List<InputProfile> inputProfiles = new List<InputProfile>()
		{
			new GameProfile(),
		};

		_inputProvider = new InputProvider(inputProfiles);
		_inputHandler.Init(_inputProvider);
	}
	
	private void InitPlayer()
	{
		_playerPrefab = Resources.Load<Player>(PLAYER_PREFAB_PATH);
		if (_playerPrefab is null)
			throw new NullReferenceException("Player is not found!");

		Vector3 startPosition = _playerSpawnPoint.position;
		
		_player = Instantiate(_playerPrefab, startPosition, quaternion.identity);
		_player.Init(_inputProvider, startPosition);
	}

	private void InitLevelBuilder()
	{
		_startLevelTemplate.Init(new ItemFactory(_moveHandler, _player));
		_platformsHandler = new PlatformsHandler(_levelContainer);
		_levelBuilder = new LevelBuilder(_platformsHandler, _startLevelTemplate);
	}
	
	private void InitMoveHandler()
	{
		_moveHandler.Init(_player, _platformsHandler);
	}
}
