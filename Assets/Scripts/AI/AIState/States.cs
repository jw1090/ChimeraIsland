namespace AI.Chimera
{
    public abstract class ChimeraBaseStates
    {
        public abstract void Enter(ChimeraBehaviors chimeraBehaviors);
        public abstract void Update(ChimeraBehaviors chimeraBehaviors);
        public abstract void Exit(ChimeraBehaviors chimeraBehaviors);
    }
}

