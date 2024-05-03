using InputSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class Player : MonoBehaviour
    {
        public event Action TouchPlatformEvent;
        public event Action GameOverEvent;

        [SerializeField] private Transform _downPosition;
        [SerializeField] private LayerMask _platformMask;
        [SerializeField] private float _horizontalSpeed;
        [SerializeField] private float _jumpSpeed;

        private  PlayerView _playerView;
        private Camera _camera;
        private Vector2 _startPosition;
        private bool _isMovingDown;

        public Vector2 Position => transform.position;
        public float JumpSpeed => _jumpSpeed;

        private void Awake()
        {
            _camera = Camera.main;
        }

        public void Init(InputProvider inputProvider, Vector2 startPosition)
        {
            GameProfile gameProfile = inputProvider.GetProfile(ProfileType.GameProfile) as GameProfile;
            GameOverProfile profile = inputProvider.GetProfile(ProfileType.GameOverProfile) as GameOverProfile;
            
            if (gameProfile is null || profile is null)
                throw new NullReferenceException();
            
            gameProfile.PlayerMovementEvent += HorizontalMove;
            profile.RestartEvent += Restart;

            _playerView = new PlayerView(GetComponentInChildren<SpriteRenderer>(), gameProfile);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Platform>())
            {
                CheckPlatform();
                return;
            }

            BonusItem bonusItem = other.GetComponent<BonusItem>();
            
            if (bonusItem)
            {
                if (!_isMovingDown)
                    return;

                if (Physics2D.OverlapCircle(_downPosition.position, 0.2f))
                    bonusItem.Interact();
            }
        }

        public void Move(Vector2 moveVector, float speed)
        {
            _playerView.SetIsJump(speed);
            
            _isMovingDown = speed > 0;

            Vector2 newPosition = Position + moveVector * (speed * Time.fixedDeltaTime);

            CheckEndGame(newPosition);

            transform.position = newPosition;
        }

        private void HorizontalMove(Vector2 moveVector)
        {
            if (moveVector.x == 0)
                return;

            Vector2 newPosition = Position + moveVector * (_horizontalSpeed * Time.fixedDeltaTime);

            if (TrySwapSide(newPosition))
                return;

            transform.position = newPosition;
        }

        private void CheckPlatform()
        {
            if (!_isMovingDown)
                return;

            if (Physics2D.OverlapCircle(_downPosition.position, 0.2f, _platformMask))
                TouchPlatformEvent.Invoke();
        }

        private void CheckEndGame(Vector2 newPosition)
        {
            float yCoordinateOnScreen = _camera.WorldToScreenPoint(newPosition).y;

            if (yCoordinateOnScreen < 0)
                GameOverEvent.Invoke();
        }

        private bool TrySwapSide(Vector2 newPosition)
        {
            float xCoordinateOnScreen = _camera.WorldToScreenPoint(newPosition).x;

            if (xCoordinateOnScreen >= Screen.width)
            {
                transform.position = new Vector2(_camera.ScreenToWorldPoint(new Vector3(0, 0)).x, Position.y);
                return true;
            }

            if (xCoordinateOnScreen <= 0)
            {
                transform.position =
                    new Vector2(_camera.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x, Position.y);
                return true;
            }

            return false;
        }

        private void Restart()
        {
            transform.position = _startPosition;
        }
    }
}