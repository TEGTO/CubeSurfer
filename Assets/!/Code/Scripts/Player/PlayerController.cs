using AxisNS;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UtilityNS;

namespace GameplayNS
{
    public enum TouchDir
    {
        NoDirection = 0, Left = -1, Right = 1
    }
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private InputManager inputManager;
        [Header("Character"), SerializeField]
        private Axis characterForwardAxis = Axis.X;
        [SerializeField]
        private float startChracterMoveSpeed = 5f;
        [SerializeField]
        private float maxChracterMoveSpeed = 15f;
        [SerializeField]
        private float timeToGetMaxSpeed = 300f;
        [Header("Player"), SerializeField]
        private Axis playerControlledAxis = Axis.X;
        [SerializeField]
        private float playerLerpSpeed = 5f;
        [SerializeField]
        private float playerInputSpeed = 2f;
        [SerializeField]
        private Vector2 maxHorizontalBoundaries;

        private float currentTimeFromGameStart;
        private Vector3 playerInputMovement;
        private Vector3 playerInputMovementDirection;
        private Vector3 characterMovementDirection;

        public float CurrentChracterSpeed { get { return Mathf.Lerp(startChracterMoveSpeed, maxChracterMoveSpeed, currentTimeFromGameStart / timeToGetMaxSpeed); } }

        private void OnEnable()
        {
            inputManager.OnTouchPerfoming += PlayerMoveInput;
        }
        private void OnDisable()
        {
            inputManager.OnTouchPerfoming -= PlayerMoveInput;
        }
        private void Start()
        {
            playerInputMovementDirection = Utilities.DefineDirection(playerControlledAxis);
            characterMovementDirection = Utilities.DefineDirection(characterForwardAxis);
        }
        private void Update()
        {
            currentTimeFromGameStart += Time.deltaTime;
            CharacterMove();
            PlayerMove();
        }
        private void CharacterMove()
        {
            Vector3 characterMovement = characterMovementDirection * CurrentChracterSpeed;
            transform.Translate(characterMovement * Time.deltaTime, Space.World);
        }
        private void PlayerMove()
        {
            Vector3 currentPosition = transform.position;
            Vector3 currentCharacterPosition = MultiplicateVectors(currentPosition, characterMovementDirection);
            Vector3 combinedVector = currentCharacterPosition + playerInputMovement;
            transform.position = Vector3.Lerp(transform.position, combinedVector, Time.deltaTime * playerLerpSpeed);
            transform.position = ClampPosition(transform.position);
        }
        private void PlayerMoveInput(TouchControl touch)
        {
            float currentTouchHorizontalPosition = touch.position.ReadValue().x;
            float touchPositionX = currentTouchHorizontalPosition / Screen.width;
            Vector2 playerInputSensivity = maxHorizontalBoundaries * playerInputSpeed;
            float movementAmount = Mathf.Lerp(playerInputSensivity.x, playerInputSensivity.y, touchPositionX);
            playerInputMovement = playerInputMovementDirection * movementAmount;
        }
        private Vector3 MultiplicateVectors(Vector3 firstVector, Vector3 secondVector)
        {
            return new Vector3(firstVector.x * secondVector.x, firstVector.y * secondVector.y,
                firstVector.z * secondVector.z);
        }
        private Vector3 ClampPosition(Vector3 movementPosition)
        {
            float positionX = movementPosition.x, positionY = movementPosition.y, positionZ = movementPosition.z;
            switch (playerControlledAxis)
            {
                case Axis.X:
                    positionX = Mathf.Clamp(positionX, maxHorizontalBoundaries.x, maxHorizontalBoundaries.y);
                    break;
                case Axis.Y:
                    positionY = Mathf.Clamp(positionY, maxHorizontalBoundaries.x, maxHorizontalBoundaries.y);
                    break;
                case Axis.Z:
                    positionZ = Mathf.Clamp(positionZ, maxHorizontalBoundaries.x, maxHorizontalBoundaries.y);
                    break;
            }
            Vector3 clampedVector = new Vector3(positionX, positionY, positionZ);
            return clampedVector;
        }
    }
}
