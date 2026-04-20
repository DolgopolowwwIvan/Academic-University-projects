namespace Calculator.Core;

// Форма калькулятора
#nullable disable
public class CalculatorForm : Form
{
    private TCtrl _controller;
    private string _buffer = string.Empty;
    private string _memoryState = string.Empty;

    private Label _display;
    private Label _memoryStatus;
    private Button _btn0;
    private Button _btn1;
    private Button _btn2;
    private Button _btn3;
    private Button _btn4;
    private Button _btn5;
    private Button _btn6;
    private Button _btn7;
    private Button _btn8;
    private Button _btn9;
    private Button _btnDot;
    private Button _btnPlusMinus;
    private Button _btnBackspace;
    private Button _btnClear;
    private Button _btnClearAll;
    private Button _btnAdd;
    private Button _btnSubtract;
    private Button _btnMultiply;
    private Button _btnDivide;
    private Button _btnEquals;
    private Button _btnSin;
    private Button _btnCos;
    private Button _btnTan;
    private Button _btnSqrt;
    private Button _btnLog;
    private Button _btnLn;
    private Button _btnAbs;
    private Button _btnExp;
    private Button _btnMPlus;
    private Button _btnMMinus;
    private Button _btnMR;
    private Button _btnMC;
    private MenuStrip _menuStrip;

    public CalculatorForm()
    {
        _controller = new TCtrl();
        InitializeComponent();
    }

