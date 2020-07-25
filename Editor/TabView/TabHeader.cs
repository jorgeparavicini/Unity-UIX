using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIX
{
    public class TabHeader : VisualElement
    {
        #region Fields

        private bool _selected;

        #endregion
        
        #region UI Resources

        private const string SelectName = "uix-tabview-header__select";
        private const string CloseName = "uix-tabview-header__close";
        private const string SelectedClass = "uix-tabview-header--selected";
        
        private static VisualTreeAsset TabHeaderTree => Resources.Load<VisualTreeAsset>("UIX_TabHeader");
        private static StyleSheet HeaderStyleSheet => Resources.Load<StyleSheet>("UIX_TabHeaderStyle");
        
        public new class UxmlFactory : UxmlFactory<TabHeader> {}

        #endregion

        #region Events

        public event Action Select = delegate { };
        public event Action Close = delegate { };

        #endregion

        #region Properties
        
        private VisualElement Content => Children().First();
        private Button SelectButton => this.Q<Button>(SelectName);

        public string Title
        {
            get => SelectButton.text;
            set => SelectButton.text = value;
        }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (Selected) Content.AddToClassList(SelectedClass);
                else Content.RemoveFromClassList(SelectedClass);
            }
        }

        #endregion

        #region Constructors

        public TabHeader()
        {
            TabHeaderTree.CloneTree(this);
            styleSheets.Add(HeaderStyleSheet);

            this.Q<Button>(SelectName).clickable.clicked += () => Select();
            this.Q<Button>(CloseName).clickable.clicked += () => Close();
        }


        #endregion

    }
}