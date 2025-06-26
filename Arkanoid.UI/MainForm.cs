using Arkanoid.Core;
using Timer = System.Windows.Forms.Timer;


namespace Arkanoid.UI
{

  public partial class MainForm : Form
  {
    private Ball ball; // Об'єкт м'яча.
    private Paddle paddle; // Об'єкт ракетки.
    private GameManager gameManager; // Об'єкт, що керує ігровою логікою.

    private Timer countdownTimer; // Таймер для зворотного відліку перед стартом гри.
    private int countdownValue; // Поточне значення зворотного відліку.
    private const int INITIAL_COUNTDOWN = 3; // Початкове значення зворотного відліку.

    // Ініціалізувати головну форму, її компоненти, розміри та ігрові об'єкти.
    public MainForm()
    {
      InitializeComponent(); // Ініціалізувати UI елементи, згенеровані дизайнером.

      // Налаштувати властивості вікна форми.
      this.ClientSize = new Size(800, 600); // Встановити розмір клієнтської області форми.
      this.FormBorderStyle = FormBorderStyle.FixedSingle; // Заборонити зміну розміру вікна.
      this.MaximizeBox = false; // Вимкнути кнопку "Розгорнути".
      this.DoubleBuffered = true; // Увімкнути подвійну буферизацію для плавного відображення графіки.
      this.Text = "Arkanoid"; // Встановити заголовок вікна.
      this.BackColor = Color.LightBlue; // Встановити колір фону форми.

      // Розрахувати висоту ігрової області (без урахування панелі управління).
      int gameAreaHeight = ClientSize.Height - controlPanel.Height;

      // Створити ігрові об'єкти: м'яча та ракетки.
      ball = new Ball(400, gameAreaHeight / 2, 20);
      paddle = new Paddle(ClientSize.Width / 2 - 50, ClientSize.Height - 30, 100, 20, Color.Green);

      // Створити менеджера гри, передаючи йому м'яч, ракетку та розміри ігрової області.
      gameManager = new GameManager(ball, paddle, ClientSize.Width, gameAreaHeight);

      // Підписатися на події фокусу форми для керування курсором миші.
      this.Enter += MainForm_Enter;
      this.Leave += MainForm_Leave;

      // Ініціалізувати таймер зворотного відліку.
      countdownTimer = new Timer();
      countdownTimer.Interval = 1000;
      countdownTimer.Tick += CountdownTimer_Tick;

      // Зупинити основний ігровий таймер на старті, до початку гри.
      gameTimer.Stop();
      UpdateUIState(GameState.Ready);
      gameManager.SetGameState(GameState.Ready);
    }

    // Обробляти подію Tick основного ігрового таймера.
    // Оновлювати логіку гри та перемальовувати форму.
    private void GameTimer_Tick(object sender, EventArgs e)
    {
      // Оновити стан всіх ігрових об'єктів (м'яч, цеглинки).
      gameManager.Update();

      // Перевірити стан гри після оновлення, щоб зупинити таймер та оновити UI відповідно.
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

      Invalidate(); // Запустити перемалювання форми, викликаючи OnPaint.
    }

    // Обробляти подію Tick таймера зворотного відліку.
    // Зменшувати значення відліку та оновлювати статусний напис. Після завершення відліку
    // запускати основний ігровий таймер.
    private void CountdownTimer_Tick(object sender, EventArgs e)
    {
      countdownValue--; // Зменшити значення відліку.
      if (countdownValue > 0)
      {
        statusLabel.Text = countdownValue.ToString(); // Відобразити поточне значення.
      }
      else
      {
        countdownTimer.Stop();
        gameTimer.Start();
        UpdateUIState(GameState.Playing);
        gameManager.SetGameState(GameState.Playing);
      }
      Invalidate(); // Перемалювати форму, щоб оновити напис відліку.
    }

    // Обробляти подію руху миші. Використовувати для переміщення ракетки.
    private void MainForm_MouseMove(object sender, MouseEventArgs e)
    {
      // Переміщувати ракетку лише якщо гра перебуває у стані "Playing".
      if (gameManager.CurrentState == GameState.Playing)
      {
        // Перемістити ракетку до позиції курсора миші, центрувати її відносно курсора.
        paddle.Move(e.X - paddle.Width / 2, ClientSize.Width);
      }
    }

