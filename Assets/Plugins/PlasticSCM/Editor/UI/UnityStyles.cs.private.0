using System;

using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Codice.UI
{
    // Assumption: Members are called from an OnGUI method ( otherwise style composition will fail)
    public static class UnityStyles
    {
        public static class Colors
        {
            public static Color GreenBackground = new Color(34f / 255, 161f / 255, 63f / 255);
            public static Color GreenText = new Color(0f / 255, 100f / 255, 0f / 255);
            public static Color Red = new Color(194f / 255, 51f / 255, 62f / 255);
            public static Color Warning = new Color(255f / 255, 255f / 255, 176f / 255);
            public static Color Splitter = new Color(100f / 255, 100f / 255, 100f / 255);
            public static Color TabUnderline = new Color(0.128f, 0.456f, 0.776f);
            public static Color Link = new Color(0f, 120f / 255, 218f / 255);
        }

        public static class HexColors
        {
            public const string LINK_COLOR = "#0078DA";
        }

        public static class Dialog
        {

            public static readonly Lazy<GUIStyle> MessageTitle = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.boldLabel);
                style.contentOffset = new Vector2(0, -5);
                style.wordWrap = true;
                style.fontSize = MODAL_FONT_SIZE + 1;
                return style;
            });

            public static readonly Lazy<GUIStyle> MessageText = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                style.wordWrap = true;
                style.fontSize = MODAL_FONT_SIZE;
                return style;
            });

            public static readonly Lazy<GUIStyle> Box = new Lazy<GUIStyle>(() =>
            {
                return GetEditorSkin().box;
            });

            public static readonly Lazy<GUIStyle> Toggle = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.boldLabel);
                style.fontSize = MODAL_FONT_SIZE;
                style.clipping = TextClipping.Overflow;
                return style;
            });

            public static readonly Lazy<GUIStyle> RadioToggle = new Lazy<GUIStyle>(() =>
            {
                var radioToggleStyle = new GUIStyle(EditorStyles.radioButton);
                radioToggleStyle.fontSize = MODAL_FONT_SIZE;
                radioToggleStyle.clipping = TextClipping.Overflow;
                radioToggleStyle.font = EditorStyles.largeLabel.font;
                return radioToggleStyle;
            });

            public static readonly Lazy<GUIStyle> Foldout = new Lazy<GUIStyle>(() =>
            {
                GUIStyle paragraphStyle = Paragraph;
                var foldoutStyle = new GUIStyle(EditorStyles.foldout);
                foldoutStyle.fontSize = MODAL_FONT_SIZE;
                foldoutStyle.font = paragraphStyle.font;
                return foldoutStyle;
            });

            public static readonly Lazy<GUIStyle> EntryLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.textField);
                style.wordWrap = true;
                style.fontSize = MODAL_FONT_SIZE;
                return style;
            });

            public static readonly Lazy<GUIStyle> AcceptButtonText = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(GetEditorSkin().GetStyle("WhiteLabel"));
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = MODAL_FONT_SIZE + 1;
                style.normal.background = null;
                return style;
            });

            public static readonly Lazy<GUIStyle> NormalButton = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(GetEditorSkin().button);
                style.alignment = TextAnchor.MiddleCenter;
                style.fontSize = MODAL_FONT_SIZE;
                return style;
            });
        }

        public static class Tree
        {
            public static readonly Lazy<GUIStyle> IconStyle = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.largeLabel);
                style.alignment = TextAnchor.MiddleLeft;
                return style;
            });

            public static readonly Lazy<GUIStyle> Label = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(TreeView.DefaultStyles.label);
                style.fontSize = 11;
                style.alignment = TextAnchor.MiddleLeft;
                return style;
            });

            public static readonly Lazy<GUIStyle> RedLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(Label);
                style.active = new GUIStyleState() { textColor = Colors.Red };
                style.focused = new GUIStyleState() { textColor = Colors.Red };
                style.hover = new GUIStyleState() { textColor = Colors.Red };
                style.normal = new GUIStyleState() { textColor = Colors.Red };
                return style;
            });

            public static readonly Lazy<GUIStyle> GreenLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(Label);
                style.active = new GUIStyleState() { textColor = Colors.GreenText };
                style.focused = new GUIStyleState() { textColor = Colors.GreenText };
                style.hover = new GUIStyleState() { textColor = Colors.GreenText };
                style.normal = new GUIStyleState() { textColor = Colors.GreenText };
                return style;
            });

            public static readonly Lazy<GUIStyle> BoldLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(TreeView.DefaultStyles.boldLabel);
                style.fontSize = 11;
                style.alignment = TextAnchor.MiddleLeft;
                return style;
            });

            public static readonly Lazy<GUIStyle> LabelRightAligned = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(TreeView.DefaultStyles.label);
                style.fontSize = 11;
                style.alignment = TextAnchor.MiddleRight;
                return style;
            });
        }

        public static class PlasticWindow
        {
            public static readonly Lazy<GUIStyle> HeaderLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                return style;
            });

            public static readonly Lazy<GUIStyle> ActiveTabButton = new Lazy<GUIStyle>(() =>
            {
                GUIStyle result = new GUIStyle(EditorStyles.toolbarButton);
                result.fontStyle = FontStyle.Bold;
                return result;
            });

            public static readonly Lazy<GUIStyle> ActiveTabUnderline = new Lazy<GUIStyle>(() =>
            {
                return CreateUnderlineStyle(
                    Colors.TabUnderline,
                    UnityConstants.ACTIVE_TAB_UNDERLINE_HEIGHT);
            });
        }

        public static class PendingChangesTab
        {
            public static readonly Lazy<GUIStyle> CommentPlaceHolder = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle();
                style.normal = new GUIStyleState() { textColor = Color.gray };
                style.padding = new RectOffset(2, 0, 0, 0);
                return style;
            });

            public static readonly Lazy<GUIStyle> CommentTextArea = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.textArea);
                style.margin = new RectOffset(6, 4, 0, 0);
                return style;
            });

            public static readonly Lazy<GUIStyle> CommentWarningIcon = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                style.fontSize = 10;
