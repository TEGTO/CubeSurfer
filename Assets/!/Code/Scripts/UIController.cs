using GameplayNS;
using GameplayNS.CubeTowerNS;
using SettingsNS;
using SoundNS;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMethodsNS;

namespace UiNS
{
    public class UIController : OnEnableMethodAfterStart
    {
        private const string HAND_ANIMATION_CLASS = "hand-right";
        private const string NEW_SCORE_ANIMATION_CLASS = "text-new-score-animated";
        private const string MUTE_MUSIC_BUTTON_ANIMATION_CLASS = "button-music-muted";
        private const string MUTE_VIBRATION_BUTTON_ANIMATION_CLASS = "button-vibration-muted";

        [SerializeField]
        private GameManager gameManager;
        [SerializeField]
        private UIDocument uIDocument;

        private VisualElement startGameMenuCanvas;
        private VisualElement handSprite;
        private VisualElement startGameButton;
        private VisualElement lostMenuCanvas;
        private Button tryAgainButton;
        private VisualElement scoreCanvas;
        private Button musicMuteButton;
        private Button vibrationMuteButton;
        private Label scoreLabel;
        private Label scoreOnLostScreenLabel;
        private Label highestScoreLabel;
        private Label newHighestScoreLabel;
        private bool isFirstFrame = false;

        protected override void OnEnableAfterStart()
        {
            CubeTower.Instance.OnCubeRemove += OnLostGame;
            CubeTower.Instance.OnCubeAdd += ChangeActiveUIScore;
        }
        private void OnDisable()
        {
            CubeTower.Instance.OnCubeRemove -= OnLostGame;
            CubeTower.Instance.OnCubeAdd -= ChangeActiveUIScore;
        }
        private new void Start()
        {
            base.Start();
            InitializeUIElements();
            ChangeActiveUIScore();
        }
        private void Update()
        {
            if (!isFirstFrame)
            {
                //To register any events related to the transition animation, you must wait for a first frame after start
                isFirstFrame = true;
                RegisterAnimationToggleClass(handSprite, HAND_ANIMATION_CLASS);
                RegisterAnimationToggleClass(newHighestScoreLabel, NEW_SCORE_ANIMATION_CLASS);
            }
        }
        private void InitializeUIElements()
        {
            var root = uIDocument.rootVisualElement;
            startGameMenuCanvas = root.Q<VisualElement>("StartGameMenuCanvas");
            handSprite = root.Q<VisualElement>("HandSprite");
            startGameButton = root.Q<VisualElement>("StartGameButton");
            lostMenuCanvas = root.Q<VisualElement>("LostMenuCanvas");
            tryAgainButton = root.Q<Button>("TryAgainButton");
            scoreOnLostScreenLabel = root.Q<Label>("ScoreOnLostScreen");
            scoreCanvas = root.Q<VisualElement>("ScoreCanvas");
            scoreLabel = root.Q<Label>("Score");
            highestScoreLabel = root.Q<Label>("HighestScore");
            newHighestScoreLabel = root.Q<Label>("NewHighestScore");
            musicMuteButton = root.Q<Button>("MuteMusicButton");
            vibrationMuteButton = root.Q<Button>("VibrationMuteButton");
            RegisterButtonClickSound(tryAgainButton);
            RegisterButtonClickSound(musicMuteButton);
            RegisterButtonClickSound(vibrationMuteButton);
            RegisterStartGameButtonEvents();
            RegisterButtonClickEvent(tryAgainButton, ResetGame);
            RegisterButtonClickEvent(musicMuteButton, MuteMusicButtonEvent);
            RegisterButtonClickEvent(vibrationMuteButton, MuteVibrationButtonEvent);
            SetMuteButtonUI(musicMuteButton, MUTE_MUSIC_BUTTON_ANIMATION_CLASS, Settings.IsMusicMuted.BoolState);
            SetMuteButtonUI(vibrationMuteButton, MUTE_VIBRATION_BUTTON_ANIMATION_CLASS, Settings.IsVibrationMuted.BoolState);
        }
        private void RegisterButtonClickSound(Button button)
        {
            button.RegisterCallback<ClickEvent>(
             e =>
             {
                 SoundManager.Instance.PlayUIButtonClick();
             },
             TrickleDown.TrickleDown);
        }
        private void RegisterAnimationToggleClass(VisualElement visualElement, string toggleNameClass)
        {
            visualElement.RegisterCallback<TransitionEndEvent>
            (
               evt => visualElement.ToggleInClassList(toggleNameClass)
            );
            //To apply any changes related to the transition animation, you must wait for a while
            visualElement.schedule.Execute(() => visualElement.ToggleInClassList(toggleNameClass)).StartingIn(100);
        }
        private void RegisterStartGameButtonEvents()
        {
            startGameButton.RegisterCallback<PointerDownEvent>(
                e =>
                {
                    StartGame();
                },
                TrickleDown.TrickleDown);
        }
        private void StartGame()
        {
            gameManager.StartGame();
            startGameMenuCanvas.visible = false;
            scoreCanvas.visible = true;
        }
        private void RegisterButtonClickEvent(Button button, Action clickEvent)
        {
            button.RegisterCallback<ClickEvent>(
               e =>
               {
                   clickEvent();
               },
               TrickleDown.TrickleDown);
        }
        private void ResetGame()
        {
            gameManager.ResetGame();
            EnableLostMenu(false);
        }
        private void MuteMusicButtonEvent()
        {
            Settings.IsMusicMuted.ToggleValue();
            SetMuteButtonUI(musicMuteButton, MUTE_MUSIC_BUTTON_ANIMATION_CLASS, Settings.IsMusicMuted.BoolState);
        }
        private void MuteVibrationButtonEvent()
        {
            Settings.IsVibrationMuted.ToggleValue();
            SetMuteButtonUI(vibrationMuteButton, MUTE_VIBRATION_BUTTON_ANIMATION_CLASS, Settings.IsVibrationMuted.BoolState);
        }
        private void SetMuteButtonUI(VisualElement visualElement, string className, bool value) =>
            visualElement.EnableInClassList(className, value);
        private void OnLostGame()
        {
            if (CubeTower.Instance.CheckIfGameLost())
            {
                EnableLostMenu(true);
                ScoreOnLostGame();
                scoreCanvas.visible = false;
            }
        }
        private void EnableLostMenu(bool enable) =>
            lostMenuCanvas.visible = enable;
        private void ScoreOnLostGame()
        {
            bool isCurrentScoreNewRecord = gameManager.CurrentScore > gameManager.HighestScore;
            int currentScore = gameManager.CurrentScore;
            int highestScore = isCurrentScoreNewRecord ? currentScore : gameManager.HighestScore;
            newHighestScoreLabel.visible = isCurrentScoreNewRecord;
            scoreOnLostScreenLabel.text = "YOUR SCORE: " + currentScore.ToString();
            highestScoreLabel.text = "HIGHEST SCORE: " + highestScore.ToString();
        }
        private void ChangeActiveUIScore() =>
            scoreLabel.text = gameManager.CurrentScore.ToString();
    }
}