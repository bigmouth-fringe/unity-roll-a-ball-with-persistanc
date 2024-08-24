using MainMenu;
using Persistence;
using Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenu : Menu.Menu
    {
        [SerializeField]
        [Header("Menu Navigation")]
        private SaveSlotsMenu saveSlotsMenu;

        [SerializeField]
        [Header("Menu Buttons")]
        private Button newGameButton;

        [SerializeField]
        private Button loadGameButton;

        [SerializeField]
        private Button continueButton;

        private void Start()
        {
            if (!PersistenceManager.Instance.HasSavedState)
            {
                loadGameButton.interactable = false;
                continueButton.interactable = false;
            }
        }

        public void OnNewGameClick()
        {
            saveSlotsMenu.Activate(isLoading: false);
            Deactivate();
        }

        public void OnLoadGameClick()
        {
            saveSlotsMenu.Activate(isLoading: true);
            Deactivate();
        }

        public void OnContinueClick()
        {
            Disable();
            SceneManager.LoadSceneAsync(Scenes.Main);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void Disable()
        {
            newGameButton.interactable = false;
            continueButton.interactable = false;
        }
    }
}
