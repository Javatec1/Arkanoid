using System;
using System.Drawing;

namespace Arkanoid.Core
{
  public class Ball
  {
    public float X { get; set; } // Отримати або встановити позицію м'яча по осі X
    public float Y { get; set; } // Отримати або встановити позицію м'яча по осі Y
    public float SpeedX { get; set; } // Отримати або встановити горизонтальну швидкість м'яча
    public float SpeedY { get; set; } // Отримати або встановити вертикальну швидкість м'яча
    public float InitialSpeed { get; } = 4.24f; // Отримати початкову швидкість м'яча
    public int Size { get; set; } // Отримати або встановити розмір м'яча
    private Brush Brush { get; } // Кисть для малювання м'яча
    private float currentSpeed; // Поточна загальна швидкість м'яча
    private readonly float maxSpeed; // Максимальна швидкість м'яча
    private readonly float speedIncrement; // Величина, на яку збільшується швидкість після зіткнення

    // Ініціалізувати новий екземпляр класу Ball
    public Ball(float x, float y, int size)
    {
      X = x;
      Y = y;
      Size = size;
      Brush = new SolidBrush(Color.Red);
      currentSpeed = InitialSpeed;
      maxSpeed = 15f;
      speedIncrement = 0.05f;
    }

    // Скинути швидкість м'яча до початкової та зупинити його рух
    public void ResetSpeed()
    {
      currentSpeed = InitialSpeed;
      SpeedX = 0;
      SpeedY = 0;
    }

    // Перемістити м'яч на основі його поточної швидкості
    public void Move()
    {
      X += SpeedX;
      Y += SpeedY;
    }

    // Намалювати м'яч на наданому графічному контексті
    public void Draw(Graphics g)
    {
      g.FillEllipse(Brush, X, Y, Size, Size);
    }

    // Збільшити поточну загальну швидкість м'яча, зберігаючи напрямок
    private void IncreaseSpeed()
    {
      currentSpeed = Math.Min(currentSpeed + speedIncrement, maxSpeed);
      float currentMagnitude = (float)Math.Sqrt((double)SpeedX * SpeedX + (double)SpeedY * SpeedY);
      if (currentMagnitude > 0)
      {
        float scale = currentSpeed / currentMagnitude;
        SpeedX *= scale;
        SpeedY *= scale;
      }
    }

    // Перевірити зіткнення м'яча зі стінами та обробити відскік
    public void CheckWallCollision(int formWidth, int formHeight)
    {
      if (X <= 0)
      {
        X = 0;
        SpeedX = -SpeedX;
        IncreaseSpeed();
      }
      else if (X + Size >= formWidth)
      {
        X = formWidth - Size;
        SpeedX = -SpeedX;
        IncreaseSpeed();
      }

      if (Y <= 0)
      {
        Y = 0;
        SpeedY = -SpeedY;
        IncreaseSpeed();
      }
    }

