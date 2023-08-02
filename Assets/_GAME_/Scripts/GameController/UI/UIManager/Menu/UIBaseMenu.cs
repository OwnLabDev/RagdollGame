using OL.Kit.Components;
using System.Collections.Generic;
using UnityEngine;

namespace OL.Game.UI {
    public abstract class UIBaseMenu : OLMonoBehaviour, IUIElement {
        #region editor
        [SerializeField] private UIManager.UIMenuType _menuType = UIManager.UIMenuType.BaseMenu;
        #endregion

        #region public properties
        public bool IsVisible => _isVisible;
        public UIManager.UIMenuType MenuType => _menuType;
        #endregion

        private bool _isVisible = false;

        private List<IUIElement> _uiElements = default;

        protected UIManager _uiManager = default;
        protected GameController _gameController = default;

        #region private
        private void initializeComponents() {

        }
        #endregion

        #region protected
        protected void addUIElement(IUIElement uiElement) {
            if (uiElement == null) {
                return;
            }

            _uiElements.Add(uiElement);
        }
        #endregion

        #region protected virtual
        protected override void awakeVirtual() {
            initializeComponents();
        }
        #endregion

        #region public
        public virtual void initializeAdditionalSettings() {
            _uiElements = new List<IUIElement>();

            _uiManager = GetComponentInParent<UIManager>();
            _gameController = GetComponentInParent<GameController>();
        }
        #endregion

        #region IUIElement
        public virtual void show() {
            gameObject.SetActive(true);

            foreach (IUIElement uiElement in _uiElements) {
                uiElement.show();
            }

            _isVisible = true;
        }

        public virtual void hide() {
            gameObject.SetActive(false);

            foreach (IUIElement uiElement in _uiElements) {
                uiElement.hide();
            }

            _isVisible = false;
        }

        public virtual void reset() {
            foreach (IUIElement uiElement in _uiElements) {
                uiElement.reset();
            }
        }
        #endregion
    }
}
