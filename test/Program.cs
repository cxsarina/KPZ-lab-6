using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        int value = 100; // Початкове значення

        Timer timer = new Timer(DecreaseValue, value, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        // Чекаємо, поки програма не буде вручну завершена
        Console.ReadLine();
    }

    static void DecreaseValue(object state)
    {
        int value = (int)state; // Отримуємо значення

        value -= 15; // Зменшуємо на 15

        Console.WriteLine($"Значення: {value}");

        // Якщо досягнута потрібна умова для завершення, можна зупинити таймер
        if (value <= 0)
        {
            Timer timer = (Timer)state;
            timer.Dispose();
        }
    }
}