    // Обробляти подію отримання формою фокусу. Ховати курсор миші.
    private void MainForm_Enter(object sender, EventArgs e)
    {
      Cursor.Hide(); // Приховати курсор миші.
    }

    // Обробляти подію втрати формою фокусу. Показувати курсор миші.
    private void MainForm_Leave(object sender, EventArgs e)
    {
      Cursor.Show(); // Показати курсор миші.
    }

    // Обробляти натискання кнопки "Start". Запустити зворотний відлік.
    private void StartButton_Click(object sender, EventArgs e)
    {
      StartCountdown(); // Викликати метод для запуску відліку.
    }

    // Обробляти натискання кнопки "Pause". Призупинити гру.
    private void PauseButton_Click(object sender, EventArgs e)
    {
      if (gameManager.CurrentState == GameState.Playing)
      {
        gameTimer.Stop();
        UpdateUIState(GameState.Paused);
        gameManager.SetGameState(GameState.Paused);
      }
    }

    // Обробляти натискання кнопки "Resume". Відновити гру після паузи.
    private void ResumeButton_Click(object sender, EventArgs e)
    {
      if (gameManager.CurrentState == GameState.Paused)
      {
        gameTimer.Start();
        UpdateUIState(GameState.Playing);
        gameManager.SetGameState(GameState.Playing);
      }
    }

    // Обробляти натискання кнопки "Restart". Запустити гру заново, починаючи з відліку.
    private void RestartButton_Click(object sender, EventArgs e)
    {
      StartCountdown(); // Викликати метод для запуску відліку, який також скидає гру.
    }

    // Ініціалізувати та запустити зворотний відлік перед початком гри.
    private void StartCountdown()
    {
      gameTimer.Stop();
      countdownTimer.Stop();

      // Скинути стан гри через GameManager, передаючи йому поточні розміри ігрової області.
      gameManager.Reset(ClientSize.Width, ClientSize.Height - controlPanel.Height);

      // Налаштування для відображення зворотного відліку.
      countdownValue = INITIAL_COUNTDOWN; // Встановити початкове значення відліку.
      statusLabel.Text = countdownValue.ToString(); // Встановити текст для статус-лейбла.
      statusLabel.Visible = true; // Зробити статус-лейбл видимим.
      statusLabel.BackColor = Color.FromArgb(150, 0, 0, 0); // Встановити напівпрозорий чорний фон.
      statusLabel.ForeColor = Color.White; // Встановити білий текст.
      // Встановити межі статус-лейбла так, щоб він займав всю ігрову область.
      statusLabel.Bounds = new Rectangle(0, controlPanel.Height, ClientSize.Width, ClientSize.Height - controlPanel.Height);

      // Деактивувати всі кнопки, поки йде відлік.
      startButton.Enabled = false;
      pauseButton.Enabled = false;
      resumeButton.Enabled = false;
      restartButton.Enabled = false;

      gameManager.SetGameState(GameState.Ready);
      countdownTimer.Start();
      Invalidate();
    }

    // Оновити стан елементів користувацького інтерфейсу (кнопок, статусних написів)
    // відповідно до поточного стану гри. Кнопки тепер завжди видимі, але їх активність 
    // (Enabled) змінюється.
    private void UpdateUIState(GameState state)
    {
      // Деактивувати всі кнопки.
      startButton.Enabled = false;
      pauseButton.Enabled = false;
      resumeButton.Enabled = false;
      restartButton.Enabled = false;

      statusLabel.Visible = false; // За замовчуванням статус-лейбл прихований.
      // Перемістити статус-лейбл на передній план, щоб він був видимим над ігровою зоною.
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

    // Малювати ігрові об'єкти на формі.
    // Викликається при необхідності перемалювати форму (наприклад, після Invalidate()).
    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e); // Викликати базовий метод для стандартного малювання форми.

      // Змістити систему координат графіки, щоб нульова точка була під controlPanel.
      // Це дозволяє малювати ігрові об'єкти, не враховуючи висоту панелі управління.
      e.Graphics.TranslateTransform(0, controlPanel.Height);

      ball.Draw(e.Graphics);
      paddle.Draw(e.Graphics);
      foreach (var brick in gameManager.Bricks)
      {
        brick.Draw(e.Graphics);
      }

      e.Graphics.ResetTransform(); // Скинути зміщення системи координат.

      // Оновити текст мітки рахунку.
      scoreLabel.Text = $"Score: {gameManager.Score}";
    }
  }
}