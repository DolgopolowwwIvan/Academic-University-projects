using System;
using System.Windows.Forms;

namespace Converter_p1_p2
{
    public partial class Конвертер : Form
    {
     
        private Control_ ctl = new Control_();

        public Конвертер()
        {
            InitializeComponent();
            // Устанавливаем обработчики событий для клавиатуры
            this.KeyPreview = true;

            // Блокируем ввод в resultTextBox
            resultTextBox.ReadOnly = true;
            resultTextBox.BackColor = System.Drawing.SystemColors.Window;

            // Блокируем ввод в baseTextBox
            baseTextBox.ReadOnly = true;
            baseTextBox.BackColor = System.Drawing.SystemColors.Window;

            this.trackBar1.Scroll += trackBar1_Scroll;
            this.trackBar2.Scroll += trackBar2_Scroll;
            this.baseUpDown.ValueChanged += baseUpDown_ValueChanged;
            this.resultUpDown.ValueChanged += resultUpDown_ValueChanged;

            trackBar1.Value = 10;
            trackBar2.Value = 16;
            baseUpDown.Value = 10;
            resultUpDown.Value = 16;

            // Установка Tag для кнопок
            SetButtonTags();

            // Обновить состояние
            UpdateP1();
            UpdateP2();
        }

        /// <summary>
        /// Загрузка формы - установка начальных значений
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Отображение текущего числа
                baseTextBox.Text = ctl.Ed.Number;
                if (string.IsNullOrEmpty(baseTextBox.Text))
                {
                    baseTextBox.Text = "0";
                }

                trackBar1.Value = ctl.Pin;
                baseUpDown.Value = ctl.Pin;

                trackBar2.Value = ctl.Pout;
                resultUpDown.Value = ctl.Pout;

                baseNumSys.Text = "Основание с. сч. исходного числа " + trackBar1.Value;
                resultNumSys.Text = "Основание с. сч. результата " + trackBar2.Value;

                resultTextBox.Text = "0";

                SetButtonTags();

                UpdateButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке формы: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Установка номеров команд в свойство Tag кнопок
        /// </summary>
        private void SetButtonTags()
        {
            try
            {
                zeroBtn.Tag = 0;
                oneBtn.Tag = 1;
                twoBtn.Tag = 2;
                theeBtn.Tag = 3;
                fourBtn.Tag = 4;
                fiveBtn.Tag = 5;
                sixBtn.Tag = 6;
                sevenBtn.Tag = 7;
                eightBtn.Tag = 8;
                nineBtn.Tag = 9;

                abtn.Tag = 10;      // A
                bBtn.Tag = 11;       // B
                cBtn.Tag = 12;       // C
                dBtn.Tag = 13;       // D
                eBtn.Tag = 14;       // E
                FBtn.Tag = 15;       // F

                // Командные кнопки
                dotBtn.Tag = 17;     // Разделитель
                bsBtn.Tag = 18;      // Backspace
                ceBtn.Tag = 19;      // Clear
                execBtn.Tag = 20;     // Execute
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при установке Tag кнопок: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик события нажатия командной кнопки
        /// </summary>
        private void button_Click(object sender, EventArgs e)
        {
            try
            {
                // Ссылка на компонент, на котором кликнули мышью
                Button but = (Button)sender;

                // Номер выбранной команды из Tag
                if (but.Tag == null) return;

                int j = Convert.ToInt32(but.Tag);

                // Выполнить команду
                DoCmnd(j);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выполнении команды: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DoCmnd(int j)
        {
            try
            {
                if (j == 20)
                {
                    string result = ctl.DoCmnd(j);
                    resultTextBox.Text = result;
                }
                else
                {
                    // Если состояние "Преобразовано" и команда не служебная 
                    if (ctl.St == State.Преобразовано && j < 20)
                    {
                        // Очистить содержимое редактора 
                        baseTextBox.Text = ctl.DoCmnd(18);
                        while (!string.IsNullOrEmpty(baseTextBox.Text))
                        {
                            baseTextBox.Text = ctl.DoCmnd(18);
                        }
                    }

                    // Выполнить команду редактирования
                    string result = ctl.DoCmnd(j);

                    // Обработка специальных команд
                    if (j == 22) // Очистка истории
                    {
                        // Ничего не делаем с текстовыми полями
                    }
                    else if (j == 23) // Количество записей
                    {
                        MessageBox.Show(result, "Информация");
                    }
                    else if (j == 24) // Последняя запись
                    {
                        if (!result.Contains("пуста"))
                        {
                            MessageBox.Show(result, "Последняя запись");
                        }
                    }
                    else
                    {
                        // Обычные команды редактирования
                        baseTextBox.Text = result;

                        // Если число пустое, отображаем 0 для наглядности
                        if (string.IsNullOrEmpty(baseTextBox.Text))
                        {
                            baseTextBox.Text = "0";
                        }

                        resultTextBox.Text = "0";
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show($"Ошибка: система счисления должна быть от 2 до 16\n{ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"Ошибка ввода: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButtons()
        {
            try
            {
                // Просмотреть все компоненты формы
                foreach (Control control in this.Controls)
                {
                    if (control is Button button && button.Tag != null)
                    {
                        int j = Convert.ToInt32(button.Tag);

                        if (j <= 15)
                        {
                            // Доступны только цифры меньше основания
                            button.Enabled = (j < ctl.Pin);
                        }
                        // Остальные командные кнопки всегда доступны
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении кнопок: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateP1()
        {
            try
            {
                // Проверяем, что значение в допустимых пределах
                int newPin = trackBar1.Value;
                if (newPin < 2 || newPin > 16)
                {
                    return; 
                }

                // Синхронизируем baseUpDown с trackBar1
                if (baseUpDown.Value != newPin)
                {
                    baseUpDown.Value = newPin;
                }

                baseNumSys.Text = "Основание с. сч. исходного числа " + newPin;

                // Сохранить р1 в объекте управление
                ctl.Pin = newPin;

                // Обновить состояние командных кнопок
                UpdateButtons();

                // Очистить редактор
                if (!string.IsNullOrEmpty(baseTextBox.Text) && baseTextBox.Text != "0")
                {
                    baseTextBox.Text = ctl.DoCmnd(18);
                    while (!string.IsNullOrEmpty(baseTextBox.Text) && baseTextBox.Text != "0")
                    {
                        baseTextBox.Text = ctl.DoCmnd(18);
                    }
                }

                baseTextBox.Text = "0";
                resultTextBox.Text = "0";
            }
            catch (ArgumentOutOfRangeException)
            {
                // Игнорируем ошибки выхода за пределы диапазона
            }
            catch (Exception ex)
            {
                // Для других ошибок показываем сообщение
                MessageBox.Show($"Ошибка при изменении основания: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateP2()
        {
            try
            {
                // Проверяем, что значение в допустимых пределах
                int newPout = trackBar2.Value;
                if (newPout < 2 || newPout > 16)
                {
                    return; 
                }

                // Синхронизируем resultUpDown с trackBar2
                if (resultUpDown.Value != newPout)
                {
                    resultUpDown.Value = newPout;
                }

                ctl.Pout = newPout;

                resultNumSys.Text = "Основание с. сч. результата " + newPout;

                // Если есть число в редакторе и оно не "0", пересчитать результат
                if (!string.IsNullOrEmpty(baseTextBox.Text) && baseTextBox.Text != "0")
                {
                    resultTextBox.Text = ctl.DoCmnd(20);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Игнорируем ошибки выхода за пределы диапазона
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при изменении основания результата: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                baseUpDown.Value = trackBar1.Value;
                UpdateP1();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Игнорируем ошибку
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Изменение основания p1 через NumericUpDown
        /// </summary>
        private void baseUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что значение в пределах допустимого
                int value = (int)baseUpDown.Value;
                if (value >= 2 && value <= 16)
                {
                    trackBar1.Value = value;
                    UpdateP1();
                }
                else
                {
                    // Если значение вышло за пределы, возвращаем предыдущее корректное значение
                    baseUpDown.Value = ctl.Pin;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                baseUpDown.Value = ctl.Pin;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            try
            {
                // Значение trackBar2 уже находится в пределах 2-16
                resultUpDown.Value = trackBar2.Value;
                UpdateP2();
            }
            catch (ArgumentOutOfRangeException)
            {
                // Игнорируем ошибку - ничего не делаем
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Изменение основания p2 через NumericUpDown
        /// </summary>
        private void resultUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Проверяем, что значение в пределах допустимого
                int value = (int)resultUpDown.Value;
                if (value >= 2 && value <= 16)
                {
                    trackBar2.Value = value;
                    UpdateP2();
                }
                else
                {
                    // Если значение вышло за пределы, возвращаем предыдущее корректное значение
                    resultUpDown.Value = ctl.Pout;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // Игнорируем ошибку - ничего не делаем
                // Возвращаем предыдущее значение
                resultUpDown.Value = ctl.Pout;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Пункт меню Выход
        /// </summary>
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Пункт меню История
        /// </summary>
        private void historyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // Создаем и показываем форму истории
                Form historyForm = new Form();
                historyForm.Text = "История преобразований";
                historyForm.Size = new System.Drawing.Size(500, 400);
                historyForm.StartPosition = FormStartPosition.CenterParent;

                TextBox textBox = new TextBox
                {
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Location = new System.Drawing.Point(12, 12),
                    Size = new System.Drawing.Size(460, 300),
                    ReadOnly = true,
                    Font = new System.Drawing.Font("Consolas", 10)
                };

                Button closeBtn = new Button
                {
                    Text = "Закрыть",
                    Location = new System.Drawing.Point(200, 320),
                    Size = new System.Drawing.Size(100, 30)
                };
                closeBtn.Click += (s, args) => historyForm.Close();

                historyForm.Controls.Add(textBox);
                historyForm.Controls.Add(closeBtn);

                // Заполняем историю
                if (ctl.His.Count() == 0)
                {
                    textBox.Text = "История пуста";
                }
                else
                {
                    for (int i = 0; i < ctl.His.Count(); i++)
                    {
                        textBox.AppendText(ctl.His[i] + "\r\n");
                    }
                }

                historyForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии истории: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Пункт меню Справка
        /// </summary>
        private void infoBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Конвертер систем счисления\n\n" +
                "Версия 1.0\n\n" +
                "Программа для конвертации чисел между различными системами счисления (2-16).\n\n" +
                "Разработано в учебных целях.",
                "О программе",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        /// <summary>
        /// Обработка алфавитно-цифровых клавиш - отключено
        /// </summary>
        private void Конвертер_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Блокируем любой ввод с клавиатуры
            e.Handled = true;
        }

        /// <summary>
        /// Обработка клавиш управления - отключено
        /// </summary>
        private void Конвертер_KeyDown(object sender, KeyEventArgs e)
        {
            // Блокируем любой ввод с клавиатуры
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        // Привязка всех кнопок к единому обработчику button_Click
        private void zeroBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void oneBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void twoBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void button19_Click(object sender, EventArgs e) => button_Click(sender, e); // theeBtn
        private void fourBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void fiveBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void sixBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void sevenBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void eightBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void nineBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void abtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void bBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void cBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void dBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void eBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void FBtn_Click(object sender, EventArgs e) => button_Click(sender, e);

        // Командные кнопки
        private void dotBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void button9_Click(object sender, EventArgs e) => button_Click(sender, e); // bsBtn
        private void ceBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void button18_Click(object sender, EventArgs e) => button_Click(sender, e); // execBtn

        // Кнопки меню
        private void button1_Click(object sender, EventArgs e) => exitBtn_Click(sender, e);

        // Остальные обработчики
        private void baseTextBox_TextChanged(object sender, EventArgs e) { }
        private void resultTextBox_TextChanged(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void button12_Click(object sender, EventArgs e) => abtn_Click(sender, e);
        private void button17_Click(object sender, EventArgs e) => sixBtn_Click(sender, e);
    }
}