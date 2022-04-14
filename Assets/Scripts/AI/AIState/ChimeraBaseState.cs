namespace AI.Chimera
{
    public abstract class ChimeraBaseState
    {
        public abstract void Enter(ChimeraBehavior chimeraBehavior);
        public abstract void Update();
        public abstract void Exit();
    }
}