using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    Transform _target;
    bool isActive = false;
    NavMeshAgent _agent;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.updateRotation = false;
        _agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //検出範囲に入った時の処理
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = true;
        }
    }

    //検出範囲から出たときの処理
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = false;
        }
    }
}
