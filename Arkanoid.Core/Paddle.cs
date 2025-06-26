using System;
using System.Drawing;

namespace Arkanoid.Core
{
  public class Paddle
  {
    public float X { get; set; } // Позиція по осі X (верхній лівий кут платформи)
    public float Y { get; set; } // Позиція по осі Y (верхній лівий кут платформи)
    public int Width { get; set; } // Ширина платформи
    public int Height { get; set; } // Висота платформи
    private Brush Brush { get; } // Кисть для відображення кольору платформи
    private float prevX; // Попередня позиція X платформи для розрахунку швидкості

    // Ініціалізувати новий екземпляр класу Paddle
    public Paddle(float x, float y, int width, int height, Color color)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
      Brush = new SolidBrush(color);
      prevX = x;
    }

    // Малювати платформу на графічному контексті
    public void Draw(Graphics g)
    {
      g.FillRectangle(Brush, X, Y, Width, Height);
    }

    // Переміщувати платформу, обмежуючи її рух межами форми та максимальною швидкістю
    public void Move(float newX, int formWidth)
    {
      prevX = X; // Зберегти поточну позицію як попередню

      // Обмежити цільову позицію X, щоб платформа не виходила за межі форми
      float targetX = Math.Max(0, Math.Min(newX, formWidth - Width));

      float maxSpeed = 20f; // Максимальна дозволена швидкість руху платформи за кадр
      float deltaX = targetX - X; // Обчислити бажане зміщення
      // Обмежити зміщення максимальною швидкістю
      deltaX = Math.Max(-maxSpeed, Math.Min(maxSpeed, deltaX));

      X += deltaX; // Оновити поточну позицію платформи
    }

    // Обчислювати горизонтальну швидкість платформи (зміна позиції X за один кадр)
    public float GetSpeed()
    {
      return X - prevX;
    }
  }
}