using UnityEngine;
using System.Collections;

namespace GameStates
{
    public class StrikingState : AbstractGameObjectState
    {
        private PoolGameController gameController;

        private GameObject cue;
        private GameObject cueBall;

        private float cueDirection = -1;
        private float speed = 7;

        public StrikingState(MonoBehaviour parent) : base(parent)
        {
            gameController = (PoolGameController)parent;
            cue = gameController.cue;
            cueBall = gameController.cueBall;
        }

        public override void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.touches[0];
                switch (touch.phase)
                {
                    case TouchPhase.Moved:
                    case TouchPhase.Canceled:
                        // if still move then waiting for next turn
                        gameController.currentState = new GameStates.WaitingForStrikeState(gameController);
                        break;

                    case TouchPhase.Ended:
                        // then strike
                        gameController.currentState = new GameStates.StrikeState(gameController);
                        break;
                }
            }

        }

        public override void FixedUpdate()
        {
            var distance = Vector3.Distance(cue.transform.position, cueBall.transform.position);
            if (distance < PoolGameController.MIN_DISTANCE || distance > PoolGameController.MAX_DISTANCE)
                cueDirection *= -1;
            cue.transform.Translate(Vector3.down * speed * cueDirection * Time.fixedDeltaTime);
        }
    }
}