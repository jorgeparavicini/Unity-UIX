using UnityEngine;
using UnityEngine.UIElements;

namespace UIX
{
    public class DefaultTabDataSource : ITabDataSource
    {
        private static VisualTreeAsset RootTree => Resources.Load<VisualTreeAsset>("UIX_DefaultTab");
        private static StyleSheet StyleSheet => Resources.Load<StyleSheet>("UIX_DefaultTabStyle");

        private static int _tabIndex;

        public Tab CreateNewTab()
        {
            var tab = new Tab {TabName = $"Tab {_tabIndex}"};
            _tabIndex++;

            RootTree.CloneTree(tab);
            tab.styleSheets.Add(StyleSheet);
            
            return tab;
        }
    }
}