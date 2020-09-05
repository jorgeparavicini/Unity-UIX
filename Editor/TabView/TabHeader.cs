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
        
        internal const string SelectName = "uix-tabview-header__select";
        internal const string CloseName = "uix-tabview-header__close";
        private const string SelectedClass = "uix-tabview-header--selected";

        private bool _selected;

        private static VisualTreeAsset TabHeaderTree => Resources.Load<VisualTreeAsset>("UIX_TabHeader");
        private static StyleSheet HeaderStyleSheet => Resources.Load<StyleSheet>("UIX_TabHeaderStyle");
        private VisualElement Content => Children().First();
        private Button SelectButton => this.Q<Button>(SelectName);
        private StyleSheet _styleSheet;

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

        internal StyleSheet StyleSheet
        {
            get => _styleSheet;
            set
            {
                styleSheets.Remove(_styleSheet);
                styleSheets.Add(value);
                _styleSheet = value;
            }
        }

        #endregion

        #region Constructors

        internal TabHeader()
        {
            TabHeaderTree.CloneTree(this);
            StyleSheet = HeaderStyleSheet;
        }

        #endregion

        #region Internal Methods

        internal void BindTitle(SerializedObject obj, string bindingPath)
        {
            SelectButton.bindingPath = bindingPath;
            SelectButton.BindProperty(obj);
        }

        #endregion

    }
}