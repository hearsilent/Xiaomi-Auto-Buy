Imports System.Threading
Imports System.Net
Imports System.ComponentModel
Imports System.IO
Imports Xiaomi_Auto_Buy.SilentWebModule
Imports System.Text
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports HtmlAgilityPack
Imports System.IO.Compression

Public Class XiaomiFrm
    Dim LoginThread As Thread
    Dim AutoThread As Thread
    Dim PinCodeThread As Thread
    Dim ReCheckThread As Thread
    Dim ZipCodeThread As Thread
    Dim GoogleSheetThread As Thread
    Dim OrderThread As Thread
    Public XmlPath As String = "Configs.xml"
    Public cookies As New CookieContainer()
    Dim SkuCount As Integer
    Dim ChooseItemID As String = "2141300041"
    Dim ChooseItemCount As Integer = "2"
    Public Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
                            (ByVal hwnd As IntPtr, _
                             ByVal wMsg As Integer, _
                             ByVal wParam As IntPtr, _
                             ByVal lParam As Byte()) _
                             As Integer
    Public Const EM_SETCUEBANNER As Integer = &H1501
    Dim User, Pwd As String
    Dim _UserId As String

    Dim _buyUrl As String = "http://buy.mi.com/tw"
    Dim _dmSite As String = "http://sgp01.tp.hd.mi.com"
    Dim _home As String = "http://www.mi.com/tw/"
    Dim _loginUrl As String = "http://buy.mi.com/tw/site/login"
    Dim _timestampUrl As String = "http://hd.global.mi.com/gettimestamp"
    Dim _hdgetUrl As String = _dmSite + "/hdget/tw?source=bigtap&product="
    Dim _hdinfoUrl As String = _dmSite + "/hdinfo/tw"
    Dim _modeUrl As String = _dmSite + "/getmode/tw/?product="
    Dim _shopCartUrl As String = _buyUrl + "/cart/add/"
    Dim _checkOut As String = "http://buy.mi.com/tw/buy/checkout"
    Dim _miniNew As String = "http://buy.mi.com/tw/cart/miniNew"
    Dim _deleteItem As String = _buyUrl + "/cart/delete/"

    Dim _orderUrl As String = "http://buy.mi.com/tw/user/order"
    Dim _orderView As String = "http://buy.mi.com/tw/user/orderView/"
    Dim _cancelOrder As String = "http://buy.mi.com/tw/user/cancelOrder/"
    Dim _confirmOrder As String = "http://buy.mi.com/tw/buy/confirm/"

    Dim _saveAddress As String = "http://buy.mi.com/tw/address/save"
    Dim _getAddress As String = "http://buy.mi.com/tw/user/address"

    Dim miniCart As New miniCartFrm(cookies)
    Public Sub New(Account As String, Password As String, UserId As String, LoginCookies As CookieContainer)
        ' 此為設計工具所需的呼叫。
        InitializeComponent()
        User = Account
        Pwd = Password
        cookies = LoginCookies
        _UserId = UserId
    End Sub
    Private Sub XiaomiFrm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        SaveSetting()
        Try
            OrderThread.Abort()
        Catch ex As Exception

        End Try
        Try
            AutoThread.Abort()
        Catch ex As Exception

        End Try
        Try
            LoginThread.Abort()
        Catch ex As Exception

        End Try
        Try
            PinCodeThread.Abort()
        Catch ex As Exception

        End Try
        Try
            ZipCodeThread.Abort()
        Catch ex As Exception

        End Try
        End
    End Sub
    Private Sub XiaomiFrm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Form.CheckForIllegalCrossThreadCalls = False
        Me.Icon = Loginfrm.Icon
        SendMessage(Consignee.Handle, _
                     EM_SETCUEBANNER, _
                     IntPtr.Zero, _
                     System.Text.Encoding.Unicode.GetBytes("姓名"))
        SendMessage(ZipCode.Handle, _
                     EM_SETCUEBANNER, _
                     IntPtr.Zero, _
                     System.Text.Encoding.Unicode.GetBytes("郵遞區號"))
        SendMessage(Address.Handle, _
                     EM_SETCUEBANNER, _
                     IntPtr.Zero, _
                     System.Text.Encoding.Unicode.GetBytes("地址"))
        SendMessage(Tel.Handle, _
             EM_SETCUEBANNER, _
             IntPtr.Zero, _
             System.Text.Encoding.Unicode.GetBytes("聯繫電話"))
        SendMessage(CityComboBox.Handle, _
             EM_SETCUEBANNER, _
             IntPtr.Zero, _
             System.Text.Encoding.Unicode.GetBytes("城市"))
        SendMessage(DistrictComboBox.Handle, _
             EM_SETCUEBANNER, _
             IntPtr.Zero, _
             System.Text.Encoding.Unicode.GetBytes("地區"))
        LoadSetting()
    End Sub
    Public Sub LoadSetting()
        If File.Exists(Application.StartupPath & "/" & XmlPath) Then
            Dim configs As BindingList(Of Config) = XmlSerialize.DeserializeFromXml(Of BindingList(Of Config))("Configs.xml")
            Try
                Consignee.Text = configs.Item(0).Consignee
                Tel.Text = configs.Item(0).Tel
                If Not configs.Item(0).City = "" Then
                    CityComboBox.Text = configs.Item(0).City
                    AddRegion()
                    DistrictComboBox.Text = configs.Item(0).District
                Else
                    CityComboBox.SelectedIndex = 0
                    AddRegion()
                End If
                ZipCode.Text = configs.Item(0).ZipCode
                Address.Text = configs.Item(0).Address
                AmountTrackBar.Value = configs.Item(0).Amount
                TimeoutTrackBar.Value = configs.Item(0).Timeout
                'DianYuanCheckBox.Checked = configs.Item(0).DianYuan
                AmountLabel.Text = "搶購數量 : (" & AmountTrackBar.Value * ChooseItemCount & "個)"
                TimeoutLabel.Text = "時間上限 : (" & TimeoutTrackBar.Value & "ms)"
                If configs.Item(0).Bank = "Family" Then
                    FamilyRadioButton.Checked = True
                ElseIf configs.Item(0).Bank = "7-11" Then
                    RadioButton711.Checked = True
                ElseIf configs.Item(0).Bank = "Paypal" Then
                    PaypalRadioButton.Checked = True
                Else
                    FamilyRadioButton.Checked = True
                End If
                If configs.Item(0).BestTime = "1" Then
                    best_time1RadioButton.Checked = True
                ElseIf configs.Item(0).BestTime = "4" Then
                    best_time4RadioButton.Checked = True
                ElseIf configs.Item(0).BestTime = "5" Then
                    best_time5RadioButton.Checked = True
                Else
                    best_time1RadioButton.Checked = True
                End If
                If Not configs.Item(0).ChooseItem = "" Then
                    ItemComboBox.SelectedIndex = configs.Item(0).ChooseItem
                Else
                    ItemComboBox.SelectedIndex = 0
                End If
                If configs.Item(0).Mode = "False" Then
                    Mode2RadioButton.Checked = True
                Else
                    Mode1RadioButton.Checked = True
                End If
                If Not configs.Item(0).Time = "" Then
                    If CDate(configs.Item(0).Time) < DateTime.Now.ToString("yyyy/MM/dd HH:mm") Then
                        TimePicker.Value = DateTime.Now.ToString("yyyy/MM/dd") + " " + "12:00"
                    Else
                        TimePicker.Value = CDate(configs.Item(0).Time)
                    End If
                    'TimePicker.Text = CDate(CDate(configs.Item(0).Time).ToString("yyyy/MM/dd HH:mm"))
                End If
                If configs.Item(0).TimeCheck = "True" Then
                    TimeCheckBox.Checked = True
                Else
                    TimeCheckBox.Checked = False
                End If
            Catch ex As Exception

            End Try
        Else
            CityComboBox.SelectedIndex = 0
            AddRegion()
            DistrictComboBox.SelectedIndex = 0
            ItemComboBox.SelectedIndex = 0
        End If
    End Sub
    Public Sub SaveSetting()
        Dim configs As BindingList(Of Config) = Nothing
        configs = New BindingList(Of Config)()
        Dim _Bank As String = Nothing, _BestTime As String = Nothing
        If FamilyRadioButton.Checked = True Then
            _Bank = "Family"
        ElseIf RadioButton711.Checked = True Then
            _Bank = "7-11"
        ElseIf PaypalRadioButton.Checked = True Then
            _Bank = "Paypal"
        End If
        If best_time1RadioButton.Checked = True Then
            _BestTime = 1
        ElseIf best_time4RadioButton.Checked = True Then
            _BestTime = 4
        ElseIf best_time5RadioButton.Checked = True Then
            _BestTime = 5
        End If
        configs.Add(New Config() With {.Account = Loginfrm.User.Text, .Pwd = Loginfrm.Encrypt(Loginfrm.Pwd.Text, "SilentKC"), .Consignee = Consignee.Text, .Tel = Tel.Text, .City = CityComboBox.Text, .District = DistrictComboBox.Text, .ZipCode = ZipCode.Text, .Address = Address.Text, .Amount = AmountTrackBar.Value, .Timeout = TimeoutTrackBar.Value, .Bank = _Bank, .BestTime = _BestTime, .ChooseItem = ItemComboBox.SelectedIndex, .Mode = Mode1RadioButton.Checked, .TimeCheck = TimeCheckBox.Checked, .Time = TimePicker.Value.ToString("yyyy/MM/dd HH:mm"), .Manager = Guid.NewGuid})
        XmlSerialize.SerializeToXml("Configs.xml", configs)
    End Sub
    Function TranCity() As String
        Select Case CityComboBox.Text
            Case "台北市" : Return 3387
            Case "基隆市" : Return 3388
            Case "宜蘭縣" : Return 3389
            Case "桃園縣" : Return 3390
            Case "新北市" : Return 3391
            Case "新竹市" : Return 3392
            Case "新竹縣" : Return 3393
            Case "苗栗縣" : Return 3394
            Case "台中市" : Return 3395
            Case "彰化縣" : Return 3396
            Case "南投縣" : Return 3397
            Case "嘉義市" : Return 3398
            Case "嘉義縣" : Return 3399
            Case "雲林縣" : Return 3400
            Case "台南市" : Return 3401
            Case "高雄市" : Return 3402
            Case "屏東縣" : Return 3405
            Case "台東縣" : Return 3406
            Case "花蓮縣" : Return 3407
            Case "台中縣" : Return 3775
            Case "台南縣" : Return 3776
            Case "高雄縣" : Return 3777
            Case Else : Return "False"
        End Select
    End Function
    Function TranRegion() As String
        Select Case CityComboBox.Text
            Case "台北市"
                Select Case DistrictComboBox.Text
                    Case "中正區" : Return 3411
                    Case "大同區" : Return 3412
                    Case "中山區" : Return 3413
                    Case "松山區" : Return 3414
                    Case "大安區" : Return 3415
                    Case "萬華區" : Return 3416
                    Case "信義區" : Return 3417
                    Case "士林區" : Return 3418
                    Case "北投區" : Return 3419
                    Case "內湖區" : Return 3420
                    Case "南港區" : Return 3421
                    Case "文山區" : Return 3422
                    Case Else : Return -1
                End Select
            Case "基隆市"
                Select Case DistrictComboBox.Text
                    Case "仁愛區" : Return 3423
                    Case "安樂區" : Return 3424
                    Case "暖暖區" : Return 3425
                    Case "七堵區" : Return 3426
                    Case "信義區" : Return 3772
                    Case "中正區" : Return 3773
                    Case "中山區" : Return 3774
                    Case Else : Return -1
                End Select
            Case "宜蘭縣"
                Select Case DistrictComboBox.Text
                    Case "宜蘭" : Return 3427
                    Case "頭城" : Return 3428
                    Case "礁溪" : Return 3429
                    Case "壯圍" : Return 3430
                    Case "員山" : Return 3431
                    Case "羅東" : Return 3432
                    Case "三星" : Return 3433
                    Case "大同" : Return 3434
                    Case "五結" : Return 3435
                    Case "冬山" : Return 3436
                    Case "蘇澳" : Return 3437
                    Case "南澳" : Return 3438
                    Case Else : Return -1
                End Select
            Case "桃園縣"
                Select Case DistrictComboBox.Text
                    Case "中壢" : Return 3439
                    Case "平鎮" : Return 3440
                    Case "龍潭" : Return 3441
                    Case "楊梅" : Return 3442
                    Case "新屋" : Return 3443
                    Case "觀音" : Return 3444
                    Case "桃園" : Return 3445
                    Case "龜山" : Return 3446
                    Case "八德" : Return 3447
                    Case "大溪" : Return 3448
                    Case "復興" : Return 3449
                    Case "大園" : Return 3450
                    Case "蘆竹" : Return 3451
                    Case Else : Return -1
                End Select
            Case "新北市"
                Select Case DistrictComboBox.Text
                    Case "萬里區" : Return 3452
                    Case "金山區" : Return 3453
                    Case "板橋區" : Return 3454
                    Case "汐止區" : Return 3455
                    Case "深坑區" : Return 3456
                    Case "石碇區" : Return 3457
                    Case "瑞芳區" : Return 3458
                    Case "平溪區" : Return 3459
                    Case "雙溪區" : Return 3460
                    Case "貢寮區" : Return 3461
                    Case "新店區" : Return 3462
                    Case "坪林區" : Return 3463
                    Case "烏來區" : Return 3464
                    Case "永和區" : Return 3465
                    Case "中和區" : Return 3466
                    Case "土城區" : Return 3467
                    Case "三峽區" : Return 3468
                    Case "樹林區" : Return 3469
                    Case "鶯歌區" : Return 3470
                    Case "三重區" : Return 3471
                    Case "新莊區" : Return 3472
                    Case "泰山區" : Return 3473
                    Case "林口區" : Return 3474
                    Case "蘆洲區" : Return 3475
                    Case "五股區" : Return 3476
                    Case "八里區" : Return 3477
                    Case "淡水區" : Return 3478
                    Case "三芝區" : Return 3479
                    Case "石門區" : Return 3480
                    Case Else : Return -1
                End Select
            Case "新竹市" : Return -1
            Case "新竹縣"
                Select Case DistrictComboBox.Text
                    Case "竹北" : Return 3481
                    Case "湖口" : Return 3482
                    Case "新豐" : Return 3483
                    Case "新埔" : Return 3484
                    Case "關西" : Return 3485
                    Case "芎林" : Return 3486
                    Case "寶山" : Return 3487
                    Case "竹東" : Return 3488
                    Case "五峰" : Return 3489
                    Case "橫山" : Return 3490
                    Case "尖石" : Return 3491
                    Case "北埔" : Return 3492
                    Case "峨眉" : Return 3493
                    Case Else : Return -1
                End Select
            Case "苗栗縣"
                Select Case DistrictComboBox.Text
                    Case "竹南" : Return 3494
                    Case "頭份" : Return 3495
                    Case "三灣" : Return 3496
                    Case "南庄" : Return 3497
                    Case "獅潭" : Return 3498
                    Case "後龍" : Return 3499
                    Case "通霄" : Return 3500
                    Case "苑裡" : Return 3501
                    Case "苗栗" : Return 3502
                    Case "造橋" : Return 3503
                    Case "頭屋" : Return 3504
                    Case "公館" : Return 3505
                    Case "大湖" : Return 3506
                    Case "泰安" : Return 3507
                    Case "銅鑼" : Return 3508
                    Case "三義" : Return 3509
                    Case "西湖" : Return 3510
                    Case "卓蘭" : Return 3511
                    Case Else : Return -1
                End Select
            Case "台中市"
                Select Case DistrictComboBox.Text
                    Case "中區" : Return 3512
                    Case "東區" : Return 3513
                    Case "南區" : Return 3514
                    Case "西區" : Return 3515
                    Case "北區" : Return 3516
                    Case "北屯區" : Return 3517
                    Case "西屯區" : Return 3518
                    Case "南屯區" : Return 3519
                    Case "太平區" : Return 3520
                    Case "大里區" : Return 3521
                    Case "霧峰區" : Return 3522
                    Case "烏日區" : Return 3523
                    Case "豐原區" : Return 3524
                    Case "后里區" : Return 3525
                    Case "石岡區" : Return 3526
                    Case "東勢區" : Return 3527
                    Case "和平區" : Return 3528
                    Case "新社區" : Return 3529
                    Case "潭子區" : Return 3530
                    Case "大雅區" : Return 3531
                    Case "神岡區" : Return 3532
                    Case "大肚區" : Return 3533
                    Case "沙鹿區" : Return 3534
                    Case "龍井區" : Return 3535
                    Case "梧棲區" : Return 3536
                    Case "清水區" : Return 3537
                    Case "大甲區" : Return 3538
                    Case "外埔區" : Return 3539
                    Case Else : Return -1
                End Select
            Case "彰化縣"
                Select Case DistrictComboBox.Text
                    Case "彰化" : Return 3540
                    Case "芬園" : Return 3541
                    Case "花壇" : Return 3542
                    Case "秀水" : Return 3543
                    Case "鹿港" : Return 3544
                    Case "福興" : Return 3545
                    Case "線西" : Return 3546
                    Case "和美" : Return 3547
                    Case "伸港" : Return 3548
                    Case "員林" : Return 3549
                    Case "社頭" : Return 3550
                    Case "永靖" : Return 3551
                    Case "埔心" : Return 3552
                    Case "溪湖" : Return 3553
                    Case "大村" : Return 3554
                    Case "埔鹽" : Return 3555
                    Case "田中" : Return 3556
                    Case "北斗" : Return 3557
                    Case "田尾" : Return 3558
                    Case "埤頭" : Return 3559
                    Case "溪州" : Return 3560
                    Case "竹塘" : Return 3561
                    Case "二林" : Return 3562
                    Case "大城" : Return 3563
                    Case "芳苑" : Return 3564
                    Case "二水" : Return 3565
                    Case Else : Return -1
                End Select
            Case "南投縣"
                Select Case DistrictComboBox.Text
                    Case "南投" : Return 3566
                    Case "中寮" : Return 3567
                    Case "草屯" : Return 3568
                    Case "國姓" : Return 3569
                    Case "埔里" : Return 3570
                    Case "仁愛" : Return 3571
                    Case "名間" : Return 3572
                    Case "集集" : Return 3573
                    Case "水里" : Return 3574
                    Case "魚池" : Return 3575
                    Case "信義" : Return 3576
                    Case "竹山" : Return 3577
                    Case "鹿谷" : Return 3578
                    Case Else : Return -1
                End Select
            Case "嘉義市" : Return -1
            Case "嘉義縣"
                Select Case DistrictComboBox.Text
                    Case "番路" : Return 3579
                    Case "梅山" : Return 3580
                    Case "竹崎" : Return 3581
                    Case "阿里山" : Return 3582
                    Case "中埔" : Return 3583
                    Case "大埔" : Return 3584
                    Case "水上" : Return 3585
                    Case "鹿草" : Return 3586
                    Case "太保" : Return 3587
                    Case "朴子" : Return 3588
                    Case "東石" : Return 3589
                    Case "六腳" : Return 3590
                    Case "新港" : Return 3591
                    Case "民雄" : Return 3592
                    Case "大林" : Return 3593
                    Case "溪口" : Return 3594
                    Case "義竹" : Return 3595
                    Case "布袋" : Return 3596
                    Case Else : Return -1
                End Select
            Case "雲林縣"
                Select Case DistrictComboBox.Text
                    Case "斗南" : Return 3597
                    Case "大埤" : Return 3598
                    Case "虎尾" : Return 3599
                    Case "土庫" : Return 3600
                    Case "褒忠" : Return 3601
                    Case "東勢" : Return 3602
                    Case "臺西" : Return 3603
                    Case "崙背" : Return 3604
                    Case "麥寮" : Return 3605
                    Case "斗六" : Return 3606
                    Case "林內" : Return 3607
                    Case "古坑" : Return 3608
                    Case "莿桐" : Return 3609
                    Case "西螺" : Return 3610
                    Case "二崙" : Return 3611
                    Case "北港" : Return 3612
                    Case "水林" : Return 3613
                    Case "口湖" : Return 3614
                    Case "四湖" : Return 3615
                    Case "元長" : Return 3616
                    Case Else : Return -1
                End Select
            Case "台南市"
                Select Case DistrictComboBox.Text
                    Case "中西區" : Return 3617
                    Case "安平區" : Return 3618
                    Case "安南區" : Return 3619
                    Case "永康區" : Return 3620
                    Case "歸仁區" : Return 3621
                    Case "新化區" : Return 3622
                    Case "左鎮區" : Return 3623
                    Case "玉井區" : Return 3624
                    Case "楠西區" : Return 3625
                    Case "南化區" : Return 3626
                    Case "仁德區" : Return 3627
                    Case "關廟區" : Return 3628
                    Case "龍崎區" : Return 3629
                    Case "官田區" : Return 3630
                    Case "麻豆區" : Return 3631
                    Case "佳里區" : Return 3632
                    Case "西港區" : Return 3633
                    Case "七股區" : Return 3634
                    Case "將軍區" : Return 3635
                    Case "學甲區" : Return 3636
                    Case "北門區" : Return 3637
                    Case "新營區" : Return 3638
                    Case "後壁區" : Return 3639
                    Case "白河區" : Return 3640
                    Case "東山區" : Return 3641
                    Case "六甲區" : Return 3642
                    Case "下營區" : Return 3643
                    Case "柳營區" : Return 3644
                    Case "鹽水區" : Return 3645
                    Case "善化區" : Return 3646
                    Case "大內區" : Return 3647
                    Case "山上區" : Return 3648
                    Case "新市區" : Return 3649
                    Case "安定區" : Return 3650
                    Case "東區" : Return 3769
                    Case "南區" : Return 3770
                    Case "北區" : Return 3771
                    Case Else : Return -1
                End Select
            Case "高雄市"
                Select Case DistrictComboBox.Text
                    Case "新興區" : Return 3651
                    Case "前金區" : Return 3652
                    Case "苓雅區" : Return 3653
                    Case "鹽埕區" : Return 3654
                    Case "鼓山區" : Return 3655
                    Case "旗津區" : Return 3656
                    Case "前鎮區" : Return 3657
                    Case "三民區" : Return 3658
                    Case "楠梓區" : Return 3659
                    Case "小港區" : Return 3660
                    Case "左營區" : Return 3661
                    Case "仁武區" : Return 3662
                    Case "大社區" : Return 3663
                    Case "岡山區" : Return 3664
                    Case "路竹區" : Return 3665
                    Case "阿蓮區" : Return 3666
                    Case "田寮區" : Return 3667
                    Case "燕巢區" : Return 3668
                    Case "橋頭區" : Return 3669
                    Case "梓官區" : Return 3670
                    Case "彌陀區" : Return 3671
                    Case "永安區" : Return 3672
                    Case "湖內區" : Return 3673
                    Case "鳳山區" : Return 3674
                    Case "大寮區" : Return 3675
                    Case "林園區" : Return 3676
                    Case "鳥松區" : Return 3677
                    Case "大樹區" : Return 3678
                    Case "旗山區" : Return 3679
                    Case "美濃區" : Return 3680
                    Case "六龜區" : Return 3681
                    Case "內門區" : Return 3682
                    Case "杉林區" : Return 3683
                    Case "甲仙區" : Return 3684
                    Case "桃源區" : Return 3685
                    Case "那瑪夏區" : Return 3686
                    Case "茂林區" : Return 3687
                    Case "茄萣區" : Return 3688
                    Case Else : Return -1
                End Select
            Case "屏東縣"
                Select Case DistrictComboBox.Text
                    Case "屏東" : Return 3701
                    Case "三地門" : Return 3702
                    Case "霧臺" : Return 3703
                    Case "瑪家" : Return 3704
                    Case "九如" : Return 3705
                    Case "里港" : Return 3706
                    Case "高樹" : Return 3707
                    Case "盬埔" : Return 3708
                    Case "長治" : Return 3709
                    Case "麟洛" : Return 3710
                    Case "竹田" : Return 3711
                    Case "內埔" : Return 3712
                    Case "萬丹" : Return 3713
                    Case "潮州" : Return 3714
                    Case "泰武" : Return 3715
                    Case "來義" : Return 3716
                    Case "萬巒" : Return 3717
                    Case "崁頂" : Return 3718
                    Case "新埤" : Return 3719
                    Case "南州" : Return 3720
                    Case "林邊" : Return 3721
                    Case "東港" : Return 3722
                    Case "琉球" : Return 3723
                    Case "佳冬" : Return 3724
                    Case "新園" : Return 3725
                    Case "枋寮" : Return 3726
                    Case "枋山" : Return 3727
                    Case "春日" : Return 3728
                    Case "獅子" : Return 3729
                    Case "車城" : Return 3730
                    Case "牡丹" : Return 3731
                    Case "恆春" : Return 3732
                    Case "滿州" : Return 3733
                    Case Else : Return -1
                End Select
            Case "台東縣"
                Select Case DistrictComboBox.Text
                    Case "臺東" : Return 3734
                    Case "綠島" : Return 3735
                    Case "蘭嶼" : Return 3736
                    Case "延平" : Return 3737
                    Case "卑南" : Return 3738
                    Case "鹿野" : Return 3739
                    Case "關山" : Return 3740
                    Case "海端" : Return 3741
                    Case "池上" : Return 3742
                    Case "東河" : Return 3743
                    Case "成功" : Return 3744
                    Case "長濱" : Return 3745
                    Case "太麻里" : Return 3746
                    Case "金峰" : Return 3747
                    Case "大武" : Return 3748
                    Case "達仁" : Return 3749
                    Case Else : Return -1
                End Select
            Case "花蓮縣"
                Select Case DistrictComboBox.Text
                    Case "花蓮" : Return 3750
                    Case "新城" : Return 3751
                    Case "秀林" : Return 3752
                    Case "吉安" : Return 3753
                    Case "壽豐" : Return 3754
                    Case "鳳林" : Return 3755
                    Case "光復" : Return 3756
                    Case "豐濱" : Return 3757
                    Case "瑞穗" : Return 3758
                    Case "萬榮" : Return 3759
                    Case "玉里" : Return 3760
                    Case "卓溪" : Return 3761
                    Case "富里" : Return 3762
                    Case Else : Return -1
                End Select
            Case "台中縣" : Return -1
            Case "台南縣" : Return -1
            Case "高雄縣" : Return -1
            Case Else : Return "False"
        End Select
    End Function
    Sub AddRegion()
        Select Case CityComboBox.Text
            Case "台北市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "中正區"
                With DistrictComboBox.Items
                    .Add("中正區")
                    .Add("大同區")
                    .Add("中山區")
                    .Add("松山區")
                    .Add("大安區")
                    .Add("萬華區")
                    .Add("信義區")
                    .Add("士林區")
                    .Add("北投區")
                    .Add("內湖區")
                    .Add("南港區")
                    .Add("文山區")
                End With
            Case "基隆市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "仁愛區"
                With DistrictComboBox.Items
                    .Add("仁愛區")
                    .Add("安樂區")
                    .Add("暖暖區")
                    .Add("七堵區")
                    .Add("信義區")
                    .Add("中正區")
                    .Add("中山區")
                End With
            Case "宜蘭縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "宜蘭"
                With DistrictComboBox.Items
                    .Add("宜蘭")
                    .Add("頭城")
                    .Add("礁溪")
                    .Add("壯圍")
                    .Add("員山")
                    .Add("羅東")
                    .Add("三星")
                    .Add("大同")
                    .Add("五結")
                    .Add("冬山")
                    .Add("蘇澳")
                    .Add("南澳")
                End With
            Case "桃園縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "中壢"
                With DistrictComboBox.Items
                    .Add("中壢")
                    .Add("平鎮")
                    .Add("龍潭")
                    .Add("楊梅")
                    .Add("新屋")
                    .Add("觀音")
                    .Add("桃園")
                    .Add("龜山")
                    .Add("八德")
                    .Add("大溪")
                    .Add("復興")
                    .Add("大園")
                    .Add("蘆竹")
                End With
            Case "新北市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "萬里區"
                With DistrictComboBox.Items
                    .Add("萬里區")
                    .Add("金山區")
                    .Add("板橋區")
                    .Add("汐止區")
                    .Add("深坑區")
                    .Add("石碇區")
                    .Add("瑞芳區")
                    .Add("平溪區")
                    .Add("雙溪區")
                    .Add("貢寮區")
                    .Add("新店區")
                    .Add("坪林區")
                    .Add("烏來區")
                    .Add("永和區")
                    .Add("中和區")
                    .Add("土城區")
                    .Add("三峽區")
                    .Add("樹林區")
                    .Add("鶯歌區")
                    .Add("三重區")
                    .Add("新莊區")
                    .Add("泰山區")
                    .Add("林口區")
                    .Add("蘆洲區")
                    .Add("五股區")
                    .Add("八里區")
                    .Add("淡水區")
                    .Add("三芝區")
                    .Add("石門區")
                End With
            Case "新竹市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = ""
            Case "新竹縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "竹北"
                With DistrictComboBox.Items
                    .Add("竹北")
                    .Add("湖口")
                    .Add("新豐")
                    .Add("新埔")
                    .Add("關西")
                    .Add("芎林")
                    .Add("寶山")
                    .Add("竹東")
                    .Add("五峰")
                    .Add("橫山")
                    .Add("尖石")
                    .Add("北埔")
                    .Add("峨眉")
                End With
            Case "苗栗縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "竹南"
                With DistrictComboBox.Items
                    .Add("竹南")
                    .Add("頭份")
                    .Add("三灣")
                    .Add("南庄")
                    .Add("獅潭")
                    .Add("後龍")
                    .Add("通霄")
                    .Add("苑裡")
                    .Add("苗栗")
                    .Add("造橋")
                    .Add("頭屋")
                    .Add("公館")
                    .Add("大湖")
                    .Add("泰安")
                    .Add("銅鑼")
                    .Add("三義")
                    .Add("西湖")
                    .Add("卓蘭")
                End With
            Case "台中市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "中區"
                With DistrictComboBox.Items
                    .Add("中區")
                    .Add("東區")
                    .Add("南區")
                    .Add("西區")
                    .Add("北區")
                    .Add("北屯區")
                    .Add("西屯區")
                    .Add("南屯區")
                    .Add("太平區")
                    .Add("大里區")
                    .Add("霧峰區")
                    .Add("烏日區")
                    .Add("豐原區")
                    .Add("后里區")
                    .Add("石岡區")
                    .Add("東勢區")
                    .Add("和平區")
                    .Add("新社區")
                    .Add("潭子區")
                    .Add("大雅區")
                    .Add("神岡區")
                    .Add("大肚區")
                    .Add("沙鹿區")
                    .Add("龍井區")
                    .Add("梧棲區")
                    .Add("清水區")
                    .Add("大甲區")
                    .Add("外埔區")
                End With
            Case "彰化縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "彰化"
                With DistrictComboBox.Items
                    .Add("彰化")
                    .Add("芬園")
                    .Add("花壇")
                    .Add("秀水")
                    .Add("鹿港")
                    .Add("福興")
                    .Add("線西")
                    .Add("和美")
                    .Add("伸港")
                    .Add("員林")
                    .Add("社頭")
                    .Add("永靖")
                    .Add("埔心")
                    .Add("溪湖")
                    .Add("大村")
                    .Add("埔鹽")
                    .Add("田中")
                    .Add("北斗")
                    .Add("田尾")
                    .Add("埤頭")
                    .Add("溪州")
                    .Add("竹塘")
                    .Add("二林")
                    .Add("大城")
                    .Add("芳苑")
                    .Add("二水")
                End With
            Case "南投縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "南投"
                With DistrictComboBox.Items
                    .Add("南投")
                    .Add("中寮")
                    .Add("草屯")
                    .Add("國姓")
                    .Add("埔里")
                    .Add("仁愛")
                    .Add("名間")
                    .Add("集集")
                    .Add("水里")
                    .Add("魚池")
                    .Add("信義")
                    .Add("竹山")
                    .Add("鹿谷")
                End With
            Case "嘉義市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = ""
            Case "嘉義縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "番路"
                With DistrictComboBox.Items
                    .Add("番路")
                    .Add("梅山")
                    .Add("竹崎")
                    .Add("阿里山")
                    .Add("中埔")
                    .Add("大埔")
                    .Add("水上")
                    .Add("鹿草")
                    .Add("太保")
                    .Add("朴子")
                    .Add("東石")
                    .Add("六腳")
                    .Add("新港")
                    .Add("民雄")
                    .Add("大林")
                    .Add("溪口")
                    .Add("義竹")
                    .Add("布袋")
                End With
            Case "雲林縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "斗南"
                With DistrictComboBox.Items
                    .Add("斗南")
                    .Add("大埤")
                    .Add("虎尾")
                    .Add("土庫")
                    .Add("褒忠")
                    .Add("東勢")
                    .Add("臺西")
                    .Add("崙背")
                    .Add("麥寮")
                    .Add("斗六")
                    .Add("林內")
                    .Add("古坑")
                    .Add("莿桐")
                    .Add("西螺")
                    .Add("二崙")
                    .Add("北港")
                    .Add("水林")
                    .Add("口湖")
                    .Add("四湖")
                    .Add("元長")
                End With
            Case "台南市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "中西區"
                With DistrictComboBox.Items
                    .Add("中西區")
                    .Add("安平區")
                    .Add("安南區")
                    .Add("永康區")
                    .Add("歸仁區")
                    .Add("新化區")
                    .Add("左鎮區")
                    .Add("玉井區")
                    .Add("楠西區")
                    .Add("南化區")
                    .Add("仁德區")
                    .Add("關廟區")
                    .Add("龍崎區")
                    .Add("官田區")
                    .Add("麻豆區")
                    .Add("佳里區")
                    .Add("西港區")
                    .Add("七股區")
                    .Add("將軍區")
                    .Add("學甲區")
                    .Add("北門區")
                    .Add("新營區")
                    .Add("後壁區")
                    .Add("白河區")
                    .Add("東山區")
                    .Add("六甲區")
                    .Add("下營區")
                    .Add("柳營區")
                    .Add("鹽水區")
                    .Add("善化區")
                    .Add("大內區")
                    .Add("山上區")
                    .Add("新市區")
                    .Add("安定區")
                    .Add("東區")
                    .Add("南區")
                    .Add("北區")
                End With
            Case "高雄市"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "新興區"
                With DistrictComboBox.Items
                    .Add("新興區")
                    .Add("前金區")
                    .Add("苓雅區")
                    .Add("鹽埕區")
                    .Add("鼓山區")
                    .Add("旗津區")
                    .Add("前鎮區")
                    .Add("三民區")
                    .Add("楠梓區")
                    .Add("小港區")
                    .Add("左營區")
                    .Add("仁武區")
                    .Add("大社區")
                    .Add("岡山區")
                    .Add("路竹區")
                    .Add("阿蓮區")
                    .Add("田寮區")
                    .Add("燕巢區")
                    .Add("橋頭區")
                    .Add("梓官區")
                    .Add("彌陀區")
                    .Add("永安區")
                    .Add("湖內區")
                    .Add("鳳山區")
                    .Add("大寮區")
                    .Add("林園區")
                    .Add("鳥松區")
                    .Add("大樹區")
                    .Add("旗山區")
                    .Add("美濃區")
                    .Add("六龜區")
                    .Add("內門區")
                    .Add("杉林區")
                    .Add("甲仙區")
                    .Add("桃源區")
                    .Add("那瑪夏區")
                    .Add("茂林區")
                    .Add("茄萣區")
                End With
            Case "屏東縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "屏東"
                With DistrictComboBox.Items
                    .Add("屏東")
                    .Add("三地門")
                    .Add("霧臺")
                    .Add("瑪家")
                    .Add("九如")
                    .Add("里港")
                    .Add("高樹")
                    .Add("盬埔")
                    .Add("長治")
                    .Add("麟洛")
                    .Add("竹田")
                    .Add("內埔")
                    .Add("萬丹")
                    .Add("潮州")
                    .Add("泰武")
                    .Add("來義")
                    .Add("萬巒")
                    .Add("崁頂")
                    .Add("新埤")
                    .Add("南州")
                    .Add("林邊")
                    .Add("東港")
                    .Add("琉球")
                    .Add("佳冬")
                    .Add("新園")
                    .Add("枋寮")
                    .Add("枋山")
                    .Add("春日")
                    .Add("獅子")
                    .Add("車城")
                    .Add("牡丹")
                    .Add("恆春")
                    .Add("滿州")
                End With
            Case "台東縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "臺東"
                With DistrictComboBox.Items
                    .Add("臺東")
                    .Add("綠島")
                    .Add("蘭嶼")
                    .Add("延平")
                    .Add("卑南")
                    .Add("鹿野")
                    .Add("關山")
                    .Add("海端")
                    .Add("池上")
                    .Add("東河")
                    .Add("成功")
                    .Add("長濱")
                    .Add("太麻里")
                    .Add("金峰")
                    .Add("大武")
                    .Add("達仁")
                End With
            Case "花蓮縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = "花蓮"
                With DistrictComboBox.Items
                    .Add("花蓮")
                    .Add("新城")
                    .Add("秀林")
                    .Add("吉安")
                    .Add("壽豐")
                    .Add("鳳林")
                    .Add("光復")
                    .Add("豐濱")
                    .Add("瑞穗")
                    .Add("萬榮")
                    .Add("玉里")
                    .Add("卓溪")
                    .Add("富里")
                End With
            Case "台中縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = ""
            Case "台南縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = ""
            Case "高雄縣"
                DistrictComboBox.Items.Clear()
                DistrictComboBox.Text = ""
        End Select
    End Sub
    Private Sub CityComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CityComboBox.SelectedIndexChanged
        AddRegion()
    End Sub

    Private Sub AmountTrackBar_Scroll(sender As Object, e As EventArgs) Handles AmountTrackBar.Scroll
        AmountLabel.Text = "搶購數量 : (" & AmountTrackBar.Value * ChooseItemCount & "個)"
    End Sub
    Private Sub TimeoutTrackBar_Scroll(sender As Object, e As EventArgs) Handles TimeoutTrackBar.Scroll
        TimeoutLabel.Text = "時間上限 : (" & TimeoutTrackBar.Value & "ms)"
    End Sub
    Private Sub XiaomiFrm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        LoginThread = New Thread(AddressOf Me.LoginBackground)
        LoginThread.Start()
        Loginfrm.Hide()
        Dim g As Graphics = Me.CreateGraphics()
        Dim ds As String = "行動電源5200mAh是我的。"
        Dim strSize As SizeF = g.MeasureString(ds, Me.Font)
        Label1.Location = New Point(278 - ((206 - strSize.Width) - (206 - g.MeasureString(Label2.Text, Me.Font).Width)) / 2, 236)
    End Sub
    Public Function UnicodeToStr(ByVal STR As String) As String
        Dim i As Integer, strChar As String = Nothing, arrChar As String() = Split(STR, "\u")
        For i = 1 To UBound(arrChar)
            Dim n As String = Microsoft.VisualBasic.Left(arrChar(i), 4)
            Dim k As String = Microsoft.VisualBasic.Right(arrChar(i), arrChar(i).Length - 4)
            strChar &= ChrW("&H" & n) & k
        Next
        Return strChar
    End Function
    Sub LoginBackground()
        Try
            '"https://account.xiaomi.com/pass/userInfoJsonP?userId=" + this.miid + "&callback=getAccountInfo"
            Dim dteStart As DateTime = Now

            Dim userName As String = User
            Dim password As String = Pwd

            Log.Text &= "[" & Now & "] 開始嘗試登入" & User & "..." & vbCrLf

            Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
            'parameters.Add("user", System.Uri.EscapeDataString(userName))
            'parameters.Add("_json", "true")
            'parameters.Add("pwd", System.Uri.EscapeDataString(password))
            'parameters.Add("sid", "passport")
            'parameters.Add("_sign", "KKkRvCpZoDC%2BgLdeyOsdMhwV0Xg%3D")
            'parameters.Add("callback", "https%3A%2F%2Faccount.xiaomi.com&qs=%253Fsid%253Dpassport")
            'parameters.Add("qs", "%253Fsid%253Dpassport")
            'parameters.Add("auto", "true")

            Dim response As HttpWebResponse '= HttpWebResponseUtility.CreatePostHttpResponse("https://account.xiaomi.com/pass/serviceLoginAuth2", parameters, Nothing, Nothing, Encoding.UTF8, cookies)
            Dim reader As StreamReader '= New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML As String '= reader.ReadToEnd()
            Dim t As Newtonsoft.Json.Linq.JObject '= Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML.Replace("&&&START&&&", ""))

            'If Not t.Item("desc").ToString = "成功" Then
            '    MsgBox("系統抓取異常 , 請稍後再試 :)", MsgBoxStyle.Critical, "Opps ! Something Error :(")
            '    Exit Sub
            'End If

            'Dim _userId As String '= t.Item("userId").ToString '= respHTML.Substring(startPos, endPos - startPos)
            Me.Text = "Xiaomi Auto Buy (By Silent) @ " & _UserId

            response = HttpWebResponseUtility.CreateGetHttpResponse(_loginUrl, Nothing, Nothing, cookies)
            reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            respHTML = reader.ReadToEnd()

            Log.Text &= "[" & Now & "] 成功登入 (小米ID : " & _UserId & ")" & vbCrLf

            Dim TS As TimeSpan = Now.Subtract(dteStart)
            Debug.Print("耗時 : " & TS.TotalMilliseconds)

            If Mode1RadioButton.Checked Then
                '第一次掃貨
                Log.Text &= "[" & Now & "] 開始第一次檢查" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & " ..." & vbCrLf

                'm -- 来源，web :1 ,app:2,m:3

                If _hdid = "" Then
                    response = HttpWebResponseUtility.CreateGetHttpResponse(_hdgetUrl & ChooseItemID & "&addcart=" & ChooseItemCount & "&m=1" & "&fk=1", Nothing, Nothing, cookies)
                    reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                    respHTML = reader.ReadToEnd()
                End If
                Dim _ServerTime As String = GetServerTime()
                Dim _FakeTime As String = CLng(Now.Subtract(New System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds)
                Dim _isCos As Boolean = True

                respHTML = Replace(Replace(respHTML, "hdcontrol(", ""), ")", "")
                If respHTML = "" And _hdid = "" Then
                    If CheckShopCart(True, False) Then : GoTo ContinueStep : End If

                    ToolStripStatusLabel.Text = "商品沒有庫存了"
                    Log.Text &= "[" & Now & "] " & ItemComboBox.Items(ItemComboBox.SelectedIndex) & "檢查完畢 , 目前沒有庫存" & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    GoTo ContinueStep
                End If

                Dim _hdurl As String
                Dim _hdstart As Boolean
                Dim _hdstop As Boolean
                Dim _isOver As Boolean
                If _hdid = "" Then
                    t = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML)
                    _hdurl = t.Item("status")(ChooseItemID.ToString)("hdurl").ToString
                    _hdstart = t.Item("status")(ChooseItemID.ToString)("hdstart").ToString
                    _hdstop = t.Item("status")(ChooseItemID.ToString)("hdstop").ToString
                    If _hdstart = False And _hdstop = True Then : _isOver = True : Else : _isOver = False : End If
                Else
                    _isOver = Not (CheckHdid())
                    _hdurl = Nothing
                End If

                If _hdurl = Nothing And _isOver = True Then
                    If CheckShopCart(True, False) Then : GoTo ContinueStep : End If

                    ToolStripStatusLabel.Text = "商品沒有庫存了"
                    Log.Text &= "[" & Now & "] " & ItemComboBox.Items(ItemComboBox.SelectedIndex) & "檢查完畢 , 目前沒有庫存" & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.Orange
                Else
