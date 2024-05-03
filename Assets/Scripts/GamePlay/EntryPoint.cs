using GamePlay;
using GamePlay.UI;
using InputSystem;
using System;
using System.Collections.Generic;
using TMPro;
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

	[Header("UI")]
	[SerializeField] private UIHandler _uiHandler;
	
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
		InitUI();
	}

	private void InitInputSystem()
	{
		List<InputProfile> inputProfiles = new List<InputProfile>()
		{
			new GameProfile(),
			new GameOverProfile(),
		};

		_inputProvider = new InputProvider(inputProfiles);
		_inputHandler.Init(_inputProvider);
	}
	
	private void InitPlayer()
	{
		Player playerPrefab = Resources.Load<Player>(PLAYER_PREFAB_PATH);
		if (playerPrefab is null)
			throw new NullReferenceException("Player is not found!");

		Vector3 startPosition = _playerSpawnPoint.position;
		
		_player = Instantiate(playerPrefab, startPosition, quaternion.identity);
		_player.Init(_inputProvider, startPosition);
		
		_inputHandler.InitGameOver(_player);
	}

	private void InitLevelBuilder()
	{
		_startLevelTemplate.Init(new ItemFactory(_moveHandler, _player));
		_platformsHandler = new PlatformsHandler(_levelContainer, _inputProvider);
		_levelBuilder = new LevelBuilder(_platformsHandler, _startLevelTemplate, _inputProvider);
	}
	
	private void InitMoveHandler()
	{
		_moveHandler.Init(_player, _platformsHandler, _inputProvider);
	}

	private void InitUI()
	{
		_uiHandler.Init(_moveHandler, _player, _inputProvider);
	}
}
