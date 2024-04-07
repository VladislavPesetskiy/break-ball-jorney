namespace Game.Data
{
    public struct BallCollisionData
    {
        public int Damage { get; set; }
        public bool IsBouncing { get; set; }
        public BallCollisionType BallCollisionType { get; set; }
    }
}