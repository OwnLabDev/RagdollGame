using UnityEngine;
using UnityEngine.UI;

namespace OL.Game.UI {
    public class UIGameMenu : UIBaseMenu {
        #region editor
        [Header("Game menu settings"), Space(10)]
        [SerializeField] private Button _restartButton = default;
        #endregion

        #region public properties
        #endregion

        #region private
        private void initializeComponents() {

        }

        private void onRestartButtonClicked() {
            _gameController.restartLevel();
        }
        #endregion

        #region protected
        protected override void awakeVirtual() {
            base.awakeVirtual();

            initializeComponents();
        }

        protected override void enableVirtual() {
            base.enableVirtual();

            _restartButton.onClick.AddListener(onRestartButtonClicked);
        }

        protected override void disableVirtual() {
            base.disableVirtual();

            _restartButton.onClick.RemoveListener(onRestartButtonClicked);
        }
        #endregion

        #region public
        public override void show() {
            base.show();

            _restartButton.gameObject.SetActive(true);
        }

        public override void hide() {
            base.hide();

            _restartButton.gameObject.SetActive(false);
        }

        public override void reset() {
            base.reset();            
        }
        #endregion
    }
}