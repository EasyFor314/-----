
using System;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;


namespace Laba4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string path = null;
        private string form_name = "Таблица.txt";

        private void split()
        {
            string[] word = path.Split('\\');
            form_name = word[word.Length - 1];
            Text = form_name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double x1, x2, h;
            try
            {
                x1 = Double.Parse(begin_box.Text);
                x2 = Double.Parse(end_box.Text);
                h = Double.Parse(step_box.Text);
            }
            catch(Exception)
            {
                MessageBox.Show("Поля ввода могут содержать только числа!!!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (x1 > x2)
            {
                MessageBox.Show("Начальное значение не может быть больше конечного!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (h <= 0)
            {
                MessageBox.Show("Шаг не может быть меньше или равен нулю!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((x2 - x1) < h)
            {
                MessageBox.Show("Шаг не может быть больше интервала!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            button2_Click(null, null);
            button3_Click(null, null);
            int i = 0;
            double res, x;
            for (x = x1; x <= x2; x += h)
            {
                res = (Math.Pow(x, 2) - Math.Log(Math.Pow(x, 2) + 1)) / (x + Math.Pow(3.2, 2 * x - 1)) * (x - Math.Sin(3 * x));
                dataGridView1.Rows.Add(x, res);
                if ((x + Math.Pow(3.2, 2 * x - 1)) == 0)
                {
                    dataGridView1.Rows[i].ErrorText += "Некорректное значение \"∞\"\n";
                }
                else
                {
                    chart1.Series["y"].Points.AddXY(x, res);
                }
                dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                i++;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int var = comboBox1.SelectedIndex;
            switch (var)
            {
                case 0:
                    chart1.Series["y"].ChartType = SeriesChartType.Line;
                    break;
                case 1:
                    chart1.Series["y"].ChartType = SeriesChartType.Spline;
                    break;
            }

        }

        private void exit(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                button3_Click(null, null);
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    chart1.Series["y"].Points.AddXY( dataGridView1[0, i].Value, dataGridView1[1, i].Value );
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Некорректные данные в таблице!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Series["y"].Points.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 1;
        }

        private void загрузитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string[] str;
                openFileDialog1.Title = "Загрузка";
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    button2_Click(null, null);
                    path = openFileDialog1.FileName;
                    string[] s = File.ReadAllLines(path);
                    s = s.Where(r => !string.IsNullOrEmpty(r)).ToArray();
                    dataGridView1.RowCount = s.Length + 1;

                    for (int i = 0; i < s.Length; i++)
                    {
                        str = s[i].Split(';');
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            try
                            {
                                Double.Parse(str[j]);
                            }
                            catch (Exception)
                            {
                                dataGridView1.Rows[i].ErrorText = " Некорректное значение ";
                            }

                            dataGridView1[j, i].Value = str[j];
                        }
                        dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
                    }
                    button4_Click(null, null);
                    split();
                    begin_box.Clear();
                    end_box.Clear();
                    step_box.Clear();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла непредвиденная ошибка, проверьте правильность файла!");
            }
        }

        private void сохранитькакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = form_name;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = saveFileDialog1.FileName;
                split();
                сохранитьToolStripMenuItem_Click(null, null);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (path != null)
            {
                string[] str = new string[dataGridView1.RowCount - 1];
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    for (int j = 0; j < dataGridView1.ColumnCount; j++)
                    {
                        if (dataGridView1[j, i].Value==null)
                            str[i] += "0";
                        else
                            str[i] += dataGridView1[j, i].Value.ToString();
                        if (j < dataGridView1.ColumnCount - 1)
                            str[i] += ";";
                    }
                File.WriteAllLines(path, str);
            }
            else
            {
                сохранитькакToolStripMenuItem_Click(null, null);
            }
        }

        private string check(string str)
        {
            string nums = "-1234567890.,∞";
            for (int i = 0; i < str.Length; i++)
                if (!nums.Contains(str[i].ToString()))
                    return str[i].ToString();
            return "True";
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string cellValue = e.FormattedValue.ToString();
            string result = check(cellValue);
            if (result=="True")
                dataGridView1.Rows[e.RowIndex].ErrorText = null;
            else
            { 
                e.Cancel = true;
                dataGridView1.Rows[e.RowIndex].ErrorText = "Некорректный символ \"" + result + "\"";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
        }

        private void сохранитьКакToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Сохранить график как...";
            saveFileDialog1.FileName = "Новый график";
            saveFileDialog1.Filter = "BMP Image|*.bmp|PNG Image|*.png|JPG Image|*.jpg";
            saveFileDialog1.FilterIndex = 2;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Bmp);
                        break;
                    case 2:
                        chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Png);
                        break;
                    case 3:
                        chart1.SaveImage(saveFileDialog1.FileName, ChartImageFormat.Jpeg);
                        break;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = chart1.Series["y"].Color;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                chart1.Series["y"].Color = colorDialog1.Color;
                button5.BackColor = colorDialog1.Color;
            }
        }
    }
}