#if !UNITY_2019_1_OR_NEWER
                style.margin = new RectOffset(0, 0, 0, 0);
#endif
                return style;
            });

            public static readonly Lazy<GUIStyle> HeaderLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                style.fontStyle = FontStyle.Bold;
                return style;
            });
        }

        public static class IncomingChangesTab
        {
            public static readonly Lazy<GUIStyle> PendingConflictsLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                style.fontSize = 10;
                style.fontStyle = FontStyle.Bold;
                return style;
            });

            public static readonly Lazy<GUIStyle> RedPendingConflictsOfTotalLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(PendingConflictsLabel);
                style.normal = new GUIStyleState() { textColor = Colors.Red };
                return style;
            });

            public static readonly Lazy<GUIStyle> GreenPendingConflictsOfTotalLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(PendingConflictsLabel);
                style.normal = new GUIStyleState() { textColor = Colors.GreenText };
                return style;
            });

            public static readonly Lazy<GUIStyle> ChangesToApplySummaryLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                style.fontSize = 10;
                return style;
            });

            public readonly static Lazy<GUIStyle> HeaderWarningLabel
                = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                style.fontSize = 10;
#if !UNITY_2019_1_OR_NEWER
                style.margin = new RectOffset(0, 0, 0, 0);
#endif
                return style;
            });
        }

        public static class DirectoryConflictResolution
        {
            public readonly static Lazy<GUIStyle> WarningLabel
                = new Lazy<GUIStyle>(() =>
                {
                    var style = new GUIStyle(EditorStyles.label);
                    style.alignment = TextAnchor.MiddleLeft;
#if !UNITY_2019_1_OR_NEWER
                    style.margin = new RectOffset(0, 0, 0, 0);
#endif
                    return style;
                });
        }

        public static class Notification
        {
            public static readonly Lazy<GUIStyle> Label = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
#if !UNITY_2019_1_OR_NEWER
                style.fontSize = 10;
#endif
                style.normal = new GUIStyleState() { textColor = Color.white };
                return style;
            });

            public static readonly Lazy<GUIStyle> GreenNotification = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle();
                style.wordWrap = true;
                style.margin = new RectOffset();
                style.padding = new RectOffset(4, 4, 2, 2);
                style.stretchWidth = true;
                style.stretchHeight = true;
                style.alignment = TextAnchor.UpperLeft;

                var bg = new Texture2D(1, 1);
                bg.SetPixel(0, 0, Colors.GreenBackground);
                bg.Apply();
                style.normal.background = bg;
                return style;
            });

            public static readonly Lazy<GUIStyle> RedNotification = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle();
                style.wordWrap = true;
                style.margin = new RectOffset();
                style.padding = new RectOffset(4, 4, 2, 2);
                style.stretchWidth = true;
                style.stretchHeight = true;
                style.alignment = TextAnchor.UpperLeft;

                var bg = new Texture2D(1, 1);
                bg.SetPixel(0, 0, Colors.Red);
                bg.Apply();
                style.normal.background = bg;
                return style;
            });
        }

        public static class DirectoryConflicts
        {
            public readonly static Lazy<GUIStyle> TitleLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.largeLabel);
                RectOffset margin = new RectOffset(
                    style.margin.left,
                    style.margin.right,
                    style.margin.top - 1,
                    style.margin.bottom);
                style.margin = margin;
                style.fontStyle = FontStyle.Bold;
                return style;
            });

            public readonly static Lazy<GUIStyle> BoldLabel = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.label);
                style.fontStyle = FontStyle.Bold;
                return style;
            });

            public readonly static Lazy<GUIStyle> FileNameTextField = new Lazy<GUIStyle>(() =>
            {
                var style = new GUIStyle(EditorStyles.textField);
                RectOffset margin = new RectOffset(
                    style.margin.left,
                    style.margin.right,
                    style.margin.top + 2,
                    style.margin.bottom);
                style.margin = margin;
                return style;
            });
        }

        public static readonly Lazy<GUIStyle> SplitterIndicator = new Lazy<GUIStyle>(() =>
        {
            return CreateUnderlineStyle(
                Colors.Splitter,
                UnityConstants.SPLITTER_INDICATOR_HEIGHT);
        });

        public static readonly Lazy<Texture2D> LinkTexture = new Lazy<Texture2D>(() =>
        {
            var bg = new Texture2D(1, 1);
            bg.SetPixel(0, 0, Colors.Link);
            bg.Apply();

            return bg;
        });

        public static readonly Lazy<GUIStyle> HelpBoxLabel = new Lazy<GUIStyle>(() =>
        {
            var style = new GUIStyle(EditorStyles.label);
            style.fontSize = 10;
            style.wordWrap = true;
            return style;
        });

        public static readonly Lazy<GUIStyle> ProgressLabel = new Lazy<GUIStyle>(() =>
        {
            var style = new GUIStyle(EditorStyles.label);
            style.fontSize = 10;
#if !UNITY_2019_1_OR_NEWER
            style.margin = new RectOffset(0, 0, 0, 0);
#endif
            return style;
        });

        public static readonly Lazy<GUIStyle> TextFieldWithWrapping = new Lazy<GUIStyle>(() =>
        {
            var style = new GUIStyle(EditorStyles.textField);
            style.wordWrap = true;
            return style;
        });

        public static readonly Lazy<GUIStyle> Search = new Lazy<GUIStyle>(() =>
        {
            var style = new GUIStyle();
            style.normal = new GUIStyleState() { textColor = Color.gray };
            style.padding = new RectOffset(18, 0, 0, 0);
            return style;
        });

        public static readonly Lazy<GUIStyle> WarningMessage = new Lazy<GUIStyle>(() =>
        {
            var style = new GUIStyle(GetEditorSkin().box);
            style.wordWrap = true;
            style.margin = new RectOffset();
            style.padding = new RectOffset(8, 8, 6, 6);
            style.stretchWidth = true;
            style.alignment = TextAnchor.UpperLeft;

            var bg = new Texture2D(1, 1);
            bg.SetPixel(0, 0, Colors.Warning);
            bg.Apply();
            style.normal.background = bg;
            return style;
        });

        public static readonly Lazy<GUIStyle> CancelButton = new Lazy<GUIStyle>(() =>
        {
            var normalIcon = Images.GetImage(Images.Name.IconCloseButton);
            var pressedIcon = Images.GetImage(Images.Name.IconPressedCloseButton);

            var style = new GUIStyle();
            style.normal = new GUIStyleState() { background = normalIcon };
            style.onActive = new GUIStyleState() { background = pressedIcon };
            style.active = new GUIStyleState() { background = pressedIcon };
            return style;
        });

        public static readonly Lazy<GUIStyle> MiniToggle = new Lazy<GUIStyle>(() =>
        {
            var style = new GUIStyle(EditorStyles.boldLabel);
            style.fontSize = MODAL_FONT_SIZE - 1;
            style.clipping = TextClipping.Overflow;
            return style;
        });

        public static readonly Lazy<GUIStyle> Paragraph = new Lazy<GUIStyle>(() =>
        {
            var style = new GUIStyle(EditorStyles.largeLabel);
            style.wordWrap = true;
            style.richText = true;
            style.fontSize = MODAL_FONT_SIZE;
            return style;
        });

        static GUISkin GetEditorSkin()
        {
            GUISkin editorSkin = null;
            if (EditorGUIUtility.isProSkin)
                editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);
            else
                editorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            
            return editorSkin;
        }

        static GUIStyle CreateUnderlineStyle(Color color, int height)
        {
            GUIStyle style = new GUIStyle();

            Texture2D pixel = new Texture2D(1, height);

            for (int i = 0; i < height; i++)
                pixel.SetPixel(0, i, color);

            pixel.wrapMode = TextureWrapMode.Repeat;
            pixel.Apply();

            style.normal.background = pixel;
            style.fixedHeight = height;

            return style;
        }

        public class Lazy<T> where T : class
        {
            public Lazy(Func<T> builder)
            {
                mBuilder = builder;
                mInitialized = false;
            }

            public static implicit operator T(Lazy<T> lazy)
            {
                if(!lazy.mInitialized)
                {
                    lazy.mValue = lazy.mBuilder();
                    lazy.mInitialized = true;
                }

                return lazy.mValue;
            }

            T mValue;

            bool mInitialized = false;

            Func<T> mBuilder;
        }

        const int MODAL_FONT_SIZE = 13;
    }
}
