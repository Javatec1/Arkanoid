using Arkanoid.Core;
using Timer = System.Windows.Forms.Timer;


namespace Arkanoid.UI
{

  public partial class MainForm : Form
  {
    private Ball ball; // ��'��� �'���.
    private Paddle paddle; // ��'��� �������.
    private GameManager gameManager; // ��'���, �� ���� ������� ������.

    private Timer countdownTimer; // ������ ��� ���������� ����� ����� ������� ���.
    private int countdownValue; // ������� �������� ���������� �����.
    private const int INITIAL_COUNTDOWN = 3; // ��������� �������� ���������� �����.

    // ������������ ������� �����, �� ����������, ������ �� ����� ��'����.
    public MainForm()
    {
      InitializeComponent(); // ������������ UI ��������, ���������� ����������.

      // ����������� ���������� ���� �����.
      this.ClientSize = new Size(800, 600); // ���������� ����� �볺������ ������ �����.
      this.FormBorderStyle = FormBorderStyle.FixedSingle; // ���������� ���� ������ ����.
      this.MaximizeBox = false; // �������� ������ "����������".
      this.DoubleBuffered = true; // �������� ������� ����������� ��� �������� ����������� �������.
      this.Text = "Arkanoid"; // ���������� ��������� ����.
      this.BackColor = Color.LightBlue; // ���������� ���� ���� �����.

      // ����������� ������ ������ ������ (��� ���������� ����� ���������).
      int gameAreaHeight = ClientSize.Height - controlPanel.Height;

      // �������� ����� ��'����: �'��� �� �������.
      ball = new Ball(400, gameAreaHeight / 2, 20);
      paddle = new Paddle(ClientSize.Width / 2 - 50, ClientSize.Height - 30, 100, 20, Color.Green);

      // �������� ��������� ���, ��������� ���� �'��, ������� �� ������ ������ ������.
      gameManager = new GameManager(ball, paddle, ClientSize.Width, gameAreaHeight);

      // ϳ��������� �� ��䳿 ������ ����� ��� ��������� �������� ����.
      this.Enter += MainForm_Enter;
      this.Leave += MainForm_Leave;

      // ������������ ������ ���������� �����.
      countdownTimer = new Timer();
      countdownTimer.Interval = 1000;
      countdownTimer.Tick += CountdownTimer_Tick;

      // �������� �������� ������� ������ �� �����, �� ������� ���.
      gameTimer.Stop();
      UpdateUIState(GameState.Ready);
      gameManager.SetGameState(GameState.Ready);
    }

    // ��������� ���� Tick ��������� �������� �������.
    // ���������� ����� ��� �� ��������������� �����.
    private void GameTimer_Tick(object sender, EventArgs e)
    {
      // ������� ���� ��� ������� ��'���� (�'��, ��������).
      gameManager.Update();

      // ��������� ���� ��� ���� ���������, ��� �������� ������ �� ������� UI ��������.
      if (gameManager.CurrentState == GameState.GameOver)
      {
        gameTimer.Stop();
        UpdateUIState(GameState.GameOver);
      }
      else if (gameManager.CurrentState == GameState.GameWon)
      {
        gameTimer.Stop();
        UpdateUIState(GameState.GameWon);
      }

      Invalidate(); // ��������� ������������� �����, ���������� OnPaint.
    }

    // ��������� ���� Tick ������� ���������� �����.
    // ���������� �������� ����� �� ���������� ��������� �����. ϳ��� ���������� �����
    // ��������� �������� ������� ������.
    private void CountdownTimer_Tick(object sender, EventArgs e)
    {
      countdownValue--; // �������� �������� �����.
      if (countdownValue > 0)
      {
        statusLabel.Text = countdownValue.ToString(); // ³��������� ������� ��������.
      }
      else
      {
        countdownTimer.Stop();
        gameTimer.Start();
        UpdateUIState(GameState.Playing);
        gameManager.SetGameState(GameState.Playing);
      }
      Invalidate(); // ������������ �����, ��� ������� ����� �����.
    }

    // ��������� ���� ���� ����. ��������������� ��� ���������� �������.
    private void MainForm_MouseMove(object sender, MouseEventArgs e)
    {
      // ����������� ������� ���� ���� ��� �������� � ���� "Playing".
      if (gameManager.CurrentState == GameState.Playing)
      {
        // ���������� ������� �� ������� ������� ����, ���������� �� ������� �������.
        paddle.Move(e.X - paddle.Width / 2, ClientSize.Width);
      }
    }

    // ��������� ���� ��������� ������ ������. ������ ������ ����.
    private void MainForm_Enter(object sender, EventArgs e)
    {
      Cursor.Hide(); // ��������� ������ ����.
    }

    // ��������� ���� ������ ������ ������. ���������� ������ ����.
    private void MainForm_Leave(object sender, EventArgs e)
    {
      Cursor.Show(); // �������� ������ ����.
    }

    // ��������� ���������� ������ "Start". ��������� ��������� ����.
    private void StartButton_Click(object sender, EventArgs e)
    {
      StartCountdown(); // ��������� ����� ��� ������� �����.
    }

    // ��������� ���������� ������ "Pause". ����������� ���.
    private void PauseButton_Click(object sender, EventArgs e)
    {
      if (gameManager.CurrentState == GameState.Playing)
      {
        gameTimer.Stop();
        UpdateUIState(GameState.Paused);
        gameManager.SetGameState(GameState.Paused);
      }
    }

