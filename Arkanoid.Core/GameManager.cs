using System;
using System.Collections.Generic;
using System.Drawing;

namespace Arkanoid.Core
{
  // Перелік можливих станів гри для контролю логіки
  public enum GameState
  {
    Ready,    // Гра готова до старту, очікувати дії гравця
    Playing,  // Гра активно триває, м'яч рухається
    Paused,   // Гру призупинено
    GameOver, // Гра завершилася поразкою гравця
    GameWon   // Гра завершилася перемогою гравця
  }

  public class GameManager
  {
    public Ball Ball { get; private set; } // Об'єкт м'яча
    public Paddle Paddle { get; private set; } // Об'єкт ракетки, якою гравець керує
    public List<Brick> Bricks { get; } = new List<Brick>(); // Колекція всіх цеглинок на ігровому полі
    public int Score { get; set; } // Поточний рахунок, набраний гравцем
    public bool IsGameOver { get; set; } // Флаг, що вказує на поразку гравця
    public bool IsGameWon { get; set; } // Флаг, що вказує на перемогу гравця
    public GameState CurrentState { get; private set; } // Поточний стан гри
    private readonly int formWidth; // Ширина ігрового вікна або області
    private readonly int formHeight; // Висота ігрового вікна або області
    private Random random; // Об'єкт для генерації випадкових значень, наприклад, для кольорів цеглинок

    // Ініціалізувати новий екземпляр GameManager, встановлюючи посилання на об'єкти та розміри
    public GameManager(Ball ball, Paddle paddle, int formWidth, int formHeight)
    {
      Ball = ball;
      Paddle = paddle;
      this.formWidth = formWidth;
      this.formHeight = formHeight;
      random = new Random();
      Reset(formWidth, formHeight); // Скинути гру до початкових налаштувань
    }

    // Скинути всі ігрові параметри
    public void Reset(int formWidth, int formHeight)
    {
      Score = 0;
      IsGameOver = false;
      IsGameWon = false;
      CurrentState = GameState.Ready;
      Bricks.Clear();

      Ball.ResetSpeed();
      Ball.X = formWidth / 2f; // Розмістити м'яч по центру горизонталі
      Ball.Y = formHeight / 2f; // Розмістити м'яч по центру вертикалі

      // Встановити початкову вертикальну швидкість м'яча випадковим чином
      Ball.SpeedY = (float)random.NextDouble() * (Ball.InitialSpeed / 2) + (Ball.InitialSpeed / 2);
      // Випадково обрати горизонтальний напрямок для м'яча (вліво або вправо)
      int invert = random.Next(0, 2) == 0 ? 1 : -1;
      // Обчислити горизонтальну швидкість, щоб підтримувати початкову загальну швидкість
      Ball.SpeedX = invert * (float)Math.Sqrt(Math.Pow(Ball.InitialSpeed, 2) - Math.Pow(Ball.SpeedY, 2));

      Paddle.X = formWidth / 2f - Paddle.Width / 2; // Розмістити ракетку по центру знизу
      Paddle.Y = formHeight - 30; // Встановити ракетку на певній відстані від нижнього краю

      random = new Random(); // Оновити генератор для створення цеглинок
      InitializeBricks(); // Створити новий набір цеглинок для поточного рівня
    }

    // Ініціалізувати та розмістити цеглинки на ігровому полі, включаючи незнищенні та кольорові
    private void InitializeBricks()
    {
      int brickWidth = 62;
      int brickHeight = 30;
      int bricksPerRow = 12;
      int rows = 5;
      int indestructibleRows = 1;
      int spacing = 4; // Відстань між цеглинками
      int startX = 5; // Початкова координата X для першої цеглинки
      int startY = 50; // Початкова координата Y для першої цеглинки

      // Створити та додати незнищенні цеглинки (сірого кольору)
      for (int row = 0; row < indestructibleRows; row++)
      {
        for (int col = 0; col < bricksPerRow; col++)
        {
          float x = startX + col * (brickWidth + spacing);
          float y = startY + row * (brickHeight + spacing);
          Bricks.Add(new Brick(x, y, brickWidth, brickHeight, isIndestructible: true, Color.Gray));
        }
      }

      // Створити та додати цеглинки, які можна зруйнувати, з випадковими кольорами
      for (int row = indestructibleRows; row < indestructibleRows + rows; row++)
      {
        for (int col = 0; col < bricksPerRow; col++)
        {
          float x = startX + col * (brickWidth + spacing);
          float y = startY + row * (brickHeight + spacing);
          // Генерувати випадковий RGB колір для цеглинки
          Color randomColor = Color.FromArgb(
            random.Next(50, 256),
            random.Next(50, 256),
            random.Next(50, 256)
          );
          Bricks.Add(new Brick(x, y, brickWidth, brickHeight, isIndestructible: false, randomColor));
        }
      }
    }

    // Оновити стан усіх ігрових об'єктів та перевірити умови завершення гри
    public void Update()
    {
      if (CurrentState != GameState.Playing)
      {
        return;
      }

      Ball.Move();
      Ball.CheckWallCollision(formWidth, formHeight);
      Ball.CheckPaddleCollision(Paddle);

      // Перевірити зіткнення м'яча з кожною цеглинкою
      foreach (var brick in Bricks)
      {
        if (!brick.IsDestroyed && Ball.CheckBrickCollision(brick))
        {
          if (!brick.IsIndestructible)
          {
            brick.IsDestroyed = true;
            Score += 10;
          }
          break;
        }
      }

      // Перевірити, чи м'яч вийшов за нижню межу ігрового поля
      if (Ball.Y > formHeight)
      {
        IsGameOver = true;
        CurrentState = GameState.GameOver;
      }

      // Перевірити, чи всі знищенні цеглинки зруйновані
      bool allDestructibleDestroyed = true;
      foreach (var brick in Bricks)
      {
        if (!brick.IsIndestructible && !brick.IsDestroyed)
        {
          allDestructibleDestroyed = false;
          break;
        }
      }
      if (allDestructibleDestroyed)
      {
        IsGameWon = true;
        CurrentState = GameState.GameWon;
      }
    }

    // Встановити новий стан гри, синхронізуючи логіку з інтерфейсом користувача
    public void SetGameState(GameState newState)
    {
      CurrentState = newState;
    }
  }
}