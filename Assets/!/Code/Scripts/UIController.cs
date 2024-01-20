using GameplayNS;
using GameplayNS.CubeTowerNS;
using SoundNS;
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
        private Label scoreLabel;
        private Label scoreOnLostScreenLabel;
        private Label highestScoreLabel;
        private Label newHighestScoreLabel;

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
            RegisterButtonClickSound(tryAgainButton);
            RegisterButtonClickSound(musicMuteButton);
            RegisterHandSpriteAnimation();
            RegisterNewHighestScoreAnimation();
            RegisterStartGameButtonEvents();
            RegisterTryAgainButtonEvents();
            RegisterMuteMusicButtonEvents();
            SetMuteMusicButtonUI();
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
        private void RegisterHandSpriteAnimation()
        {
            handSprite.RegisterCallback<TransitionEndEvent>
            (
                evt => handSprite.ToggleInClassList(HAND_ANIMATION_CLASS)
            );
            //First invoke delay, the delay is needed for everything to be initialized, 100 ms is enough
            handSprite.schedule.Execute(() => handSprite.ToggleInClassList(HAND_ANIMATION_CLASS)).StartingIn(100);
        }
        private void RegisterNewHighestScoreAnimation()
        {
            newHighestScoreLabel.RegisterCallback<TransitionEndEvent>
            (
                evt => newHighestScoreLabel.ToggleInClassList(NEW_SCORE_ANIMATION_CLASS)
            );
            newHighestScoreLabel.schedule.Execute(() => newHighestScoreLabel.ToggleInClassList(NEW_SCORE_ANIMATION_CLASS)).StartingIn(100);
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
        private void RegisterMuteMusicButtonEvents()
        {
            musicMuteButton.RegisterCallback<ClickEvent>(
              e =>
              {
                  SoundManager.Instance.ToggleMusicState();
                  SetMuteMusicButtonUI();
              },
              TrickleDown.TrickleDown);
        }
        private void SetMuteMusicButtonUI() =>
            musicMuteButton.EnableInClassList(MUTE_MUSIC_BUTTON_ANIMATION_CLASS, SoundManager.Instance.IsMusicMuted);
        private void RegisterTryAgainButtonEvents()
        {
            tryAgainButton.RegisterCallback<ClickEvent>(
               e =>
               {
                   ResetGame();
               },
               TrickleDown.TrickleDown);
        }
        private void ResetGame()
        {
            gameManager.ResetGame();
            EnableLostMenu(false);
        }
        private void StartGame()
        {
            gameManager.StartGame();
            startGameMenuCanvas.visible = false;
            scoreCanvas.visible = true;
        }
        private void OnLostGame()
        {
            if (CubeTower.Instance.CheckIfGameLost())
            {
                EnableLostMenu(true);
                ScoreOnLostGame();
                scoreCanvas.visible = false;
            }
        }
        private void EnableLostMenu(bool enable)
        {
            lostMenuCanvas.visible = enable;
        }
        private void ScoreOnLostGame()
        {
            bool isCurrentScoreNewRecord = gameManager.CurrentScore > gameManager.HighestScore;
            int currentScore = gameManager.CurrentScore;
            int highestScore = isCurrentScoreNewRecord ? currentScore : gameManager.HighestScore;
            newHighestScoreLabel.visible = isCurrentScoreNewRecord;
            scoreOnLostScreenLabel.text = "YOUR SCORE: " + currentScore.ToString();
            highestScoreLabel.text = "HIGHEST SCORE: " + highestScore.ToString();
        }
        private void ChangeActiveUIScore()
        {
            scoreLabel.text = gameManager.CurrentScore.ToString();
        }
    }
}