Imports System
Imports System.IO
Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
'
' 作者: Norman Su
' 採 LGPL 授權, 請勿用於營利用途.
'
Public Class AddressClass
    ' 負責解析地址文字字串的 Class
    Private mProvince As String = ""    ' 省
    Private mCity As String = ""        ' 縣市
    Private mTown As String = ""        ' 鄉鎮市區
    Private mVillage As String = ""     ' 村里
    Private mLin As String = ""         ' 鄰
    Private mRoad As String = ""        ' 路街
    Private mSec As String = ""         ' 段
    Private mLane As String = ""        ' 巷
    Private mAlley As String = ""       ' 弄
    Private mSubAlley As String = ""    ' 衖
    Private mNo As String = ""          ' 號
    Private mSubNo As String = ""       ' 之號
    Private mFloor As String = ""       ' 樓
    Private mSubFloor As String = ""    ' 樓之
    Private mRoom As String = ""        ' 室

    Private mShortAddress
    Private mZip As String     ' 郵遞區號
    Private mAreaTokens As ArrayList = New ArrayList
    Private mAreaTokensAlign As ArrayList = New ArrayList
    Private mDoorTokens As ArrayList = New ArrayList
    Private mDoorTokensAlign As ArrayList = New ArrayList

    Private Enum TokenAlign
        AlignLeft = 0
        AlignRight = 1
    End Enum

    Sub New(ByVal FullAddress As String)

        mAreaTokens.Add("省")
        mAreaTokensAlign.Add(TokenAlign.AlignRight)
        mAreaTokens.Add("縣市")
        mAreaTokensAlign.Add(TokenAlign.AlignRight)
        mAreaTokens.Add("鄉鎮市區")
        mAreaTokensAlign.Add(TokenAlign.AlignRight)
        mAreaTokens.Add("村里")
        mAreaTokensAlign.Add(TokenAlign.AlignRight)
        mAreaTokens.Add("鄰")
        mAreaTokensAlign.Add(TokenAlign.AlignRight)
        mAreaTokens.Add("路街")
        mAreaTokensAlign.Add(TokenAlign.AlignRight)
        mAreaTokens.Add("段")
        mAreaTokensAlign.Add(TokenAlign.AlignRight)

        '
        mDoorTokens.Add("巷")
        mDoorTokensAlign.Add(TokenAlign.AlignRight)
        mDoorTokens.Add("弄")
        mDoorTokensAlign.Add(TokenAlign.AlignRight)
        mDoorTokens.Add("衖")
        mDoorTokensAlign.Add(TokenAlign.AlignRight)
        mDoorTokens.Add("號")
        mDoorTokensAlign.Add(TokenAlign.AlignRight)
        mDoorTokens.Add("之")
        mDoorTokensAlign.Add(TokenAlign.AlignLeft)
        mDoorTokens.Add("樓")
        mDoorTokensAlign.Add(TokenAlign.AlignRight)
        mDoorTokens.Add("之")
        mDoorTokensAlign.Add(TokenAlign.AlignLeft)
        mDoorTokens.Add("室")
        mDoorTokensAlign.Add(TokenAlign.AlignRight)

        Me.ParseAddress(FullAddress)
    End Sub

    Private Sub ParseAddress(ByVal Address As String)
        mShortAddress = ParseLongAddress(Address)
        ParseShortAddress(mShortAddress)
    End Sub

    Private Sub ParseShortAddress(ByVal Address As String)
        Dim sTemp As String = Address.Trim()
        Dim iPos As Integer = 0
        Dim sTokens As String
        Dim sSub As String

        For I As Integer = 0 To mDoorTokens.Count - 1
            sSub = ""
            sTokens = mDoorTokens(I)
            Dim iTemp As Integer
            If I <= 3 Then
                iTemp = TownIndexOf(Address, sTokens, iPos)
            Else
                iTemp = AddressIndexOf(Address, sTokens, iPos)
            End If
            If iTemp <> -1 Then
                If mDoorTokensAlign(I) = TokenAlign.AlignRight Then
                    ' 把 iPos 到 iTemp 的文字取出
                    sSub = Address.Substring(iPos, iTemp - iPos + 1)
                    iPos = iTemp + 1
                Else
                    ' 把 iTemp 之後的文字取出
                    If I = mDoorTokens.Count Then
                        '直接取到最後
                        sSub = Address.Substring(iPos)
                    Else
                        Dim tmpTokens As String = mDoorTokens(I + 1)
                        Dim iTemp2 As Integer

                        iTemp2 = AddressIndexOf(Address, tmpTokens, iTemp + 1)
                        If iTemp2 <> -1 Then
                            ' 取出 iTemp 到 iTemp2 之間的文字
                            sSub = Address.Substring(iTemp, iTemp2 - iTemp)
                            iPos = iTemp2 - 1
                        Else
                            '直接取到最後
                            sSub = Address.Substring(iPos)
                        End If
                    End If
                End If
            End If
            If sSub <> "" Then
                ' 儲存到對應的變數
                StoreDoorVariable(I, sSub)
            End If
        Next
    End Sub

    Private Sub StoreDoorVariable(ByVal iStep As Integer, ByVal sPart As String)
        Dim str As String
        Dim str2 As String
        If mDoorTokensAlign(iStep) = TokenAlign.AlignRight Then
            'str = sPart.Substring(sPart.Length - 1)
            Select Case iStep
                'Case 0
                '    Me.mRoad = sPart
                'Case 1
                '    str = CNumToIntStr(sPart)
                '    Me.mSec = str
                Case 0 ' 巷
                    str = CNumToIntStr(sPart)
                    Me.mLane = str
                Case 1 ' 弄
                    str = CNumToIntStr(sPart)
                    Me.mAlley = str
                Case 2 ' 衖
                    str = CNumToIntStr(sPart)
                    Me.mSubAlley = str
                Case 3 ' 號
                    Dim iTemp As Integer = sPart.IndexOf("之")
                    Dim sPart2 As String
                    If iTemp > 0 Then
                        sPart2 = sPart.Substring(0, iTemp)
                        str = CNumToIntStr(sPart2)
                        sPart2 = sPart.Substring(iTemp)
                        str2 = CNumToIntStr(sPart2)
                        Me.mNo = str & str2
                        'Me.mSubNo = str.Substring(0, str.Length - 1)
                    Else
                        str = CNumToIntStr(sPart)
                        Me.mNo = str
                    End If
                Case 4 ' 之
                    Me.mSubNo = sPart
                Case 5 ' 樓
                    Me.mFloor = sPart
                Case 6 ' 之
                    Me.mSubFloor = sPart
                Case 7 ' 室
                    Me.mRoom = sPart
            End Select
        Else
        End If
    End Sub

    Private Function ParseLongAddress(ByVal Address As String) As String
        Dim sTemp As String = Address.Trim()
        Dim iPos As Integer = 0
        Dim sTokens As String
        Dim sSub As String

        For I As Integer = 0 To mAreaTokens.Count - 1
            sSub = ""
            sTokens = mAreaTokens(I)
            Dim iTemp As Integer
            If I <= 3 Then
                iTemp = TownIndexOf(Address, sTokens, iPos)
            Else
                iTemp = AddressIndexOf(Address, sTokens, iPos)
            End If
            If iTemp <> -1 Then
                If mAreaTokensAlign(I) = TokenAlign.AlignRight Then
                    ' 把 iPos 到 iTemp 的文字取出
                    sSub = Address.Substring(iPos, iTemp - iPos + 1)
                    iPos = iTemp + 1
                Else
                    ' 把 iTemp 之後的文字取出
                    If I = mAreaTokens.Count Then
                        '直接取到最後
                        sSub = Address.Substring(iPos)
                    Else
                        Dim tmpTokens As String = mAreaTokens(I + 1)
                        Dim iTemp2 As Integer

                        iTemp2 = AddressIndexOf(Address, tmpTokens, iTemp + 1)
                        If iTemp2 <> -1 Then
                            ' 取出 iTemp 到 iTemp2 之間的文字
                            sSub = Address.Substring(iTemp, iTemp2 - iTemp)
                            iPos = iTemp2 - 1
                        Else
                            '直接取到最後
                            sSub = Address.Substring(iPos)
                        End If
                    End If
                End If
            End If
            If sSub <> "" Then
                ' 儲存到對應的變數
                StoreAreaVariable(I, sSub)
            End If
        Next
        Return Address.Substring(iPos)
    End Function

    Private Sub StoreAreaVariable(ByVal iStep As Integer, ByVal sPart As String)
        Dim str As String = sPart.Trim
        Dim zip3 As Regex
        Dim zip5 As Regex
        zip3 = New Regex("^[1-9][0-9]{2}", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
        zip5 = New Regex("^[1-9][0-9]{4}", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
        'If mAreaTokensAlign(iStep) = TokenAlign.AlignRight Then
        'str = sPart.Substring(sPart.Length - 1)
        Select Case iStep
            Case 0 ' 省
                If zip5.IsMatch(str) Then
                    ' 有包含5碼郵遞區號
                    Me.mZip = str.Substring(0, 5)
                    Me.mProvince = str.Substring(5).Trim
                ElseIf zip3.IsMatch(str) Then
                    ' 有包含3碼郵遞區號
                    Me.mZip = str.Substring(0, 3)
                    Me.mProvince = str.Substring(3).Trim
                Else
                    Me.mProvince = str
                End If
            Case 1 '縣市
                If zip5.IsMatch(str) Then
                    ' 有包含5碼郵遞區號
                    Me.mZip = str.Substring(0, 5)
                    Me.mCity = str.Substring(5).Trim
                ElseIf zip3.IsMatch(str) Then
                    ' 有包含3碼郵遞區號
                    Me.mZip = str.Substring(0, 3)
                    Me.mCity = str.Substring(3).Trim
                Else
                    Me.mCity = str
                End If
            Case 2 '鄉鎮市區
                If zip5.IsMatch(str) Then
                    ' 有包含5碼郵遞區號
                    Me.mZip = str.Substring(0, 5)
                    Me.mTown = str.Substring(5).Trim
                ElseIf zip3.IsMatch(str) Then
                    ' 有包含3碼郵遞區號
                    Me.mZip = str.Substring(0, 3)
                    Me.mTown = str.Substring(3).Trim
                Else
                    Me.mTown = str
                End If
            Case 3 '村里
                If zip5.IsMatch(str) Then
                    ' 有包含5碼郵遞區號
                    Me.mZip = str.Substring(0, 5)
                    Me.mVillage = str.Substring(5).Trim
                ElseIf zip3.IsMatch(str) Then
                    ' 有包含3碼郵遞區號
                    Me.mZip = str.Substring(0, 3)
                    Me.mVillage = str.Substring(3).Trim
                Else
                    Me.mVillage = str
                End If
                'Dim regexp As Regex = New Regex("", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
                Dim iPos1, iPos2 As Integer
                iPos1 = Me.mVillage.IndexOf("路")
                If iPos1 = -1 Then iPos1 = Me.mVillage.IndexOf("街")
                If iPos1 > 0 Then
                    Me.mRoad = Me.mVillage.Substring(0, iPos1 + 1)
                    iPos2 = Me.mVillage.IndexOf("段")
                    If iPos2 > 0 Then
                        Me.mSec = CNumToIntStr(Me.mVillage.Substring(iPos1 + 1, iPos2 - iPos1))
                        Me.mVillage = Me.mVillage.Substring(iPos2 + 1)
                    Else
                        Me.mVillage = Me.mVillage.Substring(iPos1 + 1)
                    End If
                End If
                If zip5.IsMatch(Me.mVillage) Then
                    ' 有包含5碼郵遞區號
                    Me.mZip = Me.mVillage.Substring(0, 5)
                    Me.mVillage = Me.mVillage.Substring(5).Trim
                ElseIf zip3.IsMatch(Me.mVillage) Then
                    ' 有包含3碼郵遞區號
                    Me.mZip = Me.mVillage.Substring(0, 3)
                    Me.mVillage = Me.mVillage.Substring(3).Trim
                End If
            Case 4 '鄰
                str = CNumToIntStr(sPart)
                Me.mLin = str
            Case 5 ' 路街
                If zip5.IsMatch(str) Then
                    ' 有包含5碼郵遞區號
                    Me.mZip = str.Substring(0, 5)
                    Me.mTown = str.Substring(5).Trim
                ElseIf zip3.IsMatch(str) Then
                    ' 有包含3碼郵遞區號
                    Me.mZip = str.Substring(0, 3)
                    Me.mRoad = str.Substring(3).Trim
                Else
                    Me.mRoad = str
                End If
            Case 6 ' 段
                str = CNumToIntStr(sPart)
                Me.mSec = str
        End Select
    End Sub

    ' 從 sComp 逐一取出字元, 找出在 sSource 中是否有相符
    Private Function AddressIndexOf(ByVal sSource As String, ByVal sComp As String, Optional ByVal iStart As Integer = 0) As Integer
        Dim iPos As Integer
        Try
            For I As Integer = 0 To sComp.Length - 1
                iPos = sSource.IndexOf(sComp(I), iStart)
                If iPos <> -1 Then
                    Return iPos
                End If
            Next
            Return -1
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Private Function TownIndexOf(ByVal sSource As String, ByVal sComp As String, Optional ByVal iStart As Integer = 0) As Integer
        Dim iPos As Integer
        Try
            For I As Integer = 0 To sComp.Length - 1
                iPos = sSource.IndexOf(sComp(I), iStart)
                If iPos <> -1 Then
                    If iPos - iStart < 4 Then
                        Dim ch As Char = sSource(iPos + 1)
                        If sComp.IndexOf(ch) <> -1 Then
                            Return iPos + 1
                        Else
                            Return iPos
                        End If
                    Else
                        Return iPos
                    End If
                End If
            Next
            Return -1
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Private Function CDigitToNum(ByVal cDigit As Char) As Integer
        Dim cBigNum() As Char = {"○", "一", "二", "三", "四", "五", "六", "七", "八", "九"}
        Dim J As Integer
        For J = 0 To cBigNum.Length - 1
            If cBigNum(J) = cDigit Then Exit For
        Next
        If J = cBigNum.Length Then
            Return -1
        Else
            Return J
        End If
    End Function

    Private Function CNumToIntStr(ByVal sSource As String) As String
        Dim sResult As String = ""
        'Dim sBigNum() As String = {"○", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十"}
        'Dim sBigNum2() As String = {"○", "一", "二", "三", "四", "五", "六", "七", "八", "九"}
        Dim I As Integer = 0
        Dim Num As Integer = 0
        Dim bNum As Boolean = False

        While I < sSource.Length
            Dim J As Integer
            J = CDigitToNum(sSource(I))
            If J >= 0 Then
                bNum = True
                If I < sSource.Length - 1 Then
                    If sSource(I + 1) = "十" Then
                        Num = J * 10
                        I += 2
                        GoTo Cont
                    End If
                End If
                If I > 0 Then
                    If sSource(I - 1) = "十" Then
                        Num += J
                    Else
                        Num = Num * 10 + J
                    End If
                    I += 1
                Else
                    Num = J
                    I += 1
                End If
            Else
                If sSource(I) = "十" Then
                    bNum = True
                    If Num = 0 Then
                        Num = 10
                    Else
                        Num *= 10
                    End If
                Else
                    If bNum = True Then
                        sResult += Num.ToString & sSource.Substring(I)
                        bNum = False
                        Exit While
                    Else
                        sResult += sSource(I)
                    End If
                End If
                I += 1
            End If
Cont:
        End While
        If bNum = True Then
            sResult = Num.ToString
        End If
        Return sResult
    End Function

    Public Property Zip() As String
        Get
            Return mZip
        End Get
        Set(ByVal value As String)
            mZip = value
        End Set
    End Property

    Public Property Province() As String
        Get
            Return mProvince
        End Get
        Set(ByVal value As String)
            mProvince = value
        End Set
    End Property

    Public Property City() As String
        Get
            Return mCity
        End Get
        Set(ByVal value As String)
            mCity = value
        End Set
    End Property

    Public Property Town() As String
        Get
            Return Me.mTown
        End Get
        Set(ByVal value As String)
            Me.mTown = value
        End Set
    End Property

    Public Property Village() As String
        Get
            Return Me.mVillage
        End Get
        Set(ByVal value As String)
            Me.mVillage = value
        End Set
    End Property

    Public Property Lin() As String
        Get
            Return Me.mLin
        End Get
        Set(ByVal value As String)
            mLin = value
        End Set
    End Property

    Public Property Road() As String
        Get
            Return Me.mRoad
        End Get
        Set(ByVal value As String)
            mRoad = value
        End Set
    End Property

    Public Property Sec() As String
        Get
            Return Me.mSec
        End Get
        Set(ByVal value As String)
            mSec = value
        End Set
    End Property

    Public Property Lane() As String
        Get
            Return Me.mLane
        End Get
        Set(ByVal value As String)
            mLane = value
        End Set
    End Property

    Public Property Alley() As String
        Get
            Return Me.mAlley
        End Get
        Set(ByVal value As String)
            mAlley = value
        End Set
    End Property

    Public Property SubAlley() As String
        Get
            Return Me.mSubAlley
        End Get
        Set(ByVal value As String)
            mSubAlley = value
        End Set
    End Property

    Public Property No() As String
        Get
            Return Me.mNo
        End Get
        Set(ByVal value As String)
            mNo = value
        End Set
    End Property

    Public Property SubNo() As String
        Get
            Return Me.mSubNo
        End Get
        Set(ByVal value As String)
            mSubNo = value
        End Set
    End Property

    Public Property Floor() As String
        Get
            Return Me.mFloor
        End Get
        Set(ByVal value As String)
            mFloor = value
        End Set
    End Property

    Public Property SubFloor() As String
        Get
            Return Me.mSubFloor
        End Get
        Set(ByVal value As String)
            mSubFloor = value
        End Set
    End Property

    Public Property Room() As String
        Get
            Return Me.mRoom
        End Get
        Set(ByVal value As String)
            mRoom = value
        End Set
    End Property
End Class