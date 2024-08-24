using Persistence;
using Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class SaveSlotsMenu : Menu.Menu
    {
        [SerializeField]
        [Header("Menu Navigation")]
        private MainMenu mainMenu;

        [SerializeField]
        [Header("Menu Buttons")]
        private Button backButton;

        private SaveSlot[] _saveSlots;

        private bool _isLoading;

        private void Awake()
        {
            _saveSlots = GetComponentsInChildren<SaveSlot>();
        }

        public void OnBackClick()
        {
            mainMenu.Activate();
            Deactivate();
        }

        public void OnSlotClick(SaveSlot slot)
        {
            DisableMenu();
            PersistenceManager.Instance.ChangeCurrentSlot(slot.Id);
            if (_isLoading)
                PersistenceManager.Instance.LoadGame();
            else
                PersistenceManager.Instance.NewGame();
            SceneManager.LoadSceneAsync(Scenes.Main);
        }

        public void Activate(bool isLoading)
        {
            gameObject.SetActive(true);
            _isLoading = isLoading;

            var slotStates = PersistenceManager.Instance.GetAllSlotStates();
            var firstSelected = backButton.gameObject;
            foreach (var saveSlot in _saveSlots)
            {
                slotStates.TryGetValue(saveSlot.Id, out var slotState);
                saveSlot.SetData(slotState);
                if (slotState is null && isLoading)
                {
                    saveSlot.SetInteractable(false);
                    continue;
                }

                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                    firstSelected = saveSlot.gameObject;
            }

            StartCoroutine(SetFirstSelected(firstSelected));
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void DisableMenu()
        {
            foreach (var saveSlot in _saveSlots)
                saveSlot.SetInteractable(false);
            backButton.interactable = false;
        }
    }
}
