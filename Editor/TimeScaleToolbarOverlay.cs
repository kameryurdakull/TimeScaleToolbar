using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kamer.TimeScaleToolbar.Editor
{
    [Overlay(
        typeof(SceneView),
        TimeScaleToolbarSettings.OverlayId,
        TimeScaleToolbarSettings.DisplayName,
        defaultDisplay = true,
        defaultDockZone = DockZone.TopToolbar,
        defaultDockPosition = DockPosition.Top,
        defaultDockIndex = 1,
        defaultLayout = Layout.HorizontalToolbar)]
    internal sealed class TimeScaleToolbarOverlay : ToolbarOverlay
    {
        public TimeScaleToolbarOverlay() : base(TimeScaleToolbarSettings.ElementId)
        {
        }
    }

    [EditorToolbarElement(TimeScaleToolbarSettings.ElementId, typeof(SceneView))]
    internal sealed class TimeScaleToolbarElement : VisualElement
    {
        private readonly Slider _slider;
        private readonly Label _valueLabel;

        private bool _isAttached;
        private bool _isSynchronizing;

        public TimeScaleToolbarElement()
        {
            name = TimeScaleToolbarSettings.ElementName;
            tooltip = TimeScaleToolbarSettings.Tooltip;
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;
            AddToClassList(TimeScaleToolbarSettings.ToolbarSettingsClass);

            Add(new Label(TimeScaleToolbarSettings.LabelText));

            _slider = new Slider(
                TimeScaleToolbarSettings.MinimumTimeScale,
                TimeScaleToolbarSettings.MaximumTimeScale)
            {
                value = TimeScaleToolbarSettings.DefaultTimeScale,
                tooltip = TimeScaleToolbarSettings.Tooltip
            };
            _slider.style.width = TimeScaleToolbarSettings.SliderWidth;
            _slider.RegisterValueChangedCallback(OnSliderValueChanged);
            Add(_slider);

            _valueLabel = new Label();
            _valueLabel.style.width = TimeScaleToolbarSettings.ValueLabelWidth;
            _valueLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            Add(_valueLabel);

            var resetButton = new EditorToolbarButton(ResetTimeScale)
            {
                text = TimeScaleToolbarSettings.ResetButtonText,
                tooltip = TimeScaleToolbarSettings.ResetTooltip
            };
            Add(resetButton);

            RegisterCallback<AttachToPanelEvent>(OnAttachedToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachedFromPanel);
            RefreshState();
        }

        private void OnAttachedToPanel(AttachToPanelEvent attachEvent)
        {
            if (_isAttached)
            {
                return;
            }

            _isAttached = true;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            RefreshState();
        }

        private void OnDetachedFromPanel(DetachFromPanelEvent detachEvent)
        {
            if (!_isAttached)
            {
                return;
            }

            _isAttached = false;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            SetRuntimeSynchronization(false);
        }

        private void OnSliderValueChanged(ChangeEvent<float> changeEvent)
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            var timeScale = TimeScaleToolbarSettings.Clamp(changeEvent.newValue);
            Time.timeScale = timeScale;
            UpdateValueLabel(timeScale);
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                Time.timeScale = TimeScaleToolbarSettings.DefaultTimeScale;
            }

            RefreshState();
        }

        private void ResetTimeScale()
        {
            if (!EditorApplication.isPlaying)
            {
                return;
            }

            Time.timeScale = TimeScaleToolbarSettings.DefaultTimeScale;
            SetSliderValue(TimeScaleToolbarSettings.DefaultTimeScale);
        }

        private void RefreshState()
        {
            var isPlaying = EditorApplication.isPlaying;
            SetEnabled(isPlaying);
            SetSliderValue(
                isPlaying
                    ? TimeScaleToolbarSettings.Clamp(Time.timeScale)
                    : TimeScaleToolbarSettings.DefaultTimeScale);
            SetRuntimeSynchronization(isPlaying && _isAttached);
        }

        private void SetRuntimeSynchronization(bool shouldSynchronize)
        {
            if (_isSynchronizing == shouldSynchronize)
            {
                return;
            }

            _isSynchronizing = shouldSynchronize;
            if (shouldSynchronize)
            {
                EditorApplication.update += SynchronizeWithRuntime;
                return;
            }

            EditorApplication.update -= SynchronizeWithRuntime;
        }

        private void SynchronizeWithRuntime()
        {
            if (!EditorApplication.isPlaying)
            {
                SetRuntimeSynchronization(false);
                return;
            }

            var runtimeTimeScale = TimeScaleToolbarSettings.Clamp(Time.timeScale);
            if (TimeScaleToolbarSettings.IsSynchronized(_slider.value, runtimeTimeScale))
            {
                return;
            }

            SetSliderValue(runtimeTimeScale);
        }

        private void SetSliderValue(float value)
        {
            var clampedValue = TimeScaleToolbarSettings.Clamp(value);
            _slider.SetValueWithoutNotify(clampedValue);
            UpdateValueLabel(clampedValue);
        }

        private void UpdateValueLabel(float value)
        {
            _valueLabel.text = TimeScaleToolbarSettings.Format(value);
        }
    }
}
