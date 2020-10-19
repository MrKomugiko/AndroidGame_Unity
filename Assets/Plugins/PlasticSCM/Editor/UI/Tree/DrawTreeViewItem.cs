﻿using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Codice.UI.Tree
{
    internal static class DrawTreeViewItem
    {
        internal static void InitializeStyles()
        {
            if (EditorStyles.label == null)
                return;

            TreeView.DefaultStyles.label = UnityStyles.Tree.Label;
            TreeView.DefaultStyles.boldLabel = UnityStyles.Tree.BoldLabel;
            TreeView.DefaultStyles.labelRightAligned = UnityStyles.Tree.LabelRightAligned;
        }

        internal static void ForCategoryItem(
            Rect rowRect,
            float rowHeight,
            int depth,
            Texture icon,
            string label,
            bool isSelected,
            bool isFocused)
        {
            float indent = GetIndent(depth);

            rowRect.x += indent;
            rowRect.width -= indent;

            rowRect = DrawIconLeft(rowRect, rowHeight, icon, null);
            TreeView.DefaultGUI.Label(rowRect, label, isSelected, isFocused);
        }

        internal static bool ForCheckableCategoryItem(
            Rect rowRect,
            float rowHeight,
            int depth,
            Texture icon,
            string label,
            bool isSelected,
            bool isFocused,
            bool wasChecked,
            bool hadCheckedChildren)
        {
            float indent = GetIndent(depth);

            rowRect.x += indent;
            rowRect.width -= indent;

            Rect checkRect = GetCheckboxRect(rowRect, rowHeight);

            if (!wasChecked && hadCheckedChildren)
                EditorGUI.showMixedValue = true;

            bool isChecked = EditorGUI.Toggle(checkRect, wasChecked);
            EditorGUI.showMixedValue = false;

            rowRect.x = checkRect.xMax - 4;
            rowRect.width -= checkRect.width;

            rowRect = DrawIconLeft(rowRect, rowHeight, icon, null);

            TreeView.DefaultGUI.Label(rowRect, label, isSelected, isFocused);

            return isChecked;
        }

        internal static void ForItemCell(
            Rect rect,
            float rowHeight,
            int depth,
            Texture icon,
            GetOverlayIcon.Data overlayIconData,
            string label,
            bool isSelected,
            bool isFocused,
            bool isBoldText)
        {
            float indent = GetIndent(depth);

            rect.x += indent;
            rect.width -= indent;

            rect = DrawIconLeft(
                rect, rowHeight, icon, overlayIconData);

            if (isBoldText)
                TreeView.DefaultGUI.BoldLabel(rect, label, isSelected, isFocused);
            else
                TreeView.DefaultGUI.Label(rect, label, isSelected, isFocused);
        }

        internal static bool ForCheckableItemCell(
            Rect rect,
            float rowHeight,
            int depth,
            Texture icon,
            GetOverlayIcon.Data overlayIconData,
            string label,
            bool isSelected,
            bool isFocused,
            bool isHighlighted,
            bool wasChecked)
        {
            float indent = GetIndent(depth);

            rect.x += indent;
            rect.width -= indent;

            Rect checkRect = GetCheckboxRect(rect, rowHeight);

            bool isChecked = EditorGUI.Toggle(checkRect, wasChecked);

            rect.x = checkRect.xMax;
            rect.width -= checkRect.width;

            rect = DrawIconLeft(
                rect, rowHeight, icon, overlayIconData);

            if (isHighlighted)
                TreeView.DefaultGUI.BoldLabel(rect, label, isSelected, isFocused);
            else
                TreeView.DefaultGUI.Label(rect, label, isSelected, isFocused);

            return isChecked;
        }

        static Rect DrawIconLeft(
            Rect rect,
            float rowHeight,
            Texture icon,
            GetOverlayIcon.Data overlayIconData)
        {
            if (icon == null)
                return rect;

            float iconWidth = rowHeight * ((float)icon.width / icon.height);

            Rect iconRect = new Rect(rect.x, rect.y, iconWidth, rowHeight);

            EditorGUI.LabelField(iconRect, new GUIContent(icon), UnityStyles.Tree.IconStyle);

            if (overlayIconData != null && overlayIconData.Texture != null)
            {
                Rect overlayIconRect = new Rect(
                    iconRect.x + overlayIconData.XOffset,
                    iconRect.y + overlayIconData.YOffset,
                    overlayIconData.Size, overlayIconData.Size);

                GUI.DrawTexture(
                    overlayIconRect, overlayIconData.Texture,
                    ScaleMode.ScaleToFit);
            }

            rect.x += iconRect.width;
            rect.width -= iconRect.width;

            return rect;
        }

        static Rect GetCheckboxRect(Rect rect, float rowHeight)
        {
            return new Rect(
                rect.x,
                rect.y + UnityConstants.TREEVIEW_CHECKBOX_Y_OFFSET,
                UnityConstants.TREEVIEW_CHECKBOX_SIZE,
                rect.height);
        }

        static float GetIndent(int depth)
        {
            if (depth == 0)
                return 16;

            return 30;
        }
    }
}
