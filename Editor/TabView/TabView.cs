using System;
using System.Collections.Generic;
using System.Linq;
using UIX.Common;
using UnityEngine;
using UnityEngine.UIElements;



namespace UIX
{
    public class TabView : VisualElement
    {
        #region UI Elements

        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UIX_TabView");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UIX_TabViewStyle");
        
        private VisualElement HeaderBar => this.Q("uix-tabview__header-bar");
        private VisualElement Content => this.Q("uix-tabview__content");

        public new class UxmlFactory : UxmlFactory<TabView>
        {
        }

        #endregion

        #region Fields

        private readonly List<Tab> _tabs = new List<Tab>();
        private int _selectedTabNr;

        #endregion

        #region Properties

        /// <summary>
        /// Should this element's size fit the parent's size.
        /// </summary>
        public bool FitToParent
        {
            get => GetClasses().Contains(UixResources.ExpandClassName);
            set
            {
                if (value) AddToClassList(UixResources.ExpandClassName);
                else RemoveFromClassList(UixResources.ExpandClassName);
            }
        }

        /// <summary>
        /// Should this tab view be allowed to remove all tabs.
        /// If set to false, the last tab will not be allowed to be removed.
        /// </summary>
        public bool AllowNoTabs { get; set; } = false;
        
        /// <summary>
        /// The data source used for creating new tabs.
        /// </summary>
        public ITabDataSource DataSource { get; set; } = new DefaultTabDataSource();

        #endregion

        #region Constructors

        /// <summary>
        /// Create a new empty Tab View.
        /// </summary>
        public TabView()
        {
            // Construct Element
            styleSheets.Add(StyleSheet);
            styleSheets.Add(UixResources.CommonStyleSheet);
            RootTree.CloneTree(this);
            // Set default
            FitToParent = true;

            // Reconfigure 
            // TODO
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a new tab to the tab view.
        /// <remarks>
        /// Adding the same tab multiple times is currently not supported, but if need arises could be done.
        /// </remarks>
        /// </summary>
        /// <param name="tab">The tab to be added.</param>
        /// <exception cref="ArgumentNullException">Thrown if the passed tab is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the tab has already been added.</exception>
        public void AddTab(Tab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            // REVIEW: Not sure if the same tab should be allowed to be added. Would cause problems to the RemoveTabAt Method.
            if (_tabs.Contains(tab)) throw new ArgumentException("Tab has already been added", nameof(tab));
            if (string.IsNullOrWhiteSpace(tab.TabName))
            {
                tab.TabName = $"Tab {_tabs.Count + 1}";
            }

            _tabs.Add(tab);
            AddTabHeader(tab);
            SelectTab(tab);
        }

        /// <summary>
        /// Adds a new tab using the data source.
        /// </summary>
        public void AddNewTab()
        {
            AddTab(DataSource.CreateNewTab());
        }

        /// <summary>
        /// Removes an existing tab from this tab view.
        /// </summary>
        /// <param name="tab">The tab to be removed</param>
        /// <exception cref="ArgumentNullException">Thrown if the passed tab is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the tab is not in this tab view.</exception>
        public void RemoveTab(Tab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab))
                throw new ArgumentException("Tab has not been added to Terminal Window", nameof(tab));

