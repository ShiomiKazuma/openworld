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

    //���o�͈͂ɓ��������̏���
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = true;
        }
    }

    //���o�͈͂���o���Ƃ��̏���
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isActive = false;
        }
    }
}
