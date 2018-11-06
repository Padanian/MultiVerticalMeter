﻿Public Class MultiVerticalMeter
    Const Pi As Double = Math.PI
    Dim x1, y1, x2, y2 As Integer
    Dim xp1, yp1, xp2, yp2 As Int32
    Private m_value As Integer
    Private m_minimum As Integer
    Private m_maximum As Integer
    Private m_WarningThreshold As Integer = 80
    Private m_isLedVisible As Boolean = True
    Private WarningActive As Boolean = False
    Dim centreX As Double = 32
    Dim centreY As Double = 32
    Dim apen As New Pen(Color.LightGray, 1)
    Dim lpen As New Pen(Color.Black, 2)
    Dim tpen As New Pen(Color.Transparent, 2)
    Dim radius As Double = 30
    Dim blinkingLedTimer As New Timer


    Public Property WarningThreshold As Integer
        Get
            WarningThreshold = m_WarningThreshold
        End Get
        Set(WarningThreshold As Integer)
            m_WarningThreshold = WarningThreshold
            Dim c = value
            value = c
            Me.Refresh()
        End Set
    End Property
    Public Property isLedVisible As Boolean
        Get
            isLedVisible = m_isLedVisible
        End Get
        Set(isLedVisible As Boolean)
            m_isLedVisible = isLedVisible
            Dim c = value
            value = c
            Me.Refresh()
        End Set
    End Property
    Public Property value As Integer
        Get
            value = m_value
        End Get
        Set(value As Integer)
            If value > maximum Or value < minimum Then
                MsgBox("Value needs to be between Min and Max")
                Exit Property
            End If
            m_value = value
            If value >= WarningThreshold / 100 * maximum Then
                WarningActive = True
            Else
                WarningActive = False
            End If
            Me.Refresh()
        End Set
    End Property
    Public Property minimum As Integer
        Get
            minimum = m_minimum
        End Get
        Set(minimum As Integer)
            If minimum > maximum Then
                MsgBox("Min can't be greater than Max")
                Exit Property
            End If
            m_minimum = minimum
            If value < minimum Then
                value = minimum
            End If
            For Each lbl In Me.Controls
                Select Case lbl.tag
                    Case "lbl0"
                        lbl.text = minimum
                    Case "lbl2"
                        lbl.text = (maximum - minimum) / 10 * (2.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl4"
                        lbl.text = (maximum - minimum) / 10 * (4.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl6"
                        lbl.text = (maximum - minimum) / 10 * (6.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl8"
                        lbl.text = (maximum - minimum) / 10 * (8.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl10"
                        lbl.text = (maximum - minimum) / 10 * (10.ToString.PadLeft(2, "0")) + minimum
                End Select
            Next
            Dim c = value
            value = c
            Me.Refresh()
        End Set
    End Property
    Public Property maximum As Integer
        Get
            maximum = m_maximum
        End Get
        Set(maximum As Integer)
            If maximum < minimum Then
                MsgBox("Max can't be smaller than Min")
                Exit Property
            End If
            m_maximum = maximum
            If value > maximum Then
                value = maximum
            End If
            For Each lbl In Me.Controls
                Select Case lbl.tag
                    Case "lbl0"
                        lbl.text = minimum
                    Case "lbl2"
                        lbl.text = (maximum - minimum) / 10 * (2.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl4"
                        lbl.text = (maximum - minimum) / 10 * (4.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl6"
                        lbl.text = (maximum - minimum) / 10 * (6.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl8"
                        lbl.text = (maximum - minimum) / 10 * (8.ToString.PadLeft(2, "0")) + minimum
                    Case "lbl10"
                        lbl.text = (maximum - minimum) / 10 * (10.ToString.PadLeft(2, "0")) + minimum
                End Select
            Next
            Dim c = value
            value = c
            Me.Refresh()
        End Set
    End Property
    Private Sub GaugePaint(sender As Object, e As PaintEventArgs) Handles Me.Paint

        y1 = 132
        x1 = 12
        e.Graphics.DrawLine(apen, x1, y1, x1, y1 - 120)
        Dim i As Integer = 0
        For num As Double = 132 To 12 Step -12
            x1 = 10
            y1 = num
            x2 = 16
            y2 = num

            e.Graphics.DrawLine(apen, x1, y1, x2, y2)
            addlabel(i)
            i += 1
        Next

        redrawLine(e)

        If isLedVisible And WarningActive And Not blinkingLedTimer.Enabled Then
            With blinkingLedTimer
                .Interval = 1000
                .Enabled = True
                .Start()
            End With
        ElseIf Not isLedVisible Or Not WarningActive Then
            With blinkingLedTimer
                .Enabled = False
                .Stop()
                pbLED.Visible = False
            End With
        End If

    End Sub
    Private Sub addlabel(ByVal i As Integer)
        Dim lblScale As New Label
        With lblScale
            .Location = New Point(x2, y2 - 4)
            .Size = New Size(18, 10)
            .Text = (maximum - minimum) / 10 * (i.ToString.PadLeft(2, "0"))
            .Font = New Font("Segoe UI", 5, FontStyle.Regular)
            .ForeColor = Color.Black
            .BackColor = Color.Transparent
            .TextAlign = ContentAlignment.TopLeft
            .Tag = "lbl" & i.ToString
        End With
        For Each lbl In Me.Controls
            If TypeOf (lbl) Is Label And lbl.tag = "lbl" & i.ToString Then
                Exit Sub
            End If
        Next
        If i Mod 2 = 0 Then
            Me.Controls.Add(lblScale)
        End If
    End Sub
    Sub New()

        ' La chiamata è richiesta dalla finestra di progettazione.
        InitializeComponent()
        AddHandler blinkingLedTimer.Tick, AddressOf blinkingLED

        ' Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
        minimum = 0
        maximum = 100
        value = 0

    End Sub
    Public Sub redrawLine(e As PaintEventArgs)
        Dim Lend As Double = 132 - (minimum + value) * 1.2 * (100 / maximum)
        Dim Lstart = 132


        xp1 = 8
        yp1 = Lstart
        xp2 = 8
        yp2 = Lend
        e.Graphics.DrawLine(lpen, xp1, yp1, xp2, yp2)

    End Sub
    Private Sub blinkingLED()

        pbLED.Visible = Not pbLED.Visible

    End Sub
End Class