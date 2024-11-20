using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
    {
        [SerializeField] GameObject player;

        [SerializeField] float timeOffset;

        [SerializeField] Vector3 positionOffset;

        [SerializeField] float leftLimit;
        [SerializeField] float rightLimit;
        [SerializeField] float upLimit;
        [SerializeField] float downLimit;

        internal bool followPlayer = true;
        private void Awake()
        {
            transform.position = player.transform.position + positionOffset;
            transform.position = new Vector3
                (
                    Mathf.Clamp(transform.position.x, leftLimit, rightLimit),
                    Mathf.Clamp(transform.position.y, downLimit, upLimit),
                    transform.position.z
                );
        }
        private void FixedUpdate()
        {
            if (followPlayer)
            {
                Vector3 startPos = transform.position;
                Vector3 endPos = player.transform.position;
                endPos += positionOffset;

                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, positionOffset.z);
            }
        }

        private void OnDrawGizmos()
        {
            float height = Camera.main.orthographicSize * 2f;
            float width = height * Camera.main.aspect;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector2(leftLimit - (width / 2), upLimit + (height / 2)), new Vector2(rightLimit + (width / 2), upLimit + (height / 2)));
            Gizmos.DrawLine(new Vector2(rightLimit + (width / 2), upLimit + (height / 2)), new Vector2(rightLimit + (width / 2), downLimit - (height / 2)));
            Gizmos.DrawLine(new Vector2(rightLimit + (width / 2), downLimit - (height / 2)), new Vector2(leftLimit - (width / 2), downLimit - (height / 2)));
            Gizmos.DrawLine(new Vector2(leftLimit - (width / 2), downLimit - (height / 2)), new Vector2(leftLimit - (width / 2), upLimit + (height / 2)));
        }

        void EnableFollowPlayer()
        {
            followPlayer = true;
        }

        void DisableFollowPlayer()
        {
            followPlayer = false;
        }
}
