using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    /// <summary> AIのNavMeshAgent </summary>
    NavMeshAgent _navMesh;
    /// <summary> AIの追跡するプレイヤーのトランスフォーム </summary>
    Transform _playerTransform;
    /// <summary> Raycastのオーバーロードのレイヤーマスクに渡すため </summary>
    [SerializeField] LayerMask _groundLayer, _playerLayer;
    /// <summary> 追跡中かの判定 </summary>
    public bool _isChasing;
    //徘徊
    Vector3 _movePoint = Vector3.zero;
    bool _movePointIsSet = false;
    [SerializeField] float _movePointRange = 0;
    //攻撃
    [SerializeField] float _intervalAttack = 0;
    bool _isAttacked = false;
    //状態
    [SerializeField] float _sightRange, _attackRange;
    bool _playerFound, _playerCanAttack;
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform != null)
            this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform;//プレイヤーの検索
        if (this.gameObject.GetComponent<NavMeshAgent>() != null)
            this._navMesh = this.gameObject.GetComponent<NavMeshAgent>();//NavMeshのコンポーネント取得
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navMesh = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        //攻撃か補足範囲の検索
        this._playerFound = Physics.CheckSphere(this.gameObject.transform.position, this._sightRange, this._playerLayer);
        this._playerCanAttack = Physics.CheckSphere(this.gameObject.transform.position, this._attackRange, this._playerLayer);
        if (!this._playerFound && !this._playerCanAttack)//もし補足範囲と攻撃範囲内にいない場合徘徊する
            PatrollingNow();
        if (this._playerFound && !this._playerCanAttack)//もし補足だけできたらプレイヤーを追跡する
            ChaseWithPlayer();
        if (this._playerFound && this._playerCanAttack)//もしプレイヤーに追いついて攻撃範囲内にいる場合
            AttackPlayerNow();
    }

    void PatrollingNow()
    {
        if (!this._movePointIsSet)
            SearchMovePoint();//徘徊する座標を見つける
        if (this._movePointIsSet)
            this._navMesh.SetDestination(this._movePoint);//見つけたらそこに向かう
        Vector3 distance = transform.position - _movePoint;//徘徊する目標の座標との距離
        if (distance.magnitude > 1)
            this._movePointIsSet = false;//移動するフラグを外す
        //追跡中フラグ設定
        this._isChasing = false;
        Debug.Log("Walking");
    }
    void SearchMovePoint()
    {
        //X、Z軸での徘徊の目標の座標を乱数で発生させる
        float randX = Random.Range(-this._movePointRange, this._movePointRange);
        float randZ = Random.Range(-this._movePointRange, this._movePointRange);
        //Vector3にする
        this._movePoint = new Vector3(transform.position.x + randX, transform.position.y, this.gameObject.transform.position.z + randZ);
        //床にRaycastして光線が当たればそこに行けるので徘徊のフラグを立てる
        if (Physics.Raycast(this._movePoint, -transform.up, 2f, this._groundLayer))
            this._movePointIsSet = true;
    }

    void ChaseWithPlayer()
    {
        //プレイヤーの追尾
        this._navMesh.SetDestination(this._playerTransform.position);
        this._isChasing = true;//追跡中フラグ設定
    }

    void AttackPlayerNow()
    {
        //その時にいる座標にとどまる
        this._navMesh.SetDestination(this.gameObject.transform.position);
        //プレイヤーを向く
        this.gameObject.transform.LookAt(this._playerTransform.position);
        if (!this._isAttacked)
        {
            //攻撃処理をしたので攻撃をしたフラグを立てる
            this._isAttacked = true;
            //フラグを時差で立てる
            Invoke(nameof(RasetAttackCondition), this._intervalAttack);
        }
    }
    void RasetAttackCondition()
    {
        this._isAttacked = false;
    }
    void DestroyThisObject()
    {
        Destroy(this.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, this._attackRange);//攻撃範囲のギズモを描写
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this._sightRange);//補足範囲のギズモを描写
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(this.gameObject.transform.position, Vector3.one * 10);
    }
}