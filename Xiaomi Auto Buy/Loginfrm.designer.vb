<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Loginfrm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Loginfrm))
        Me.LoginBtn = New System.Windows.Forms.Button()
        Me.User = New System.Windows.Forms.TextBox()
        Me.Pwd = New System.Windows.Forms.TextBox()
        Me.Timer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'LoginBtn
        '
        Me.LoginBtn.Font = New System.Drawing.Font("微軟正黑體", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.LoginBtn.Location = New System.Drawing.Point(255, 12)
        Me.LoginBtn.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.LoginBtn.Name = "LoginBtn"
        Me.LoginBtn.Size = New System.Drawing.Size(100, 52)
        Me.LoginBtn.TabIndex = 0
        Me.LoginBtn.Text = "Login"
        Me.LoginBtn.UseVisualStyleBackColor = True
        '
        'User
        '
        Me.User.AccessibleDescription = ""
        Me.User.Location = New System.Drawing.Point(12, 12)
        Me.User.Name = "User"
        Me.User.Size = New System.Drawing.Size(237, 23)
        Me.User.TabIndex = 3
        '
        'Pwd
        '
        Me.Pwd.Location = New System.Drawing.Point(12, 41)
        Me.Pwd.Name = "Pwd"
        Me.Pwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(9679)
        Me.Pwd.Size = New System.Drawing.Size(237, 23)
        Me.Pwd.TabIndex = 4
        '
        'Timer
        '
        Me.Timer.Interval = 250
        '
        'Loginfrm
        '
        Me.AcceptButton = Me.LoginBtn
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(365, 76)
        Me.Controls.Add(Me.Pwd)
        Me.Controls.Add(Me.User)
        Me.Controls.Add(Me.LoginBtn)
        Me.Font = New System.Drawing.Font("微軟正黑體", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Loginfrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Xiaomi Auto Buy (By Silent)"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LoginBtn As System.Windows.Forms.Button
    Friend WithEvents User As System.Windows.Forms.TextBox
    Friend WithEvents Pwd As System.Windows.Forms.TextBox
    Friend WithEvents Timer As System.Windows.Forms.Timer

End Class
