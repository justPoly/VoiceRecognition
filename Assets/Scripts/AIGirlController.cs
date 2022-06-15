using UnityEngine;
using System.Collections;

namespace UnityChan
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class AIGirlController : MonoBehaviour
    {
        //private string AIMovingGirl = "AIMovingGirl";
        public GameObject objectToFollow;
        //private float comeToMeSpeed = 2f;
        private float stoppingDistance = 1.5f;

        public VoiceController voiceController;
        public float animSpeed = 1.5f;              // アニメーション再生速度設定
        public float lookSmoother = 3.0f;           // a smoothing setting for camera motion
        public bool useCurves = true;               // Mecanimでカーブ調整を使うか設定する
                                                    // このスイッチが入っていないとカーブは使われない
        public float useCurvesHeight = 0.5f;        // カーブ補正の有効高さ（地面をすり抜けやすい時には大きくする）

        // 以下キャラクターコントローラ用パラメタ
        // 前進速度
        public float forwardSpeed = 2.0f;
        // 後退速度
        public float backwardSpeed = 2.0f;
        // 旋回速度
        public float rotateSpeed = 2.0f;
        // ジャンプ威力
        public float jumpPower = 1.0f;
        // キャラクターコントローラ（カプセルコライダ）の参照
        private CapsuleCollider col;
        private Rigidbody rb;
        // キャラクターコントローラ（カプセルコライダ）の移動量
        private Vector3 velocity;
        // CapsuleColliderで設定されているコライダのHeiht、Centerの初期値を収める変数
        private float orgColHight;
        private Vector3 orgVectColCenter;
        private Animator anim;                          // キャラにアタッチされるアニメーターへの参照
        private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照

        //private GameObject cameraObject;	// メインカメラへの参照

        // アニメーター各ステートへの参照
        static int idleState = Animator.StringToHash("Base Layer.Idle");
        static int locoState = Animator.StringToHash("Base Layer.Locomotion");
        static int jumpState = Animator.StringToHash("Base Layer.Jump");
        static int restState = Animator.StringToHash("Base Layer.Rest");

        // 初期化
        void Start()
        {
            // Animatorコンポーネントを取得する
            anim = GetComponent<Animator>();
            // CapsuleColliderコンポーネントを取得する（カプセル型コリジョン）
            col = GetComponent<CapsuleCollider>();
            rb = GetComponent<Rigidbody>();
            //objectToFollow = GameObject.FindGameObjectWithTag(AIMovingGirl);
            //メインカメラを取得する
            //cameraObject = GameObject.FindWithTag ("MainCamera");
            // CapsuleColliderコンポーネントのHeight、Centerの初期値を保存する
            orgColHight = col.height;
            orgVectColCenter = col.center;
        }
        void FixedUpdate()
        {

            //CYJu start
            float h = 0;
            float v = 0;
            if (voiceController.robotWork == "come to me" || voiceController.robotWork == "come on")
            {
                if (objectToFollow != null)
                {
                    Vector3 tempPos = objectToFollow.transform.position;
                    tempPos.y = transform.position.y;

                    if (Vector3.Distance(transform.position, tempPos) > stoppingDistance)
                    {
                        var lookPos = objectToFollow.transform.position - transform.position;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);
                        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
                        v = 1f;
                    }
                    else
                    {
                        v = 0f;
                    }
                }
            }
            if (voiceController.robotWork == "left" || voiceController.robotWork == "turn left")
                h = -1f;
            if (voiceController.robotWork == "right" || voiceController.robotWork == "turn right")
                h = 1f;
            if (voiceController.robotWork == "run" || voiceController.robotWork == "let's run")
                v = 1f;
            if (voiceController.robotWork == "back" || voiceController.robotWork == "walk back")
                v = -1f;
            //CYJu End

            anim.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
            anim.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
            anim.speed = animSpeed;                             // Animatorのモーション再生速度に animSpeedを設定する
            currentBaseState = anim.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
            rb.useGravity = true;//ジャンプ中に重力を切るので、それ以外は重力の影響を受けるようにする

            velocity = new Vector3(0, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得
                                                    // キャラクターのローカル空間での方向に変換
            velocity = transform.TransformDirection(velocity);

            if (v > 0.1)
            {
                velocity *= forwardSpeed;       // 移動速度を掛ける
            }
            else if (v < -0.1)
            {
                velocity *= backwardSpeed;  // 移動速度を掛ける
            }

            transform.localPosition += velocity * Time.fixedDeltaTime;

            transform.Rotate(0, h * rotateSpeed, 0);

            // IDLE中の処理
            // 現在のベースレイヤーがidleStateの時
            if (currentBaseState.nameHash == idleState)
            {
                //カーブでコライダ調整をしている時は、念のためにリセットする
                if (useCurves)
                {
                    resetCollider();
                }
                // スペースキーを入力したらRest状態になる
                //if (Input.GetButtonDown("Jump"))
                if (voiceController.robotWork == "rest" || voiceController.robotWork == "take a rest")
                {
                    anim.SetBool("Rest", true);
                }
                if (voiceController.robotWork == "jump" || voiceController.robotWork == "jump jump")
                {
                    anim.SetBool("Jump", true);
                }
            }
            // REST中の処理
            else if (currentBaseState.nameHash == restState)
            {
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Rest", false);
                }
            }
            else if (currentBaseState.nameHash == jumpState)
            {
                //cameraObject.SendMessage("setCameraPositionFrontView");		// カメラを正面に切り替える
                // ステートが遷移中でない場合、Rest bool値をリセットする（ループしないようにする）
                if (!anim.IsInTransition(0))
                {
                    anim.SetBool("Jump", false);
                }
            }
        }
        void resetCollider()
        {
            // コンポーネントのHeight、Centerの初期値を戻す
            col.height = orgColHight;
            col.center = orgVectColCenter;
        }
    }
}