TryGet:
                    If _hdurl = Nothing And _isOver = False Then
                        response = HttpWebResponseUtility.CreateGetHttpResponse(_shopCartUrl & ChooseItemID & "-0-" & ChooseItemCount, Nothing, Nothing, cookies)
                        reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        respHTML = reader.ReadToEnd()
                    Else
                        response = HttpWebResponseUtility.CreateGetHttpResponse(_shopCartUrl & ChooseItemID & "-0-" & ChooseItemCount & "?source=bigtap&token=" & _hdurl, Nothing, Nothing, cookies)
                        reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        respHTML = reader.ReadToEnd()
                    End If

                    If CheckShopCart(False, False) Then : GoTo ContinueStep : End If

                    If respHTML Like "*商品沒有庫存了*" Or respHTML Like "*該商品已缺貨*" Or respHTML Like "*您沒有搶購成功*" Then
                        ToolStripStatusLabel.Text = "商品沒有庫存了"
                        Log.Text &= "[" & Now & "] " & ItemComboBox.Items(ItemComboBox.SelectedIndex) & "檢查完畢 , 目前沒有庫存" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                    ElseIf respHTML Like "*防止黃牛*" Then
                        ToolStripStatusLabel.Text = "發現野生的驗證碼"
                        Log.Text &= "[" & Now & "] 被野外露出的驗證碼擊殺" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                    ElseIf respHTML Like "*系统繁忙*" Then
                        ToolStripStatusLabel.Text = "系統繁忙"
                        ToolStripStatusLabel.ForeColor = Color.Orange
                    ElseIf respHTML Like "*校驗碼*" Or respHTML Like "*添加購物車需要登入，請先登入！*" Then
                        ToolStripStatusLabel.Text = "怪怪Der"
                        Log.Text &= "[" & Now & "] 怪怪Der" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                    ElseIf respHTML Like "*非常抱歉*" Then
                        ToolStripStatusLabel.Text = "伺服器壓力過大"
                        Log.Text &= "[" & Now & "] 伺服器壓力過大" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                    ElseIf respHTML Like "*超出購買限制*" Or respHTML Like "*超過單品的最大購買數量*" Then
                        If CheckShopCart(False) = True Then : GoTo ContinueStep : End If

                        ToolStripStatusLabel.Text = "怪怪Der"
                        Log.Text &= "[" & Now & "] 怪怪Der" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                    Else
                        ToolStripStatusLabel.Text = "發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                        Log.Text &= "[" & Now & "] 發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.DodgerBlue
                    End If
                End If
