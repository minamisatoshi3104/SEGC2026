using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 画面フェードを行うシングルトン。自動でオーバーレイを生成します。
    /// 使用例:
    /// await FadeManager.Instance.FadeOut(0.5f);
    /// // シーン切り替え
    /// await FadeManager.Instance.FadeIn(0.5f);
    /// </summary>
    public class FadeManager : MonoBehaviour
    {
        public static FadeManager Instance { get; private set; }

        CanvasGroup _canvasGroup;
        Image _image;

        // フェード実行中かを外部から確認するためのプロパティ
        bool _isFading = false;
        public bool IsFading => _isFading;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                EnsureOverlay();
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void EnsureOverlay()
        {
            // Create a full-screen Canvas + Image with CanvasGroup if not present
            if (_canvasGroup != null && _image != null) return;

            var overlayGO = new GameObject("FadeOverlay");
            overlayGO.transform.SetParent(transform, false);

            var canvas = overlayGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            overlayGO.AddComponent<CanvasScaler>();
            overlayGO.AddComponent<GraphicRaycaster>();

            var imgGO = new GameObject("Image");
            imgGO.transform.SetParent(overlayGO.transform, false);
            _image = imgGO.AddComponent<Image>();
            _image.color = Color.black;

            var rect = imgGO.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            _canvasGroup = imgGO.AddComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }

        public Task FadeIn(float duration = 0.5f)
        {
            // Fade from black (1) to transparent (0)
            return Fade(1f, 0f, duration);
        }

        public Task FadeOut(float duration = 0.5f)
        {
            // Fade from transparent (0) to black (1)
            return Fade(0f, 1f, duration);
        }

        Task Fade(float from, float to, float duration)
        {
            if (_canvasGroup == null)
            {
                EnsureOverlay();
            }

            var tcs = new TaskCompletionSource<bool>();
            // mark as running
            _isFading = true;
            StartCoroutine(FadeCoroutine(from, to, duration, tcs));
            return tcs.Task;
        }

        IEnumerator FadeCoroutine(float from, float to, float duration, TaskCompletionSource<bool> tcs)
        {
            float elapsed = 0f;
            _canvasGroup.alpha = from;
            _canvasGroup.blocksRaycasts = true; // block input during fade

            if (duration <= 0f)
            {
                _canvasGroup.alpha = to;
                _canvasGroup.blocksRaycasts = to > 0.5f;
                // mark finished
                _isFading = false;
                tcs.SetResult(true);
                yield break;
            }

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                _canvasGroup.alpha = Mathf.Lerp(from, to, Mathf.Clamp01(elapsed / duration));
                yield return null;
            }

            _canvasGroup.alpha = to;
            _canvasGroup.blocksRaycasts = to > 0.5f;
            // mark finished
            _isFading = false;
            tcs.SetResult(true);
        }

        // Immediate set
        public void SetAlpha(float a)
        {
            if (_canvasGroup == null) EnsureOverlay();
            _canvasGroup.alpha = a;
            _canvasGroup.blocksRaycasts = a > 0.5f;
        }
    }
}
