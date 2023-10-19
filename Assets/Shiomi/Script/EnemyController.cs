using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class EnemyController : MonoBehaviour
{
    /// <summary> AI��NavMeshAgent </summary>
    NavMeshAgent _navMesh;
    /// <summary> AI�̒ǐՂ���v���C���[�̃g�����X�t�H�[�� </summary>
    Transform _playerTransform;
    /// <summary> Raycast�̃I�[�o�[���[�h�̃��C���[�}�X�N�ɓn������ </summary>
    [SerializeField] LayerMask _groundLayer, _playerLayer;
    /// <summary> �ǐՒ����̔��� </summary>
    public bool _isChasing;
    //�p�j
    Vector3 _movePoint = Vector3.zero;
    bool _movePointIsSet = false;
    [SerializeField] float _movePointRange = 0;
    //�U��
    [SerializeField] float _intervalAttack = 0;
    bool _isAttacked = false;
    //���
    [SerializeField] float _sightRange, _attackRange;
    bool _playerFound, _playerCanAttack;
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").transform != null)
            this._playerTransform = GameObject.FindGameObjectWithTag("Player").transform;//�v���C���[�̌���
        if (this.gameObject.GetComponent<NavMeshAgent>() != null)
            this._navMesh = this.gameObject.GetComponent<NavMeshAgent>();//NavMesh�̃R���|�[�l���g�擾
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _navMesh = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        //�U�����⑫�͈͂̌���
        this._playerFound = Physics.CheckSphere(this.gameObject.transform.position, this._sightRange, this._playerLayer);
        this._playerCanAttack = Physics.CheckSphere(this.gameObject.transform.position, this._attackRange, this._playerLayer);
        if (!this._playerFound && !this._playerCanAttack)//�����⑫�͈͂ƍU���͈͓��ɂ��Ȃ��ꍇ�p�j����
            PatrollingNow();
        if (this._playerFound && !this._playerCanAttack)//�����⑫�����ł�����v���C���[��ǐՂ���
            ChaseWithPlayer();
        if (this._playerFound && this._playerCanAttack)//�����v���C���[�ɒǂ����čU���͈͓��ɂ���ꍇ
            AttackPlayerNow();
    }

    void PatrollingNow()
    {
        if (!this._movePointIsSet)
            SearchMovePoint();//�p�j������W��������
        if (this._movePointIsSet)
            this._navMesh.SetDestination(this._movePoint);//�������炻���Ɍ�����
        Vector3 distance = transform.position - _movePoint;//�p�j����ڕW�̍��W�Ƃ̋���
        if (distance.magnitude > 1)
            this._movePointIsSet = false;//�ړ�����t���O���O��
        //�ǐՒ��t���O�ݒ�
        this._isChasing = false;
        Debug.Log("Walking");
    }
    void SearchMovePoint()
    {
        //X�AZ���ł̜p�j�̖ڕW�̍��W�𗐐��Ŕ���������
        float randX = Random.Range(-this._movePointRange, this._movePointRange);
        float randZ = Random.Range(-this._movePointRange, this._movePointRange);
        //Vector3�ɂ���
        this._movePoint = new Vector3(transform.position.x + randX, transform.position.y, this.gameObject.transform.position.z + randZ);
        //����Raycast���Č�����������΂����ɍs����̂Ŝp�j�̃t���O�𗧂Ă�
        if (Physics.Raycast(this._movePoint, -transform.up, 2f, this._groundLayer))
            this._movePointIsSet = true;
    }

    void ChaseWithPlayer()
    {
        //�v���C���[�̒ǔ�
        this._navMesh.SetDestination(this._playerTransform.position);
        this._isChasing = true;//�ǐՒ��t���O�ݒ�
    }

    void AttackPlayerNow()
    {
        //���̎��ɂ�����W�ɂƂǂ܂�
        this._navMesh.SetDestination(this.gameObject.transform.position);
        //�v���C���[������
        this.gameObject.transform.LookAt(this._playerTransform.position);
        if (!this._isAttacked)
        {
            //�U�������������̂ōU���������t���O�𗧂Ă�
            this._isAttacked = true;
            //�t���O�������ŗ��Ă�
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
        Gizmos.DrawWireSphere(this.transform.position, this._attackRange);//�U���͈͂̃M�Y����`��
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, this._sightRange);//�⑫�͈͂̃M�Y����`��
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(this.gameObject.transform.position, Vector3.one * 10);
    }
}