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
                        gameController.touchPosition = touch.position;
                        break;
                    case TouchPhase.Moved:
                        var x = touch.position.x - gameController.touchPosition.x;
                        var y = touch.position.y - gameController.touchPosition.y;
                        if (y < -100)
                        {
                            Debug.DrawLine(cueBall.transform.position, cueBall.transform.position + gameController.strikeDirection * 10);
                            gameController.touchPosition = touch.position;
                            gameController.currentState = new GameStates.StrikingState(gameController);
                        }
                        else if (x != 0)
                        {
                            var angle = x * 0.5f * Time.deltaTime;
                            gameController.strikeDirection = Quaternion.AngleAxis(angle, Vector3.up) * gameController.strikeDirection;
                            mainCamera.transform.RotateAround(cueBall.transform.position, Vector3.up, angle);
                            cue.transform.RotateAround(cueBall.transform.position, Vector3.up, angle);
                        }

                        break;
                }
            }

        }
    }
}