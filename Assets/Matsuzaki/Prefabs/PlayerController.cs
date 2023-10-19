using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : MonoBehaviour
{
    ///<summary>移動速度</summary>
    public float _velocity = 5.0f;
    ///<summary>ジャンプする力</summary>
    public float _jumpPower = 5.0f;
    ///<summary>ジャンプ用のY軸ベクトル</summary>
    public float _jumpVelocity;
    ///<summary>水平方向の入力値</summary>
    private float _inputX;
    ///<summary>垂直方向の入力値</summary>
    private float _inputZ;
    ///<summary>入力値の絶対値</summary>
    private float _speed;
    ///<summary>プレイヤーの移動方向を格納する変数</summary>
    private Vector3 _desiredMoveDirection;
    ///<summary>移動方向の算出に用いる定数</summary>
    public float desiredRotationSpeed = 0.1f;
    ///<summary>プレイヤー移動の閾値</summary>
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

    ///<summary>プレイヤーを移動させる関数</summary>
    void PlayerMoveAndRotation()
    {
        // 入力値を取得
        _inputX = Input.GetAxis("Horizontal");
        _inputZ = Input.GetAxis("Vertical");
        
        // カメラのローカルなベクトルを取得
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        // Yベクトルを0に更新
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        _desiredMoveDirection = forward * _inputZ + right * _inputX;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_desiredMoveDirection), desiredRotationSpeed);
        controller.Move(_desiredMoveDirection * Time.deltaTime * _velocity);
    }

    ///<summary>プレイヤーの入力値が閾値に達しているかを判断する関数</summary>
    void InputMagnitude()
    {
        _inputX = Input.GetAxis("Horizontal");
        _inputZ = Input.GetAxis("Vertical");

        _speed = new Vector2(_inputX, _inputZ).sqrMagnitude;

        if (_speed > allowPlayerRotation) PlayerMoveAndRotation();
    }

    ///<summary>プレイヤーをジャンプさせる関数</summary>
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
