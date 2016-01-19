<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class XiaomiFrm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(XiaomiFrm))
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.LogTabPage = New System.Windows.Forms.TabPage()
        Me.Mode2RadioButton = New System.Windows.Forms.RadioButton()
        Me.Mode1RadioButton = New System.Windows.Forms.RadioButton()
        Me.DianYuanCheckBox = New System.Windows.Forms.CheckBox()
        Me.Log = New System.Windows.Forms.TextBox()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.OrderTabPage = New System.Windows.Forms.TabPage()
        Me.OrderListView = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader3 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader4 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader5 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SettingTabPage = New System.Windows.Forms.TabPage()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TimeSetGroupBox = New System.Windows.Forms.GroupBox()
        Me.TimeCheckBox = New System.Windows.Forms.CheckBox()
        Me.TimePicker = New System.Windows.Forms.DateTimePicker()
        Me.best_timeGroupBox = New System.Windows.Forms.GroupBox()
        Me.best_time5RadioButton = New System.Windows.Forms.RadioButton()
        Me.best_time4RadioButton = New System.Windows.Forms.RadioButton()
        Me.best_time1RadioButton = New System.Windows.Forms.RadioButton()
        Me.PayGroupBox = New System.Windows.Forms.GroupBox()
        Me.PaypalRadioButton = New System.Windows.Forms.RadioButton()
        Me.RadioButton711 = New System.Windows.Forms.RadioButton()
        Me.FamilyRadioButton = New System.Windows.Forms.RadioButton()
        Me.ItemGroupBox = New System.Windows.Forms.GroupBox()
        Me.ItemComboBox = New System.Windows.Forms.ComboBox()
        Me.SettingGroupBox = New System.Windows.Forms.GroupBox()
        Me.TimeoutLabel = New System.Windows.Forms.Label()
        Me.TimeoutTrackBar = New System.Windows.Forms.TrackBar()
        Me.AmountLabel = New System.Windows.Forms.Label()
        Me.AmountTrackBar = New System.Windows.Forms.TrackBar()
        Me.InfoGroupBox = New System.Windows.Forms.GroupBox()
        Me.DistrictComboBox = New System.Windows.Forms.ComboBox()
        Me.CityComboBox = New System.Windows.Forms.ComboBox()
        Me.Tel = New System.Windows.Forms.TextBox()
        Me.Address = New System.Windows.Forms.TextBox()
        Me.ZipCode = New System.Windows.Forms.TextBox()
        Me.Consignee = New System.Windows.Forms.TextBox()
        Me.MainNotifyIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.ExitContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.離開程式ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.付款ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FamilyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.取消訂單ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MiniNewLabel = New System.Windows.Forms.Label()
        Me.MiniNewPictureBox = New System.Windows.Forms.PictureBox()
        Me.TabControl.SuspendLayout()
        Me.LogTabPage.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.OrderTabPage.SuspendLayout()
        Me.SettingTabPage.SuspendLayout()
        Me.TimeSetGroupBox.SuspendLayout()
        Me.best_timeGroupBox.SuspendLayout()
        Me.PayGroupBox.SuspendLayout()
        Me.ItemGroupBox.SuspendLayout()
        Me.SettingGroupBox.SuspendLayout()
        CType(Me.TimeoutTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AmountTrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.InfoGroupBox.SuspendLayout()
        Me.ExitContextMenuStrip.SuspendLayout()
        Me.ContextMenuStrip.SuspendLayout()
        CType(Me.MiniNewPictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Controls.Add(Me.LogTabPage)
        Me.TabControl.Controls.Add(Me.OrderTabPage)
        Me.TabControl.Controls.Add(Me.SettingTabPage)
        Me.TabControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl.Location = New System.Drawing.Point(0, 0)
        Me.TabControl.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(503, 305)
        Me.TabControl.TabIndex = 0
        '
        'LogTabPage
        '
        Me.LogTabPage.Controls.Add(Me.Mode2RadioButton)
        Me.LogTabPage.Controls.Add(Me.Mode1RadioButton)
        Me.LogTabPage.Controls.Add(Me.DianYuanCheckBox)
        Me.LogTabPage.Controls.Add(Me.Log)
        Me.LogTabPage.Controls.Add(Me.StatusStrip)
        Me.LogTabPage.Location = New System.Drawing.Point(4, 25)
        Me.LogTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.LogTabPage.Name = "LogTabPage"
        Me.LogTabPage.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.LogTabPage.Size = New System.Drawing.Size(495, 276)
        Me.LogTabPage.TabIndex = 0
        Me.LogTabPage.Text = "程式運行紀錄"
        Me.LogTabPage.UseVisualStyleBackColor = True
        '
        'Mode2RadioButton
        '
        Me.Mode2RadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.Mode2RadioButton.Font = New System.Drawing.Font("微軟正黑體", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Mode2RadioButton.ForeColor = System.Drawing.Color.DodgerBlue
        Me.Mode2RadioButton.Location = New System.Drawing.Point(306, 251)
        Me.Mode2RadioButton.Name = "Mode2RadioButton"
        Me.Mode2RadioButton.Size = New System.Drawing.Size(82, 20)
        Me.Mode2RadioButton.TabIndex = 4
        Me.Mode2RadioButton.Text = "送單模式"
        Me.Mode2RadioButton.UseVisualStyleBackColor = False
        '
        'Mode1RadioButton
        '
        Me.Mode1RadioButton.BackColor = System.Drawing.SystemColors.Control
        Me.Mode1RadioButton.Checked = True
        Me.Mode1RadioButton.Font = New System.Drawing.Font("微軟正黑體", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Mode1RadioButton.ForeColor = System.Drawing.Color.DodgerBlue
        Me.Mode1RadioButton.Location = New System.Drawing.Point(218, 251)
        Me.Mode1RadioButton.Name = "Mode1RadioButton"
        Me.Mode1RadioButton.Size = New System.Drawing.Size(82, 20)
        Me.Mode1RadioButton.TabIndex = 3
        Me.Mode1RadioButton.TabStop = True
        Me.Mode1RadioButton.Text = "搶購模式"
        Me.Mode1RadioButton.UseVisualStyleBackColor = False
        '
        'DianYuanCheckBox
        '
        Me.DianYuanCheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.DianYuanCheckBox.Enabled = False
        Me.DianYuanCheckBox.Font = New System.Drawing.Font("微軟正黑體", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.DianYuanCheckBox.ForeColor = System.Drawing.Color.DodgerBlue
        Me.DianYuanCheckBox.Location = New System.Drawing.Point(394, 251)
        Me.DianYuanCheckBox.Name = "DianYuanCheckBox"
        Me.DianYuanCheckBox.Size = New System.Drawing.Size(85, 20)
        Me.DianYuanCheckBox.TabIndex = 2
        Me.DianYuanCheckBox.Text = "開始收刮"
        Me.DianYuanCheckBox.UseVisualStyleBackColor = False
        '
        'Log
        '
        Me.Log.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Log.Location = New System.Drawing.Point(3, 4)
        Me.Log.Multiline = True
        Me.Log.Name = "Log"
        Me.Log.ReadOnly = True
        Me.Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Log.Size = New System.Drawing.Size(489, 246)
        Me.Log.TabIndex = 1
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(3, 250)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(489, 22)
        Me.StatusStrip.TabIndex = 0
        Me.StatusStrip.Text = "StatusStrip1"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Font = New System.Drawing.Font("微軟正黑體", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.ToolStripStatusLabel.ForeColor = System.Drawing.Color.Orange
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(59, 17)
        Me.ToolStripStatusLabel.Text = "登入中 ..."
        '
        'OrderTabPage
        '
        Me.OrderTabPage.Controls.Add(Me.OrderListView)
        Me.OrderTabPage.Location = New System.Drawing.Point(4, 25)
        Me.OrderTabPage.Name = "OrderTabPage"
        Me.OrderTabPage.Size = New System.Drawing.Size(495, 276)
        Me.OrderTabPage.TabIndex = 2
        Me.OrderTabPage.Text = "訂單查詢"
        Me.OrderTabPage.UseVisualStyleBackColor = True
        '
        'OrderListView
        '
        Me.OrderListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.OrderListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.OrderListView.FullRowSelect = True
        Me.OrderListView.Location = New System.Drawing.Point(0, 0)
        Me.OrderListView.MultiSelect = False
        Me.OrderListView.Name = "OrderListView"
        Me.OrderListView.ShowItemToolTips = True
        Me.OrderListView.Size = New System.Drawing.Size(495, 276)
        Me.OrderListView.TabIndex = 4
        Me.OrderListView.UseCompatibleStateImageBehavior = False
        Me.OrderListView.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "發貨單號"
        Me.ColumnHeader1.Width = 142
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "目前狀態"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader2.Width = 63
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "金額"
        Me.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader3.Width = 61
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "運輸單號"
        Me.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader4.Width = 98
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "收貨資料"
        Me.ColumnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ColumnHeader5.Width = 105
        '
        'SettingTabPage
        '
        Me.SettingTabPage.Controls.Add(Me.Label2)
        Me.SettingTabPage.Controls.Add(Me.Label1)
        Me.SettingTabPage.Controls.Add(Me.TimeSetGroupBox)
        Me.SettingTabPage.Controls.Add(Me.best_timeGroupBox)
        Me.SettingTabPage.Controls.Add(Me.PayGroupBox)
        Me.SettingTabPage.Controls.Add(Me.ItemGroupBox)
        Me.SettingTabPage.Controls.Add(Me.SettingGroupBox)
        Me.SettingTabPage.Controls.Add(Me.InfoGroupBox)
        Me.SettingTabPage.Location = New System.Drawing.Point(4, 25)
        Me.SettingTabPage.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.SettingTabPage.Name = "SettingTabPage"
        Me.SettingTabPage.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.SettingTabPage.Size = New System.Drawing.Size(495, 276)
        Me.SettingTabPage.TabIndex = 1
        Me.SettingTabPage.Text = "設定"
        Me.SettingTabPage.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(299, 252)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(188, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "行動電源5200mAh是我的。"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(278, 236)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(68, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "永遠相信，"
        '
        'TimeSetGroupBox
        '
        Me.TimeSetGroupBox.Controls.Add(Me.TimeCheckBox)
        Me.TimeSetGroupBox.Controls.Add(Me.TimePicker)
        Me.TimeSetGroupBox.Location = New System.Drawing.Point(8, 214)
        Me.TimeSetGroupBox.Name = "TimeSetGroupBox"
        Me.TimeSetGroupBox.Size = New System.Drawing.Size(256, 54)
        Me.TimeSetGroupBox.TabIndex = 5
        Me.TimeSetGroupBox.TabStop = False
        Me.TimeSetGroupBox.Text = "設定搶購時間"
        '
        'TimeCheckBox
        '
        Me.TimeCheckBox.AutoSize = True
        Me.TimeCheckBox.Location = New System.Drawing.Point(9, 25)
        Me.TimeCheckBox.Name = "TimeCheckBox"
        Me.TimeCheckBox.Size = New System.Drawing.Size(99, 20)
        Me.TimeCheckBox.TabIndex = 5
        Me.TimeCheckBox.Text = "設定搶購時間"
        Me.TimeCheckBox.UseVisualStyleBackColor = True
        '
        'TimePicker
        '
        Me.TimePicker.CustomFormat = "yyyy/MM/dd HH:mm"
        Me.TimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.TimePicker.Location = New System.Drawing.Point(114, 22)
        Me.TimePicker.Name = "TimePicker"
        Me.TimePicker.ShowUpDown = True
        Me.TimePicker.Size = New System.Drawing.Size(136, 23)
        Me.TimePicker.TabIndex = 4
        '
        'best_timeGroupBox
        '
        Me.best_timeGroupBox.Controls.Add(Me.best_time5RadioButton)
        Me.best_timeGroupBox.Controls.Add(Me.best_time4RadioButton)
        Me.best_timeGroupBox.Controls.Add(Me.best_time1RadioButton)
        Me.best_timeGroupBox.Location = New System.Drawing.Point(270, 125)
        Me.best_timeGroupBox.Name = "best_timeGroupBox"
        Me.best_timeGroupBox.Size = New System.Drawing.Size(217, 102)
        Me.best_timeGroupBox.TabIndex = 4
        Me.best_timeGroupBox.TabStop = False
        Me.best_timeGroupBox.Text = "送貨時間"
        '
        'best_time5RadioButton
        '
        Me.best_time5RadioButton.AutoSize = True
        Me.best_time5RadioButton.Location = New System.Drawing.Point(11, 73)
        Me.best_time5RadioButton.Name = "best_time5RadioButton"
        Me.best_time5RadioButton.Size = New System.Drawing.Size(87, 20)
        Me.best_time5RadioButton.TabIndex = 2
        Me.best_time5RadioButton.Text = "12點~17點"
        Me.best_time5RadioButton.UseVisualStyleBackColor = True
        '
        'best_time4RadioButton
        '
        Me.best_time4RadioButton.AutoSize = True
        Me.best_time4RadioButton.Location = New System.Drawing.Point(11, 47)
        Me.best_time4RadioButton.Name = "best_time4RadioButton"
        Me.best_time4RadioButton.Size = New System.Drawing.Size(80, 20)
        Me.best_time4RadioButton.TabIndex = 1
        Me.best_time4RadioButton.Text = "9點~12點"
        Me.best_time4RadioButton.UseVisualStyleBackColor = True
        '
        'best_time1RadioButton
        '
        Me.best_time1RadioButton.AutoSize = True
        Me.best_time1RadioButton.Checked = True
        Me.best_time1RadioButton.Location = New System.Drawing.Point(11, 21)
        Me.best_time1RadioButton.Name = "best_time1RadioButton"
        Me.best_time1RadioButton.Size = New System.Drawing.Size(110, 20)
        Me.best_time1RadioButton.TabIndex = 0
        Me.best_time1RadioButton.TabStop = True
        Me.best_time1RadioButton.Text = "不限定時間送貨"
        Me.best_time1RadioButton.UseVisualStyleBackColor = True
        '
        'PayGroupBox
        '
        Me.PayGroupBox.Controls.Add(Me.PaypalRadioButton)
        Me.PayGroupBox.Controls.Add(Me.RadioButton711)
        Me.PayGroupBox.Controls.Add(Me.FamilyRadioButton)
        Me.PayGroupBox.Location = New System.Drawing.Point(270, 70)
        Me.PayGroupBox.Name = "PayGroupBox"
        Me.PayGroupBox.Size = New System.Drawing.Size(217, 49)
        Me.PayGroupBox.TabIndex = 3
        Me.PayGroupBox.TabStop = False
        Me.PayGroupBox.Text = "付款方式"
        '
        'PaypalRadioButton
        '
        Me.PaypalRadioButton.AutoSize = True
        Me.PaypalRadioButton.Enabled = False
        Me.PaypalRadioButton.Location = New System.Drawing.Point(146, 22)
        Me.PaypalRadioButton.Name = "PaypalRadioButton"
        Me.PaypalRadioButton.Size = New System.Drawing.Size(64, 20)
        Me.PaypalRadioButton.TabIndex = 2
        Me.PaypalRadioButton.Text = "Paypal"
        Me.PaypalRadioButton.UseVisualStyleBackColor = True
        '
        'RadioButton711
        '
        Me.RadioButton711.AutoSize = True
        Me.RadioButton711.Location = New System.Drawing.Point(77, 22)
        Me.RadioButton711.Name = "RadioButton711"
        Me.RadioButton711.Size = New System.Drawing.Size(52, 20)
        Me.RadioButton711.TabIndex = 1
        Me.RadioButton711.Text = "7-11"
        Me.RadioButton711.UseVisualStyleBackColor = True
        '
        'FamilyRadioButton
        '
        Me.FamilyRadioButton.AutoSize = True
        Me.FamilyRadioButton.Checked = True
        Me.FamilyRadioButton.Location = New System.Drawing.Point(11, 22)
        Me.FamilyRadioButton.Name = "FamilyRadioButton"
        Me.FamilyRadioButton.Size = New System.Drawing.Size(50, 20)
        Me.FamilyRadioButton.TabIndex = 0
        Me.FamilyRadioButton.TabStop = True
        Me.FamilyRadioButton.Text = "全家"
        Me.FamilyRadioButton.UseVisualStyleBackColor = True
        '
        'ItemGroupBox
        '
        Me.ItemGroupBox.Controls.Add(Me.ItemComboBox)
        Me.ItemGroupBox.Location = New System.Drawing.Point(270, 7)
        Me.ItemGroupBox.Name = "ItemGroupBox"
        Me.ItemGroupBox.Size = New System.Drawing.Size(217, 57)
        Me.ItemGroupBox.TabIndex = 2
        Me.ItemGroupBox.TabStop = False
        Me.ItemGroupBox.Text = "搶購商品"
        '
        'ItemComboBox
        '
        Me.ItemComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ItemComboBox.FormattingEnabled = True
        Me.ItemComboBox.Items.AddRange(New Object() {"行動電源10400mAh", "行動電源5200mAh", "紅米1S", "紅米1S 白色版", "紅米Note4G", "小米手環 石墨黑", "小米路由器mini", "小米手環（光感版）"})
        Me.ItemComboBox.Location = New System.Drawing.Point(11, 22)
        Me.ItemComboBox.Name = "ItemComboBox"
        Me.ItemComboBox.Size = New System.Drawing.Size(199, 24)
        Me.ItemComboBox.TabIndex = 0
        '
        'SettingGroupBox
        '
        Me.SettingGroupBox.Controls.Add(Me.TimeoutLabel)
        Me.SettingGroupBox.Controls.Add(Me.TimeoutTrackBar)
        Me.SettingGroupBox.Controls.Add(Me.AmountLabel)
        Me.SettingGroupBox.Controls.Add(Me.AmountTrackBar)
        Me.SettingGroupBox.Location = New System.Drawing.Point(8, 125)
        Me.SettingGroupBox.Name = "SettingGroupBox"
        Me.SettingGroupBox.Size = New System.Drawing.Size(256, 83)
        Me.SettingGroupBox.TabIndex = 1
        Me.SettingGroupBox.TabStop = False
        Me.SettingGroupBox.Text = "搶購設定"
        '
        'TimeoutLabel
        '
        Me.TimeoutLabel.AutoSize = True
        Me.TimeoutLabel.Location = New System.Drawing.Point(6, 54)
        Me.TimeoutLabel.Name = "TimeoutLabel"
        Me.TimeoutLabel.Size = New System.Drawing.Size(110, 16)
        Me.TimeoutLabel.TabIndex = 3
        Me.TimeoutLabel.Text = "時間上限 : (200ms)"
        '
        'TimeoutTrackBar
        '
        Me.TimeoutTrackBar.AutoSize = False
        Me.TimeoutTrackBar.BackColor = System.Drawing.Color.White
        Me.TimeoutTrackBar.Location = New System.Drawing.Point(129, 52)
        Me.TimeoutTrackBar.Maximum = 5000
        Me.TimeoutTrackBar.Minimum = 200
        Me.TimeoutTrackBar.Name = "TimeoutTrackBar"
        Me.TimeoutTrackBar.Size = New System.Drawing.Size(121, 24)
        Me.TimeoutTrackBar.TabIndex = 2
        Me.TimeoutTrackBar.TickStyle = System.Windows.Forms.TickStyle.None
        Me.TimeoutTrackBar.Value = 200
        '
        'AmountLabel
        '
        Me.AmountLabel.AutoSize = True
        Me.AmountLabel.Location = New System.Drawing.Point(6, 25)
        Me.AmountLabel.Name = "AmountLabel"
        Me.AmountLabel.Size = New System.Drawing.Size(92, 16)
        Me.AmountLabel.TabIndex = 1
        Me.AmountLabel.Text = "搶購數量 : (2個)"
        '
        'AmountTrackBar
        '
        Me.AmountTrackBar.AutoSize = False
        Me.AmountTrackBar.BackColor = System.Drawing.Color.White
        Me.AmountTrackBar.Location = New System.Drawing.Point(129, 22)
        Me.AmountTrackBar.Maximum = 25
        Me.AmountTrackBar.Minimum = 1
        Me.AmountTrackBar.Name = "AmountTrackBar"
        Me.AmountTrackBar.Size = New System.Drawing.Size(121, 24)
        Me.AmountTrackBar.TabIndex = 0
        Me.AmountTrackBar.TickStyle = System.Windows.Forms.TickStyle.None
        Me.AmountTrackBar.Value = 1
        '
        'InfoGroupBox
        '
        Me.InfoGroupBox.Controls.Add(Me.DistrictComboBox)
        Me.InfoGroupBox.Controls.Add(Me.CityComboBox)
        Me.InfoGroupBox.Controls.Add(Me.Tel)
        Me.InfoGroupBox.Controls.Add(Me.Address)
        Me.InfoGroupBox.Controls.Add(Me.ZipCode)
        Me.InfoGroupBox.Controls.Add(Me.Consignee)
        Me.InfoGroupBox.Location = New System.Drawing.Point(8, 7)
        Me.InfoGroupBox.Name = "InfoGroupBox"
        Me.InfoGroupBox.Size = New System.Drawing.Size(256, 112)
        Me.InfoGroupBox.TabIndex = 0
        Me.InfoGroupBox.TabStop = False
        Me.InfoGroupBox.Text = "收貨資料"
        '
        'DistrictComboBox
        '
        Me.DistrictComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DistrictComboBox.FormattingEnabled = True
        Me.DistrictComboBox.Items.AddRange(New Object() {"中正區", "大同區", "中山區", "松山區", "大安區", "萬華區", "信義區", "士林區", "北投區", "內湖區", "南港區", "文山區"})
        Me.DistrictComboBox.Location = New System.Drawing.Point(87, 51)
        Me.DistrictComboBox.Name = "DistrictComboBox"
        Me.DistrictComboBox.Size = New System.Drawing.Size(75, 24)
        Me.DistrictComboBox.TabIndex = 7
        '
        'CityComboBox
        '
        Me.CityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CityComboBox.FormattingEnabled = True
        Me.CityComboBox.Items.AddRange(New Object() {"台北市", "基隆市", "宜蘭縣", "桃園縣", "新北市", "新竹市", "新竹縣", "苗栗縣", "台中市", "彰化縣", "南投縣", "嘉義市", "嘉義縣", "雲林縣", "台南市", "高雄市", "屏東縣", "台東縣", "花蓮縣", "台中縣", "台南縣", "高雄縣  "})
        Me.CityComboBox.Location = New System.Drawing.Point(6, 51)
        Me.CityComboBox.Name = "CityComboBox"
        Me.CityComboBox.Size = New System.Drawing.Size(75, 24)
        Me.CityComboBox.TabIndex = 6
        '
        'Tel
        '
        Me.Tel.Location = New System.Drawing.Point(135, 22)
        Me.Tel.Name = "Tel"
        Me.Tel.Size = New System.Drawing.Size(115, 23)
        Me.Tel.TabIndex = 5
        '
        'Address
        '
        Me.Address.Location = New System.Drawing.Point(6, 81)
        Me.Address.Name = "Address"
        Me.Address.Size = New System.Drawing.Size(244, 23)
        Me.Address.TabIndex = 2
        '
        'ZipCode
        '
        Me.ZipCode.Location = New System.Drawing.Point(168, 52)
        Me.ZipCode.Name = "ZipCode"
        Me.ZipCode.Size = New System.Drawing.Size(82, 23)
        Me.ZipCode.TabIndex = 1
        '
        'Consignee
        '
        Me.Consignee.Location = New System.Drawing.Point(6, 22)
        Me.Consignee.Name = "Consignee"
        Me.Consignee.Size = New System.Drawing.Size(123, 23)
        Me.Consignee.TabIndex = 0
        '
        'MainNotifyIcon
        '
        Me.MainNotifyIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.MainNotifyIcon.BalloonTipTitle = "Xiaomi Auto Buy"
        Me.MainNotifyIcon.ContextMenuStrip = Me.ExitContextMenuStrip
        Me.MainNotifyIcon.Text = "Xiaomi Auto Buy"
        Me.MainNotifyIcon.Visible = True
        '
        'ExitContextMenuStrip
        '
        Me.ExitContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.離開程式ToolStripMenuItem})
        Me.ExitContextMenuStrip.Name = "ExitContextMenuStrip"
        Me.ExitContextMenuStrip.Size = New System.Drawing.Size(123, 26)
        '
        '離開程式ToolStripMenuItem
        '
        Me.離開程式ToolStripMenuItem.Name = "離開程式ToolStripMenuItem"
        Me.離開程式ToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.離開程式ToolStripMenuItem.Text = "離開程式"
        '
        'ContextMenuStrip
        '
        Me.ContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.付款ToolStripMenuItem, Me.FamilyToolStripMenuItem, Me.ToolStripSeparator1, Me.取消訂單ToolStripMenuItem})
        Me.ContextMenuStrip.Name = "ContextMenuStrip"
        Me.ContextMenuStrip.Size = New System.Drawing.Size(135, 76)
        '
        '付款ToolStripMenuItem
        '
        Me.付款ToolStripMenuItem.Name = "付款ToolStripMenuItem"
        Me.付款ToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.付款ToolStripMenuItem.Text = "7-11付款"
        '
        'FamilyToolStripMenuItem
        '
        Me.FamilyToolStripMenuItem.Name = "FamilyToolStripMenuItem"
        Me.FamilyToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.FamilyToolStripMenuItem.Text = "Family付款"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(131, 6)
        '
        '取消訂單ToolStripMenuItem
        '
        Me.取消訂單ToolStripMenuItem.Name = "取消訂單ToolStripMenuItem"
        Me.取消訂單ToolStripMenuItem.Size = New System.Drawing.Size(134, 22)
        Me.取消訂單ToolStripMenuItem.Text = "取消訂單"
        '
        'MiniNewLabel
        '
        Me.MiniNewLabel.AutoSize = True
        Me.MiniNewLabel.Cursor = System.Windows.Forms.Cursors.Hand
        Me.MiniNewLabel.Location = New System.Drawing.Point(451, 4)
        Me.MiniNewLabel.Name = "MiniNewLabel"
        Me.MiniNewLabel.Size = New System.Drawing.Size(44, 16)
        Me.MiniNewLabel.TabIndex = 2
        Me.MiniNewLabel.Text = "購物車"
        '
        'MiniNewPictureBox
        '
        Me.MiniNewPictureBox.Cursor = System.Windows.Forms.Cursors.Hand
        Me.MiniNewPictureBox.Image = CType(resources.GetObject("MiniNewPictureBox.Image"), System.Drawing.Image)
        Me.MiniNewPictureBox.Location = New System.Drawing.Point(431, 4)
        Me.MiniNewPictureBox.Name = "MiniNewPictureBox"
        Me.MiniNewPictureBox.Size = New System.Drawing.Size(21, 17)
        Me.MiniNewPictureBox.TabIndex = 3
        Me.MiniNewPictureBox.TabStop = False
        '
        'XiaomiFrm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(503, 305)
        Me.Controls.Add(Me.MiniNewPictureBox)
        Me.Controls.Add(Me.MiniNewLabel)
        Me.Controls.Add(Me.TabControl)
        Me.Font = New System.Drawing.Font("微軟正黑體", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.Name = "XiaomiFrm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Xiaomi Auto Buy (By Silent)"
        Me.TabControl.ResumeLayout(False)
        Me.LogTabPage.ResumeLayout(False)
        Me.LogTabPage.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.OrderTabPage.ResumeLayout(False)
        Me.SettingTabPage.ResumeLayout(False)
        Me.SettingTabPage.PerformLayout()
        Me.TimeSetGroupBox.ResumeLayout(False)
        Me.TimeSetGroupBox.PerformLayout()
        Me.best_timeGroupBox.ResumeLayout(False)
        Me.best_timeGroupBox.PerformLayout()
        Me.PayGroupBox.ResumeLayout(False)
        Me.PayGroupBox.PerformLayout()
        Me.ItemGroupBox.ResumeLayout(False)
        Me.SettingGroupBox.ResumeLayout(False)
        Me.SettingGroupBox.PerformLayout()
        CType(Me.TimeoutTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AmountTrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.InfoGroupBox.ResumeLayout(False)
        Me.InfoGroupBox.PerformLayout()
        Me.ExitContextMenuStrip.ResumeLayout(False)
        Me.ContextMenuStrip.ResumeLayout(False)
        CType(Me.MiniNewPictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents LogTabPage As System.Windows.Forms.TabPage
    Friend WithEvents Log As System.Windows.Forms.TextBox
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SettingTabPage As System.Windows.Forms.TabPage
    Friend WithEvents SettingGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents InfoGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents DistrictComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents CityComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Tel As System.Windows.Forms.TextBox
    Friend WithEvents Address As System.Windows.Forms.TextBox
    Friend WithEvents ZipCode As System.Windows.Forms.TextBox
    Friend WithEvents Consignee As System.Windows.Forms.TextBox
    Friend WithEvents DianYuanCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents TimeoutLabel As System.Windows.Forms.Label
    Friend WithEvents TimeoutTrackBar As System.Windows.Forms.TrackBar
    Friend WithEvents AmountLabel As System.Windows.Forms.Label
    Friend WithEvents AmountTrackBar As System.Windows.Forms.TrackBar
    Friend WithEvents PayGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents PaypalRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton711 As System.Windows.Forms.RadioButton
    Friend WithEvents FamilyRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents ItemGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents best_timeGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents best_time5RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents best_time4RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents best_time1RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents ItemComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Mode2RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents Mode1RadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents MainNotifyIcon As System.Windows.Forms.NotifyIcon
    Friend WithEvents OrderTabPage As System.Windows.Forms.TabPage
    Friend WithEvents OrderListView As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 付款ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FamilyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 取消訂單ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TimeSetGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents TimePicker As System.Windows.Forms.DateTimePicker
    Friend WithEvents TimeCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents ExitContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 離開程式ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MiniNewLabel As System.Windows.Forms.Label
    Friend WithEvents MiniNewPictureBox As System.Windows.Forms.PictureBox
End Class
