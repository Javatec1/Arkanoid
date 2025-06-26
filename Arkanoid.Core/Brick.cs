using System.Drawing;

namespace Arkanoid.Core
{
  public class Brick
  {
    public float X { get; set; } // Отримати або встановити позицію цеглинки по осі X
    public float Y { get; set; } // Отримати або встановити позицію цеглинки по осі Y
    public int Width { get; set; } // Отримати або встановити ширину цеглинки
    public int Height { get; set; } // Отримати або встановити висоту цеглинки
    public bool IsDestroyed { get; set; } // Визначити, чи зруйнована цеглинка
    public bool IsIndestructible { get; set; } // Визначити, чи є цеглинка незнищенною
    private Brush Brush { get; } // Отримати кисть для заливки кольору цеглинки
    private Pen OutlinePen { get; } // Отримати перо для малювання контуру цеглинки

    // Ініціалізувати новий екземпляр класу Brick
    public Brick(float x, float y, int width, int height, bool isIndestructible, Color color)
    {
      X = x;
      Y = y;
      Width = width;
      Height = height;
      IsDestroyed = false;
      IsIndestructible = isIndestructible;
      Brush = new SolidBrush(color); // Встановити колір заливки цеглинки
      // Встановити чорний контур товщиною в 1 піксель
      OutlinePen = new Pen(Color.Black, 1);
    }

    // Намалювати цеглинку на наданому графічному контексті
    public void Draw(Graphics g)
    {
      if (!IsDestroyed)
      {
        g.FillRectangle(Brush, X, Y, Width, Height);
        g.DrawRectangle(OutlinePen, X, Y, Width, Height);
      }
    }
  }
}