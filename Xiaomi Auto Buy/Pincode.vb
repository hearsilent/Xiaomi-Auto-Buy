Public Class Pincode
    Public Sub New(_Pincode As String, _Bank As String)
        ' 此為設計工具所需的呼叫。
        InitializeComponent()
        ' 在 InitializeComponent() 呼叫之後加入任何初始設定。
        Me.Icon = Loginfrm.Icon
        PincodeLabel.Text = "PINCODE：" & _Pincode
        If _Bank = "7-11" Then
            Me.Text = "PinCode (請至7-11繳費)"
            BankLabel.Text = "本超商付款由7-11關係企業提供服務，" & vbCrLf & "取得代碼後必須在24小時內前往7-11便利商店付款。"
        ElseIf _Bank = "Family" Then
            Me.Text = "PinCode (請至全家繳費)"
            BankLabel.Text = "本超商付款由全家關係企業提供服務，" & vbCrLf & "取得代碼後必須在24小時內前往全家便利商店付款。"
        End If
        QRCodePictureBox.ImageLocation = "https://chart.googleapis.com/chart?cht=qr&chs=120x120&choe=UTF-8&chld=H|0&chl=" & _Pincode
        Clipboard.Clear()
        Clipboard.SetText(_Pincode)
        MsgBox("Pincode已複製至剪貼簿 !", MsgBoxStyle.Information)

        ' Set up ToolTip. 
        ToolTip.AutoPopDelay = 5000
        ToolTip.InitialDelay = 1
        ToolTip.ReshowDelay = 1
        ToolTip.ShowAlways = True
        ToolTip.UseAnimation = True
        ToolTip.UseFading = True

        ' Set up the ToolTip text for the Button and Checkbox. 
        ToolTip.SetToolTip(Me.QRCodePictureBox, "QRCode技術由Google API提供")
    End Sub
End Class