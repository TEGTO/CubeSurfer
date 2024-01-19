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
        private float playerInputMoveSpeed = 5f;
        [SerializeField]
        private Vector2 maxHorizontalBoundaries;

        private float currentTimeFromGameStart;
        private Vector3 playerInputMovement;
        private Vector3 playerInputMovementDirection;
        private Vector3 characterMovementDirection;

        protected float CurrentChracterSpeed { get { return Mathf.Lerp(startChracterMoveSpeed, maxChracterMoveSpeed, currentTimeFromGameStart / timeToGetMaxSpeed); } }

        private void OnEnable()
        {
            inputManager.OnTouchPerfoming += PlayerMoveInput;
            inputManager.OnTouchCanceled += PlayerMoveCancel;
        }
        private void Start()
        {
            playerInputMovementDirection = Utilities.DefineDirection(playerControlledAxis);
            characterMovementDirection = Utilities.DefineDirection(characterForwardAxis);
        }
        private void OnDisable()
        {
            inputManager.OnTouchPerfoming -= PlayerMoveInput;
        }
        private void Update()
        {
            currentTimeFromGameStart += Time.deltaTime;
            CombineMoveVectors();
        }
        private void PlayerMoveInput(TouchControl touch)
        {
            float prevTouchHorizontalPosition = touch.ReadValueFromPreviousFrame().position.x;
            float currentTouchHorizontalPosition = touch.position.ReadValue().x;
            float horizontalDirectionOfPlayerInput;
            horizontalDirectionOfPlayerInput = currentTouchHorizontalPosition.CompareTo(prevTouchHorizontalPosition);
            playerInputMovement = playerInputMovementDirection * horizontalDirectionOfPlayerInput * playerInputMoveSpeed;
        }
        private void PlayerMoveCancel(TouchControl touch)
        {
            playerInputMovement = Vector3.zero;
        }
        private void CombineMoveVectors()
        {
            Vector3 characterMovement = characterMovementDirection * CurrentChracterSpeed;
            Vector3 combinedMovement = characterMovement + playerInputMovement;
            transform.Translate(combinedMovement * Time.deltaTime, Space.World);
            transform.position = ClampedPlayerInput(transform.position);
        }
        private Vector3 ClampedPlayerInput(Vector3 movementPosition)
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