            var index = _tabs.IndexOf(tab);
            RemoveTabAt(index);
        }

        /// <summary>
        /// Removes the tab at the given index.
        /// </summary>
        /// <param name="tabNr">The index of the tab to be removed</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is out of range.</exception>
        public void RemoveTabAt(int tabNr)
        {
            if (tabNr < 0)
                throw new ArgumentOutOfRangeException( nameof(tabNr), "Tab to be removed can not have negative index");
            if (tabNr >= _tabs.Count)
                throw new ArgumentOutOfRangeException(nameof(tabNr), "Tab to be removed is out of range");
            if (!AllowNoTabs && _tabs.Count == 1) return;
            if (_selectedTabNr == tabNr)
            {
                if (tabNr == _tabs.Count - 1) SelectTab(tabNr - 1);
                else SelectTab(tabNr + 1);
            }
            else
            {
                _selectedTabNr -= 1;
            }

            _tabs.RemoveAt(tabNr);
            RemoveTabHeaderAt(tabNr);
        }

        public void RemoveAllTabs()
        {
            _tabs.ForEach(RemoveTab);
        }

        /// <summary>
        /// Selects the passed tab showing its content in the content view.
        /// </summary>
        /// <param name="tab">The tab to be selected.</param>
        /// <exception cref="ArgumentNullException">Thrown if the passed tab is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the tab is not part of this tabview.</exception>
        public void SelectTab(Tab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab))
                throw new ArgumentException(
                    "Tab has not been added to terminal, but is tried to be selected",
                    nameof(tab));
            var index = _tabs.IndexOf(tab);
            if (_tabs.Count >= 1)
            {
                HeaderBar.Query<TabHeader>().AtIndex(_selectedTabNr).Selected = false;
            }

            HeaderBar.Query<TabHeader>().AtIndex(index).Selected = true;
            _selectedTabNr = index;

            SetTabContent(tab);
        }

        /// <summary>
        /// Selects the passed tab showing its content in the content view.
        /// </summary>
        /// <param name="tabNr">The index of the tab to be selected.</param>
        /// <exception cref="ArgumentNullException">Thrown if the passed tab is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the tab is not part of this tabview.</exception>
        public void SelectTab(int tabNr)
        {
            if (tabNr < 0) throw new ArgumentException("Can not select negative indexed tab", nameof(tabNr));
            if (tabNr >= _tabs.Count)
                throw new ArgumentException("Tab index to be selected exceeds the amount of available tabs",
                    nameof(tabNr));

            SelectTab(_tabs[tabNr]);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a new Tab Header for a given tab and starts to listen to its events.
        /// </summary>
        /// <param name="tab">The tab for which the tab header should be created for.</param>
        /// <exception cref="ArgumentNullException">Thrown if the passed tab is null</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the passed tab has not been registered in the _tabs list.
        /// </exception>
        private void AddTabHeader(Tab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab)) throw new ArgumentException("Tab has not been added to the registered tabs");

            HeaderBar.Add(tab.Header);
            tab.Select += () => SelectTab(tab);
            tab.Close += () => RemoveTab(tab);
        }

        /// <summary>
        /// Removes the nth tab header from the header bar where n is the passed tabNr.
        /// </summary>
        /// <param name="tabNr">The index of the header to be removed.</param>
        /// <exception cref="ArgumentException">Thrown if the index is outside the range of the headers.</exception>
        private void RemoveTabHeaderAt(int tabNr)
        {
            if (tabNr < 0)
                throw new ArgumentException($"Preview to be removed can not be negative. Passed value: {tabNr}",
                    nameof(tabNr));
            if (tabNr >= HeaderBar.childCount)
                throw new ArgumentException($"Preview to be removed is out of bounds. Passed value: {tabNr}",
                    nameof(tabNr));

            HeaderBar.RemoveAt(tabNr);
        }

        /// <summary>
        /// Replaces the current content with the Tab passed.
        /// </summary>
        /// <param name="tab">The element to be added to the content.</param>
        /// <exception cref="ArgumentNullException">Thrown if the tab is null</exception>
        /// <exception cref="ArgumentException">
        /// Thrown if the passed tab has not been registered in the <see cref="_tabs"/> list.
        /// </exception>
        private void SetTabContent(Tab tab)
        {
            if (tab is null) throw new ArgumentNullException(nameof(tab));
            if (!_tabs.Contains(tab)) throw new ArgumentException("Tab has not been added to the registered tabs");

            RemoveTabContent();
            Content.Add(tab);
        }

        /// <summary>
        /// Removes all children from the Content Element.
        /// </summary>
        private void RemoveTabContent()
        {
            Content.Clear();
        }

        #endregion
    }
}