    // Перевірити зіткнення м'яча з ракеткою та обробити відскік
    public bool CheckPaddleCollision(object paddleObject)
    {
      // Перетворити object на Paddle, щоб отримати доступ до його властивостей
      if (!(paddleObject is Paddle paddle))
      {
        return false;
      }

      // Створити прямокутники для м'яча та ракетки для спрощеної перевірки зіткнень
      Rectangle ballRect = new Rectangle((int)X, (int)Y, Size, Size);
      Rectangle paddleRect = new Rectangle((int)paddle.X, (int)paddle.Y, paddle.Width, paddle.Height);

      // Якщо прямокутники не перетинаються, зіткнення відсутнє
      if (!ballRect.IntersectsWith(paddleRect))
      {
        return false;
      }

      // Розрахувати центральні точки м'яча та ракетки
      float ballCenterX = X + Size / 2f;
      float ballCenterY = Y + Size / 2f;
      float paddleCenterX = paddle.X + paddle.Width / 2f;
      float paddleCenterY = paddle.Y + paddle.Height / 2f;

      // Розрахувати вектори від центра ракетки до центра м'яча
      float dx = ballCenterX - paddleCenterX;
      float dy = ballCenterY - paddleCenterY;

      /*
       * Визначити, з якої сторони відбулося зіткнення.
       * Порівняти співвідношення dx до ширини ракетки та dy до висоти ракетки.
       * Це допомагає визначити, чи зіткнення було по горизонталі чи по вертикалі.
       */
      if (Math.Abs(dx / (float)paddle.Width) > Math.Abs(dy / (float)paddle.Height))
      {
        // Зіткнення з бічною стороною ракетки
        SpeedX = -SpeedX; // Інвертувати горизонтальну швидкість
        // Коригувати позицію м'яча, щоб він не "застрягав" у ракетці
        if (dx > 0)
          X = paddle.X + paddle.Width; // М'яч відскочив праворуч від ракетки
        else
          X = paddle.X - Size; // М'яч відскочив ліворуч від ракетки
      }
      else
      {
        // Зіткнення з верхньою або нижньою стороною ракетки
        // Отримати швидкість ракетки (для впливу на відскік м'яча)
        float paddleSpeed = paddle.GetSpeed();

        // Якщо ракетка майже не рухається (швидкість близька до нуля)
        if (Math.Abs(paddleSpeed) < 0.1f)
        {
          // Відбити м'яч вертикально вгору. Math.Abs(SpeedY) гарантує, що SpeedY буде позитивною
          // (тобто м'яч летить вгору), а знак мінус робить її від'ємною.
          SpeedY = -(float)Math.Abs(SpeedY);
          // Додаткова перевірка: якщо м'яч якимось чином опинився нижче центру ракетки,
          // відбити його в тому ж напрямку, як він рухався.
          if (dy > 0)
            SpeedY = (float)Math.Abs(SpeedY);
        }
        else // Якщо ракетка рухається
        {
          // Вплив швидкості ракетки на горизонтальну швидкість м'яча
          float speedInfluence = 0.5f;
          // Додати частину швидкості ракетки до швидкості м'яча
          SpeedX += paddleSpeed * speedInfluence;

          // Обмежити максимальну горизонтальну швидкість м'яча
          float maxSpeedX = 5f; // Максимальна дозволена горизонтальна швидкість
          SpeedX = Math.Max(-maxSpeedX, Math.Min(maxSpeedX, SpeedX));

          // Відбити м'яч вертикально вгору
          SpeedY = -(float)Math.Abs(SpeedY);
          if (dy > 0)
            SpeedY = (float)Math.Abs(SpeedY);
        }

        // Встановити мінімальний кут відскоку, щоб м'яч не літав суворо горизонтально
        float minAngle = 15f * (float)Math.PI / 180f; // Перевести 15 градусів в радіани
        // Розрахувати мінімальну горизонтальну швидкість
        float minSpeedX = currentSpeed * (float)Math.Sin(minAngle);

        if (Math.Abs(SpeedX) < minSpeedX)
        {
          // Якщо поточна SpeedX менша за мінімально дозволену,
          // привласнити їй мінімальне значення, зберігаючи напрямок (знак)
          SpeedX = (float)Math.Sign(SpeedX) * minSpeedX;
          // Якщо SpeedX була нульовою (або дуже близькою до нуля),
          // надати їй випадковий напрямок (вліво або вправо)
          if (SpeedX == 0f)
            SpeedX = minSpeedX * (new Random().Next(0, 2) == 0 ? 1f : -1f);
        }

        // Перерахувати SpeedY для збереження загальної швидкості м'яча
        // Розрахувати квадрат SpeedY: currentSpeed^2 - SpeedX^2
        double newSpeedYSquaredDouble = (double)currentSpeed * currentSpeed - (double)SpeedX * SpeedX;

        if (newSpeedYSquaredDouble > 0)
        {
          // Обчислити нову SpeedY, зберігаючи напрямок відскоку
          SpeedY = (dy > 0 ? 1f : -1f) * (float)Math.Sqrt(newSpeedYSquaredDouble);
        }
        else
        {
          /*
           * У рідкісних випадках (через округлення або екстремальні значення),
           * якщо newSpeedYSquaredDouble виявився від'ємним, просто використати
           * абсолютне значення SpeedY з правильним напрямком, щоб уникнути помилок.
           */
          SpeedY = (dy > 0 ? 1f : -1f) * (float)Math.Abs(SpeedY);
        }

        // Коригувати позицію м'яча, щоб він був поза ракеткою.
        // Це запобігає повторним зіткненням за один кадр.
        if (dy > 0) // Якщо м'яч рухався вниз
          Y = paddle.Y + paddle.Height; // Перемістити м'яч за нижній край ракетки
        else // Якщо м'яч рухався вгору
          Y = paddle.Y - Size; // Перемістити м'яч над верхнім краєм ракетки
      }
      IncreaseSpeed();
      return true;
    }

