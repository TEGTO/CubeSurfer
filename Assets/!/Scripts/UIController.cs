using GameplayNS;
using GameplayNS.CubeTowerNS;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMethodsNS;

namespace UiNS
{
    public class UIController : OnEnableMethodAfterStart
    {
        [SerializeField]
        private GameStatusManager gameStatusManager;
        [SerializeField]
        private UIDocument uIDocument;

        private VisualElement startGameMenuCanvas;
        private VisualElement handSprite;
        private VisualElement startGameButton;
        private VisualElement lostMenuCanvas;
        private VisualElement lostOverlay;
        private VisualElement tryAgainButton;
        private VisualElement scoreCanvas;
        private Label scoreOnLostScreen;
        private Label scoreLabel;

        protected override void OnEnableAfterStart()
        {
            CubeTower.Instance.OnCubeRemove += OnLostGame;
            CubeTower.Instance.OnCubeRemove += OnLostGame;
            CubeTower.Instance.OnCubeAdd += ChangeUIScore;
        }
        private void OnDisable()
        {
            CubeTower.Instance.OnCubeRemove -= OnLostGame;
            CubeTower.Instance.OnCubeAdd -= ChangeUIScore;
        }
        private new void Start()
        {
            base.Start();
            InitializeUIElements();
            ChangeUIScore();
        }
        private void InitializeUIElements()
        {
            var root = uIDocument.rootVisualElement;
            startGameMenuCanvas = root.Q<VisualElement>("StartGameMenuCanvas");
            handSprite = root.Q<VisualElement>("HandSprite");
            startGameButton = root.Q<VisualElement>("StartGameButton");
            lostMenuCanvas = root.Q<VisualElement>("LostMenuCanvas");
            lostOverlay = root.Q<VisualElement>("LostOverlay");
            tryAgainButton = root.Q<VisualElement>("TryAgainButton");
            scoreOnLostScreen = root.Q<Label>("ScoreOnLostScreen");
            scoreCanvas = root.Q<VisualElement>("ScoreCanvas");
            scoreLabel = root.Q<Label>("Score");
            RegisterHandSpriteAnimation();
            RegisterStartGameButtonEvents();
            RegisterTryAgainButtonEvents();
        }
        private void RegisterHandSpriteAnimation()
        {
            string handMoveRightClass = "hand-right";
            handSprite.RegisterCallback<TransitionEndEvent>
            (
                evt => handSprite.ToggleInClassList(handMoveRightClass)
            );
            //First invoke delay, the delay is needed for everything to be initialized, 100 ms is enough
            handSprite.schedule.Execute(() => handSprite.ToggleInClassList(handMoveRightClass)).StartingIn(100);
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
        private void RegisterTryAgainButtonEvents()
        {
            tryAgainButton.RegisterCallback<ClickEvent>(
               e =>
               {
                   ResetGame();
               },
               TrickleDown.TrickleDown);
        }
        private void StartGame()
        {
            gameStatusManager.StartGame();
            startGameMenuCanvas.visible = false;
            scoreCanvas.visible = true;
        }
        private void OnLostGame()
        {
            if (CubeTower.Instance.CheckIfGameLost())
            {
                EnableLostMenu(true);
                scoreCanvas.visible = false;
            }
        }
        private void ResetGame()
        {
            gameStatusManager.ResetGame();
            EnableLostMenu(false);
        }
        private void EnableLostMenu(bool enable)
        {
            lostMenuCanvas.visible = enable;
            lostOverlay.EnableInClassList("overlay-hidden", enable);
        }
        private void ChangeUIScore()
        {
            scoreLabel.text = gameStatusManager.CurrentScore.ToString();
            scoreOnLostScreen.text = gameStatusManager.CurrentScore.ToString();
        }
    }
}