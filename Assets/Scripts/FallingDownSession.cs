using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

internal class FallingDownSession : MonoBehaviour, IPauseEventSource
{
    public static FallingDownSession Instance { get; private set; }
    private GameSession m_gameSession;
    private FallingDownPlayer[] m_players;
    private TimeUtil m_time_util;
    [SerializeField]
    private float m_logarithmVerticalScaler, m_logarithmHorizontalScaler,
        m_natrualDifficultyGrowth;
    [SerializeField] private GameObject m_playerPrefab, m_scanLinePrefab;
    private FallingDownScanLine m_scanLine;
    [SerializeField] private FallingDownItem[] m_itemList;
    [SerializeField] private Image m_gameOverPanel;
    [SerializeField] private Text m_depthText, m_recordText;
    [SerializeField] private Animator m_pauseObjcectAnimatorPrefab;
    private Animator m_pauseObjectAnimatorInstance;
    public event Action OnPause;
    public event Action OnUnpause;
    private float m_difficulty = 0.0f;
    public float Difficulty
        => MathUtils.EvaluateGrowthCurve(m_difficulty,
            m_logarithmHorizontalScaler, m_logarithmVerticalScaler);
    public FallingDownItem[] ItemList => m_itemList;
    public FallingDownGameState State { get; private set; }
    public IEnumerable<FallingDownPlayer> EnumeratePlayers()
    {
        Debug.Assert(m_players != null);
        return m_players;
    }
    public int GetMaxPlayerDepth()
    {
        return Mathf.FloorToInt(-Mathf.Min(0f,
                Mathf.Min(m_players[0].transform.position.y,
                m_players[1].transform.position.y)));
    }
    private void Awake()
    {
        Instance = this;
        //Time.timeScale = 2.0f;
        Init(GameSession.Instance);//TODO: init with a valid game session;
        m_time_util = new TimeUtil(this);
    }
    private void OnDestroy()
    {
        Instance = null;
        m_time_util.Dispose();
    }
    public void Init(GameSession parentSession)
    {
        //TODO: Implement the waiting
        State = FallingDownGameState.InGame;
        m_gameSession = parentSession;
        m_players = new FallingDownPlayer[2];

        m_players[0] = CreatePlayer(new Vector2(6, 3), Color.red,
            new PlayerInputDefault(KeyCode.A, KeyCode.D, KeyCode.W));
        m_players[1] = CreatePlayer(new Vector2(18, 3), Color.cyan,
            new PlayerInputDefault(KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow));

        m_scanLine = CreateScanLine();
    }
    private FallingDownPlayer CreatePlayer(Vector2 position, Color color, IPlayerInput input)
    {
        Debug.Assert(!(m_playerPrefab is null));

        var playerObject = Instantiate(m_playerPrefab);
        playerObject.GetComponent<SpriteRenderer>().color = color;
        playerObject.transform.position = position;
        var player = playerObject.GetComponent<FallingDownPlayer>();
        Debug.Assert(!(player is null));
        player.Init(input);
        return player;
    }
    private FallingDownScanLine CreateScanLine()
    {
        Debug.Assert(!(m_scanLinePrefab is null));
        var gm = Instantiate<GameObject>(m_scanLinePrefab);
        var scanLine = gm.GetComponent<FallingDownScanLine>();
        Debug.Assert(!(scanLine is null));
        return scanLine;
    }

    private void Update()
    {
        if (State == FallingDownGameState.InGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Pause();
            }
            //Debug.Log(m_difficulty);
            //Debug.Log(MathUtils.EvaluateGrowthCurve(m_difficulty, m_logarithmHorizontalScaler, m_logarithmVerticalScaler));
            AddDifficulty(m_natrualDifficultyGrowth * Time.deltaTime);
            if (m_scanLine.transform.position.y <= Mathf.Max(m_players[0].transform.position.y,
                m_players[1].transform.position.y))
            {
                EndGame();
            }
        }
        else if (State == FallingDownGameState.Paused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Unpause();
            }
        }
    }

    public void EndGame()
    {
        if (State == FallingDownGameState.InGame)
        {
            State = FallingDownGameState.Ended;
            Debug.Assert(m_depthText != null);
            Debug.Assert(m_gameOverPanel != null);
            m_gameOverPanel.gameObject.SetActive(true);
            int depth = Mathf.FloorToInt(-Mathf.Min(0f,
                Mathf.Min(m_players[0].transform.position.y,
                m_players[1].transform.position.y)));
            m_depthText.text = $"{depth}M";

            FallingDownRecord record;
            if (m_gameSession.Record.TryGetGameRecord<FallingDownRecord>("FallingDown", out record))
            {
                if (record.MaxDepth < depth)
                {
                    record.MaxDepth = depth;
                    m_gameSession.SyncRecord();
                }
            }
            else
            {
                record = new FallingDownRecord() { MaxDepth = depth };
                m_gameSession.Record.SetGameRecord("FallingDown", record);
                m_gameSession.SyncRecord();
            }

            Debug.Assert(!(m_recordText is null));
            m_recordText.text = $"{record.MaxDepth}M";
        }

        //TODO: display the end game window
    }

    public void AddDifficulty(float addition)
    {
        m_difficulty += addition;
    }

    public void Pause()
    {
        if (State == FallingDownGameState.InGame)
        {
            m_pauseObjectAnimatorInstance = Instantiate(m_pauseObjcectAnimatorPrefab,
                m_pauseObjcectAnimatorPrefab.transform.parent);
            m_pauseObjectAnimatorInstance.gameObject.SetActive(true);
            State = FallingDownGameState.Paused;
            OnPause?.Invoke();
        }
    }

    public void Unpause()
    {
        if (State == FallingDownGameState.Paused)
        {
            m_pauseObjectAnimatorInstance.SetBool("shouldExit", true);
            State = FallingDownGameState.InGame;
            OnUnpause?.Invoke();
        }
    }
}
