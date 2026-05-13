namespace Calculator.Core;

using Calculator.Core.Numbers;

public class CalculatorForm : Form
{
    private TCtrl _controller;
    private string _buffer = string.Empty;
    private string _memoryState = string.Empty;
    private NumberType _currentNumberType = NumberType.Real;
    private int _currentBaseSystem = 10;

    private Label _display;
    private Label _memoryStatus;
    private Label _modeStatus;
    private ComboBox _baseComboBox;
    private Button _btn0, _btn1, _btn2, _btn3, _btn4, _btn5, _btn6, _btn7, _btn8, _btn9;
    private Button _btnA, _btnB, _btnC, _btnD, _btnE, _btnF;
    private Button _btnDot, _btnPlusMinus, _btnBackspace, _btnClear, _btnClearAll;
    private Button _btnAdd, _btnSubtract, _btnMultiply, _btnDivide, _btnEquals;
    private Button _btnSin, _btnCos, _btnTan, _btnSqrt, _btnLog, _btnLn, _btnAbs, _btnExp;
    private Button _btnSqr, _btnRev;
    private Button _btnMPlus, _btnMMinus, _btnMR, _btnMC;
    private Button _btnFracSep, _btnComplexSep;
    private MenuStrip _menuStrip;

    // Размеры в 2 раза больше
    private const int BTN_W = 150;
    private const int BTN_H = 80;
    private const int GAP = 12;
    private const int START_X = 30;
    private const int START_Y = 220;

    public CalculatorForm()
    {
        _controller = new TCtrl();
        InitializeComponent();
        UpdateModeDisplay();
    }

