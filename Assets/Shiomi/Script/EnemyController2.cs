using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyController2 : MonoBehaviour
{
    Transform _target;
    bool isChase = false;
    EnmeyState _enemyState = EnmeyState.None;
    //õ“G”ÍˆÍ
    [SerializeField] float _enemySerchArea = 5.0f;
    //UŒ‚”ÍˆÍ
    [SerializeField] float _enemyAttackArea = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_enemyState == EnmeyState.Chase)
        {
            transform.LookAt(Vector3.Lerp(transform.forward + transform.position, _target.transform.position, 0.02f), Vector3.up);
        }
    }

    //ƒvƒŒƒCƒ„[‚ªõ“G”ÍˆÍ‚É“ü‚Á‚½‚Ìˆ—
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isChase = true;
        }
    }

    void CheckDistance()
    {
        float diff = (_target.position - transform.position).sqrMagnitude;

        if(diff < _enemyAttackArea * _enemyAttackArea)
        {
            _enemyState = EnmeyState.Attack;
        }
        else if(diff < _enemySerchArea * _enemySerchArea)
        {
            ene
        }
    }

    public enum EnmeyState
    {
        None,
        Chase,
        Attack,
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, _enemySerchArea);//UŒ‚”ÍˆÍ‚ÌƒMƒYƒ‚‚ğ•`Ê
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _enemyAttackArea);//•â‘«”ÍˆÍ‚ÌƒMƒYƒ‚‚ğ•`Ê
    }

}
