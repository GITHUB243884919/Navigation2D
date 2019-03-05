using UnityEngine;
using System.Collections;
namespace Game
{
    public class Move : MonoBehaviour
    {
        Vector3 dest;
        bool click = false;
        void Update()
        {

            if (Input.GetMouseButton(0))
            {
                click = true;
                dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            }
            if (Input.touchCount == 1)
            {
                click = true;
                dest = Camera.main.ScreenToWorldPoint(Input.touches[0].position);

            }

            if (click)
            {
                GetComponent<NavMeshAgent2D>().destination = dest;
                click = false;
            }

        }
    }
}
