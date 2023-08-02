using UnityEngine;

namespace OL.Game.UI {
    public class UISwipeHint : MonoBehaviour, IUIElement {
        #region editor
        #endregion

        #region private
        private void Awake() {
            initializeComponents();
        }

        private void initializeComponents() {

        }
        #endregion

        #region public
        #endregion

        #region IUIElement
        public void show() {
            gameObject.SetActive(true);
        }

        public void hide() {
            gameObject.SetActive(false);
        }

        public void reset() {
            
        }
        #endregion
    }
}
