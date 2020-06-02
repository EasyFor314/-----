using System;
using System.IO;
using System.Windows.Forms;

namespace Laba2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // путь файла
        private string path="";
        // стандартное название файла
        private static string default_name = "Новый документ.txt";
        private string form_name = default_name;
        // диалоговое окно сохранения файла
        private DialogResult ansDio()
        {
            return MessageBox.Show("Вы хотите сохранить изменения в файле\n" + Text + "?", "Уведомление", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }
        // берем последную часть пути( имя файла)
        private void split()
        {
            // разбиваем на блоки \\ 
            string[] words = path.Split(new char[] { '\\' });
            // сохраняем имя формы
            form_name = words[words.Length - 1];
        }
        // сохранение файла
        private void save(object sender, EventArgs e)
        {
            // если путь к файлу нету и текст заполнен то вызываем диалог сохранения или путь файла есть 
            if ((path == "" && text_box.Text != "") || (path != "" && text_box.Text != "" &&  text_box.Text != File.ReadAllText(path)))
            {
                // вызывае диалог сохранения
                DialogResult res = ansDio();
                // если нажать да то вызываем сохранение файла
                if (res == DialogResult.Cancel)
                    return;
                if (res == DialogResult.Yes)
                    сохранитьToolStripMenuItem1_Click(sender, e);
            }
            // Записываем имя файла в заголовок
            Text = path;
        }
        // выход из программы
        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // вызов функции сохранение
            save(sender, e);
            // закрыть форму
            Close();
        }
        // закрытие формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //если пути файла нету и текст не пусто или если есть путь и внутренности файла и значений поля
            if ((path == "" && text_box.Text != "") || (path != "" && text_box.Text != File.ReadAllText(path)))
            {
                // вызываем окно сохранения
                DialogResult res = ansDio();
                if (res == DialogResult.Cancel)
                    e.Cancel = true;
                // если нажали да то вызываем сохранение
                if (res == DialogResult.Yes)
                    // если путь пустой то сохраняем как новый файл, иначе просто перезаписываем файл
                    if (path == "" || !File.Exists(path))
                    {
                        saveFileDialog1.FileName = form_name;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            // сохраняем путь файла
                            path = saveFileDialog1.FileName;
                            // сохраняем файл
                            File.WriteAllText(path, text_box.Text);
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        // перезаписываем файл
                        File.WriteAllText(path, text_box.Text);
                    }
            }
        }
        // открытие файла
        private void открытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // вызовем функцию сохранения
            save(sender, e);
            // вызываем окно открытия
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            // сохраняем путь файла
            path = openFileDialog1.FileName;
            // получаем имя файла
            split();
            // в заголовок формы пишется имя файла
            Text = form_name;
            // записываем внутренности файла в поле
            string fileText = File.ReadAllText(path);
            text_box.Text = fileText;
            // ставим курсор в конец файла
            text_box.SelectionStart = text_box.Text.Length;
        }
        // выбор цвета текста
        private void цветТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // записываем текущий цвет в диалог цвета
            colorDialog1.Color = text_box.ForeColor;
            // вызов диалога цвета
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // устанавливаем цвет в поле текста
                text_box.ForeColor = colorDialog1.Color;
            }
        }
        // выбор шрифта
        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // записываем текущей шрифт в диалог шрифта
            fontDialog1.Font = text_box.Font;
            // вызов диалога выбора текста
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                // устанавливаем шрифт в поле текста
                text_box.Font = fontDialog1.Font;
            }
        }
        // выбор цвета фона 
        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // записываем текущий цвет в диалог цвета
            colorDialog1.Color = text_box.BackColor;
            // вызов диалога цвета
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                // устанавливаем цвет фона в поле текста
                text_box.BackColor = colorDialog1.Color;
            }
        }
        // вызов окна о программе
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Соболева Елизавета Петровна");
        }
        // вызов окна сохранения файла
        private void сохранитькакToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // запись имени файла из заголовка формы 
            saveFileDialog1.FileName = Text;
            // вызову диалога сохранения
            if (saveFileDialog1.ShowDialog()==DialogResult.OK)
            {        
                // сохраняем путь
                path = saveFileDialog1.FileName;
                // сохраняем файл
                File.WriteAllText(path, text_box.Text);
                // сохраняем имя файла
                split();
                // меняе заголовок в форме на название файла
                Text = form_name;
            }
        }
        // функция при изменении 
        private void txt_TextChanged_1(object sender, EventArgs e)
        {
            string s = "";
            // если файл есть то получим его внутренности и сравним с текстом в поле то поменяем имя формы
            if (path!="")
            {
                s = File.ReadAllText(path).ToString();
            }
            if ((path == "" && text_box.Text != "") || (path != "" && text_box.Text != s))
            {
                Text = default_name;
            }
            else
            {
                Text = form_name;
            }
        }

        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // если файл не сохраняли и его нет по пути, то вызовем окно сохранение иначе обвноим файл
            if (path=="" || !File.Exists(path))
            {
                // вызов функции сохранить как
                сохранитькакToolStripMenuItem1_Click(sender, e);
            }
            else
            {
                // перезаписать файл
                File.WriteAllText(path, text_box.Text);
            }
        }
        // кнопка нового файла
        private void new_file_Click(object sender, EventArgs e)
        {
            // вызов функции сохранение файла
            save(sender, e);
            // обнуляем значения
            text_box.Text = "";
            path = "";
            Text = "";
        }
    }
}
