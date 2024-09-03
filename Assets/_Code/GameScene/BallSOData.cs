using UnityEngine;

namespace _Code.GameScene
{
    [CreateAssetMenu(menuName = "BallType")]
    public sealed class BallSOData : ScriptableObject
    {
        [field: SerializeField] public EBallType Type { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [field: SerializeField] public int Score { get; private set; }
    }
}