using System;
using UnityEngine;

namespace Assets.Scripts.Core.Chimera
{
    internal class ChimeraEvolutionIcon : MonoBehaviour
    {
        private Camera _camera = null;
        public void Initialize()
        {
            _camera = ServiceLocator.Get<CameraUtil>().CameraCO;
        }

        private void Update()
        {
            transform.LookAt(_camera.transform);
        }
    }
}