ContinueStep:
                DianYuanCheckBox.Enabled = True
                ToolStripStatusLabel.Text = "程式尚未開始收刮 ..."
                ToolStripStatusLabel.ForeColor = Color.Orange
            ElseIf Mode2RadioButton.Checked Then
                DianYuanCheckBox.Enabled = True
                ToolStripStatusLabel.Text = "程式尚未開始自動送單 ..."
            End If
        Catch ex As Exception
            MsgBox("系統抓取異常 , 請稍後再試 :)", MsgBoxStyle.Critical, "Opps ! Something Error :(")
            End
        End Try
    End Sub
    Dim OrderCheck As Boolean = False
    Sub OrderBackground()
        Try
            If OrderCheck = True Then
                Exit Sub
            End If
            If DianYuanCheckBox.Enabled = True Then
                OrderCheck = True
                OrderListView.Items.Clear()
                Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_orderUrl, Nothing, Nothing, cookies)
                Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                Dim respHTML As String = reader.ReadToEnd
                Dim _Payment As String = Nothing
                Dim _Tcat As String = Nothing
                Dim _MiStatus As String = Nothing
                Dim _Place As String = Nothing
                Dim _Status As String = Nothing
                Dim _Xiaomi As String = Nothing
                Dim _Detail As String = Nothing
                Dim _OrderDetail As String = Nothing
                Dim PageList As New ListBox

                Dim doc As New HtmlDocument()
                doc.LoadHtml(respHTML)
                Dim node As HtmlNode = doc.DocumentNode
                PageList.Items.Add(_orderUrl)
                If node.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div/div/ul/div/a[1]") IsNot Nothing Then
                    For i = 1 To node.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div/div/ul/div/a").Count - 1
                        PageList.Items.Add(node.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div/div/ul/div/a[" & i & "]")(0).Attributes("href").Value)
                    Next
                End If
                For z = 0 To PageList.Items.Count - 1
                    If Not PageList.Items.Item(z) = _orderUrl Then
                        response = HttpWebResponseUtility.CreateGetHttpResponse(PageList.Items.Item(z), Nothing, Nothing, cookies)
                        reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        respHTML = reader.ReadToEnd
                        doc.LoadHtml(respHTML)
                        node = doc.DocumentNode
                    End If
                    '/html/body/div[4]/div/div/div[2]/div/div/div/ul/li[1]/table
                    '/html/body/div[4]/div/div/div[2]/div/div/div/ul/li[2]
                    '/html/body/div[4]/div/div/div[2]/div/div/div/ul/li[1]/table/thead/tr/th/div/span[1]/a
                    For i = 1 To node.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div/div/ul/li").Count
                        If node.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div/div/ul/li[" & i & "]/table/thead/tr/th/div/span[1]/a") Is Nothing Then
                            Continue For
                        End If
                        _Xiaomi = WebUtility.HtmlDecode(node.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div/div/ul/li[" & i & "]/table/thead/tr/th/div/span[1]/a")(0).InnerText).TrimEnd
                        'Debug.Print(WebUtility.HtmlDecode(node.SelectNodes("//table[" & i & "]/tbody/tr/td[1]/table/tbody/tr/td[2]/span")(0).InnerText).Trim) '外層運送狀態
                        Dim responsex As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_orderView & WebUtility.HtmlDecode(node.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div/div/ul/li[" & i & "]/table/thead/tr/th/div/span[1]/a")(0).InnerText).TrimEnd, Nothing, Nothing, cookies)
                        Dim readerx As StreamReader = New StreamReader(responsex.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        Dim respHTMLx As String = readerx.ReadToEnd
                        Dim docx As New HtmlDocument()
                        docx.LoadHtml(respHTMLx)
                        Dim nodex As HtmlNode = docx.DocumentNode
                        Dim TcatCheck As Boolean = False
                        '/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[3]/table/tbody/tr[2]/td
                        If nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[3]/table/tbody/tr[2]") IsNot Nothing Then
                            If nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[3]/table/tbody/tr[2]/td") IsNot Nothing Then
                                _Tcat = WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[3]/table/tbody/tr[2]/td")(0).InnerText).Trim
                            End If
                            TcatCheck = True
                        End If
                        _Detail &= WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[1]/td")(0).InnerText) & "/"
                        _Detail &= Replace(Replace(WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(1) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(2) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(3), WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(1) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(2) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(1), WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(1) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(2)), WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(1) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(2) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(2), WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(1) & WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[2]/td")(0).InnerText).Split(" ")(2)) & "/"
                        _Detail &= WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[2]/div[1]/table/tbody/tr[3]/td")(0).InnerText).Trim
                        If TcatCheck = False Then
                            For j = 1 To nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[1]/ul/li").Count
                                If nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[1]/ul/li[" & j & "]/a[2]") IsNot Nothing Then
                                    Try
                                        _OrderDetail &= StrConv(WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[1]/ul/li[" & j & "]/a[2]")(0).InnerText).TrimEnd, VbStrConv.TraditionalChinese, 2052) & "|"
                                    Catch ex As Exception

                                    End Try
                                End If
                            Next
                            _Payment = (WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[3]/div/span/span")(0).InnerText).Trim)
                            _Status = (StrConv(WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[4]/div/span")(0).InnerText).Trim, VbStrConv.TraditionalChinese, 2052))
                        Else
                            For j = 1 To nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[1]/ul/li").Count
                                If nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[1]/ul/li[" & j & "]/a[2]") IsNot Nothing Then
                                    Try
                                        _OrderDetail &= StrConv(WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[1]/ul/li[" & j & "]/a[2]")(0).InnerText).TrimEnd, VbStrConv.TraditionalChinese, 2052) & "|"
                                    Catch ex As Exception

                                    End Try
                                End If
                            Next
                            _Payment = (WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[3]/div/span/span")(0).InnerText).Trim)
                            _Status = (StrConv(WebUtility.HtmlDecode(nodex.SelectNodes("/html/body/div[4]/div/div/div[2]/div/div[2]/div[1]/table/tbody/tr[1]/td[4]/div/span")(0).InnerText).Trim, VbStrConv.TraditionalChinese, 2052))
                        End If
                        AddNewOrderItem(_Xiaomi, _Status, _Payment, _Tcat, _Detail, _OrderDetail)
                        _Xiaomi = Nothing
                        _Status = Nothing
                        _Payment = Nothing
                        _Tcat = Nothing
                        _Detail = Nothing
                        _OrderDetail = Nothing
                        responsex.Close()
                    Next
                    response.Close()
                Next
                OrderCheck = False
            End If
        Catch ex As Exception
            OrderCheck = False
        End Try
    End Sub
    Dim _Bank As String
    Sub NewPinCodeBackground(_OrderId As String)
        Try
            Dim startPos As Integer, endPos As Integer
            Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
            parameters.Clear()
            If _Bank = "Family" Then
                parameters.Add("bank", "familymart")
            ElseIf _Bank = "7-11" Then
                parameters.Add("bank", "seven_eleven")
            End If
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreatePostHttpResponse(_confirmOrder & _OrderId & "#", parameters, Nothing, Nothing, Encoding.UTF8, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML As String = reader.ReadToEnd()
            Debug.Print(respHTML)
            Try
                '/html/body/div[3]/div/div[2]/div/div[1]/ul/li[2]/text()
                startPos = respHTML.IndexOf("http://buy.mi.com/tw/buy/qrcode?location=aws&code=", StringComparison.CurrentCultureIgnoreCase) + 50
                endPos = respHTML.IndexOf("&key=", StringComparison.CurrentCultureIgnoreCase)
                Dim _PinCode As String = respHTML.Substring(startPos, endPos - startPos)
                Dim Loginfrm As New Pincode(_PinCode, _Bank)
                Loginfrm.Show()
                Debug.Print("PinCode : " & _PinCode) '全家 : 14碼 '7-11 : 12碼
                'MsgBox("PinCode : " & _PinCode)
            Catch ex As Exception
                Debug.Print("系統繁忙請自行去訂單抓PinCode")
                MsgBox("系統繁忙請稍後再試", MsgBoxStyle.Exclamation)
            End Try
        Catch ex As Exception

        End Try
    End Sub
    'Private Sub OrderListView_MouseMove(sender As Object, e As MouseEventArgs) Handles OrderListView.MouseMove
    '    SilentEngineLabel.Visible = False
    'End Sub
    'Private Sub OrderListView_MouseLeave(sender As Object, e As EventArgs) Handles OrderListView.MouseLeave
    '    SilentEngineLabel.Visible = True
    'End Sub
    Private Sub 付款ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 付款ToolStripMenuItem.Click
        _Bank = "7-11"
        NewPinCodeBackground(OrderListView.Items(OrderListView.FocusedItem.Index).Text)
    End Sub
    Private Sub 取消訂單ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 取消訂單ToolStripMenuItem.Click
        If MsgBox("是否取消訂單 (" & OrderListView.Items(OrderListView.FocusedItem.Index).Text & ") ?", MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_cancelOrder & OrderListView.Items(OrderListView.FocusedItem.Index).Text, Nothing, Nothing, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML As String = reader.ReadToEnd()
            response.Close()
            response = HttpWebResponseUtility.CreateGetHttpResponse(_orderView & OrderListView.Items(OrderListView.FocusedItem.Index).Text, Nothing, Nothing, cookies)
            reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            respHTML = reader.ReadToEnd()
            If respHTML Like "*已關閉*" Then
                MsgBox("取消訂單成功 !", MsgBoxStyle.Information)
                Try
                    OrderThread.Abort()
                Catch ex As Exception

                End Try
                OrderThread = New Thread(AddressOf Me.OrderBackground)
                OrderThread.Start()
            Else
                MsgBox("取消訂單失敗 , 請稍後再試 !", MsgBoxStyle.Critical)
            End If
            response.Close()
        End If
    End Sub
    Private Sub FamilyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FamilyToolStripMenuItem.Click
        _Bank = "Family"
        NewPinCodeBackground(OrderListView.Items(OrderListView.FocusedItem.Index).Text)
    End Sub
    Private Sub OrderListView_MouseClick(sender As Object, e As MouseEventArgs) Handles OrderListView.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Right And OrderListView.Items(OrderListView.FocusedItem.Index).SubItems(1).Text = "等待付款" And Application.OpenForms().OfType(Of Pincode).Any = False Then
            ContextMenuStrip.Show(OrderListView, New Point(e.X, e.Y))
        End If
    End Sub
    Private Sub OrderListView_DoubleClick(sender As Object, e As EventArgs) Handles OrderListView.DoubleClick
        If Not OrderListView.Items(OrderListView.FocusedItem.Index).SubItems(3).Text = Nothing Then
            Process.Start("http://www.t-cat.com.tw/Inquire/TraceDetail.aspx?BillID=" & OrderListView.Items(OrderListView.FocusedItem.Index).SubItems(3).Text & "&ReturnUrl=Trace.aspx")
        End If
    End Sub
    Sub AddNewOrderItem(Xiaomi As String, Status As String, Payment As String, Tcat As String, Detail As String, OrderDetail As String)
        Dim item As New ListViewItem
        Dim _OrderDetail As String = Nothing
        item.Text = Xiaomi
        item.SubItems.Add(Status)
        item.SubItems.Add(Payment)
        item.SubItems.Add(Tcat)
        item.SubItems.Add(Detail)
        If Not OrderDetail = Nothing Then
            For i = 0 To OrderDetail.Split("|").Length - 2
                _OrderDetail &= OrderDetail.Split("|")(i) & vbCrLf
            Next
        End If
        item.ToolTipText = "姓名 : " & Detail.Split("/")(0) & vbCrLf & "地址 : " & Detail.Split("/")(1) & vbCrLf & "聯繫電話 : " & Detail.Split("/")(2) & vbCrLf & vbCrLf & _OrderDetail
        OrderListView.Items.Add(item)
    End Sub
    Sub ReCheckBackground()
        Try
            Dim response As HttpWebResponse
            Dim reader As StreamReader
            Dim respHTML As String = Nothing
            'Dim startPos As Integer, endPos As Integer

            Log.Text &= "[" & Now & "] 檢查" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & " ..." & vbCrLf
            If _hdid = "" Then
                response = HttpWebResponseUtility.CreateGetHttpResponse(_hdgetUrl & ChooseItemID & "&addcart=" & ChooseItemCount & "&m=1" & "&fk=1", Nothing, Nothing, cookies)
                reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                respHTML = reader.ReadToEnd()
            End If
            Dim _ServerTime As String = GetServerTime()
            Dim _FakeTime As String = CLng(Now.Subtract(New System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds)
            Dim _isCos As Boolean = True

            respHTML = Replace(Replace(respHTML, "hdcontrol(", ""), ")", "")
            If respHTML = "" And _hdid = "" Then
                If CheckShopCart(True, False) = True Then : GoTo ContinueStep : End If

                ToolStripStatusLabel.Text = "商品沒有庫存了"
                Log.Text &= "[" & Now & "] " & ItemComboBox.Items(ItemComboBox.SelectedIndex) & "檢查完畢 , 目前沒有庫存" & vbCrLf
                ToolStripStatusLabel.ForeColor = Color.Orange
                GoTo ContinueStep
            End If
            Dim t As Newtonsoft.Json.Linq.JObject
            Dim _hdurl As String
            Dim _hdstart As Boolean
            Dim _hdstop As Boolean
            Dim _isOver As Boolean
            If _hdid = "" Then
                t = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML)
                _hdurl = t.Item("status")(ChooseItemID.ToString)("hdurl").ToString
                _hdstart = t.Item("status")(ChooseItemID.ToString)("hdstart").ToString
                _hdstop = t.Item("status")(ChooseItemID.ToString)("hdstop").ToString
                If _hdstart = False And _hdstop = True Then : _isOver = True : Else : _isOver = False : End If
            Else
                _isOver = Not (CheckHdid())
                _hdurl = Nothing
            End If


            If _hdurl = Nothing And _isOver = True Then
                If CheckShopCart(True, False) = True Then : GoTo ContinueStep : End If

                ToolStripStatusLabel.Text = "商品沒有庫存了"
                Log.Text &= "[" & Now & "] " & ItemComboBox.Items(ItemComboBox.SelectedIndex) & "檢查完畢 , 目前沒有庫存" & vbCrLf
                ToolStripStatusLabel.ForeColor = Color.Orange
            Else
TryGet:
                If _hdurl = Nothing And _isOver = False Or _isCos = False Then
                    response = HttpWebResponseUtility.CreateGetHttpResponse(_shopCartUrl & ChooseItemID & "-0-" & ChooseItemCount & "?source=bigtap&token=8c48b274b1543c3350209141aec67a8e,259189338," & ChooseItemID & "," & _ServerTime & ",1," & ChooseItemCount & ",000,bigtap,", Nothing, Nothing, cookies)
                    reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                    respHTML = reader.ReadToEnd()
                Else
                    response = HttpWebResponseUtility.CreateGetHttpResponse(_shopCartUrl & ChooseItemID & "-0-" & ChooseItemCount & "?source=bigtap&token=" & _hdurl, Nothing, Nothing, cookies)
                    reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                    respHTML = reader.ReadToEnd()
                End If

                If CheckShopCart(False) = True Then : GoTo ContinueStep : End If

                If respHTML Like "*商品沒有庫存了*" Or respHTML Like "*該商品已缺貨*" Or respHTML Like "*您沒有搶購成功*" Then
                    ToolStripStatusLabel.Text = "商品沒有庫存了"
                    Log.Text &= "[" & Now & "] " & ItemComboBox.Items(ItemComboBox.SelectedIndex) & "檢查完畢 , 目前沒有庫存" & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.Orange
                ElseIf respHTML Like "*防止黃牛*" Then
                    ToolStripStatusLabel.Text = "發現野生的驗證碼"
                    Log.Text &= "[" & Now & "] 被野外露出的驗證碼擊殺" & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.Orange
                ElseIf respHTML Like "*系统繁忙*" Then
                    ToolStripStatusLabel.Text = "系統繁忙"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                ElseIf respHTML Like "*校驗碼*" Or respHTML Like "*添加購物車需要登入，請先登入！*" Then
                    ToolStripStatusLabel.Text = "怪怪Der"
                    Log.Text &= "[" & Now & "] 怪怪Der" & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.Orange
                ElseIf respHTML Like "*非常抱歉*" Then
                    ToolStripStatusLabel.Text = "伺服器壓力過大"
                    Log.Text &= "[" & Now & "] 伺服器壓力過大" & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.Orange
                ElseIf respHTML Like "*超出購買限制*" Or respHTML Like "*超過單品的最大購買數量*" Then
                    If CheckShopCart(False) = True Then : GoTo ContinueStep : End If

                    ToolStripStatusLabel.Text = "怪怪Der"
                    Log.Text &= "[" & Now & "] 怪怪Der" & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.Orange
                Else
                    ToolStripStatusLabel.Text = "發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                    Log.Text &= "[" & Now & "] 發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    ToolStripStatusLabel.ForeColor = Color.DodgerBlue
                End If
            End If
ContinueStep:
            DianYuanCheckBox.Enabled = True
        Catch ex As Exception
            ToolStripStatusLabel.Text = "發生異常"
            Log.Text &= "[" & Now & "] 發生異常" & vbCrLf
            ToolStripStatusLabel.ForeColor = Color.Orange
            DianYuanCheckBox.Enabled = False
            Try
                AutoThread.Abort()
                ReCheckThread.Abort()
            Catch x As Exception

            End Try
        End Try
    End Sub
    Function GetServerTime() As String
        Try
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_timestampUrl, Nothing, Nothing, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML As String = reader.ReadToEnd()
            response.Close()
            Return respHTML.Split("=")(1).Trim
        Catch ex As Exception
            Return CLng(Now.Subtract(New System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds)
        End Try
    End Function
    Function CheckShopCart(AddCart As Boolean, Optional ToolText As Boolean = True) As Boolean
        Try
            Dim _ServerTime As String = GetServerTime()

            If AddCart Then
                Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_shopCartUrl & ChooseItemID & "-0-" & ChooseItemCount, Nothing, Nothing, cookies)
                Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                Dim respHTML As String = reader.ReadToEnd()
            End If

            Dim responsex As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_miniNew, Nothing, Nothing, cookies)
            Dim readerx As StreamReader = New StreamReader(responsex.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTMLx = readerx.ReadToEnd()
            Dim tx As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTMLx)
            Dim _totalItem As Integer = tx.Item("totalItem").ToString
            For i = 0 To _totalItem - 1
                If tx.Item("items")(i)("itemId").ToString.Substring(0, 10) = ChooseItemID.ToString Then
                    If tx.Item("items")(i)("num").ToString = ChooseItemCount Then
                        If ToolText Then : ToolStripStatusLabel.Text = "發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) : End If
                        Log.Text &= "[" & Now & "] 發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                        If ToolText Then : ToolStripStatusLabel.ForeColor = Color.DodgerBlue : End If
                        Return True
                    End If
                End If
            Next
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Dim DisConnectCheck As Boolean = False
    Function CheckHdid() As Boolean
        Try
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse("http://buy.mi.com/tw/misc/getstarstock/hdid/" + _hdid, Nothing, Nothing, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML As String = reader.ReadToEnd()
            Dim t As Newtonsoft.Json.Linq.JArray = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML)
            Dim _isCos As Boolean
            Dim _isDamiao As Boolean
            For i = 0 To t.Count - 1
                If t.Item(i)("id").ToString = ChooseItemID Then
                    _isCos = t.Item(i)("is_cos")
                    _isDamiao = t.Item(i)("is_damiao")
                    Exit For
                End If
            Next
            response.Close()
            Return Not (_isCos) And _isDamiao
        Catch ex As Exception
            Return False
        End Try
    End Function
    Sub AutoBuyBackground()
        While (1)
            Try
                If TimeCheckBox.Checked = True Then
                    If DateTime.Now.ToLocalTime() < CType(CType(TimePicker.Value, DateTime).AddMinutes(-10).ToString("yyyy/MM/dd HH:mm"), DateTime) Then
                        ToolStripStatusLabel.Text = "未達設定時間"
                        Thread.Sleep(500)
                        GoTo ReTry
                    End If
                End If

                If SkuCount >= AmountTrackBar.Value * ChooseItemCount Then
                    Exit Sub
                End If

                Dim dteStart As DateTime = Now
                Dim CartCheck As Boolean = False
                Dim response As HttpWebResponse '= HttpWebResponseUtility.CreateGetHttpResponse("http://www.xiaomi.tw/cart/add/2141300041-0-2", Nothing, Nothing, cookies)
                Dim reader As StreamReader '= New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                Dim respHTML As String = Nothing '= reader.ReadToEnd()
                Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
                Dim startPos As Integer, endPos As Integer
                Dim _OrderId As String = Nothing
                Dim _hdurl As String = Nothing

                If CheckShopCart(False) = True Then : CartCheck = True : GoTo ReTrySend : End If

                If _hdid = "" Or ChooseItemID.StartsWith("4") Then
                    response = HttpWebResponseUtility.CreateGetHttpResponse(_hdgetUrl & ChooseItemID & "&addcart=" & ChooseItemCount & "&m=1" & "&fk=1", Nothing, Nothing, cookies)
                    reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                    respHTML = reader.ReadToEnd()
                    Debug.Print(respHTML)
                End If
                Dim _ServerTime As String = GetServerTime()
                Dim _FakeTime As String = CLng(Now.Subtract(New System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds)
                Dim _isCos As Boolean = True

                respHTML = Replace(Replace(respHTML, "hdcontrol(", ""), ")", "")
                If respHTML = "" And _hdid = "" Then
                    Dim TSx As TimeSpan = Now.Subtract(dteStart)
                    Debug.Print("耗時 : " & TSx.TotalMilliseconds)

                    If CheckShopCart(True) = True Then : GoTo ReTrySend : End If

                    ToolStripStatusLabel.Text = "商品沒有庫存了"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    GoTo ReTry
                End If

                Dim t As Newtonsoft.Json.Linq.JObject
                Dim _hdstart As Boolean
                Dim _hdstop As Boolean
                Dim _isOver As Boolean
                If (_hdid = "" Or ChooseItemID.StartsWith("4")) And Not respHTML = "" Then
                    Try
                        t = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML)
                        _hdurl = t.Item("status")(ChooseItemID.ToString)("hdurl").ToString
                        _hdstart = t.Item("status")(ChooseItemID.ToString)("hdstart").ToString
                        _hdstop = t.Item("status")(ChooseItemID.ToString)("hdstop").ToString
                        If _hdstart = False And _hdstop = True Then : _isOver = True : Else : _isOver = False : End If
                    Catch ex As Exception
                        _isOver = Not (CheckHdid())
                        _hdurl = Nothing
                    End Try
                Else
                    _isOver = Not (CheckHdid())
                    _hdurl = Nothing
                End If
                Debug.Print("hdstart : " & _hdstart & ", hdstop : " & _hdstop & ", hdurl : " & _hdurl & ", isCos : " & _isCos)

                If Not _hdurl = Nothing Then
                    Log.Text &= "[" & Now & "] Find HdUrl. (" & _hdurl & ")" & vbCrLf
                End If

                If _hdurl = Nothing And _isOver = True Then
                    Dim TSx As TimeSpan = Now.Subtract(dteStart)
                    Debug.Print("耗時 : " & TSx.TotalMilliseconds)

                    If CheckShopCart(True) = True Then : GoTo ReTrySend : End If

                    ToolStripStatusLabel.Text = "商品沒有庫存了"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    GoTo ReTry
                Else
TryGet:
                    If _hdurl = Nothing And _isOver = False Or _isCos = False Then
                        response = HttpWebResponseUtility.CreateGetHttpResponse(_shopCartUrl & ChooseItemID & "-0-" & ChooseItemCount, Nothing, Nothing, cookies)
                        reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        respHTML = reader.ReadToEnd()
                    Else
                        response = HttpWebResponseUtility.CreateGetHttpResponse(_shopCartUrl & ChooseItemID & "-0-" & ChooseItemCount & "?source=bigtap&token=" & _hdurl, Nothing, Nothing, cookies)
                        reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        respHTML = reader.ReadToEnd()
                    End If
                    Dim TSx As TimeSpan = Now.Subtract(dteStart)
                    Debug.Print("耗時 : " & TSx.TotalMilliseconds)

                    If CheckShopCart(False) = True Then : CartCheck = True : GoTo ReTrySend : End If

                    If respHTML Like "*商品沒有庫存了*" Or respHTML Like "*該商品已缺貨*" Or respHTML Like "*您沒有搶購成功*" Then
                        ToolStripStatusLabel.Text = "商品沒有庫存了"
                        ToolStripStatusLabel.ForeColor = Color.Orange
                        GoTo ReTry
                    ElseIf respHTML Like "*防止黃牛*" Then
                        ToolStripStatusLabel.Text = "發現野生的驗證碼"
                        Log.Text &= "[" & Now & "] 被野外露出的驗證碼擊殺" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                        GoTo ReTry
                    ElseIf respHTML Like "*系统繁忙*" Then
                        ToolStripStatusLabel.Text = "系統繁忙"
                        ToolStripStatusLabel.ForeColor = Color.Orange
                        GoTo ReTry
                    ElseIf respHTML Like "*校驗碼*" Or respHTML Like "*添加購物車需要登入，請先登入！*" Then
                        ToolStripStatusLabel.Text = "怪怪Der"
                        ToolStripStatusLabel.ForeColor = Color.Orange
                        GoTo ReTry
                    ElseIf respHTML Like "*非常抱歉*" Then
                        ToolStripStatusLabel.Text = "伺服器壓力過大"
                        Log.Text &= "[" & Now & "] 伺服器壓力過大" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                        GoTo ReTry
                    ElseIf respHTML Like "*超出購買限制*" Or respHTML Like "*超過單品的最大購買數量*" Then
                        If CheckShopCart(False) = True Then : GoTo ReTrySend : End If

                        ToolStripStatusLabel.Text = "怪怪Der"
                        Log.Text &= "[" & Now & "] 怪怪Der" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                        GoTo ReTry
                    ElseIf respHTML Like "*添加商品成功*" Then
                        ToolStripStatusLabel.Text = "發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                        Log.Text &= "[" & Now & "] 發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.DodgerBlue
                        GoTo ReTrySend
                    Else
                        ToolStripStatusLabel.Text = "怪怪Der"
                        Log.Text &= "[" & Now & "] 怪怪Der" & vbCrLf
                        ToolStripStatusLabel.ForeColor = Color.Orange
                        GoTo ReTry
                    End If
                End If
ReTrySend:
                Dim _addressId As String = getAddressId()
                If _addressId = "-1" Then : _addressId = saveAddress() : End If
                parameters = New Dictionary(Of String, String)()
                parameters.Clear()
                If best_time1RadioButton.Checked = True Then
                    parameters.Add("Checkout[best_time]", "1")
                ElseIf best_time4RadioButton.Checked = True Then
                    parameters.Add("Checkout[best_time]", "4")
                ElseIf best_time5RadioButton.Checked = True Then
                    parameters.Add("Checkout[best_time]", "5")
                End If
                parameters.Add("Checkout[invoice_type]", "1")
                parameters.Add("Checkout[invoice_title]", "")
                parameters.Add("Checkout[invoice_company_code]", "")
                parameters.Add("Checkout[email]", "hear.silent1995@gmail.com")
                parameters.Add("Checkout[is_donate]", "0")
                parameters.Add("Checkout[couponsValue]", "0")
                parameters.Add("Checkout[couponsType]", "no")
                parameters.Add("Checkout[address_id]", _addressId)
                parameters.Add("Checkout[submit]", "提交訂單")
                parameters.Add("needCity", "0")
                response = HttpWebResponseUtility.CreatePostHttpResponse(_checkOut, parameters, Nothing, Nothing, Encoding.UTF8, cookies)
                reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                respHTML = reader.ReadToEnd()
                If respHTML Like "*您下单过快,请喝杯咖啡休息一下再来*" Or StrConv(respHTML, VbStrConv.TraditionalChinese, 2052) Like "*頻繁*" Then
                    Log.Text &= "[" & Now & "] 下單過快" & vbCrLf
                    Thread.Sleep(1500)
                    GoTo ReTry
                ElseIf respHTML Like "*系统繁忙*" Then
                    ToolStripStatusLabel.Text = "系統繁忙"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    GoTo ReTrySend
                ElseIf respHTML Like "*非常抱歉*" Or respHTML Like "*您未獲取購買資格*" Then
                    ToolStripStatusLabel.Text = "伺服器壓力過大"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    GoTo ReTrySend
                ElseIf respHTML Like "*没有庫存*" Or respHTML Like "*沒有庫存*" Then
                    ToolStripStatusLabel.Text = "商品沒有庫存了"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    Log.Text &= "[" & Now & "] 很抱歉 , 沒有發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    response.Close()
                    If CartCheck = True Then
                        CartCheck = False
                        Log.Text &= "[" & Now & "] 很抱歉 , 沒有發現野生的行動電源" & vbCrLf
                        response = HttpWebResponseUtility.CreateGetHttpResponse(_deleteItem & ChooseItemID & "_0_buy" + "?ajax=cart-grid", Nothing, Nothing, cookies)
                        reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        If respHTML Like "*{""deleteBatch"":1*" Then
                            Debug.Print("刪除成功")
                        Else
                            Debug.Print("刪除失敗")
                        End If
                        response.Close()
                    End If
                    GoTo ReTry
                End If

                Try
                    startPos = respHTML.IndexOf("var order_id=""", StringComparison.CurrentCultureIgnoreCase) + 14
                    endPos = respHTML.IndexOf(""";", StringComparison.CurrentCultureIgnoreCase)
                    _OrderId = respHTML.Substring(startPos, endPos - startPos)
                    Debug.Print("OrderId : " & _OrderId)
                    CartCheck = False
                Catch ex As Exception
                    Debug.Print("無法取得Order Id")
                    Log.Text &= "[" & Now & "] 很抱歉 , 沒有發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    response.Close()
                    If CartCheck = True Then
                        CartCheck = False
                        Log.Text &= "[" & Now & "] 很抱歉 , 沒有發現野生的行動電源" & vbCrLf
                        response = HttpWebResponseUtility.CreateGetHttpResponse(_deleteItem & ChooseItemID & "_0_buy" + "?ajax=cart-grid", Nothing, Nothing, cookies)
                        reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                        respHTML = reader.ReadToEnd()
                        If respHTML Like "*{""deleteBatch"":1*" Then
                            Debug.Print("刪除成功")
                        Else
                            Debug.Print("刪除失敗")
                        End If
                        response.Close()
                    End If
                    GoTo ReTry
                End Try

                SkuCount += ChooseItemCount
                PinCodeThread = New Thread(AddressOf PinCodeBackground)
                PinCodeThread.Start(_OrderId)
                Thread.Sleep(800)
ReTry:
                If Not _hdurl = Nothing Then
                    GoTo TryGet
                End If
                DisConnectCheck = False
            Catch ex As Exception
                ToolStripStatusLabel.Text = "連線逾時"
                If DisConnectCheck = False Then
                    DisConnectCheck = True
                    Log.Text &= "[" & Now & "] 連線逾時" & vbCrLf
                End If
            End Try
        End While
    End Sub
    Sub AutoFillBackground()
        While (1)
            Try
                If SkuCount >= AmountTrackBar.Value * ChooseItemCount Then
                    Exit Sub
                End If

                Dim dteStart As DateTime = Now
                Dim CartCheck As Boolean = False
                Dim response As HttpWebResponse '= HttpWebResponseUtility.CreateGetHttpResponse(_miniNew, Nothing, Nothing, cookies)
                Dim reader As StreamReader '= New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                Dim respHTML As String '= reader.ReadToEnd()
                Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
                Dim startPos As Integer, endPos As Integer
                Dim _OrderId As String = Nothing

                If CheckShopCart(False) Then : GoTo ReTrySend : End If

                GoTo retry
ReTrySend:
                Dim _addressId As String = getAddressId()
                If _addressId = "-1" Then : _addressId = saveAddress() : End If
                parameters = New Dictionary(Of String, String)()
                parameters.Clear()
                If best_time1RadioButton.Checked = True Then
                    parameters.Add("Checkout[best_time]", "1")
                ElseIf best_time4RadioButton.Checked = True Then
                    parameters.Add("Checkout[best_time]", "4")
                ElseIf best_time5RadioButton.Checked = True Then
                    parameters.Add("Checkout[best_time]", "5")
                End If
                parameters.Add("Checkout[invoice_type]", "1")
                parameters.Add("Checkout[invoice_title]", "")
                parameters.Add("Checkout[invoice_company_code]", "")
                parameters.Add("Checkout[email]", "hear.silent1995@gmail.com")
                parameters.Add("Checkout[is_donate]", "0")
                parameters.Add("Checkout[couponsValue]", "0")
                parameters.Add("Checkout[couponsType]", "no")
                parameters.Add("Checkout[address_id]", _addressId)
                parameters.Add("Checkout[submit]", "提交訂單")
                parameters.Add("needCity", "0")
                response = HttpWebResponseUtility.CreatePostHttpResponse(_checkOut, parameters, Nothing, Nothing, Encoding.UTF8, cookies)
                reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
                respHTML = reader.ReadToEnd()
                If respHTML Like "*您下单过快,请喝杯咖啡休息一下再来*" Then
                    Log.Text &= "[" & Now & "] 下單過快" & vbCrLf
                    GoTo ReTry
                ElseIf respHTML Like "*系统繁忙*" Then
                    ToolStripStatusLabel.Text = "系統繁忙"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    GoTo ReTrySend
                ElseIf respHTML Like "*非常抱歉*" Then
                    ToolStripStatusLabel.Text = "伺服器壓力過大"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    GoTo ReTrySend
                ElseIf respHTML Like "*商品沒有庫存了*" Then
                    ToolStripStatusLabel.Text = "商品沒有庫存了"
                    ToolStripStatusLabel.ForeColor = Color.Orange
                    Log.Text &= "[" & Now & "] 很抱歉 , 沒有發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    GoTo ReTry
                End If

                Try
                    startPos = respHTML.IndexOf("var order_id=""", StringComparison.CurrentCultureIgnoreCase) + 14
                    endPos = respHTML.IndexOf(""";", StringComparison.CurrentCultureIgnoreCase)
                    _OrderId = respHTML.Substring(startPos, endPos - startPos)
                    Debug.Print("OrderId : " & _OrderId)
                    If Not _OrderId.Length = 16 Then
                        Debug.Print("無法取得Order Id")
                        Log.Text &= "[" & Now & "] 很抱歉 , 沒有發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                        GoTo ReTry
                    End If
                    CartCheck = False
                Catch ex As Exception
                    Debug.Print("無法取得Order Id")
                    Log.Text &= "[" & Now & "] 很抱歉 , 沒有發現野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    GoTo ReTry
                End Try

                SkuCount += ChooseItemCount
                PinCodeThread = New Thread(AddressOf PinCodeBackground)
                PinCodeThread.Start(_OrderId)
ReTry:
            Catch ex As Exception

            End Try
        End While
    End Sub
    Sub PinCodeBackground(_OrderId As String)
        Try
            Dim startPos As Integer, endPos As Integer
            Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
            parameters.Clear()
            If FamilyRadioButton.Checked = True Then
                parameters.Add("bank", "familymart")
            ElseIf RadioButton711.Checked = True Then
                parameters.Add("bank", "seven_eleven")
            End If
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreatePostHttpResponse(_confirmOrder & _OrderId & "#", parameters, Nothing, Nothing, Encoding.UTF8, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML As String = reader.ReadToEnd()

            Try
                startPos = respHTML.IndexOf("http://buy.mi.com/tw/buy/qrcode?location=aws&code=", StringComparison.CurrentCultureIgnoreCase) + 50
                endPos = respHTML.IndexOf("&key=", StringComparison.CurrentCultureIgnoreCase)
                Dim _PinCode As String = respHTML.Substring(startPos, endPos - startPos)
                Debug.Print("PinCode : " & _PinCode) '全家 : 14碼 '7-11 : 12碼

                GoogleSheetThread = New Thread(AddressOf GoogleSheetBackground)
                GoogleSheetThread.Start(_PinCode)
                If ChooseItemCount = 1 Then
                    Log.Text &= "[" & Now & "] 搶到一個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    If FamilyRadioButton.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : " & _PinCode & " (請至全家繳費)" & vbCrLf
                    ElseIf RadioButton711.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : " & _PinCode & " (請至7-11繳費)" & vbCrLf
                    End If
                    If Me.WindowState = FormWindowState.Minimized Then
                        MainNotifyIcon.BalloonTipText = "搶到一個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                        MainNotifyIcon.ShowBalloonTip(1000)
                    End If
                    MainNotifyIcon.Text = "Xiaomi Auto Buy (" & User & ") @ " & SkuCount
                Else
                    Log.Text &= "[" & Now & "] 搶到兩個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    If FamilyRadioButton.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : " & _PinCode & " (請至全家繳費)" & vbCrLf
                    ElseIf RadioButton711.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : " & _PinCode & " (請至7-11繳費)" & vbCrLf
                    End If
                    If Me.WindowState = FormWindowState.Minimized Then
                        MainNotifyIcon.BalloonTipText = "搶到兩個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                        MainNotifyIcon.ShowBalloonTip(1000)
                    End If
                    MainNotifyIcon.Text = "Xiaomi Auto Buy (" & User & ") @ " & SkuCount
                End If
                'SkuCount += 2
            Catch ex As Exception
                Debug.Print("系統繁忙請自行去訂單抓PinCode")
                GoogleSheetThread = New Thread(AddressOf GoogleSheetBackground)
                GoogleSheetThread.Start("系統繁忙請自行去訂單抓PinCode")
                If ChooseItemCount = 1 Then
                    Log.Text &= "[" & Now & "] 搶到一個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    If FamilyRadioButton.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : 系統繁忙 (請至全家繳費)" & vbCrLf
                    ElseIf RadioButton711.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : 系統繁忙 (請至7-11繳費)" & vbCrLf
                    End If
                    If Me.WindowState = FormWindowState.Minimized Then
                        MainNotifyIcon.BalloonTipText = "搶到一個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                        MainNotifyIcon.ShowBalloonTip(1000)
                    End If
                    MainNotifyIcon.Text = "Xiaomi Auto Buy (" & User & ") @ " & SkuCount
                Else
                    Log.Text &= "[" & Now & "] 搶到兩個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                    If FamilyRadioButton.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : 系統繁忙 (請至全家繳費)" & vbCrLf
                    ElseIf RadioButton711.Checked = True Then
                        Log.Text &= "[" & Now & "] PinCode : 系統繁忙 (請至7-11繳費)" & vbCrLf
                    End If
                    If Me.WindowState = FormWindowState.Minimized Then
                        MainNotifyIcon.BalloonTipText = "搶到兩個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                        MainNotifyIcon.ShowBalloonTip(1000)
                    End If
                    MainNotifyIcon.Text = "Xiaomi Auto Buy (" & User & ") @ " & SkuCount
                End If
                'SkuCount += 2
            End Try

            If SkuCount >= AmountTrackBar.Value * ChooseItemCount Then
                Log.Text &= "[" & Now & "] 成功搶完" & SkuCount & "個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                ToolStripStatusLabel.Text = "搶購完畢"
                MainNotifyIcon.BalloonTipText = "成功搶完" & SkuCount & "個野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                MainNotifyIcon.ShowBalloonTip(1000)
                ToolStripStatusLabel.ForeColor = Color.DodgerBlue
                Exit Sub
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub Log_TextChanged(sender As Object, e As EventArgs) Handles Log.TextChanged
        Log.SelectionStart = Log.TextLength
        Log.ScrollToCaret()
    End Sub
    Private Sub TabControl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl.SelectedIndexChanged
        If TabControl.SelectedIndex = 1 Or TabControl.SelectedIndex = 0 Then
            MiniNewLabel.Visible = True
            MiniNewPictureBox.Visible = True
        Else
            MiniNewLabel.Visible = False
            MiniNewPictureBox.Visible = False
        End If

        If TabControl.SelectedIndex = 0 Then
            Log.SelectionStart = Log.TextLength
            Log.ScrollToCaret()
        ElseIf TabControl.SelectedIndex = 1 Then
            'Try
            '    OrderThread.Abort()
            'Catch ex As Exception

            'End Try
            OrderThread = New Thread(AddressOf Me.OrderBackground)
            OrderThread.Start()
        End If
    End Sub
    Private Sub DianYuanCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles DianYuanCheckBox.CheckedChanged
        If DianYuanCheckBox.Checked = True Then
            Mode1RadioButton.Enabled = False
            Mode2RadioButton.Enabled = False
            SkuCount = 0
            If Mode1RadioButton.Checked Then
                Log.Text &= "[" & Now & "] 開始收刮野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                AutoThread = New Thread(AddressOf Me.AutoBuyBackground)
                AutoThread.Start()
            ElseIf Mode2RadioButton.Checked Then
                Log.Text &= "[" & Now & "] 開始收刮野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                AutoThread = New Thread(AddressOf Me.AutoBuyBackground)
                AutoThread.Start()
            End If
        Else
            If Mode1RadioButton.Checked Then
                ToolStripStatusLabel.Text = "程式尚未開始自動送單 ..."
                ToolStripStatusLabel.ForeColor = Color.Orange
            Else
                ToolStripStatusLabel.Text = "程式尚未開始自動收刮 ..."
                ToolStripStatusLabel.ForeColor = Color.Orange
            End If

            Mode1RadioButton.Enabled = True
            Mode2RadioButton.Enabled = True
            Try
                AutoThread.Abort()
            Catch ex As Exception

            End Try
        End If
    End Sub
    Dim _hdid As String = "dianyuan5200"
    Private Sub ItemComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ItemComboBox.SelectedIndexChanged
        _hdid = ""
        Select Case ItemComboBox.SelectedIndex
            Case 0 '行動電源 10400mAh
                ChooseItemID = 2130100011
                ChooseItemCount = 2
                _hdid = "dianyuan"
            Case 1 '行動電源 5200mAh
                ChooseItemID = 2141300041
                ChooseItemCount = 2
                _hdid = "dianyuan5200"
            Case 2 '紅米1S
                ChooseItemID = 2142600002
                ChooseItemCount = 1
                _hdid = "hongmi1s"
            Case 3 '紅米1S 白色版
                ChooseItemID = 2143000009
                ChooseItemCount = 1
                _hdid = "hongmi1s"
            Case 4 '紅米Note4G
                ChooseItemID = 4144500005
                ChooseItemCount = 1
                _hdid = "note4g"
            Case 5 '小米手環 石墨黑
                ChooseItemID = 4144500003
                ChooseItemCount = 2
                _hdid = "miband"
            Case 6 '小米路由器mini
                ChooseItemID = 4144700003
                ChooseItemCount = 1
                _hdid = "miwifimini"
            Case Else '行動電源 5200mAh
                ChooseItemID = 2141300041
                ChooseItemCount = 2
                _hdid = "dianyuan5200"
                ItemComboBox.Text = "行動電源5200mAh"
        End Select

        If Mode1RadioButton.Checked Then
            Try
                If DianYuanCheckBox.Enabled = True And DianYuanCheckBox.Checked = True Then
                    ToolStripStatusLabel.Text = "正在檢查" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & " ..."
                    Log.Text &= "[" & Now & "] 開始收刮野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                End If
            Catch ex As Exception

            End Try
            If DianYuanCheckBox.Enabled = True And DianYuanCheckBox.Checked = False Then
                Try
                    ReCheckThread.Abort()
                Catch ex As Exception

                End Try
                ReCheckThread = New Thread(AddressOf Me.ReCheckBackground)
                ReCheckThread.Start()
            End If
        ElseIf Mode2RadioButton.Checked Then
            If DianYuanCheckBox.Checked = True Then
                Log.Text &= "[" & Now & "] 開始收刮野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex) & vbCrLf
                ToolStripStatusLabel.Text = "等待野生的" & ItemComboBox.Items(ItemComboBox.SelectedIndex)
                ToolStripStatusLabel.ForeColor = Color.Orange
            End If
        End If

        AmountLabel.Text = "搶購數量 : (" & AmountTrackBar.Value * ChooseItemCount & "個)"
        Label2.Text = ItemComboBox.Items(ItemComboBox.SelectedIndex) & "是我的。"

        If Me.Visible Then
            Dim g As Graphics = Me.CreateGraphics()
            Dim ds As String = "行動電源5200mAh是我的。"
            Dim strSize As SizeF = g.MeasureString(ds, Me.Font)
            Label1.Location = New Point(278 - ((206 - strSize.Width) - (206 - g.MeasureString(Label2.Text, Me.Font).Width)) / 2, 236)
        End If
    End Sub

    Private Sub Mode1RadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles Mode1RadioButton.CheckedChanged
        If Not Mode1RadioButton.Checked Then
            Exit Sub
        End If
        ToolStripStatusLabel.Text = "程式尚未開始收刮 ..."
        ToolStripStatusLabel.ForeColor = Color.Orange
        If DianYuanCheckBox.Enabled = True And DianYuanCheckBox.Checked = False And Me.Visible = True Then
            Try
                ReCheckThread.Abort()
            Catch ex As Exception

            End Try
            ReCheckThread = New Thread(AddressOf Me.ReCheckBackground)
            ReCheckThread.Start()
        End If
    End Sub
    Private Sub Mode2RadioButton_CheckedChanged(sender As Object, e As EventArgs) Handles Mode2RadioButton.CheckedChanged
        If Not Mode2RadioButton.Checked Then
            Exit Sub
        End If
        ToolStripStatusLabel.Text = "程式尚未開始自動送單 ..."
        ToolStripStatusLabel.ForeColor = Color.Orange
        'If DianYuanCheckBox.Enabled = True And DianYuanCheckBox.Checked = False And Me.Visible = True Then
        '    Try
        '        ReCheckThread.Abort()
        '    Catch ex As Exception

        '    End Try
        '    ReCheckThread = New Thread(AddressOf Me.ReCheckBackground)
        '    ReCheckThread.Start()
        'End If
    End Sub
    Private Sub MainNotifyIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles MainNotifyIcon.MouseDoubleClick
        MainNotifyIcon.Visible = False
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        Log.SelectionStart = Log.TextLength
        Log.ScrollToCaret()
    End Sub
    Private Sub Loginfrm_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            MainNotifyIcon.Icon = Me.Icon
            MainNotifyIcon.Visible = True
            MainNotifyIcon.Text = "Xiaomi Auto Buy (" & User & ")"
            If Mode1RadioButton.Checked Then
                If DianYuanCheckBox.Checked Then
                    MainNotifyIcon.BalloonTipText = ItemComboBox.Items(ItemComboBox.SelectedIndex) & "收刮中"
                Else
                    MainNotifyIcon.BalloonTipText = "程式尚未開始收刮"
                End If
            ElseIf Mode2RadioButton.Checked Then
                If DianYuanCheckBox.Checked = True Then
                    MainNotifyIcon.BalloonTipText = ItemComboBox.Items(ItemComboBox.SelectedIndex) & "自動送單中"
                Else
                    MainNotifyIcon.BalloonTipText = "程式尚未開始自動送單"
                End If
            End If
            MainNotifyIcon.ShowBalloonTip(1000)
            Me.Visible = False
        End If
    End Sub
    Private Sub Address_TextChanged(sender As Object, e As EventArgs) Handles Address.TextChanged
        ZipCodeThread = New Thread(AddressOf Me.ZipCodeBackground)
        ZipCodeThread.Start()
    End Sub
    Sub ZipCodeBackground()
        'Dim newAddress As New SilentAddressClass(Address.Text)
        'If Not newAddress.No = "" Then
        '    Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse("http://zipcode.mosky.tw/api/find?address=" & System.Uri.EscapeDataString(Address.Text), Nothing, Nothing, cookies)
        '    Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
        '    Dim respHTML As String = reader.ReadToEnd()
        '    'Debug.Print(respHTML)
        '    Dim t As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML)
        '    If Len(t.Item("result").ToString) = 5 Then
        '        ZipCode.Text = t.Item("result")
        '    ElseIf Len(t.Item("result").ToString) = 3 Then
        '        ZipCode.Text = t.Item("result").ToString & "00"
        '    End If
        'End If
        Dim newAddress As New SilentAddressClass(Address.Text)

        If (Not newAddress.City = "" Or Not newAddress.Town = "") And newAddress.Road = "" Then
            ZipCode.Text = newAddress.Zip
        End If
        If Not newAddress.No = "" And Address.Text.Contains("號") Then
            ZipCode.Text = newAddress.Zip
            'ZipCodeThread = New Thread(AddressOf ZipCodeBackground)
            'ZipCodeThread.Start()
        End If

        If CityComboBox.Items.Contains(newAddress.City) Then
            For i = 0 To CityComboBox.Items.Count - 1
                If CityComboBox.Items.Item(i) = newAddress.City Then
                    CityComboBox.SelectedIndex = i
                    GoTo District
                End If
            Next
            Exit Sub
District:
            If DistrictComboBox.Items.Contains(newAddress.Town) Then
                For i = 0 To DistrictComboBox.Items.Count - 1
                    If DistrictComboBox.Items.Item(i) = newAddress.Town Then
                        DistrictComboBox.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub TimeCheckBox_CheckedChanged(sender As Object, e As EventArgs) Handles TimeCheckBox.CheckedChanged
        If TimeCheckBox.Checked = True Then
            TimePicker.Enabled = False
            Log.Text &= "[" & Now & "] 搶購時間設定為" & TimePicker.Value.ToString("yyyy/MM/dd HH:mm") & vbCrLf
        Else
            TimePicker.Enabled = True
        End If
    End Sub

    Private Sub 離開程式ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 離開程式ToolStripMenuItem.Click
        SaveSetting()
        Try
            OrderThread.Abort()
        Catch ex As Exception

        End Try
        Try
            AutoThread.Abort()
        Catch ex As Exception

        End Try
        Try
            LoginThread.Abort()
        Catch ex As Exception

        End Try
        Try
            PinCodeThread.Abort()
        Catch ex As Exception

        End Try
        Try
            ZipCodeThread.Abort()
        Catch ex As Exception

        End Try
        End
    End Sub
    Sub GoogleSheetBackground(PinCode As String)
        Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
        parameters.Add("entry.1679830414", System.Uri.EscapeDataString(User))
        parameters.Add("entry.249496265", System.Uri.EscapeDataString(Consignee.Text))
        parameters.Add("entry.1585160036", System.Uri.EscapeDataString(Address.Text))
        parameters.Add("entry.2055704454", System.Uri.EscapeDataString(ItemComboBox.Items(ItemComboBox.SelectedIndex)))
        parameters.Add("entry.707548291", System.Uri.EscapeDataString(PinCode))
        parameters.Add("draftResponse", "[,,""470159741597888403""]")
        parameters.Add("pageHistory", "0")
        parameters.Add("fbzx", "470159741597888403")

        Dim response As HttpWebResponse = HttpWebResponseUtility.CreatePostHttpResponse("https://docs.google.com/forms/d/1QIulMMEKezhMSIbDAaVe1yN2dY4RAHJkQcnP3TBnkJg/formResponse", parameters, Nothing, Nothing, Encoding.UTF8, cookies)
        Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
        Dim respHTML As String = reader.ReadToEnd()
    End Sub
    Function getAddressId()
        Try
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_getAddress, Nothing, Nothing, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim readLine As String = reader.ReadLine
            Dim _addressList As String = Nothing
            Dim _addressId As String = -1
            While (Not reader.EndOfStream)
                If (readLine.Contains("addressList")) Then
                    _addressList = readLine.Split("=")(1).Trim.Substring(0, readLine.Split("=")(1).Trim.Length - 1)
                    Exit While
                End If
                readLine = reader.ReadLine
            End While
            response.Close()
            If _addressList = "" Or _addressList Like "*[];*" Then : Return "-1" : End If
            Dim t As Newtonsoft.Json.Linq.JArray = Newtonsoft.Json.JsonConvert.DeserializeObject(_addressList)
            For i = 0 To t.Count - 1
                If Address.Text = t.Item(i)("address") And Tel.Text = t.Item(i)("tel") And Consignee.Text = t.Item(i)("consignee") Then
                    Return t.Item(i)("address_id")
                End If
            Next
            Return "-1"
        Catch ex As Exception
            Return "-1"
        End Try
    End Function
    Function saveAddress()
        Try
            Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
            parameters.Add("address[consignee]", Consignee.Text)
            parameters.Add("address[city]", TranCity)
            parameters.Add("address[cityStr]", CityComboBox.Items.Item(CityComboBox.SelectedIndex))
            parameters.Add("address[district]", TranRegion)
            parameters.Add("address[districtStr]", DistrictComboBox.Items.Item(DistrictComboBox.SelectedIndex))
            parameters.Add("address[address]", Address.Text)
            parameters.Add("address[tel]", Tel.Text)
            parameters.Add("address[zipcode]", ZipCode.Text)
            parameters.Add("address[address_id]", "")
            If best_time1RadioButton.Checked = True Then
                parameters.Add("address[best_time]", "1")
            ElseIf best_time4RadioButton.Checked = True Then
                parameters.Add("address[best_time]", "4")
            ElseIf best_time5RadioButton.Checked = True Then
                parameters.Add("address[best_time]", "5")
            End If
            parameters.Add("addressId", "")
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreatePostHttpResponse(_saveAddress, parameters, Nothing, Nothing, Encoding.UTF8, cookies)
            Dim reader As StreamReader = New StreamReader(New GZipStream(response.GetResponseStream, CompressionMode.Decompress), Encoding.[Default])
            Dim t As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(reader.ReadToEnd)
            response.Close()
            If t.Item("code") = "0" Then
                Return t.Item("data")("result")
            Else
                Return "-1"
            End If
        Catch ex As Exception
            Return "-1"
        End Try
    End Function
    Public MiniCartFrmCheck As Boolean = False
    Private Sub MiniNew_Click(sender As Object, e As EventArgs) Handles MiniNewLabel.Click, MiniNewPictureBox.Click
        miniCart = New miniCartFrm(cookies)
        miniCart.Show()
        miniCart.Location = New Point(Me.Location.X + (MiniNewLabel.Location.X + MiniNewLabel.Size.Width) - 315, Me.Location.Y + MiniNewPictureBox.Location.Y + 50)
    End Sub
End Class