    // ��������� ���������� ������ "Resume". ³������� ��� ���� �����.
    private void ResumeButton_Click(object sender, EventArgs e)
    {
      if (gameManager.CurrentState == GameState.Paused)
      {
        gameTimer.Start();
        UpdateUIState(GameState.Playing);
        gameManager.SetGameState(GameState.Playing);
      }
    }

    // ��������� ���������� ������ "Restart". ��������� ��� ������, ��������� � �����.
    private void RestartButton_Click(object sender, EventArgs e)
    {
      StartCountdown(); // ��������� ����� ��� ������� �����, ���� ����� ����� ���.
    }

    // ������������ �� ��������� ��������� ���� ����� �������� ���.
    private void StartCountdown()
    {
      gameTimer.Stop();
      countdownTimer.Stop();

      // ������� ���� ��� ����� GameManager, ��������� ���� ������ ������ ������ ������.
      gameManager.Reset(ClientSize.Width, ClientSize.Height - controlPanel.Height);

      // ������������ ��� ����������� ���������� �����.
      countdownValue = INITIAL_COUNTDOWN; // ���������� ��������� �������� �����.
      statusLabel.Text = countdownValue.ToString(); // ���������� ����� ��� ������-������.
      statusLabel.Visible = true; // ������� ������-����� �������.
      statusLabel.BackColor = Color.FromArgb(150, 0, 0, 0); // ���������� ������������ ������ ���.
      statusLabel.ForeColor = Color.White; // ���������� ���� �����.
      // ���������� ��� ������-������ ���, ��� �� ������ ��� ������ �������.
      statusLabel.Bounds = new Rectangle(0, controlPanel.Height, ClientSize.Width, ClientSize.Height - controlPanel.Height);

      // ������������ �� ������, ���� ��� ����.
      startButton.Enabled = false;
      pauseButton.Enabled = false;
      resumeButton.Enabled = false;
      restartButton.Enabled = false;

      gameManager.SetGameState(GameState.Ready);
      countdownTimer.Start();
      Invalidate();
    }

    // ������� ���� �������� ��������������� ���������� (������, ��������� ������)
    // �������� �� ��������� ����� ���. ������ ����� ������ �����, ��� �� ��������� 
    // (Enabled) ���������.
    private void UpdateUIState(GameState state)
    {
      // ������������ �� ������.
      startButton.Enabled = false;
      pauseButton.Enabled = false;
      resumeButton.Enabled = false;
      restartButton.Enabled = false;

      statusLabel.Visible = false; // �� ������������� ������-����� ����������.
      // ���������� ������-����� �� ������� ����, ��� �� ��� ������� ��� ������� �����.
      statusLabel.BringToFront();

      switch (state)
      {
        case GameState.Ready:
          startButton.Enabled = true;
          statusLabel.Text = "Press Start to Play!";
          statusLabel.Visible = true;
          statusLabel.BackColor = Color.FromArgb(150, 0, 0, 0);
          statusLabel.ForeColor = Color.White;
          statusLabel.Bounds = new Rectangle(0, controlPanel.Height, ClientSize.Width, ClientSize.Height - controlPanel.Height);
          break;

        case GameState.Playing:
          pauseButton.Enabled = true;
          restartButton.Enabled = true;
          statusLabel.Visible = false;
          statusLabel.BackColor = Color.Transparent;
          break;

        case GameState.Paused:
          resumeButton.Enabled = true;
          restartButton.Enabled = true;
          statusLabel.Text = "Paused";
          statusLabel.Visible = true;
          statusLabel.BackColor = Color.FromArgb(150, 0, 0, 0);
          statusLabel.ForeColor = Color.White;
          statusLabel.Bounds = new Rectangle(0, controlPanel.Height, ClientSize.Width, ClientSize.Height - controlPanel.Height);
          break;

        case GameState.GameOver:
          restartButton.Enabled = true;
          statusLabel.Text = $"Game Over!\nScore: {gameManager.Score}";
          statusLabel.Visible = true;
          statusLabel.BackColor = Color.FromArgb(150, 0, 0, 0);
          statusLabel.ForeColor = Color.Red;
          statusLabel.Bounds = new Rectangle(0, controlPanel.Height, ClientSize.Width, ClientSize.Height - controlPanel.Height);
          break;

        case GameState.GameWon:
          restartButton.Enabled = true;
          statusLabel.Text = $"You Win!\nScore: {gameManager.Score}";
          statusLabel.Visible = true;
          statusLabel.BackColor = Color.FromArgb(150, 0, 0, 0);
          statusLabel.ForeColor = Color.Green;
          statusLabel.Bounds = new Rectangle(0, controlPanel.Height, ClientSize.Width, ClientSize.Height - controlPanel.Height);
          break;
      }
    }

    // �������� ����� ��'���� �� ����.
    // ����������� ��� ����������� ������������ ����� (���������, ���� Invalidate()).
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e); // ��������� ������� ����� ��� ������������ ��������� �����.

      // ������� ������� ��������� �������, ��� ������� ����� ���� �� controlPanel.
      // �� �������� �������� ����� ��'����, �� ���������� ������ ����� ���������.
      e.Graphics.TranslateTransform(0, controlPanel.Height);

      ball.Draw(e.Graphics);
      paddle.Draw(e.Graphics);
      foreach (var brick in gameManager.Bricks)
      {
        brick.Draw(e.Graphics);
      }

      e.Graphics.ResetTransform(); // ������� ������� ������� ���������.

      // ������� ����� ���� �������.
      scoreLabel.Text = $"Score: {gameManager.Score}";
    }
  }
}