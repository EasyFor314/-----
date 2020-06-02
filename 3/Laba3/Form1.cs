using System;
using System.IO;
using System.Windows.Forms;

namespace Laba3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // путь файла
        private string path = "";
        // стандартное имя файла
        private static string default_name = "Новый документ.txt";
        // название формы
        private string form_name = default_name;
        // диалоговое окно сохранения
        private DialogResult ansDio()
        {
            return MessageBox.Show("Вы хотите сохранить изменения в файле\n" + form_name + "?", "Уведомление", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
        }
        // оставляем имя файла  и сохраняем в переменную
        private void split()
        {
            string[] words = path.Split(new char[] { '\\' });
            form_name = words[words.Length - 1];
        }
        // функция сохранения
        private void save(object sender, EventArgs e)
        { 
            // новое поле
            RichTextBox rich_text_box = new RichTextBox();
            // если путь не пусто то загружаем в новое поле
            if (path != "")
            {
                rich_text_box.LoadFile(path);
            }
            // если поле отличается от исходного файла или файла нет то вызываем диалог сохранения
            if ((path == "" && text_box.Text != "") || (path != "" && text_box.Text != "" && text_box.Rtf != rich_text_box.Rtf))
            {
                // вызываем диалог сохранения
                DialogResult res = ansDio();
                if (res == DialogResult.Cancel)
                {
                    rich_text_box.Dispose();
                    return;
                }
                // если да то вызываем сохранения
                if (res == DialogResult.Yes)
                    сохранитьToolStripMenuItem1_Click(sender, e);
            }
            // удаляем новое поле
            rich_text_box.Dispose();
            // устанавливаем заголовок формы из пути
            Text = path;
        }
        // кнопка выхода
        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // вызов функции сохранения
            save(sender, e);
            // закрытие окна
            Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // новое поле ввода
            RichTextBox rich_text_box = new RichTextBox();
            // если путь не пусто то сохраняем в файл
            if (path != "")
            {
                rich_text_box.LoadFile(path);
            }
            // если поле отличается от исходного файла или файла нет то вызываем диалог сохранения
            if ((path == "" && text_box.Text != "") || (path != "" && text_box.Rtf != rich_text_box.Rtf))
            {
                // вызываем диалог сохранения
                DialogResult res = ansDio();
                if (res == DialogResult.Cancel)
                    e.Cancel = true;
                // если да то сохраняем
                if (res == DialogResult.Yes)
                    // если пути не то вызывае окно сохранения
                    if (path == "" || !File.Exists(path))
                    {
                        SaveFileDialog1.FileName = form_name;
                        if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            path = SaveFileDialog1.FileName;
                            text_box.SaveFile(path);
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                //иначе сохраняем файл в путь
                    else
                    {
                        text_box.SaveFile(path);
                    }
            }
            rich_text_box.Dispose();
        }

        private void открытьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // вызывавем функцию сохранения
            save(sender, e);
            // задаем пустое имя файла в форме открытия
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            // сохарняем путь файла
            path = openFileDialog1.FileName;
            // получаем имя файла
            split();
            // задаем заголовок формы
            Text = form_name;
            // сохраняем файл
            text_box.LoadFile(path);
            // ставим курсор в конец
            text_box.SelectionStart = text_box.Rtf.Length;
        }
        // выбираем цвет текста
        private void цветТекстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = text_box.SelectionColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                text_box.SelectionColor = colorDialog1.Color;
            }
        }
        // выбираем шрифт
        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fontDialog1.Font = text_box.SelectionFont;
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                text_box.SelectionFont = fontDialog1.Font;
            }
        }
        // выбираем цвет фона
        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = text_box.SelectionBackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                text_box.SelectionBackColor = colorDialog1.Color;
            }
        }
        // о программе
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Соболева Елизавета Петровна");
        }
        // вызов окна сохранить
        private void сохранитькакToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog1.FileName = form_name;

            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = SaveFileDialog1.FileName;
                text_box.SaveFile(path);
                split();
                Text = form_name;
            }
        }

        private void txt_TextChanged_1(object sender, EventArgs e)
        {
            RichTextBox rich_text_box = new RichTextBox();
            if (path != "")
            {
                rich_text_box.LoadFile(path);
            }
            if ((path == "" && text_box.Text != "") || (path != "" && text_box.Rtf != rich_text_box.Rtf))
            {
                Text = default_name;
            }
            else
            {
                Text = form_name;
            }
            rich_text_box.Dispose();
        }
        // окно сохранения
        private void сохранитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (path == "" || !File.Exists(path))
            {
                сохранитькакToolStripMenuItem1_Click(sender, e);
            }
            else
            {
                text_box.SaveFile(path);
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // копируем из поля
            text_box.Copy();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            // вставляем в поле
            text_box.Paste();
        }

        // вырезаем из поля
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            text_box.Cut();
        }

        private void создатьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // вызываем функцию сохранения
            save(sender, e);
            // очищаем поле ввода
            text_box.Text = "";
            // очищаем путь
            path = "";
            // очищаем заголовок формы
            Text = "";
        }
    }
}
