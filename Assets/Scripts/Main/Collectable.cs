using System;
using System.Linq;
using Persistence;
using UnityEngine;

namespace Main
{
    public class Collectable : MonoBehaviour, IPersisted
    {
        [SerializeField]
        private string id;

        [ContextMenu("Generate Id")]
        private void GenerateId() => id = Guid.NewGuid().ToString();

        // Update is called once per frame
        private void Update()
        {
            var rotation = new Vector3(15, 30, 45);
            transform.Rotate(rotation * Time.deltaTime);
        }

        public void Load(GameState state)
        {
            var collected = state.collectables.FirstOrDefault(c => c.id == id);
            if (collected is not null)
                gameObject.SetActive(false);
        }

        public void Save(ref GameState state)
        {
            var gatheredIndex = state.collectables.FindIndex(c => c.id == id);
            if (gatheredIndex != -1)
                state.collectables.RemoveAt(gatheredIndex);

            var collected = !gameObject.activeSelf;
            if (!collected)
                return;

            var gathered = new Persistence.Collectable { id = id, collected = true };
            state.collectables.Add(gathered);
        }
    }
}