    // Перевірити зіткнення м'яча з цеглинкою та обробити відскік і руйнування
    public bool CheckBrickCollision(object brickObject)
    {
      // Перетворити object на Brick, щоб отримати доступ до його властивостей
      if (!(brickObject is Brick brick))
      {
        return false;
      }

      RectangleF ballRect = new RectangleF(X, Y, Size, Size);
      RectangleF brickRect = new RectangleF(brick.X, brick.Y, brick.Width, brick.Height);

      if (ballRect.IntersectsWith(brickRect) && !brick.IsDestroyed)
      {
        // Зберегти попередню позицію м'яча, щоб визначити напрямок удару
        float prevBallX = X - SpeedX;
        float prevBallY = Y - SpeedY;
        RectangleF prevBallRect = new RectangleF(prevBallX, prevBallY, Size, Size);

        bool collidedHorizontally = false;
        bool collidedVertically = false;

        // Перевірити, чи м'яч перетнув горизонтальні межі цеглинки
        if (prevBallRect.Bottom <= brickRect.Top && ballRect.Bottom > brickRect.Top && SpeedY > 0)
        {
          // Удар зверху по верхній грані цеглинки
          collidedVertically = true;
          Y = brickRect.Top - Size; // Коригування позиції
        }
        else if (prevBallRect.Top >= brickRect.Bottom && ballRect.Top < brickRect.Bottom && SpeedY < 0)
        {
          // Удар знизу по нижній грані цеглинки
          collidedVertically = true;
          Y = brickRect.Bottom; // Коригування позиції
        }

        // Перевірити, чи м'яч перетнув вертикальні межі цеглинки
        if (prevBallRect.Right <= brickRect.Left && ballRect.Right > brickRect.Left && SpeedX > 0)
        {
          // Удар зліва по лівій грані цеглинки
          collidedHorizontally = true;
          X = brickRect.Left - Size; // Коригування позиції
        }
        else if (prevBallRect.Left >= brickRect.Right && ballRect.Left < brickRect.Right && SpeedX < 0)
        {
          // Удар справа по правій грані цеглинки
          collidedHorizontally = true;
          X = brickRect.Right; // Коригування позиції
        }

        if (collidedHorizontally)
        {
          SpeedX = -SpeedX;
        }
        if (collidedVertically)
        {
          SpeedY = -SpeedY;
        }

        // Обробити кутові зіткнення, якщо не було чіткого осьового зіткнення
        if (!collidedHorizontally && !collidedVertically)
        {
          float ballCenterX = X + Size / 2;
          float ballCenterY = Y + Size / 2;
          float brickCenterX = brick.X + brick.Width / 2;
          float brickCenterY = brick.Y + brick.Height / 2;

          float dx = ballCenterX - brickCenterX;
          float dy = ballCenterY - brickCenterY;

          if (Math.Abs(dx / brick.Width) > Math.Abs(dy / brick.Height))
          {
            SpeedX = -SpeedX;
            if (dx > 0) X = brickRect.X + brickRect.Width;
            else X = brickRect.X - Size;
          }
          else
          {
            SpeedY = -SpeedY;
            if (dy > 0) Y = brickRect.Y + brickRect.Height;
            else Y = brickRect.Y - Size;
          }
        }

        IncreaseSpeed();

        if (!brick.IsIndestructible)
        {
          brick.IsDestroyed = true;
        }
        return true;
      }
      return false;
    }
  }
}