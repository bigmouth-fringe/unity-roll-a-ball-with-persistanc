using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        [Header("First Selected Button")]
        private GameObject firstSelected;

        protected void OnEnable()
        {
            StartCoroutine(SetFirstSelected(firstSelected));
        }

        public IEnumerator SetFirstSelected(GameObject firstSelectedObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(firstSelectedObject);
        }
    }
}
