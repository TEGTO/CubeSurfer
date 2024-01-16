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
        [SerializeField]
        private Axis characterForwardAxis = Axis.X;
        [SerializeField]
        private Axis playerControlledAxis = Axis.X;
        [SerializeField]
        private float chracterMoveSpeed = 5f;
        [SerializeField]
        private float playerInputMoveSpeed = 5f;
        [SerializeField]
        private Vector2 characterHorizontalMaxBoundaries;

        private Vector3 playerInputMovement;
        private Vector3 playerInputMovementDirection;
        private Vector3 characterMovementDirection;

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
            Vector3 characterMovement = characterMovementDirection * chracterMoveSpeed;
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
                    positionX = Mathf.Clamp(positionX, characterHorizontalMaxBoundaries.x, characterHorizontalMaxBoundaries.y);
                    break;
                case Axis.Y:
                    positionY = Mathf.Clamp(positionY, characterHorizontalMaxBoundaries.x, characterHorizontalMaxBoundaries.y);
                    break;
                case Axis.Z:
                    positionZ = Mathf.Clamp(positionZ, characterHorizontalMaxBoundaries.x, characterHorizontalMaxBoundaries.y);
                    break;
            }
            Vector3 clampedVector = new Vector3(positionX, positionY, positionZ);
            return clampedVector;
        }
    }
}
