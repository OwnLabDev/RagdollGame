using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OL.Game.UI {
    public class UIManager : Manager {
        public enum UIMenuType {
            BaseMenu,
            MainMenu,
            GameMenu,
        }

        #region editor
        [Header("UIManager settings"), Space(10)]
        [SerializeField] private UIMenuType _firstMenuToLoad = UIMenuType.MainMenu;
        #endregion

        #region public properties
        public UIMenuType FirstMenuToLoad => _firstMenuToLoad;

        public UIMainMenu MainMenu => _menuDictionary[UIMenuType.MainMenu] as UIMainMenu;
        public UIGameMenu GameMenu => _menuDictionary[UIMenuType.GameMenu] as UIGameMenu;
        #endregion

        private List<UIBaseMenu> _menuList = new List<UIBaseMenu>();
        private Dictionary<UIMenuType, UIBaseMenu> _menuDictionary = new Dictionary<UIMenuType, UIBaseMenu>();

        #region private
        private List<UIMenuType> availableMenuTypes(params UIMenuType[] except) {
            List<UIMenuType> menuTypesList = Enum.GetValues(typeof(UIMenuType))
                .OfType<UIMenuType>().Except(except).ToList();

            return menuTypesList;
        }
        #endregion

        #region public
        public override void initializeVirtual() {
            base.initializeVirtual();

            List<UIMenuType> menuTypes = availableMenuTypes(except: UIMenuType.BaseMenu);

            foreach (UIBaseMenu menu in GetComponentsInChildren<UIBaseMenu>(includeInactive: true)
                .Where(menu => menuTypes.Contains(menu.MenuType)).ToList()) {

                menu.initializeAdditionalSettings();

                _menuList.Add(menu);
                _menuDictionary.Add(menu.MenuType, menu);
            }

            CurrentStatus = Status.Ready;
        }

        public void hideAllMenu(bool resetBefore = false) {
            foreach (UIBaseMenu menu in _menuList) {
                if (menu != null) {
                    if (resetBefore) {
                        menu.reset();
                    }

                    menu.hide();
                }
            }
        }

        public void resetAllMenu(bool hideAfter = false) {
            foreach (UIBaseMenu menu in _menuList) {
                if (menu != null) {
                    menu.reset();

                    if (hideAfter) {
                        menu.hide();
                    }
                }
            }
        }

        public void showMenu(UIMenuType menuType, bool hideOther = true) {
            if (_menuDictionary.ContainsKey(menuType)) {
                if (hideOther) {
                    hideAllMenu();
                }

                UIBaseMenu menu = _menuDictionary[menuType];
                if (menu != null) {
                    menu.show();
                }
            }
        }

        public void hideMenu(UIMenuType menuType, bool hideOther = true) {
            if (_menuDictionary.ContainsKey(menuType)) {
                if (hideOther) {
                    hideAllMenu();
                }

                UIBaseMenu menu = _menuDictionary[menuType];
                if (menu != null) {
                    menu.hide();
                }
            }
        }
        #endregion
    }
}