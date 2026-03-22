using System;
using System.Windows.Forms;

namespace Converter_p1_p2
{
    public partial class ��������� : Form
    {
        // ������ ������ ����������
        private Control_ ctl = new Control_();

        public ���������()
        {
            InitializeComponent();
            // ������������� ����������� ������� ��� ����������
            this.KeyPreview = true;

            // ��������� ���� � resultTextBox
            resultTextBox.ReadOnly = true;
            resultTextBox.BackColor = System.Drawing.SystemColors.Window;

            // ��������� ���� � baseTextBox
            baseTextBox.ReadOnly = true;
            baseTextBox.BackColor = System.Drawing.SystemColors.Window;

            // ����������� ������������
            this.trackBar1.Scroll += trackBar1_Scroll;
            this.trackBar2.Scroll += trackBar2_Scroll;
            this.baseUpDown.ValueChanged += baseUpDown_ValueChanged;
            this.resultUpDown.ValueChanged += resultUpDown_ValueChanged;

            // ��������� ��������� �������� (�������������� � �������� 2-16)
            trackBar1.Value = 10;
            trackBar2.Value = 16;
            baseUpDown.Value = 10;
            resultUpDown.Value = 16;

            // ��������� Tag ��� ������
            SetButtonTags();

            // �������� ���������
            UpdateP1();
            UpdateP2();
        }

        /// <summary>
        /// �������� ����� - ��������� ��������� ��������
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // ����������� �������� �����
                baseTextBox.Text = ctl.Ed.Number;
                if (string.IsNullOrEmpty(baseTextBox.Text))
                {
                    baseTextBox.Text = "0";
                }

                // ��������� �.��. ��������� ����� �1
                trackBar1.Value = ctl.Pin;
                baseUpDown.Value = ctl.Pin;

                // ��������� �.��. ���������� �2
                trackBar2.Value = ctl.Pout;
                resultUpDown.Value = ctl.Pout;

                // �������
                baseNumSys.Text = "��������� �. ��. ��������� ����� " + trackBar1.Value;
                resultNumSys.Text = "��������� �. ��. ���������� " + trackBar2.Value;

                // ���������
                resultTextBox.Text = "0";

                // ��������� Tag ��� ���� ������
                SetButtonTags();

