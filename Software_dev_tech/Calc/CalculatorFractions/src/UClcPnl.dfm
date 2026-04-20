object ClcPnl: TClcPnl
  Left = 0
  Top = 0
  Caption = 'Калькулятор простых дробей'
  ClientHeight = 350
  ClientWidth = 400
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -12
  Font.Name = 'Segoe UI'
  Font.Style = []
  Menu = MainMenu
  Position = poScreenCenter
  OnKeyPress = FormKeyPress
  PixelsPerInch = 96
  TextHeight = 15
  object DisplayLabel: TStaticText
    Left = 20
    Top = 20
    Width = 360
    Height = 40
    Alignment = taRightJustify
    AutoSize = False
    BevelEdges = []
    BevelInner = bvLowered
    Caption = '0/1'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -24
    Font.Name = 'Consolas'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 0
  end
  object MemoryStatusLabel: TStaticText
    Left = 20
    Top = 65
    Width = 30
    Height = 17
    Caption = '   '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'Segoe UI'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 1
  end
  object BtnMC: TBitButton
    Left = 20
    Top = 90
    Width = 60
    Height = 35
    Caption = 'MC'
    TabOrder = 2
    OnClick = ButtonClick
  end
  object BtnMR: TBitButton
    Left = 90
    Top = 90
    Width = 60
    Height = 35
    Caption = 'MR'
    TabOrder = 3
    OnClick = ButtonClick
  end
  object BtnMPlus: TBitButton
    Left = 160
    Top = 90
    Width = 60
    Height = 35
    Caption = 'M+'
    TabOrder = 4
    OnClick = ButtonClick
  end
  object BtnMS: TBitButton
    Left = 230
    Top = 90
    Width = 60
    Height = 35
    Caption = 'MS'
    TabOrder = 5
    OnClick = ButtonClick
  end
  object BtnC: TBitButton
    Left = 20
    Top = 135
    Width = 60
    Height = 35
    Caption = 'C'
    TabOrder = 6
    OnClick = ButtonClick
  end
  object BtnCE: TBitButton
    Left = 90
    Top = 135
    Width = 60
    Height = 35
    Caption = 'CE'
    TabOrder = 7
    OnClick = ButtonClick
  end
  object BtnBackspace: TBitButton
    Left = 160
    Top = 135
    Width = 60
    Height = 35
    Caption = '<-'
    TabOrder = 8
    OnClick = ButtonClick
  end
  object BtnDiv: TBitButton
    Left = 230
    Top = 135
    Width = 60
    Height = 35
    Caption = '/'
    TabOrder = 9
    OnClick = ButtonClick
  end
  object Btn7: TBitButton
    Left = 20
    Top = 180
    Width = 60
    Height = 35
    Caption = '7'
    TabOrder = 10
    OnClick = ButtonClick
  end
  object Btn8: TBitButton
    Left = 90
    Top = 180
    Width = 60
    Height = 35
    Caption = '8'
    TabOrder = 11
    OnClick = ButtonClick
  end
  object Btn9: TBitButton
    Left = 160
    Top = 180
    Width = 60
    Height = 35
    Caption = '9'
    TabOrder = 12
    OnClick = ButtonClick
  end
  object BtnMul: TBitButton
    Left = 230
    Top = 180
    Width = 60
    Height = 35
    Caption = '*'
    TabOrder = 13
    OnClick = ButtonClick
  end
  object BtnSqr: TBitButton
    Left = 300
    Top = 180
    Width = 80
    Height = 35
    Caption = 'Sqr'
    TabOrder = 14
    OnClick = ButtonClick
  end
  object Btn4: TBitButton
    Left = 20
    Top = 225
    Width = 60
    Height = 35
    Caption = '4'
    TabOrder = 15
    OnClick = ButtonClick
  end
  object Btn5: TBitButton
    Left = 90
    Top = 225
    Width = 60
    Height = 35
    Caption = '5'
    TabOrder = 16
    OnClick = ButtonClick
  end
  object Btn6: TBitButton
    Left = 160
    Top = 225
    Width = 60
    Height = 35
    Caption = '6'
    TabOrder = 17
    OnClick = ButtonClick
  end
  object BtnSub: TBitButton
    Left = 230
    Top = 225
    Width = 60
    Height = 35
    Caption = '-'
    TabOrder = 18
    OnClick = ButtonClick
  end
  object BtnRev: TBitButton
    Left = 300
    Top = 225
    Width = 80
    Height = 35
    Caption = 'Rev'
    TabOrder = 19
    OnClick = ButtonClick
  end
  object Btn1: TBitButton
    Left = 20
    Top = 270
    Width = 60
    Height = 35
    Caption = '1'
    TabOrder = 20
    OnClick = ButtonClick
  end
  object Btn2: TBitButton
    Left = 90
    Top = 270
    Width = 60
    Height = 35
    Caption = '2'
    TabOrder = 21
    OnClick = ButtonClick
  end
  object Btn3: TBitButton
    Left = 160
    Top = 270
    Width = 60
    Height = 35
    Caption = '3'
    TabOrder = 22
    OnClick = ButtonClick
  end
  object BtnAdd: TBitButton
    Left = 230
    Top = 270
    Width = 60
    Height = 35
    Caption = '+'
    TabOrder = 23
    OnClick = ButtonClick
  end
  object BtnSign: TBitButton
    Left = 300
    Top = 270
    Width = 80
    Height = 35
    Caption = '+/-'
    TabOrder = 24
    OnClick = ButtonClick
  end
  object Btn0: TBitButton
    Left = 20
    Top = 315
    Width = 60
    Height = 35
    Caption = '0'
    TabOrder = 25
    OnClick = ButtonClick
  end
  object BtnSeparator: TBitButton
    Left = 90
    Top = 315
    Width = 60
    Height = 35
    Caption = '/'
    Hint = 'Разделитель дроби'
    TabOrder = 26
    OnClick = ButtonClick
  end
  object BtnEqual: TBitButton
    Left = 160
    Top = 315
    Width = 130
    Height = 35
    Caption = '='
    TabOrder = 27
    OnClick = ButtonClick
  end
  object MainMenu: TMainMenu
    Left = 320
    Top = 10
    object EditMenu: TMenuItem
      Caption = '&Правка'
      object CopyItem: TMenuItem
        Caption = '&Копировать'
        OnClick = CopyClick
      end
      object PasteItem: TMenuItem
        Caption = '&Вставить'
        OnClick = PasteClick
      end
    end
    object ViewMenu: TMenuItem
      Caption = '&Вид'
      object FractionModeItem: TMenuItem
        Caption = '&Дробь'
        OnClick = FractionModeClick
      end
      object NumberModeItem: TMenuItem
        Caption = '&Число'
        OnClick = NumberModeClick
      end
    end
    object HelpMenu: TMenuItem
      Caption = '&Справка'
      object AboutItem: TMenuItem
        Caption = '&О программе...'
        OnClick = AboutClick
      end
    end
  end
end
