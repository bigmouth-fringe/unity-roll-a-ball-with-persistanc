using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Persistence
{
    public partial class PersistenceManager : MonoBehaviour
    {
        public static PersistenceManager Instance { get; private set; }
        public bool HasSavedState => _state is not null;

        private List<IPersisted> _persisted = new();

        private FileSavingService _fileSavingService;

        private void Awake()
        {
            if (Instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            var dirPath = Application.persistentDataPath;
            _fileSavingService = new FileSavingService(dirPath, fileName);
            _slotId = _fileSavingService.GetMostRecentSlotId();
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
            SceneManager.sceneUnloaded += OnSceneUnload;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            SceneManager.sceneUnloaded -= OnSceneUnload;
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            _persisted = FindAllPersisted();
            LoadGame();
        }

        private void OnSceneUnload(Scene scene)
        {
            if (scene.name == Scenes.Menu)
                return;
            SaveGame();
        }

        private static List<IPersisted> FindAllPersisted() =>
            FindObjectsOfType<MonoBehaviour>().OfType<IPersisted>().ToList();
    }

    public partial class PersistenceManager
    {
        [SerializeField]
        [Header("File Storage Config")]
        private string fileName;

        private GameState _state;
        private string _slotId = string.Empty;

        public void NewGame()
        {
            _state = new GameState();
        }

        public void LoadGame()
        {
            var loaded = _fileSavingService.Load(_slotId);
            if (loaded is null)
                return;

            _state = loaded;
            foreach (var persisted in _persisted)
                persisted.Load(_state);
        }

        public void SaveGame()
        {
            if (_state is null)
                return;

            foreach (var persisted in _persisted)
                persisted.Save(ref _state);
            _state.lastUpdatedAt = DateTime.Now.ToBinary();
            _fileSavingService.Save(_slotId, _state);
        }

        public void ChangeCurrentSlot(string slotId)
        {
            _slotId = slotId;
            LoadGame();
        }

        public Dictionary<string, GameState> GetAllSlotStates() =>
            _fileSavingService.LoadAllSlots();
    }
}
