using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    ///<summary>�ړ����x</summary>
    public float _velocity = 5.0f;
    ///<summary>�W�����v�����</summary>
    public float _jumpPower = 5.0f;
    ///<summary>�W�����v�p��Y���x�N�g��</summary>
    public float _jumpVelocity;
    ///<summary>���������̓��͒l</summary>
    private float _inputX;
    ///<summary>���������̓��͒l</summary>
    private float _inputZ;
    ///<summary>���͒l�̐�Βl</summary>
    private float _speed;
    ///<summary>�v���C���[�̈ړ��������i�[����ϐ�</summary>
    private Vector3 _desiredMoveDirection;
    ///<summary>�ړ������̎Z�o�ɗp����萔</summary>
    public float desiredRotationSpeed = 0.1f;
    ///<summary>�v���C���[�ړ���臒l</summary>
    public float allowPlayerRotation = 0.1f;

    private Camera cam;
    private CharacterController controller;
    private Animator anim;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        cam = Camera.main;
        controller = this.GetComponent<CharacterController>();
    }
    void Update()
    {
        InputMagnitude();
        Jump();
    }

    ///<summary>�v���C���[���ړ�������֐�</summary>
    void PlayerMoveAndRotation()
    {
        // ���͒l���擾
        _inputX = Input.GetAxis("Horizontal");
        _inputZ = Input.GetAxis("Vertical");
        
        // �J�����̃��[�J���ȃx�N�g�����擾
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        // Y�x�N�g����0�ɍX�V
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        _desiredMoveDirection = forward * _inputZ + right * _inputX;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_desiredMoveDirection), desiredRotationSpeed);
        controller.Move(_desiredMoveDirection * Time.deltaTime * _velocity);
    }

    ///<summary>�v���C���[�̓��͒l��臒l�ɒB���Ă��邩�𔻒f����֐�</summary>
    void InputMagnitude()
    {
        _inputX = Input.GetAxis("Horizontal");
        _inputZ = Input.GetAxis("Vertical");

        _speed = new Vector2(_inputX, _inputZ).sqrMagnitude;

        if (_speed > allowPlayerRotation) PlayerMoveAndRotation();
    }

    ///<summary>�v���C���[���W�����v������֐�</summary>
    void Jump()
    {
        if (_jumpVelocity > Physics.gravity.y)
        {
            _jumpVelocity += Physics.gravity.y * Time.deltaTime;
        }

        controller.Move(new Vector3(0, _jumpVelocity, 0) * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Jump");
            _jumpVelocity = _jumpPower;
        }
    }
}
