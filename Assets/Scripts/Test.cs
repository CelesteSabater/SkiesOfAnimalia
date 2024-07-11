using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private Vector3 amountDir;
    [SerializeField] private Vector3 amountScale;
    [SerializeField] private float pokeForce;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GetComponent<SpringToTarget3D>().Nudge(amountDir);
            GetComponent<SpringToScale>().Nudge(amountScale);
        }
        /*if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.gameObject.name);
                GetComponent<SpringToTarget3D>().SpringTo(hit.point);
            }
            else
            {
                Debug.Log("Miss");
            }
        }*/
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForceAtPosition(ray.direction * pokeForce, hit.point);
                }
            }
            else
            {
                Debug.Log("Miss");
            }
        }
    }
}
