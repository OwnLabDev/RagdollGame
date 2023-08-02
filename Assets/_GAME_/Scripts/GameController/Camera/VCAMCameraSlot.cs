using UnityEngine;

using VCAM = Cinemachine.CinemachineVirtualCamera;

namespace OL.Game {
    [RequireComponent(typeof(VCAM))]
    public class VCAMCameraSlot : MonoBehaviour {
        #region editor
        [SerializeField] private CameraManager.VCAMType _vcamType = CameraManager.VCAMType.DefaultView;

        [Header("Perlin settings"), Space(10)]
        [SerializeField] private bool _resetPerlin = true;
        #endregion

        #region public events
        #endregion

        #region public properties
        public VCAM VCAM => _vcam;
        public bool ResetPerlin => _resetPerlin;
        public CameraManager.VCAMType VCAMType => _vcamType;
        #endregion

        private VCAM _vcam = default;

        #region private
        private void Awake() {
            initializeComponents();
        }

        private void OnValidate() {
            name = $"{_vcamType}CameraSlot";
        }

        private void initializeComponents() {
            _vcam = GetComponent<VCAM>();
        }
        #endregion

        #region public
        #endregion
    }
}
