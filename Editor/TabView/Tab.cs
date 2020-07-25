using System;
using System.Linq;
using UIX.Common;
using UnityEngine.UIElements;

namespace UIX
{
    public class Tab : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<Tab> {}
        
        public event Action NameChanged = delegate {  };

        private string _tabName;

        public string TabName
        {
            get => _tabName;
            set
            {
                _tabName = value;
                NameChanged();
            }
        }

        public bool FitToParent
        {
            get => GetClasses().Contains(UixResources.ExpandClassName);
            set
            {
                if (value) AddToClassList(UixResources.ExpandClassName);
                else RemoveFromClassList(UixResources.ExpandClassName);
            }
        }

        public Tab()
        {
            styleSheets.Add(UixResources.CommonStyleSheet);
            FitToParent = true;
        }
    }
}