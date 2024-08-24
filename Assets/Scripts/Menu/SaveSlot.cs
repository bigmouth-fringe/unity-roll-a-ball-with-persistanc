using System;
using Persistence;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
    public class SaveSlot : MonoBehaviour
    {
        [SerializeField]
        [Header("Save Slot")]
        private string id;
        public string Id => id;

        [SerializeField]
        [Header("Content")]
        private GameObject noStateContent;

        [SerializeField]
        private GameObject hasStateContent;

        [SerializeField]
        private TextMeshProUGUI percentageComplete;

        [SerializeField]
        private TextMeshProUGUI collectedCount;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void SetData(GameState state)
        {
            if (state is null)
            {
                noStateContent.SetActive(true);
                hasStateContent.SetActive(false);
                return;
            }

            noStateContent.SetActive(false);
            hasStateContent.SetActive(true);
            percentageComplete.text = $"{state.PercentageComplete}% COMPLETE";
            collectedCount.text = $"COLLECTED: {state.collectedCount}";
        }

        public void SetInteractable(bool interactable)
        {
            button.interactable = interactable;
        }
    }
}
