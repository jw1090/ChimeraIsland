namespace AI.Chimera
{
    public abstract class ChimeraBaseStates
    {
        public abstract void Enter(ChimeraStates chimeraStates);
        public abstract void Update(ChimeraStates chimeraStates);
        public abstract void Exit(ChimeraStates chimeraStates);
    }
}

