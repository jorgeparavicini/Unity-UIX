using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[assembly: UxmlNamespacePrefix("UIX", "uix")]

namespace UIX.Common
{
    public static class UixResources
    {
        public const string ExpandClassName = "uix--expand";

        public static StyleSheet CommonStyleSheet => Resources.Load<StyleSheet>("UIX_Styles");
    }
}