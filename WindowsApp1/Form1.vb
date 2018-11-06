﻿Public Class Form1
    Dim m1 As New MultiVerticalMeter.MultiVerticalMeter
    Dim m2 As New TrackBar


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        With m1
            .Location = New Point(50, 50)
            .Name = "m1"
        End With

        Me.Controls.Add(m1)

        With m2
            .Location = New Point(150, 100)
            .Name = "m2"
            .Maximum = 100
        End With

        Me.Controls.Add(m2)
        AddHandler m2.ValueChanged, AddressOf nup

    End Sub

    Private Sub nup()
        m1.value = m2.value
    End Sub

End Class
