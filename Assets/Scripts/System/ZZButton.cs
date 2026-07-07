using TMPro;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ZZ
{
    /// <summary>
    /// 自作ボタン
    /// </summary>
    public class ZZButton : Button
    {
        public class ZZButtonClickedEvent : UnityEvent<EButtonType> { }
        public ZZButtonClickedEvent onClickWithType = new ZZButtonClickedEvent();

        [SerializeField] EButtonType _buttonType;   // ボタンの種類
        int _lockCount; // ロックの数

        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(OnClick);
        }

        protected override void OnDestroy()
        {
            onClick.RemoveListener(OnClick);
        }

        /// <summary>
        /// ロック追加
        /// </summary>
        public void AddLock()
        {
            _lockCount++;
            UpdateInteractable();
        }

        /// <summary>
        /// ロック解除
        /// </summary>
        public void RemoveLock()
        {
            _lockCount--;
            UpdateInteractable();
        }

        /// <summary>
        /// 有効状態更新
        /// </summary>
        private void UpdateInteractable()
        {
            interactable = _lockCount <= 0;
        }

        /// <summary>
        /// クリックされた
        /// </summary>
        private void OnClick()
        {
            onClickWithType.Invoke(_buttonType);
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(ZZButton))]
        public class ZZButtonEditor : ButtonEditor
        {
            SerializedProperty _buttonType;

            protected override void OnEnable()
            {
                base.OnEnable();
                _buttonType = serializedObject.FindProperty("_buttonType");
            }

            public override void OnInspectorGUI()
            {
                base.OnInspectorGUI();
                serializedObject.Update();

                EditorGUILayout.PropertyField(_buttonType);

                serializedObject.ApplyModifiedProperties();
            }
        }

        [MenuItem("GameObject/ZZ/Button", false, 10)]
        public static void CreateObject(MenuCommand menuCommand)
        {
            // ボタンオブジェクト
            var btnObj = new GameObject("Button", typeof(RectTransform));
            var img = btnObj.AddComponent<Image>();
            var btn = btnObj.AddComponent<ZZButton>();
            img.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
            img.type = Image.Type.Sliced;
            btn.targetGraphic = img;

            var btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.sizeDelta = new Vector2(300, 100);

            // テキストオブジェクト
            var textObj = new GameObject("Text (TMP)", typeof(RectTransform));
            var tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = "Button";
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.color = Color.black;
            tmp.raycastTarget = false;

            var textRect = textObj.GetComponent<RectTransform>();
            textRect.SetParent(btnObj.transform, false);
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            // 標準機能
            Undo.RegisterCreatedObjectUndo(btnObj, "Create ZZButton Object");
            GameObjectUtility.SetParentAndAlign(btnObj, menuCommand.context as GameObject);
            Selection.activeObject = btnObj;
        }
#endif
    }
}