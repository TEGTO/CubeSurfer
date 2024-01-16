using GameplayNS;
using UnityEngine;
using UnityEngine.UIElements;

namespace UiNS
{
    public class UIController : MonoBehaviour
    {
        [SerializeField]
        private GameStatusManager gameStatusManager;
        [SerializeField]
        private UIDocument uIDocument;

        private VisualElement startGameMenuCanvas;
        private VisualElement handSprite;
        private VisualElement startGameButton;

        private void Start()
        {
            InitializeUIElements(); ;
        }
        private void InitializeUIElements()
        {
            var root = uIDocument.rootVisualElement;
            startGameMenuCanvas = root.Q<VisualElement>("StartGameMenuCanvas");
            handSprite = root.Q<VisualElement>("HandSprite");
            startGameButton = root.Q<VisualElement>("StartGameButton");
            RegisterHandSpriteAnimation();
            RegisterStartGameButtonEvents();
        }
        private void RegisterHandSpriteAnimation()
        {
            handSprite.RegisterCallback<TransitionEndEvent>
            (
                evt => handSprite.ToggleInClassList("hand-right")
            );
            //First invoke delay, the delay is needed for everything to be initialized, 100 ms is enough
            handSprite.schedule.Execute(() => handSprite.ToggleInClassList("hand-right")).StartingIn(100);
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
            gameStatusManager.StartGame();
            startGameMenuCanvas.visible = false;
        }
    }
}