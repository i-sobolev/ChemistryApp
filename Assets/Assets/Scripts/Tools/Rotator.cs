using UnityEngine;

namespace DrawFortress
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _axis = Vector3.right;
        [SerializeField] private float _speed = 1;

        private void Start()
        {
            transform.Rotate(_axis, Random.Range(0, 360));
        }

        private void Update()
        {
            transform.Rotate(_axis, _speed);
        }
    }
}
