using UnityEngine;

namespace _Code.GameScene
{
    public sealed class DestroyTrigger : MonoBehaviour
    {
        private const float DELAY_BEFORE_DESTROY = 1.5f;
        private Ball _ballToDestroy;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Ball ball))
                return;

            ball.StartDestroyTimer(DELAY_BEFORE_DESTROY).Forget();
            _ballToDestroy = ball;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.TryGetComponent(out Ball ball))
                return;

            _ballToDestroy.StopDestroyTimer().Forget();
            _ballToDestroy = null;
        }
    }
}