    private void InitializeComponent()
    {
        this.Text = "Универсальный калькулятор";
        this.Size = new Size(1400, 1400);
        this.MinimumSize = new Size(1100, 1200);
        this.FormBorderStyle = FormBorderStyle.Sizable;
        this.MaximizeBox = true;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.KeyPreview = true;
        this.KeyDown += CalculatorForm_KeyDown;
        this.BackColor = Color.FromArgb(30, 30, 30);

        // Меню
        _menuStrip = new MenuStrip();
        _menuStrip.BackColor = Color.FromArgb(50, 50, 50);
        _menuStrip.ForeColor = Color.White;
        _menuStrip.Font = new Font("Segoe UI", 12);

        var editMenuItem = new ToolStripMenuItem("Правка");
        editMenuItem.DropDownItems.Add(new ToolStripMenuItem("Копировать", null, MenuCopy_Click, Keys.Control | Keys.C));
        editMenuItem.DropDownItems.Add(new ToolStripMenuItem("Вставить", null, MenuPaste_Click, Keys.Control | Keys.V));

        var viewMenuItem = new ToolStripMenuItem("Вид");
        viewMenuItem.DropDownItems.Add(new ToolStripMenuItem("Обычный", null, MenuNormal_Click));
        viewMenuItem.DropDownItems.Add(new ToolStripMenuItem("Инженерный", null, MenuEngineering_Click));

        var numberTypeMenuItem = new ToolStripMenuItem("Тип числа");
        numberTypeMenuItem.DropDownItems.Add(new ToolStripMenuItem("P-ичные числа", null, MenuReal_Click));
        numberTypeMenuItem.DropDownItems.Add(new ToolStripMenuItem("Рациональные дроби", null, MenuFrac_Click));
        numberTypeMenuItem.DropDownItems.Add(new ToolStripMenuItem("Комплексные числа", null, MenuComplex_Click));

        var helpMenuItem = new ToolStripMenuItem("Справка");
        helpMenuItem.DropDownItems.Add(new ToolStripMenuItem("Справка (F1)", null, MenuHelp_Click, Keys.F1));
        helpMenuItem.DropDownItems.Add(new ToolStripMenuItem("О программе", null, MenuAbout_Click));

        _menuStrip.Items.Add(editMenuItem);
        _menuStrip.Items.Add(viewMenuItem);
        _menuStrip.Items.Add(numberTypeMenuItem);
        _menuStrip.Items.Add(helpMenuItem);
        this.MainMenuStrip = _menuStrip;

        // Дисплей
        _display = new Label
        {
            Text = "0",
            Font = new Font("Consolas", 48, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleRight,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(30, 45),
            Size = new Size(1050, 100),
            BackColor = Color.FromArgb(20, 20, 20),
            ForeColor = Color.FromArgb(0, 255, 100)
        };

        _memoryStatus = new Label
        {
            Text = "",
            Font = new Font("Arial", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Location = new Point(30, 155),
            Size = new Size(80, 35),
            ForeColor = Color.FromArgb(255, 200, 0),
            BackColor = Color.FromArgb(30, 30, 30)
        };

        _modeStatus = new Label
        {
            Text = "P-ичные | основание=10",
            Font = new Font("Arial", 14, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Location = new Point(120, 155),
            Size = new Size(400, 35),
            ForeColor = Color.FromArgb(100, 200, 255),
            BackColor = Color.FromArgb(30, 30, 30)
        };

        // ComboBox для выбора основания
        var baseLabel = new Label
        {
            Text = "Основание:",
            Font = new Font("Arial", 14, FontStyle.Bold),
            Location = new Point(550, 155),
            Size = new Size(120, 35),
            ForeColor = Color.White,
            BackColor = Color.FromArgb(30, 30, 30)
        };

        _baseComboBox = new ComboBox
        {
            Location = new Point(680, 155),
            Size = new Size(100, 35),
            Font = new Font("Arial", 14),
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = Color.FromArgb(60, 60, 60),
            ForeColor = Color.White
        };
        for (int i = 2; i <= 16; i++)
            _baseComboBox.Items.Add(i.ToString());
        _baseComboBox.SelectedIndex = 8; // 10 по умолчанию
        _baseComboBox.SelectedIndexChanged += BaseComboBox_SelectedIndexChanged;

        // Кнопки
        int x = START_X, y = START_Y;

        // Ряд 1: Инженерные функции
        _btnSin = CreateButton("sin", TCtrl.CMD_FUNC_SIN, x, y, Color.FromArgb(70, 70, 90));
        _btnCos = CreateButton("cos", TCtrl.CMD_FUNC_COS, x + BTN_W + GAP, y, Color.FromArgb(70, 70, 90));
        _btnTan = CreateButton("tan", TCtrl.CMD_FUNC_TAN, x + 2 * (BTN_W + GAP), y, Color.FromArgb(70, 70, 90));
        _btnSqrt = CreateButton("√x", TCtrl.CMD_FUNC_SQRT, x + 3 * (BTN_W + GAP), y, Color.FromArgb(70, 70, 90));
        _btnSqr = CreateButton("x²", TCtrl.CMD_FUNC_SQR, x + 4 * (BTN_W + GAP), y, Color.FromArgb(70, 70, 90));
        _btnRev = CreateButton("1/x", TCtrl.CMD_FUNC_REV, x + 5 * (BTN_W + GAP), y, Color.FromArgb(70, 70, 90));
        _btnAbs = CreateButton("|x|", TCtrl.CMD_FUNC_ABS, x + 6 * (BTN_W + GAP), y, Color.FromArgb(70, 70, 90));

        y += BTN_H + GAP;
        // Ряд 2: Логарифмы и память
        _btnLog = CreateButton("log₁₀", TCtrl.CMD_FUNC_LOG, x, y, Color.FromArgb(70, 70, 90));
        _btnLn = CreateButton("ln", TCtrl.CMD_FUNC_LN, x + BTN_W + GAP, y, Color.FromArgb(70, 70, 90));
        _btnExp = CreateButton("eˣ", TCtrl.CMD_FUNC_EXP, x + 2 * (BTN_W + GAP), y, Color.FromArgb(70, 70, 90));
        _btnMPlus = CreateButton("M+", TCtrl.CMD_MEMORY_ADD, x + 3 * (BTN_W + GAP), y, Color.FromArgb(90, 70, 70));
        _btnMMinus = CreateButton("M-", TCtrl.CMD_MEMORY_SUBTRACT, x + 4 * (BTN_W + GAP), y, Color.FromArgb(90, 70, 70));
        _btnMR = CreateButton("MR", TCtrl.CMD_MEMORY_RECALL, x + 5 * (BTN_W + GAP), y, Color.FromArgb(90, 70, 70));
        _btnMC = CreateButton("MC", TCtrl.CMD_MEMORY_CLEAR, x + 6 * (BTN_W + GAP), y, Color.FromArgb(90, 70, 70));

        y += BTN_H + GAP;
        // Ряд 3: Цифры 7-9 и операции
        _btn7 = CreateButton("7", TCtrl.CMD_DIGIT_7, x, y);
        _btn8 = CreateButton("8", TCtrl.CMD_DIGIT_8, x + BTN_W + GAP, y);
        _btn9 = CreateButton("9", TCtrl.CMD_DIGIT_9, x + 2 * (BTN_W + GAP), y);
        _btnDivide = CreateButton("÷", TCtrl.CMD_DIVIDE, x + 3 * (BTN_W + GAP), y, Color.FromArgb(255, 140, 0));
        _btnClear = CreateButton("C", TCtrl.CMD_CLEAR, x + 4 * (BTN_W + GAP), y, Color.FromArgb(200, 50, 50));
        _btnClearAll = CreateButton("CA", TCtrl.CMD_CLEAR_ALL, x + 5 * (BTN_W + GAP), y, Color.FromArgb(200, 50, 50));
        _btnBackspace = CreateButton("⌫", TCtrl.CMD_BACKSPACE, x + 6 * (BTN_W + GAP), y, Color.FromArgb(200, 50, 50));

        y += BTN_H + GAP;
        // Ряд 4: Цифры 4-6
        _btn4 = CreateButton("4", TCtrl.CMD_DIGIT_4, x, y);
        _btn5 = CreateButton("5", TCtrl.CMD_DIGIT_5, x + BTN_W + GAP, y);
        _btn6 = CreateButton("6", TCtrl.CMD_DIGIT_6, x + 2 * (BTN_W + GAP), y);
        _btnMultiply = CreateButton("×", TCtrl.CMD_MULTIPLY, x + 3 * (BTN_W + GAP), y, Color.FromArgb(255, 140, 0));
        _btnPlusMinus = CreateButton("±", TCtrl.CMD_CHANGE_SIGN, x + 4 * (BTN_W + GAP), y);
        _btnSubtract = CreateButton("−", TCtrl.CMD_SUBTRACT, x + 5 * (BTN_W + GAP), y, Color.FromArgb(255, 140, 0));
        _btnEquals = CreateButton("=", TCtrl.CMD_EQUALS, x + 6 * (BTN_W + GAP), y, Color.FromArgb(0, 200, 100));

        y += BTN_H + GAP;
        // Ряд 5: Цифры 1-3
        _btn1 = CreateButton("1", TCtrl.CMD_DIGIT_1, x, y);
        _btn2 = CreateButton("2", TCtrl.CMD_DIGIT_2, x + BTN_W + GAP, y);
        _btn3 = CreateButton("3", TCtrl.CMD_DIGIT_3, x + 2 * (BTN_W + GAP), y);
        _btnAdd = CreateButton("+", TCtrl.CMD_ADD, x + 3 * (BTN_W + GAP), y, Color.FromArgb(255, 140, 0));
        _btnDot = CreateButton(".", TCtrl.CMD_DECIMAL_POINT, x + 4 * (BTN_W + GAP), y);
        _btnFracSep = CreateButton("/", TCtrl.CMD_SEPARATOR_FRAC, x + 5 * (BTN_W + GAP), y, Color.FromArgb(100, 100, 200));
        _btnComplexSep = CreateButton("i*", TCtrl.CMD_SEPARATOR_COMPLEX, x + 6 * (BTN_W + GAP), y, Color.FromArgb(100, 100, 200));

        y += BTN_H + GAP;
        // Ряд 6: Цифра 0 и шестнадцатеричные
        _btn0 = CreateButton("0", TCtrl.CMD_DIGIT_0, x, y, width: 2 * BTN_W + GAP);
        _btnA = CreateButton("A", TCtrl.CMD_DIGIT_A, x + 2 * (BTN_W + GAP), y, Color.FromArgb(80, 80, 120));
        _btnB = CreateButton("B", TCtrl.CMD_DIGIT_B, x + 3 * (BTN_W + GAP), y, Color.FromArgb(80, 80, 120));
        _btnC = CreateButton("C", TCtrl.CMD_DIGIT_C, x + 4 * (BTN_W + GAP), y, Color.FromArgb(80, 80, 120));
        _btnD = CreateButton("D", TCtrl.CMD_DIGIT_D, x + 5 * (BTN_W + GAP), y, Color.FromArgb(80, 80, 120));
        _btnE = CreateButton("E", TCtrl.CMD_DIGIT_E, x + 6 * (BTN_W + GAP), y, Color.FromArgb(80, 80, 120));

        y += BTN_H + GAP;
        _btnF = CreateButton("F", TCtrl.CMD_DIGIT_F, x, y, Color.FromArgb(80, 80, 120));

        // Добавление элементов
        this.Controls.Add(_display);
        this.Controls.Add(_memoryStatus);
        this.Controls.Add(_modeStatus);
        this.Controls.Add(baseLabel);
        this.Controls.Add(_baseComboBox);
        this.Controls.Add(_menuStrip);

        AddButton(_btn0); AddButton(_btn1); AddButton(_btn2); AddButton(_btn3);
        AddButton(_btn4); AddButton(_btn5); AddButton(_btn6); AddButton(_btn7);
        AddButton(_btn8); AddButton(_btn9);
        AddButton(_btnA); AddButton(_btnB); AddButton(_btnC);
        AddButton(_btnD); AddButton(_btnE); AddButton(_btnF);
        AddButton(_btnDot); AddButton(_btnPlusMinus); AddButton(_btnBackspace);
        AddButton(_btnClear); AddButton(_btnClearAll);
        AddButton(_btnAdd); AddButton(_btnSubtract); AddButton(_btnMultiply); AddButton(_btnDivide); AddButton(_btnEquals);
        AddButton(_btnSin); AddButton(_btnCos); AddButton(_btnTan);
        AddButton(_btnSqrt); AddButton(_btnLog); AddButton(_btnLn); AddButton(_btnAbs); AddButton(_btnExp);
        AddButton(_btnSqr); AddButton(_btnRev);
        AddButton(_btnMPlus); AddButton(_btnMMinus); AddButton(_btnMR); AddButton(_btnMC);
        AddButton(_btnFracSep); AddButton(_btnComplexSep);

        UpdateButtonVisibility();
    }

    private void AddButton(Button btn)
    {
        if (btn != null) this.Controls.Add(btn);
    }

    private Button CreateButton(string text, int command, int x, int y, Color? color = null, int width = BTN_W, int height = BTN_H)
    {
        var button = new Button
        {
            Text = text,
            Font = new Font("Segoe UI", 18, FontStyle.Regular),
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

    private void UpdateButtonVisibility()
    {
        bool isHex = _currentBaseSystem == 16;
        bool isHighBase = _currentBaseSystem > 10;

        _btnA.Visible = isHighBase;
        _btnB.Visible = isHighBase;
        _btnC.Visible = isHighBase;
        _btnD.Visible = isHighBase;
        _btnE.Visible = isHighBase;
        _btnF.Visible = isHex;

        _btnFracSep.Visible = _currentNumberType == NumberType.Fraction;
        _btnComplexSep.Visible = _currentNumberType == NumberType.Complex;
        _btnDot.Visible = _currentNumberType != NumberType.Fraction;

        _baseComboBox.SelectedIndex = _currentBaseSystem - 2;
    }

    private void UpdateModeDisplay()
    {
        string mode = _currentNumberType switch
        {
            NumberType.Real => "P-ичные",
            NumberType.Fraction => "Дроби",
            NumberType.Complex => "Комплексные",
            _ => "Real"
        };
        _modeStatus.Text = $"{mode} | основание={_currentBaseSystem}";
    }

    private void BaseComboBox_SelectedIndexChanged(object? sender, EventArgs e)
    {
        int newBase = _baseComboBox.SelectedIndex + 2;
        if (newBase != _currentBaseSystem)
        {
            _currentBaseSystem = newBase;
            _controller.Editor.BaseSystem = newBase;
            _controller.Number = _controller.Editor.GetNumber();
            UpdateButtonVisibility();
            UpdateModeDisplay();
            _display.Text = _controller.Number.ReadNumberAsString();
        }
    }

    private void Button_Click(object? sender, EventArgs e)
    {
        if (sender is Button button && button.Tag is int command)
        {
            string result = _controller.ExecuteCalculatorCommand(command, ref _buffer, ref _memoryState);
            _display.Text = result;
            _memoryStatus.Text = _memoryState;
        }
    }

    private void CalculatorForm_KeyDown(object? sender, KeyEventArgs e)
    {
        int command = -1;

        if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
            command = e.KeyCode - Keys.D0;
        else if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
            command = e.KeyCode - Keys.NumPad0;
        else if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.F)
            command = TCtrl.CMD_DIGIT_A + (e.KeyCode - Keys.A);
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
                Keys.F1 => -2,
                _ => -1
            };
        }

        if (command == -2)
        {
            MenuHelp_Click(sender, e);
            e.SuppressKeyPress = true;
        }
        else if (command >= 0)
        {
            string result = _controller.ExecuteCalculatorCommand(command, ref _buffer, ref _memoryState);
            _display.Text = result;
            _memoryStatus.Text = _memoryState;
            e.SuppressKeyPress = true;
        }
    }

    private void MenuCopy_Click(object? sender, EventArgs e)
    {
        _controller.ExecuteCalculatorCommand(TCtrl.CMD_COPY, ref _buffer, ref _memoryState);
        Clipboard.SetText(_buffer);
    }

    private void MenuPaste_Click(object? sender, EventArgs e)
    {
        try
        {
            string text = Clipboard.GetText();
            if (_currentNumberType == NumberType.Real)
            {
                if (double.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double val))
                {
                    _controller.Number = new TPNumber(val, _currentBaseSystem);
                    _controller.Editor.SetNumber(_controller.Number);
                    _display.Text = _controller.Number.ReadNumberAsString();
                }
            }
            else if (_currentNumberType == NumberType.Fraction)
            {
                _controller.Number = new Frac(text);
                _controller.Editor.SetNumber(_controller.Number);
                _display.Text = _controller.Number.ReadNumberAsString();
            }
            else if (_currentNumberType == NumberType.Complex)
            {
                _controller.Number = new TComplex(text);
                _controller.Editor.SetNumber(_controller.Number);
                _display.Text = _controller.Number.ReadNumberAsString();
            }
        }
        catch { }
    }

    private void MenuHelp_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(_controller.Help.ShowHelpMenu(), "Справка", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void MenuReal_Click(object? sender, EventArgs e) => SetNumberType(NumberType.Real, _currentBaseSystem);
    private void MenuFrac_Click(object? sender, EventArgs e) => SetNumberType(NumberType.Fraction, _currentBaseSystem);
    private void MenuComplex_Click(object? sender, EventArgs e) => SetNumberType(NumberType.Complex, _currentBaseSystem);

    private void SetNumberType(NumberType type, int baseSystem)
    {
        _currentNumberType = type;
        _currentBaseSystem = baseSystem;
        _controller.Editor = new TEditor(type, baseSystem);
        _controller.Number = type switch
        {
            NumberType.Real => new TPNumber(0, baseSystem),
            NumberType.Fraction => new Frac(0, 1),
            NumberType.Complex => new TComplex(0, 0),
            _ => new TPNumber(0, baseSystem)
        };
        _controller.Processor.ReSet();
        _display.Text = _controller.Number.ReadNumberAsString();
        UpdateButtonVisibility();
        UpdateModeDisplay();
    }

    private void MenuNormal_Click(object? sender, EventArgs e) => HideEngineeringButtons();
    private void MenuEngineering_Click(object? sender, EventArgs e) => ShowEngineeringButtons();

    private void MenuAbout_Click(object? sender, EventArgs e)
    {
        MessageBox.Show(_controller.Help.GetTopic("about"), "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

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
        _btnSqr.Visible = false;
        _btnRev.Visible = false;
        _btnMPlus.Visible = false;
        _btnMMinus.Visible = false;
        _btnMR.Visible = false;
        _btnMC.Visible = false;
        _btnBackspace.Visible = false;
        _btnPlusMinus.Visible = false;
    }

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
        _btnSqr.Visible = true;
        _btnRev.Visible = true;
        _btnMPlus.Visible = true;
        _btnMMinus.Visible = true;
        _btnMR.Visible = true;
        _btnMC.Visible = true;
        _btnBackspace.Visible = true;
        _btnPlusMinus.Visible = true;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _display?.Dispose();
            _memoryStatus?.Dispose();
            _modeStatus?.Dispose();
            _baseComboBox?.Dispose();
            _menuStrip?.Dispose();
            _controller = null;
        }
        base.Dispose(disposing);
    }
}
