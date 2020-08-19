using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIX
{
    internal class TabHeader : VisualElement
    {
        #region Fields, Properties & Events
        
        private const string SelectName = "uix-tabview-header__select";
        private const string CloseName = "uix-tabview-header__close";
        private const string SelectedClass = "uix-tabview-header--selected";

        private bool _selected;

        private static VisualTreeAsset TabHeaderTree => Resources.Load<VisualTreeAsset>("UIX_TabHeader");
        private static StyleSheet HeaderStyleSheet => Resources.Load<StyleSheet>("UIX_TabHeaderStyle");
        private VisualElement Content => Children().First();
        private Button SelectButton => this.Q<Button>(SelectName);

        internal string Title
        {
            get => SelectButton.text;
            set => SelectButton.text = value;
        }

        internal bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (Selected) Content.AddToClassList(SelectedClass);
                else Content.RemoveFromClassList(SelectedClass);
            }
        }
        
        internal event Action Select = delegate { };
        internal event Action Close = delegate { };

        #endregion

        #region Constructors

        internal TabHeader()
        {
            TabHeaderTree.CloneTree(this);
            styleSheets.Add(HeaderStyleSheet);

            this.Q<Button>(SelectName).clickable.clicked += () => Select();
            this.Q<Button>(CloseName).clickable.clicked += () => Close();
        }

        #endregion

        #region Public Methods

        internal void BindTitle(SerializedObject obj, string bindingPath)
        {
            SelectButton.bindingPath = bindingPath;
            SelectButton.Bind(obj);
        }

        #endregion

    }
}