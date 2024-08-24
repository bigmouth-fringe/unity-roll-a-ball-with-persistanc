using UnityEngine;

namespace Main
{
    public class Camera : MonoBehaviour
    {
        public GameObject player;

        private Vector3 _offset;

        // Start is called before the first frame update
        private void Start()
        {
            _offset = transform.position - player.transform.position;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            transform.position = player.transform.position + _offset;
        }
    }
}
