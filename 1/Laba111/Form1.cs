using System;
using System.Data;
using System.Windows.Forms;

namespace Laba111
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // проверка количества математический (для отрицательных чисел)
        private int Check = 0;
        private void btTap()
        {
            // активируем поле для ввода
            value.Select();
            value.SelectionLength = 0;
        }
        private bool lastSymbol()
        {
            // проверяем является ли последний символ, символом математической операции
            if (value.Text.EndsWith("/") || value.Text.EndsWith("*") || value.Text.EndsWith("-") || value.Text.EndsWith("+"))
                 return true;
            
            return false;
            
        }
        // расчёт результатов 
        private void result()
        {
            // переменная для хранения математической операции
            string symbol = "";
            // проверим ввелили последним символом математическую операцию
            if (lastSymbol())
            {
                // получим математическую операцию
                symbol = value.Text[value.Text.Length - 1].ToString();
                value.Text = value.Text.Substring(0, value.Text.Length - 1);
            }
            try
            {
                // расчитываем поля
                value.Text = new DataTable().Compute(value.Text, null).ToString() + symbol;
                // выделяем поле
                btTap();
                // переносим курсор в конец
                value.SelectionStart = value.Text.Length;

                if (symbol != "")
                {
                    Check += 1;
                }
            }
            catch (Exception)
            {
                btTap();
            }
        }
        // нажатие на любую клавишу кроме очистить,равно и стереть символ
        private void btNum_Click(object sender, EventArgs e)
        {
            // заносим значение в поле
            value.Text += (sender as Button).Text;
            // выделяем поле
            btTap();
            // переносим курсор в конец
            value.SelectionStart = value.Text.Length;
        }
        // полностью очищаем поле
        private void btClear_Click(object sender, EventArgs e)
        {
            // очищаем поле
            value.Clear();
            // выделяем поле
            btTap();
        }
        // удаляем последний символ
        private void btBackspace_Click(object sender, EventArgs e)
        {
            if (value.Text.Length > 0)
            {
                // стиреаем последний символ
                value.Text = value.Text.Substring(0, value.Text.Length - 1);
                // выделяем поле
                btTap();
                // переносим курсор в конец
                value.SelectionStart = value.Text.Length;
            }
        }
        // равно
        private void btEqual_Click(object sender, EventArgs e)
        {
            // вызываем подсчет результатов
            result();
        }
        // при нажатии любой клавишы, проверяем не enter ли или '=', если они то вызываем подсчет результата иначе просто записывается в поле нажатый символ
        private void txt_KeyDown(object sender, KeyEventArgs e)
        {
            // если равно Enter или символу =
            if (e.KeyCode == Keys.Enter || e.KeyValue == 187)
            {
                // вызываем подсчет результатов
                result();
            }
        }
        private string keyboard_Click()
        {
            // конечная строка
            string str = "";
            // массив оставляемых символов
            string[] mass = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "/", "*", "-", "+", ",", "." };
            for (int i = 0; i < value.Text.Length; ++i)
            {
                foreach (string j in mass)
                {
                    // если равно массиву то записывавем в конечную строку
                    if (value.Text[i].ToString() == j)
                    {
                        str += value.Text[i];
                        break;
                    }
                }
                // если точка или запятая, то очищаем поле
                if (value.Text[i] == '.' || value.Text[i] == ',')
                {
                    string s = value.Text;
                    value.Clear();
                    str = "";
                    btTap();
                    break;
                }
            }
            return str;
        }
        // вызываетя при изменении поля ввода, для проверки вводимого символа
        private void txt_TextChanged(object sender, EventArgs e)
        {
            // Проверяем не пустое ли поле (value) и  не ввели ли ли точку или запятую
            if (value.Text.Length > 0 && (value.Text[value.Text.Length - 1] == '.' || value.Text[value.Text.Length - 1] == ','))
            {
                // если ввели то удаляем символ
                MessageBox.Show("Калькулятор предназначен\nисключительно для\nцелочисленных операций!!!\nСимвол будет удален!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                value.Text = value.Text.Substring(0, value.Text.Length - 1);
                value.SelectionStart = value.Text.Length;
            }
            // если введен просто математический оператор и не ноль то пишем ноль и делаем действие
            if (value.Text.Length == 1 && lastSymbol() && value.Text[value.Text.Length-1]!='-')
            {
                // Добавляем ноль перед математическим оператором
                value.Text = "0" + value.Text[value.Text.Length-1];
            }
            // если ввели математический оператор минус и до этого был другой математический оператор то его считать как минус
            if (lastSymbol() && value.Text.Length > 1)
            {
                if (value.Text[value.Text.Length - 1] != '-' && (value.Text[value.Text.Length - 2] == '/' || value.Text[value.Text.Length - 2] == '+' || value.Text[value.Text.Length - 2] == '-' || value.Text[value.Text.Length - 2] == '*'))
                {
                    string last = value.Text[value.Text.Length - 1].ToString();
                    value.Text = value.Text.Substring(0, value.Text.Length - 2) + last;
                }   
            }
            // проверяем что за символы введены, оставляем только цифры и символы математичеких операций
            string str = keyboard_Click();

            int cursor = value.SelectionStart;
            value.Text = str;
            value.SelectionStart = cursor;
            // если три минуса то вызываем расчеты
            // если есть минус то добавляем в переменную check
            if (lastSymbol())
            {
                if (!(int.TryParse(value.Text[value.Text.Length - 2].ToString(), out int h)) && value.Text[value.Text.Length - 2] != '-' && value.Text[value.Text.Length - 1] == '-')
                    Check = 1;
                else
                    Check += 1;
                if (Check >= 2)
                {
                    Check = 0;
                    result();
                }
            }
        }
        // загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            btTap();
        }
    }
}
