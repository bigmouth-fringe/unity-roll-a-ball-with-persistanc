using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Persistence
{
    public class FileSavingService
    {
        private readonly string _dirPath;
        private readonly string _fileName;

        private string SlotPath(string slotId) => Path.Combine(_dirPath, slotId);

        private string FullPath(string slotId) => Path.Combine(SlotPath(slotId), _fileName);

        public FileSavingService(string dirPath, string fileName)
        {
            _dirPath = dirPath;
            _fileName = fileName;
        }

        public Dictionary<string, GameState> LoadAllSlots() =>
            Directory
                .GetDirectories(_dirPath)
                .Select(Path.GetFileName)
                .Where(slotId => File.Exists(FullPath(slotId)))
                .ToDictionary(slotId => slotId, Load);

        public GameState Load(string slotId)
        {
            if (string.IsNullOrEmpty(slotId))
                return null;

            var fullPath = FullPath(slotId);
            if (!File.Exists(fullPath))
                return null;
            using var stream = new FileStream(fullPath, FileMode.Open);
            using var reader = new StreamReader(stream);
            var serialized = reader.ReadToEnd();
            return JsonUtility.FromJson<GameState>(serialized);
        }

        public void Save(string slotId, GameState state)
        {
            if (string.IsNullOrEmpty(slotId) || state is null)
                return;

            var slotPath = SlotPath(slotId);
            Directory.CreateDirectory(slotPath);

            var fullPath = FullPath(slotId);
            var serialized = JsonUtility.ToJson(state, prettyPrint: true);
            using var stream = new FileStream(fullPath, FileMode.Create);
            using var writer = new StreamWriter(stream);
            writer.Write(serialized);
        }

        public string GetMostRecentSlotId() =>
            LoadAllSlots()
                .OrderByDescending(s => s.Value.lastUpdatedAt)
                .Select(s => s.Key)
                .FirstOrDefault();
    }
}
