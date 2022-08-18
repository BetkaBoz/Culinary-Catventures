using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGrabber : MonoBehaviour
{
    [SerializeField] private Sprite handOpen;
    [SerializeField] private Sprite handClosed;
    [SerializeField] private float speed = 1;
    [SerializeField] private float speedAfterCatchModifier = 1;
    [SerializeField] private float battleThreshold;

    private LineRenderer lr;
    private SpriteRenderer hand;
    private Vector3 challengePosition;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        hand = GetComponent<SpriteRenderer>();
        hand.sprite = handOpen;

        challengePosition = GameObject.FindGameObjectWithTag("Challenge").transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("PlayerCharacter").transform;
        transform.position = challengePosition;

        lr = GetComponent<LineRenderer>();

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) },
            new[] { new GradientAlphaKey(0.7f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lr.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsChallenge();
        CatchPlayer();
        RendererLine();
        StartBattleIfClose();
    }

    private void StartBattleIfClose()
    {
        if (Vector2.Distance(transform.position, challengePosition) < battleThreshold)
        {
            playerTransform.gameObject.GetComponent<PlayerControl>().ReleasePlayer();
            SceneManager.LoadScene("Battle", LoadSceneMode.Single);
        }
    }

    private void RendererLine()
    {
        lr.SetPosition(0, challengePosition);
        lr.SetPosition(1, transform.position);
    }

    private void CatchPlayer()
    {
        var position = transform.position;
        position = Vector2.MoveTowards(position, playerTransform.position, speed * Time.deltaTime);
        //ABY BOLA VIDITELNA RUKA
        position = new Vector3(position.x, position.y, 20);
        transform.position = position;

        if (Vector2.Distance(transform.position, playerTransform.position) < 0.5)
        {
            hand.sprite = handClosed;

            float newSpeed = speed * speedAfterCatchModifier;
            playerTransform.gameObject.GetComponent<PlayerControl>().DragPlayer(newSpeed);
            this.speed = newSpeed;
        }
    }

    private void RotateTowardsChallenge()
    {
        Vector2 direction = challengePosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5 * Time.deltaTime);
        transform.rotation = rotation;
    }
}