                // �������� ��������� ��������� ������
                UpdateButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� �������� �����: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ������� ������ � �������� Tag ������
        /// </summary>
        private void SetButtonTags()
        {
            try
            {
                // �������� ������ 0-9
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

                // ��������� ������ A-F
                abtn.Tag = 10;      // A
                bBtn.Tag = 11;       // B
                cBtn.Tag = 12;       // C
                dBtn.Tag = 13;       // D
                eBtn.Tag = 14;       // E
                FBtn.Tag = 15;       // F

                // ��������� ������
                dotBtn.Tag = 17;     // �����������
                bsBtn.Tag = 18;      // Backspace
                ceBtn.Tag = 19;      // Clear
                execBtn.Tag = 20;     // Execute
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ��������� Tag ������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ���������� ������� ������� ��������� ������
        /// </summary>
        private void button_Click(object sender, EventArgs e)
        {
            try
            {
                // ������ �� ���������, �� ������� �������� �����
                Button but = (Button)sender;

                // ����� ��������� ������� �� �������� Tag
                if (but.Tag == null) return;

                int j = Convert.ToInt32(but.Tag);

                // ��������� �������
                DoCmnd(j);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ���������� �������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ������� ����������
        /// </summary>
        /// <param name="j">����� �������</param>
        private void DoCmnd(int j)
        {
            try
            {
                // ������� ���������� (Enter/Execute) - ����� 20
                if (j == 20)
                {
                    string result = ctl.DoCmnd(j);
                    resultTextBox.Text = result;
                }
                else
                {
                    // ���� ��������� "�������������" � ������� �� ��������� (21-24)
                    if (ctl.St == State.������������� && j < 20)
                    {
                        // �������� ���������� ��������� (������� 18 - BS �� ������ �������)
                        baseTextBox.Text = ctl.DoCmnd(18);
                        while (!string.IsNullOrEmpty(baseTextBox.Text))
                        {
                            baseTextBox.Text = ctl.DoCmnd(18);
                        }
                    }

                    // ��������� ������� ��������������
                    string result = ctl.DoCmnd(j);

                    // ��������� ����������� ������
                    if (j == 22) // ������� �������
                    {
                        // ������ �� ������ � ���������� ������
                    }
                    else if (j == 23) // ���������� �������
                    {
                        MessageBox.Show(result, "����������");
                    }
                    else if (j == 24) // ��������� ������
                    {
                        if (!result.Contains("�����"))
                        {
                            MessageBox.Show(result, "��������� ������");
                        }
                    }
                    else
                    {
                        // ������� ������� ��������������
                        baseTextBox.Text = result;

                        // ���� ����� ������, ���������� 0 ��� �����������
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
                MessageBox.Show($"������: ������� ��������� ������ ���� �� 2 �� 16\n{ex.Message}",
                    "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"������ �����: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ��������� ��������� ������ �� ��������� �.��. ��������� �����
        /// </summary>
        private void UpdateButtons()
        {
            try
            {
                // ����������� ��� ���������� �����
                foreach (Control control in this.Controls)
                {
                    if (control is Button button && button.Tag != null)
                    {
                        int j = Convert.ToInt32(button.Tag);

                        // ��� �������� ������ (0-15)
                        if (j <= 15)
                        {
                            // �������� ������ ����� ������ ���������
                            button.Enabled = (j < ctl.Pin);
                        }
                        // ��������� ��������� ������ ������ ��������
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ���������� ������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ����������� ���������� ��� ����� ��. �.��. �1
        /// </summary>
        private void UpdateP1()
        {
            try
            {
                // ���������, ��� �������� � ���������� ��������
                int newPin = trackBar1.Value;
                if (newPin < 2 || newPin > 16)
                {
                    return; // ������ �� ������, ���� �������� ��� ���������
                }

                // �������������� baseUpDown � trackBar1
                if (baseUpDown.Value != newPin)
                {
                    baseUpDown.Value = newPin;
                }

                baseNumSys.Text = "��������� �. ��. ��������� ����� " + newPin;

                // ��������� �1 � ������� ����������
                ctl.Pin = newPin;

                // �������� ��������� ��������� ������
                UpdateButtons();

                // �������� ��������
                if (!string.IsNullOrEmpty(baseTextBox.Text) && baseTextBox.Text != "0")
                {
                    baseTextBox.Text = ctl.DoCmnd(18);
                    while (!string.IsNullOrEmpty(baseTextBox.Text) && baseTextBox.Text != "0")
                    {
                        baseTextBox.Text = ctl.DoCmnd(18);
                    }
                }

                // ������������� 0
                baseTextBox.Text = "0";
                resultTextBox.Text = "0";
            }
            catch (ArgumentOutOfRangeException)
            {
                // ���������� ������ ������ �� ������� ���������
            }
            catch (Exception ex)
            {
                // ��� ������ ������ ���������� ���������
                MessageBox.Show($"������ ��� ��������� ���������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ����������� ���������� ��� ����� ��. �.��. �2
        /// </summary>
        private void UpdateP2()
        {
            try
            {
                // ���������, ��� �������� � ���������� ��������
                int newPout = trackBar2.Value;
                if (newPout < 2 || newPout > 16)
                {
                    return; // ������ �� ������, ���� �������� ��� ���������
                }

                // �������������� resultUpDown � trackBar2
                if (resultUpDown.Value != newPout)
                {
                    resultUpDown.Value = newPout;
                }

                // ���������� ��������� ����������
                ctl.Pout = newPout;

                // �������� �������
                resultNumSys.Text = "��������� �. ��. ���������� " + newPout;

                // ���� ���� ����� � ��������� � ��� �� "0", ����������� ���������
                if (!string.IsNullOrEmpty(baseTextBox.Text) && baseTextBox.Text != "0")
                {
                    resultTextBox.Text = ctl.DoCmnd(20);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // ���������� ������ ������ �� ������� ���������
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ��������� ��������� ����������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region ����������� ������� ��� ��������� p1

        #region ����������� ������� ��� ��������� p1

        /// <summary>
        /// ��������� ��������� p1 ����� TrackBar
        /// </summary>
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                // �������� trackBar1 ��� ��������� � �������� 2-16
                baseUpDown.Value = trackBar1.Value;
                UpdateP1();
            }
            catch (ArgumentOutOfRangeException)
            {
                // ���������� ������ - ������ �� ������
            }
            catch (Exception ex)
            {
                // ��� ������ ������ ���������� ���������
                MessageBox.Show($"������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ��������� p1 ����� NumericUpDown
        /// </summary>
        private void baseUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // ���������, ��� �������� � �������� �����������
                int value = (int)baseUpDown.Value;
                if (value >= 2 && value <= 16)
                {
                    trackBar1.Value = value;
                    UpdateP1();
                }
                else
                {
                    // ���� �������� ����� �� �������, ���������� ���������� ���������� ��������
                    baseUpDown.Value = ctl.Pin;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // ���������� ������ - ������ �� ������
                // ���������� ���������� ��������
                baseUpDown.Value = ctl.Pin;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ����������� ������� ��� ��������� p2

        /// <summary>
        /// ��������� ��������� p2 ����� TrackBar
        /// </summary>
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            try
            {
                // �������� trackBar2 ��� ��������� � �������� 2-16
                resultUpDown.Value = trackBar2.Value;
                UpdateP2();
            }
            catch (ArgumentOutOfRangeException)
            {
                // ���������� ������ - ������ �� ������
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ��������� ��������� p2 ����� NumericUpDown
        /// </summary>
        private void resultUpDown_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // ���������, ��� �������� � �������� �����������
                int value = (int)resultUpDown.Value;
                if (value >= 2 && value <= 16)
                {
                    trackBar2.Value = value;
                    UpdateP2();
                }
                else
                {
                    // ���� �������� ����� �� �������, ���������� ���������� ���������� ��������
                    resultUpDown.Value = ctl.Pout;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                // ���������� ������ - ������ �� ������
                // ���������� ���������� ��������
                resultUpDown.Value = ctl.Pout;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #endregion

        #region ����������� ������� ����

        /// <summary>
        /// ����� ���� �����
        /// </summary>
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// ����� ���� �������
        /// </summary>
        private void historyBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // ������� � ���������� ����� �������
                Form historyForm = new Form();
                historyForm.Text = "������� ��������������";
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
                    Text = "�������",
                    Location = new System.Drawing.Point(200, 320),
                    Size = new System.Drawing.Size(100, 30)
                };
                closeBtn.Click += (s, args) => historyForm.Close();

                historyForm.Controls.Add(textBox);
                historyForm.Controls.Add(closeBtn);

                // ��������� �������
                if (ctl.His.Count() == 0)
                {
                    textBox.Text = "������� �����";
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
                MessageBox.Show($"������ ��� �������� �������: {ex.Message}", "������",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// ����� ���� �������
        /// </summary>
        private void infoBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "��������� ������ ���������\n\n" +
                "������ 1.0\n\n" +
                "��������� ��� ����������� ����� ����� ���������� ��������� ��������� (2-16).\n\n" +
                "����������� � ������� �����.",
                "� ���������",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        #endregion

        #region ����������� ������� ���������� (���������)

        /// <summary>
        /// ��������� ���������-�������� ������ - ���������
        /// </summary>
        private void ���������_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ��������� ����� ���� � ����������
            e.Handled = true;
        }

        /// <summary>
        /// ��������� ������ ���������� - ���������
        /// </summary>
        private void ���������_KeyDown(object sender, KeyEventArgs e)
        {
            // ��������� ����� ���� � ����������
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        #endregion

        #region ������������ ����������� (�������� � button_Click)

        // �������� ���� ������ � ������� ����������� button_Click
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

        // ��������� ������
        private void dotBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void button9_Click(object sender, EventArgs e) => button_Click(sender, e); // bsBtn
        private void ceBtn_Click(object sender, EventArgs e) => button_Click(sender, e);
        private void button18_Click(object sender, EventArgs e) => button_Click(sender, e); // execBtn

        // ������ ����
        private void button1_Click(object sender, EventArgs e) => exitBtn_Click(sender, e);

        // ��������� ����������� (������)
        private void baseTextBox_TextChanged(object sender, EventArgs e) { }
        private void resultTextBox_TextChanged(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void button12_Click(object sender, EventArgs e) => abtn_Click(sender, e);
        private void button17_Click(object sender, EventArgs e) => sixBtn_Click(sender, e);

        #endregion
    }
}