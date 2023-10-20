using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]  
public class EnemyController2 : MonoBehaviour
{
    Transform _target;
    bool isChase = false;
    EnmeyState _enemyState = EnmeyState.None;
    //õ“G”ÍˆÍ
    [SerializeField] float _enemySerchArea = 5.0f;
    //UŒ‚”ÍˆÍ
    [SerializeField] float _enemyAttackArea = 1.0f;
    [SerializeField] float _enemySpeed = 5.0f;
    Rigidbody _rb;
    bool isAttack = false;
    /// <summary>UŒ‚Œã‚ÌƒCƒ“ƒ^[ƒoƒ‹ </summary>
    [SerializeField] int _interval = 3;
    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
        if (_enemyState == EnmeyState.Chase)
        {
            transform.LookAt(Vector3.Lerp(transform.forward + transform.position, _target.transform.position, 0.02f), Vector3.up);
            _rb.velocity = (_target.transform.position - this.transform.position).normalized * _enemySpeed;
            //_rb.AddForce((_target.transform.position - this.transform.position).normalized * _enemySpeed);
        }
        else if(_enemyState == EnmeyState.Attack && !isAttack)
        {
            _rb.velocity = new Vector3(0, 0, 0);
            StartCoroutine(Attack());
        }
        else if(_enemyState == EnmeyState.None)
        {
            _rb.velocity = new Vector3(0, 0, 0);
        }

        Debug.Log(_enemyState);
    }

    private IEnumerator Attack()
    {
        //UŒ‚ˆ—
        _animator.SetBool("isMove", true);
        isAttack = true;
        transform.LookAt(Vector3.Lerp(transform.forward + transform.position, _target.transform.position, 0.02f), Vector3.up);
        Ray ray = new Ray(transform.forward, _target.transform.position);
        //if(Physics.Raycast(ray, out RaycastHit hit, _enemyAttackArea))
        //{
        //    if(hit.collider.gameObject.CompareTag("Player"))
        //    {
        //        //UŒ‚‚ª“–‚½‚Á‚½‚Ìˆ—‚ğ‘‚­
        //        Debug.Log("ƒqƒbƒg");
        //    }
        //}

        yield return new WaitForSeconds(_interval);
        isAttack = false;
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
            _enemyState = EnmeyState.Chase;
            _animator.SetBool("isMove", true);
        }
        else
        {
            _enemyState= EnmeyState.None;
            _animator.SetBool("isMove", false);
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
