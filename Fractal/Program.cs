using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractal
{
    static class Program
    {

        private static string[] subtitles = new string[]
        {
            "Теперь и в консоли!",
            "Действительно бесконечный",
            "Гениален",
            "Первое рабочие название 'Fractalizer'",
            "Зона свободная от конечного периметра",
            "Действительно двухмерен!",
            "На самом деле на 1.2683 объемен",
            "Просто приблизить еще чуть-чуть",
            "Использовать double глупая идея",
            "Почему я вижу пиксели?",
            "42",
            "Boss of this set",
            "ФРАКТАЛЫ ЭТО МОЯ ЖИЗНЬ",
            "1,506.591.884.9 или около того",
            "Не содержит генетически модифицированных рекурсивных вычислений",
            "Z^2 + C",
            "(a + b)^2 = a^2 + 2ab + b^2",
            "(a - b)^2 = a^2 - 2ab + b^2",
            "(a + b)^3 = a^3 + 3a^2 * b + 3ab^2 + b^3",
            "(a - b)^3 = a^3 - 3a^2 * b + 3ab^2 - b^3",
            "(a + b)(a - b) = a^2 - b^2",
            "Вы тоже хотите фрактальных кошкодевочек?",
            "А вы знали: 'Вы можете обновить сплэш нажав F13'",
            "А вы знали: 'Скорее всего у вас на клавиатуре нету клавиши F13'",
            "Скоро с поддержкой геймпада"
        };
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var Form = new Form1();
            Random random = new Random();
            Form.Text += " - " + subtitles[random.Next(0, subtitles.Length)];
            Application.Run(Form);
        }
    }
}
