using UnityEngine;
using UnityEngine.UI;

namespace OL.Game.UI {
    public class UIMainMenu : UIBaseMenu {
        #region editor
        [Header("Main menu settings"), Space(10)]
        [SerializeField] private UISwipeHint _uiSwipeHint = default;
        #endregion

        #region public properties
        public UISwipeHint UISwipeHint => _uiSwipeHint;
        #endregion

        #region private
        #endregion

        #region protected
        #endregion

        #region public
        public override void show() {
            base.show();

            _uiSwipeHint.show();
        }

        public override void hide() {
            base.hide();

            _uiSwipeHint.hide();
        }

        public override void reset() {
            base.reset();

            _uiSwipeHint.reset();
        }
        #endregion
    }
}