    // Инициализация компонентов
    private void InitializeComponent()
    {
        this.Text = "Калькулятор";
        this.Size = new Size(700, 700);
        this.MinimumSize = new Size(550, 700);
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.KeyPreview = true;
        this.KeyDown += CalculatorForm_KeyDown;
        this.BackColor = Color.FromArgb(30, 30, 30);

        // Меню
        _menuStrip = new MenuStrip();
        _menuStrip.BackColor = Color.FromArgb(50, 50, 50);
        _menuStrip.ForeColor = Color.White;

        var editMenuItem = new ToolStripMenuItem("Правка");
        editMenuItem.DropDownItems.Add(new ToolStripMenuItem("Копировать", null, MenuCopy_Click, Keys.Control | Keys.C));
        editMenuItem.DropDownItems.Add(new ToolStripMenuItem("Вставить", null, MenuPaste_Click, Keys.Control | Keys.V));

        var viewMenuItem = new ToolStripMenuItem("Вид");
        viewMenuItem.DropDownItems.Add(new ToolStripMenuItem("Обычный", null, MenuNormal_Click));
        viewMenuItem.DropDownItems.Add(new ToolStripMenuItem("Инженерный", null, MenuEngineering_Click));

        var helpMenuItem = new ToolStripMenuItem("Справка");
        helpMenuItem.DropDownItems.Add(new ToolStripMenuItem("О программе", null, MenuAbout_Click));

        _menuStrip.Items.Add(editMenuItem);
        _menuStrip.Items.Add(viewMenuItem);
        _menuStrip.Items.Add(helpMenuItem);

        this.MainMenuStrip = _menuStrip;

        // Дисплей
        _display = new Label
        {
            Text = "0",
            Font = new Font("Consolas", 32, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleRight,
            BorderStyle = BorderStyle.None,
            Location = new Point(20, 35),
            Size = new Size(500, 60),
            BackColor = Color.FromArgb(20, 20, 20),
            ForeColor = Color.FromArgb(0, 255, 100)
        };

        _memoryStatus = new Label
        {
            Text = "",
            Font = new Font("Arial", 11, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Location = new Point(20, 95),
            Size = new Size(60, 25),
            ForeColor = Color.FromArgb(255, 200, 0),
            BackColor = Color.FromArgb(30, 30, 30)
        };

        // Кнопки
        int btnWidth = 75;
        int btnHeight = 50;
        int startX = 20;
        int startY = 140;
        int gap = 8;

        // Инженерные функции
        _btnSin = CreateButton("sin", TCtrl.CMD_FUNC_SIN, startX, startY, color: Color.FromArgb(70, 70, 90));
        _btnCos = CreateButton("cos", TCtrl.CMD_FUNC_COS, startX + btnWidth + gap, startY, color: Color.FromArgb(70, 70, 90));
        _btnTan = CreateButton("tan", TCtrl.CMD_FUNC_TAN, startX + 2 * (btnWidth + gap), startY, color: Color.FromArgb(70, 70, 90));
        _btnSqrt = CreateButton("√x", TCtrl.CMD_FUNC_SQRT, startX + 3 * (btnWidth + gap), startY, color: Color.FromArgb(70, 70, 90));
        _btnLog = CreateButton("log₁₀", TCtrl.CMD_FUNC_LOG, startX + 4 * (btnWidth + gap), startY, color: Color.FromArgb(70, 70, 90));
        _btnLn = CreateButton("ln", TCtrl.CMD_FUNC_LN, startX + 5 * (btnWidth + gap), startY, color: Color.FromArgb(70, 70, 90));
        _btnExp = CreateButton("eˣ", TCtrl.CMD_FUNC_EXP, startX + 6 * (btnWidth + gap), startY, color: Color.FromArgb(70, 70, 90));

        startY += btnHeight + gap;
        _btnAbs = CreateButton("|x|", TCtrl.CMD_FUNC_ABS, startX, startY, color: Color.FromArgb(70, 70, 90));
        _btnMPlus = CreateButton("M+", TCtrl.CMD_MEMORY_ADD, startX + btnWidth + gap, startY, color: Color.FromArgb(90, 70, 70));
        _btnMMinus = CreateButton("M-", TCtrl.CMD_MEMORY_SUBTRACT, startX + 2 * (btnWidth + gap), startY, color: Color.FromArgb(90, 70, 70));
        _btnMR = CreateButton("MR", TCtrl.CMD_MEMORY_RECALL, startX + 3 * (btnWidth + gap), startY, color: Color.FromArgb(90, 70, 70));
        _btnMC = CreateButton("MC", TCtrl.CMD_MEMORY_CLEAR, startX + 4 * (btnWidth + gap), startY, color: Color.FromArgb(90, 70, 70));

        startY += btnHeight + gap;
        // Цифры и операции
        _btn7 = CreateButton("7", TCtrl.CMD_DIGIT_7, startX, startY, color: Color.FromArgb(60, 60, 60));
        _btn8 = CreateButton("8", TCtrl.CMD_DIGIT_8, startX + btnWidth + gap, startY, color: Color.FromArgb(60, 60, 60));
        _btn9 = CreateButton("9", TCtrl.CMD_DIGIT_9, startX + 2 * (btnWidth + gap), startY, color: Color.FromArgb(60, 60, 60));
        _btnDivide = CreateButton("÷", TCtrl.CMD_DIVIDE, startX + 3 * (btnWidth + gap), startY, color: Color.FromArgb(255, 140, 0));
        _btnClear = CreateButton("C", TCtrl.CMD_CLEAR, startX + 4 * (btnWidth + gap), startY, color: Color.FromArgb(200, 50, 50));
        _btnClearAll = CreateButton("CA", TCtrl.CMD_CLEAR_ALL, startX + 5 * (btnWidth + gap), startY, color: Color.FromArgb(200, 50, 50));
        _btnBackspace = CreateButton("⌫", TCtrl.CMD_BACKSPACE, startX + 6 * (btnWidth + gap), startY, color: Color.FromArgb(200, 50, 50));

        startY += btnHeight + gap;
        _btn4 = CreateButton("4", TCtrl.CMD_DIGIT_4, startX, startY, color: Color.FromArgb(60, 60, 60));
        _btn5 = CreateButton("5", TCtrl.CMD_DIGIT_5, startX + btnWidth + gap, startY, color: Color.FromArgb(60, 60, 60));
        _btn6 = CreateButton("6", TCtrl.CMD_DIGIT_6, startX + 2 * (btnWidth + gap), startY, color: Color.FromArgb(60, 60, 60));
        _btnMultiply = CreateButton("×", TCtrl.CMD_MULTIPLY, startX + 3 * (btnWidth + gap), startY, color: Color.FromArgb(255, 140, 0));
        _btnPlusMinus = CreateButton("±", TCtrl.CMD_CHANGE_SIGN, startX + 4 * (btnWidth + gap), startY, color: Color.FromArgb(60, 60, 60));
        _btnSubtract = CreateButton("−", TCtrl.CMD_SUBTRACT, startX + 5 * (btnWidth + gap), startY, color: Color.FromArgb(255, 140, 0));
        _btnEquals = CreateButton("=", TCtrl.CMD_EQUALS, startX + 6 * (btnWidth + gap), startY, color: Color.FromArgb(0, 200, 100));

        startY += btnHeight + gap;
        _btn1 = CreateButton("1", TCtrl.CMD_DIGIT_1, startX, startY, color: Color.FromArgb(60, 60, 60));
        _btn2 = CreateButton("2", TCtrl.CMD_DIGIT_2, startX + btnWidth + gap, startY, color: Color.FromArgb(60, 60, 60));
        _btn3 = CreateButton("3", TCtrl.CMD_DIGIT_3, startX + 2 * (btnWidth + gap), startY, color: Color.FromArgb(60, 60, 60));
        _btnDot = CreateButton(".", TCtrl.CMD_DECIMAL_POINT, startX + 3 * (btnWidth + gap), startY, color: Color.FromArgb(60, 60, 60));
        _btnAdd = CreateButton("+", TCtrl.CMD_ADD, startX + 4 * (btnWidth + gap), startY, width: 2 * btnWidth + gap, color: Color.FromArgb(255, 140, 0));

        startY += btnHeight + gap;
        _btn0 = CreateButton("0", TCtrl.CMD_DIGIT_0, startX, startY, width: 2 * btnWidth + gap, color: Color.FromArgb(60, 60, 60));

        // Добавление элементов
        this.Controls.Add(_display);
        this.Controls.Add(_memoryStatus);
        this.Controls.Add(_btn0);
        this.Controls.Add(_btn1);
        this.Controls.Add(_btn2);
        this.Controls.Add(_btn3);
        this.Controls.Add(_btn4);
        this.Controls.Add(_btn5);
        this.Controls.Add(_btn6);
        this.Controls.Add(_btn7);
        this.Controls.Add(_btn8);
        this.Controls.Add(_btn9);
        this.Controls.Add(_btnDot);
        this.Controls.Add(_btnPlusMinus);
        this.Controls.Add(_btnBackspace);
        this.Controls.Add(_btnClear);
        this.Controls.Add(_btnClearAll);
        this.Controls.Add(_btnAdd);
        this.Controls.Add(_btnSubtract);
        this.Controls.Add(_btnMultiply);
        this.Controls.Add(_btnDivide);
        this.Controls.Add(_btnEquals);
        this.Controls.Add(_btnSin);
        this.Controls.Add(_btnCos);
        this.Controls.Add(_btnTan);
        this.Controls.Add(_btnSqrt);
        this.Controls.Add(_btnLog);
        this.Controls.Add(_btnLn);
        this.Controls.Add(_btnAbs);
        this.Controls.Add(_btnExp);
        this.Controls.Add(_btnMPlus);
        this.Controls.Add(_btnMMinus);
        this.Controls.Add(_btnMR);
        this.Controls.Add(_btnMC);

        this.Controls.Add(_menuStrip);
    }

    // Создать кнопку
    private Button CreateButton(string text, int command, int x, int y, int width = 75, int height = 50, Color? color = null)
    {
        var button = new Button
        {
            Text = text,
            Font = new Font("Segoe UI", 14, FontStyle.Regular),
            Size = new Size(width, height),
            Location = new Point(x, y),
            Tag = command,
            BackColor = color ?? Color.FromArgb(60, 60, 60),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(80, 80, 80);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(100, 100, 100);
        button.Click += Button_Click;
        return button;
    }

    // Обработчик нажатия кнопки
    private void Button_Click(object sender, EventArgs e)
    {
        if (sender is Button button && button.Tag is int command)
        {
            string result = _controller.ExecuteCalculatorCommand(command, ref _buffer, ref _memoryState);
            _display.Text = result;
            _memoryStatus.Text = _memoryState;
        }
    }

    // Обработчик нажатия клавиши
    private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
    {
        int command = -1;

        if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
        {
            command = e.KeyCode - Keys.D0;
        }
        else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
        {
            command = e.KeyCode - Keys.NumPad0;
        }
        else
        {
            command = e.KeyCode switch
            {
                Keys.OemPeriod or Keys.Decimal => TCtrl.CMD_DECIMAL_POINT,
                Keys.Add => TCtrl.CMD_ADD,
                Keys.Subtract => TCtrl.CMD_SUBTRACT,
                Keys.Multiply => TCtrl.CMD_MULTIPLY,
                Keys.Divide => TCtrl.CMD_DIVIDE,
                Keys.Enter or Keys.Oemplus => TCtrl.CMD_EQUALS,
                Keys.Escape => TCtrl.CMD_CLEAR_ALL,
                Keys.Back => TCtrl.CMD_BACKSPACE,
                _ => -1
            };
        }

        if (command >= 0)
        {
            string result = _controller.ExecuteCalculatorCommand(command, ref _buffer, ref _memoryState);
            _display.Text = result;
            _memoryStatus.Text = _memoryState;
            e.SuppressKeyPress = true;
        }
    }

    // Копировать в буфер обмена
    private void MenuCopy_Click(object sender, EventArgs e)
    {
        int command = TCtrl.CMD_COPY;
        _controller.ExecuteCalculatorCommand(command, ref _buffer, ref _memoryState);
        Clipboard.SetText(_buffer);
    }

    // Вставить из буфера обмена
    private void MenuPaste_Click(object sender, EventArgs e)
    {
        try
        {
            string clipboardText = Clipboard.GetText();
            if (double.TryParse(clipboardText, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double value))
            {
                _controller.Number.Value = value;
                _controller.Editor.SetNumber(_controller.Number);
                _display.Text = _controller.Number.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        catch
        {
            // Игнорируем ошибки буфера обмена
        }
    }

    // Обычный режим
    private void MenuNormal_Click(object sender, EventArgs e)
    {
        HideEngineeringButtons();
    }

    // Инженерный режим
    private void MenuEngineering_Click(object sender, EventArgs e)
    {
        ShowEngineeringButtons();
    }

    // О программе
    private void MenuAbout_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Калькулятор v1.0\nРазработано на C#\n\nФункции:\n• Арифметические операции (+, −, ×, ÷)\n• Тригонометрические (sin, cos, tan)\n• Логарифмы (log₁₀, ln)\n• Степени и корни (√x, eˣ)\n• Память (M+, M-, MR, MC)", 
            "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    // Скрыть инженерные кнопки
    private void HideEngineeringButtons()
    {
        _btnSin.Visible = false;
        _btnCos.Visible = false;
        _btnTan.Visible = false;
        _btnSqrt.Visible = false;
        _btnLog.Visible = false;
        _btnLn.Visible = false;
        _btnExp.Visible = false;
        _btnAbs.Visible = false;
        _btnMPlus.Visible = false;
        _btnMMinus.Visible = false;
        _btnMR.Visible = false;
        _btnMC.Visible = false;
        _btnBackspace.Visible = false;
        _btnPlusMinus.Visible = false;
        this.Size = new Size(550, 520);
        this.MinimumSize = new Size(550, 520);
    }

    // Показать инженерные кнопки
    private void ShowEngineeringButtons()
    {
        _btnSin.Visible = true;
        _btnCos.Visible = true;
        _btnTan.Visible = true;
        _btnSqrt.Visible = true;
        _btnLog.Visible = true;
        _btnLn.Visible = true;
        _btnExp.Visible = true;
        _btnAbs.Visible = true;
        _btnMPlus.Visible = true;
        _btnMMinus.Visible = true;
        _btnMR.Visible = true;
        _btnMC.Visible = true;
        _btnBackspace.Visible = true;
        _btnPlusMinus.Visible = true;
        this.Size = new Size(550, 700);
        this.MinimumSize = new Size(550, 700);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _display?.Dispose();
            _memoryStatus?.Dispose();
            _btn0?.Dispose();
            _btn1?.Dispose();
            _btn2?.Dispose();
            _btn3?.Dispose();
            _btn4?.Dispose();
            _btn5?.Dispose();
            _btn6?.Dispose();
            _btn7?.Dispose();
            _btn8?.Dispose();
            _btn9?.Dispose();
            _btnDot?.Dispose();
            _btnPlusMinus?.Dispose();
            _btnBackspace?.Dispose();
            _btnClear?.Dispose();
            _btnClearAll?.Dispose();
            _btnAdd?.Dispose();
            _btnSubtract?.Dispose();
            _btnMultiply?.Dispose();
            _btnDivide?.Dispose();
            _btnEquals?.Dispose();
            _btnSin?.Dispose();
            _btnCos?.Dispose();
            _btnTan?.Dispose();
            _btnSqrt?.Dispose();
            _btnLog?.Dispose();
            _btnLn?.Dispose();
            _btnAbs?.Dispose();
            _btnExp?.Dispose();
            _btnMPlus?.Dispose();
            _btnMMinus?.Dispose();
            _btnMR?.Dispose();
            _btnMC?.Dispose();
            _menuStrip?.Dispose();
            _controller = null;
        }
        base.Dispose(disposing);
    }
}
#nullable enable
