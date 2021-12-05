using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinischCollision : MonoBehaviour
{
    [SerializeField] private GameObject blot;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Instantiate Blote
            var newBlot = Instantiate(blot, new Vector3(other.gameObject.transform.position.x,transform.position.y + 0.31f, other.gameObject.transform.position.z) , Quaternion.identity);
            newBlot.transform.parent = transform;
        }
    }
}
