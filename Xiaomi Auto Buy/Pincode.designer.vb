<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Pincode
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意:  以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.QRCodePictureBox = New System.Windows.Forms.PictureBox()
        Me.BankLabel = New System.Windows.Forms.Label()
        Me.PincodeLabel = New System.Windows.Forms.Label()
        Me.BarcodeLabel = New System.Windows.Forms.Label()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.QRCodePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'QRCodePictureBox
        '
        Me.QRCodePictureBox.ErrorImage = Nothing
        Me.QRCodePictureBox.InitialImage = Nothing
        Me.QRCodePictureBox.Location = New System.Drawing.Point(347, 9)
        Me.QRCodePictureBox.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.QRCodePictureBox.Name = "QRCodePictureBox"
        Me.QRCodePictureBox.Size = New System.Drawing.Size(120, 120)
        Me.QRCodePictureBox.TabIndex = 0
        Me.QRCodePictureBox.TabStop = False
        '
        'BankLabel
        '
        Me.BankLabel.AutoSize = True
        Me.BankLabel.Font = New System.Drawing.Font("微軟正黑體", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.BankLabel.ForeColor = System.Drawing.Color.Sienna
        Me.BankLabel.Location = New System.Drawing.Point(12, 32)
        Me.BankLabel.Name = "BankLabel"
        Me.BankLabel.Size = New System.Drawing.Size(314, 34)
        Me.BankLabel.TabIndex = 1
        Me.BankLabel.Text = "本超商付款由7-11關係企業提供服務，" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "取得代碼後必須在24小時內前往7-11便利商店付款。"
        Me.BankLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PincodeLabel
        '
        Me.PincodeLabel.Font = New System.Drawing.Font("微軟正黑體", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.PincodeLabel.ForeColor = System.Drawing.Color.Sienna
        Me.PincodeLabel.Location = New System.Drawing.Point(11, 76)
        Me.PincodeLabel.Name = "PincodeLabel"
        Me.PincodeLabel.Size = New System.Drawing.Size(315, 34)
        Me.PincodeLabel.TabIndex = 2
        Me.PincodeLabel.Text = "PINCODE：208758392269"
        Me.PincodeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BarcodeLabel
        '
        Me.BarcodeLabel.AutoSize = True
        Me.BarcodeLabel.Font = New System.Drawing.Font("微軟正黑體", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.BarcodeLabel.ForeColor = System.Drawing.Color.Sienna
        Me.BarcodeLabel.Location = New System.Drawing.Point(324, 133)
        Me.BarcodeLabel.Name = "BarcodeLabel"
        Me.BarcodeLabel.Size = New System.Drawing.Size(161, 17)
        Me.BarcodeLabel.TabIndex = 3
        Me.BarcodeLabel.Text = "掃描二維碼取得PIN碼資訊"
        '
        'Pincode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(506, 160)
        Me.Controls.Add(Me.QRCodePictureBox)
        Me.Controls.Add(Me.BarcodeLabel)
        Me.Controls.Add(Me.PincodeLabel)
        Me.Controls.Add(Me.BankLabel)
        Me.Font = New System.Drawing.Font("微軟正黑體", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Pincode"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PinCode"
        Me.TopMost = True
        CType(Me.QRCodePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents QRCodePictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents BankLabel As System.Windows.Forms.Label
    Friend WithEvents PincodeLabel As System.Windows.Forms.Label
    Friend WithEvents BarcodeLabel As System.Windows.Forms.Label
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
End Class
