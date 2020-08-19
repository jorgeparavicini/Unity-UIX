using System;
using System.Linq;
using JetBrains.Annotations;
using UIX.Common;
using UnityEditor;
using UnityEngine.UIElements;

namespace UIX
{
    public class Tab : VisualElement
    {
        [UsedImplicitly]
        public new class UxmlFactory : UxmlFactory<Tab>
        {
        }

        #region Properties & Events

        internal TabHeader Header { get; } = new TabHeader();

        public string TabName
        {
            get => Header.Title;
            set => Header.Title = value;
        }

        public bool FitToParent
        {
            [UsedImplicitly] get => GetClasses().Contains(UixResources.ExpandClassName);
            set
            {
                if (value) AddToClassList(UixResources.ExpandClassName);
                else RemoveFromClassList(UixResources.ExpandClassName);
            }
        }
        
        public event Action Select = delegate { };
        public event Action Close = delegate { };

        #endregion
        
        #region Constructors

        public Tab()
        {
            styleSheets.Add(UixResources.CommonStyleSheet);
            FitToParent = true;
            
            Header.Select += Select;
            Header.Close += Close;
        }

        public Tab(VisualElement content) : this()
        {
            Add(content);
        }

        #endregion

        #region Public Methods

        [UsedImplicitly]
        public void BindTitle(SerializedObject obj, string bindingPath)
        {
            Header.BindTitle(obj, bindingPath);
        }

        #endregion

    }
}