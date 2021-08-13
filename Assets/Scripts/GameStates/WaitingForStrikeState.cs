using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class WaitingForStrikeState : AbstractGameObjectState
    {
        private GameObject cue;
        private GameObject cueBall;
        private GameObject mainCamera;

        private PoolGameController gameController;

        public WaitingForStrikeState(MonoBehaviour parent) : base(parent)
        {
            gameController = (PoolGameController)parent;
            cue = gameController.cue;
            cueBall = gameController.cueBall;
            mainCamera = gameController.mainCamera;

            cue.GetComponent<Renderer>().enabled = true;
        }

        public override void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        gameController.pressPosX = Input.mousePosition.x;
                        break;

                    case TouchPhase.Moved:
                        var x = (Input.touches[0].position.x - gameController.pressPosX) / 2;
                        if (x != 0)
                        {
                            var angle = x * Time.deltaTime;
                            gameController.strikeDirection = Quaternion.AngleAxis(angle, Vector3.up) * gameController.strikeDirection;
                            mainCamera.transform.RotateAround(cueBall.transform.position, Vector3.up, angle);
                            cue.transform.RotateAround(cueBall.transform.position, Vector3.up, angle);
                        }
                        break;

                    case TouchPhase.Stationary:
                        gameController.currentState = new GameStates.StrikingState(gameController);
                        break;
                }
            }

            Debug.DrawLine(cueBall.transform.position, cueBall.transform.position + gameController.strikeDirection * 10);

        }
    }
}