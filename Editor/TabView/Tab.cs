using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using JetBrains.Annotations;
using UIX.Common;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIX
{
    public class Tab : VisualElement
    {
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

        public StyleSheet HeaderStyleSheet
        {
            get => Header.StyleSheet;
            set => Header.StyleSheet = value;
        }

        public event Action Select = delegate { };
        public event Action Close = delegate { };

        #endregion
        
        #region Constructors

        public Tab()
        {
            styleSheets.Add(UixResources.CommonStyleSheet);
            FitToParent = true;
            
            Header.Q<Button>(TabHeader.SelectName).clicked += () => Select();
            Header.Q<Button>(TabHeader.CloseName).clicked += () => Close();
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