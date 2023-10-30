using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonConsoleApp.Utils
{
    internal class InputUtils
    {
        public static void PrintColoredText(string text, string colorName)
        {
            if (Enum.TryParse(colorName, out ConsoleColor color))
            {
                Console.ForegroundColor = color;
                Console.WriteLine(text);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"Invalid color: {colorName}");
            }
        }

/*        ConsoleColor.Black: Màu đen.
        ConsoleColor.Blue: Màu xanh dương.
        ConsoleColor.Cyan: Màu xanh lam ngọc (cyan).
        ConsoleColor.DarkBlue: Màu xanh đậm (tối).
        ConsoleColor.DarkCyan: Màu xanh lam ngọc đậm (tối).
        ConsoleColor.DarkGray: Màu xám đậm (tối).
        ConsoleColor.DarkGreen: Màu xanh lá cây đậm (tối).
        ConsoleColor.DarkMagenta: Màu magenta đậm (tối).
        ConsoleColor.DarkRed: Màu đỏ đậm (tối).
        ConsoleColor.DarkYellow: Màu vàng đậm (tối).
        ConsoleColor.Gray: Màu xám.
        ConsoleColor.Green: Màu xanh lá cây.
        ConsoleColor.Magenta: Màu magenta.
        ConsoleColor.Red: Màu đỏ.
        ConsoleColor.White: Màu trắng.
        ConsoleColor.Yellow: Màu vàng.*/

        public static int GetValidIntegerInput(string message)
        {
            int input;
            bool validInput = false;

            do
            {
                Console.Write(message);
                string inputString = Console.ReadLine();

                if (int.TryParse(inputString, out input))
                {
                    validInput = true;
                }
                else
                {
                    // Thay đổi màu của console thành đỏ
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập một số nguyên.");

                    // Khôi phục màu mặc định của console
                    Console.ResetColor();
                }
            }
            while (!validInput);

            return input;
        }
    }
}
