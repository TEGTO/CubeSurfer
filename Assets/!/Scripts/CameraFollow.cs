using AxisNS;
using DG.Tweening;
using UnityEngine;
namespace CameraNS
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Axis followAxis;
        [SerializeField]
        private Transform target;
        [SerializeField]
        private float smoothSpeed = 0.125f;

        private Vector3 startPosition;
        private Vector3 tartgetStartPosition;
        private Vector3 direction;

        private void Start()
        {
            startPosition = transform.position;
            tartgetStartPosition = target.position;
            direction = UtilityNS.Utilities.DefineDirection(followAxis);
        }
        private void LateUpdate()
        {
            if (target)
                MoveCamera();
        }
        private void MoveCamera()
        {
            Vector3 targetPosition = target.position;
            Vector3 targetPositionOfFollowAxis = new Vector3(
                direction.x * targetPosition.x,
                direction.y * targetPosition.y,
                direction.z * targetPosition.z
            );
            Vector3 desiredPosition = targetPositionOfFollowAxis - tartgetStartPosition + startPosition;
            transform.DOMove(desiredPosition, smoothSpeed);
        }
    }
}