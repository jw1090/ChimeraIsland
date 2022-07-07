namespace AI.Behavior
{
    public class TrainingState : ChimeraBaseState
    {
        private ChimeraBehavior _chimeraBehavior = null;
        private string _trainingAnim = "Idle";

        public override void Enter(ChimeraBehavior chimeraBehavior)
        {
            _chimeraBehavior = chimeraBehavior;
            _chimeraBehavior.BoxCollider.enabled = false;
            _chimeraBehavior.Agent.enabled = false;

            _chimeraBehavior.EnterAnim(_trainingAnim);
        }

        public override void Update()
        {

        }

        public override void Exit()
        {
            _chimeraBehavior.BoxCollider.enabled = true;
            _chimeraBehavior.Agent.enabled = true;

            _chimeraBehavior.ExitAnim(_trainingAnim);
        }
    }
}