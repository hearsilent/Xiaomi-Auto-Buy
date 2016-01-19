Imports System.Text.RegularExpressions
Imports System.IO
Imports System.Xml

'''
''' 作者: Silent
''' 日期: 2014/8/24
''' 採 LGPL 授權, 請勿用於營利用途.
'''
''' ZipCode3+2 XML : http://www.post.gov.tw/post/internet/Download/all_list.jsp?ID=2201#dl_txt_s_A0206
''' Json.Net : https://json.codeplex.com/releases
'''

Public Class SilentAddressClass
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
    Private mZip As String = ""         ' 郵遞區號
    Private mFullAddress As String = "" ' 地址
    Private mZipPath As String = Application.StartupPath & "\Zip.txt"
    Dim mLastPos As Integer = 0

    Sub New(ByVal FullAddress As String)
        FullAddress = FullAddress.Replace("臺", "台")
        FullAddress = FullAddress.Replace("-", "之")
        FullAddress = FullAddress.Replace("~", "之")
        FullAddress = FullAddress.Replace(" ", "")
        mFullAddress = FullAddress
        Dim zip3 As Regex = New Regex("^[1-9][0-9]{2}", RegexOptions.Singleline Or RegexOptions.IgnoreCase)
        Dim zip5 As Regex = New Regex("^[1-9][0-9]{4}", RegexOptions.Singleline Or RegexOptions.IgnoreCase)

        If zip5.IsMatch(FullAddress) Then
            mZip = FullAddress.Substring(0, 5)
            mFullAddress = FullAddress.Substring(5)
        ElseIf zip3.IsMatch(FullAddress) Then
            mZip = FullAddress.Substring(0, 3) + "00"
            mFullAddress = FullAddress.Substring(3)
        Else
            mZip = ""
        End If

        mProvince = SplitAddress(FullAddress, "省")
        mCity = SplitAddress(FullAddress, "縣市")
        If FullAddress.Contains("縣") Then
            mTown = SplitAddress(FullAddress, "鄉鎮市區台島")
        ElseIf FullAddress.Contains("台") Then
            mTown = SplitAddress(FullAddress, "鄉鎮區島")
        Else
            mTown = SplitAddress(FullAddress, "鄉鎮區台島")
        End If
        mVillage = SplitAddress(FullAddress, "村里")
        mLin = CNumToIntStr(SplitAddress(FullAddress, "鄰"))
        mRoad = SplitAddress(FullAddress, "路街")
        mSec = CNumToIntStr(SplitAddress(FullAddress, "段"))
        mLane = CNumToIntStr(SplitAddress(FullAddress, "巷"))
        mAlley = CNumToIntStr(SplitAddress(FullAddress, "弄"))
        mSubAlley = CNumToIntStr(SplitAddress(FullAddress, "衖"))
        mNo = CNumToIntStr(SplitAddress(FullAddress, "號"))
        mFloor = CNumToIntStr(SplitAddress(FullAddress, "樓Ff"))
        mRoom = CNumToIntStr(SplitAddress(FullAddress, "室"))
        'ZipCodeThread = New Thread(AddressOf GetZipCode)
        'ZipCodeThread.Start(FullAddress)
        Try
            If mRoad = "" And FullAddress.Contains("號") Then
                Dim MaxLen As Integer = 0
                Dim myFile As FileStream = File.Open(mZipPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                Dim myReader As New StreamReader(myFile)
                Dim Line As String = myReader.ReadLine
                While (Not Line = Nothing)
                    If Line.Split(",")(1).Contains(mCity) And Line.Split(",")(2).Contains(mTown) And Len(Line.Split(",")(3)) > MaxLen Then
                        If FullAddress.Substring(FullAddress.IndexOf(mTown.Substring(Len(mTown) - 1)) + 1).Contains(Line.Split(",")(3)) = True Then
                            mRoad = Line.Split(",")(3)
                            MaxLen = Len(mRoad)
                        End If
                    End If
                    Line = myReader.ReadLine
                End While
                If mRoad = "" Then
                    Exit Sub
                End If
                mSec = mSec.Replace(mRoad, "")
                mLane = mLane.Replace(mRoad, "")
                mAlley = mAlley.Replace(mRoad, "")
                mSubAlley = mSubAlley.Replace(mRoad, "")
                mNo = mNo.Replace(mRoad, "")
                myReader.Close()
                myFile.Close()
            End If
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' 將ZipCode 3+2 XML檔轉換為程式用TXT檔 (http://www.post.gov.tw/post/internet/Download/all_list.jsp?ID=2201#dl_txt_s_A0206)
    ''' <para>使用本函式需要將Json.Net加入參考 (https://json.codeplex.com/releases)</para>
    ''' </summary>
    ''' <param name="SourceFile">來源檔案完整路徑 (*.xml)</param>
    ''' <param name="OutputFile">輸出檔案完整路徑 (*.txt)</param>
    ''' <example>
    ''' <code>
    ''' Xml2ZipTxt("C:\Zip32_10307.xml", "C:\Zip.txt")
    ''' </code>
    ''' </example>
    Public Sub Xml2ZipTxt(SourceFile As String, OutputFile As String)
        Dim myFile As FileStream = File.Open(SourceFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
        Dim myReader As New StreamReader(myFile)
        Dim doc As New XmlDocument()
        doc.LoadXml(myReader.ReadToEnd)

        Dim jsonText As String = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc)
        Dim jsonDoc As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonText)

        Dim swWriter As StreamWriter = New StreamWriter(OutputFile, False, System.Text.Encoding.UTF8)
        swWriter.WriteLine("郵遞區號,縣市名稱,鄉鎮市區,原始路名,投遞範圍")
        Try
            For i = 0 To jsonDoc.Item("NewDataSet")("Zip32").Count - 1
                swWriter.WriteLine(jsonDoc.Item("NewDataSet")("Zip32")(i)("Zip5").ToString & "," & jsonDoc.Item("NewDataSet")("Zip32")(i)("City").ToString & "," & jsonDoc.Item("NewDataSet")("Zip32")(i)("Area").ToString & "," & jsonDoc.Item("NewDataSet")("Zip32")(i)("Road").ToString & "," & jsonDoc.Item("NewDataSet")("Zip32")(i)("Scope").ToString)
            Next
            swWriter.Close()
        Catch ex As Exception

        End Try
    End Sub
    Private Function GetZipCode(Address As String) As String
        If Not mZip = "" Then : Return mZip : End If

        Dim dteStart As DateTime = Now
        Try
            If mCity = "" Then
                Return ""
            End If
            Dim myFile As FileStream = File.Open(mZipPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
            Dim myReader As New StreamReader(myFile)
            Dim Line As String = myReader.ReadLine
            Dim Zip32 As String = ""
            While (Not Line = Nothing)
                If Line.Split(",")(1).Contains(mCity) And Line.Split(",")(2).Contains(mTown) And Line.Split(",")(3).Contains(mRoad & StrConv(mSec, VbStrConv.Wide)) Then
                    'Debug.Print(Line)
                    'Debug.Print(Val(Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString))
                    'Debug.Print(Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString))
                    If (mRoad = "" And mTown = "") Or Not mFullAddress.Contains("號") Then
                        Zip32 = Line.Split(",")(0).Substring(0, 3) & "00"
                        Exit While
                    End If
                    Line = Line.Replace("連", "")
                    If Line.Split(",")(Line.Split(",").Length - 1).Contains("全") Then
                        If Line.Split(",")(Line.Split(",").Length - 1).Contains("巷") Then
                            Dim Lane As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                            If Line.Split(",")(Line.Split(",").Length - 1).Contains("單") And Val(Regex.Match(mLane, "[0-9]+").ToString) = Lane Then
                                Zip32 = Line.Split(",")(0)
                                mTown = Line.Split(",")(2)
                                Exit While
                            ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") And Val(Regex.Match(mLane, "[0-9]+").ToString) = Lane Then
                                Zip32 = Line.Split(",")(0)
                                mTown = Line.Split(",")(2)
                                Exit While
                            ElseIf Val(Regex.Match(mLane, "[0-9]+").ToString) = Lane Then
                                Zip32 = Line.Split(",")(0)
                                mTown = Line.Split(",")(2)
                                Exit While
                            End If
                        ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("附號") Then
                            If mNo.Contains("之") Then
                                If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) = Val(Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString) Then
                                    Zip32 = Line.Split(",")(0)
                                    mTown = Line.Split(",")(2)
                                    Exit While
                                End If
                            End If
                        Else
                            If Line.Split(",")(Line.Split(",").Length - 1).Contains("單") And Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString Mod 2 = 1 Then
                                Zip32 = Line.Split(",")(0)
                                mTown = Line.Split(",")(2)
                                Exit While
                            ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") And Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString Mod 2 = 0 Then
                                Zip32 = Line.Split(",")(0)
                                mTown = Line.Split(",")(2)
                                Exit While
                            Else
                                Zip32 = Line.Split(",")(0)
                                mTown = Line.Split(",")(2)
                                Exit While
                            End If
                        End If
                    Else
                        If mNo.Contains("之") Then
                            If Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString Mod 2 = 1 Then
                                If Not Line.Split(",")(Line.Split(",").Length - 1).Contains("單") And Not Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") Then
                                    GoTo ReFind1
                                End If
                                If Line.Split(",")(Line.Split(",").Length - 1).Contains("單") Then
ReFind1:
                                    If Line.Split(",")(Line.Split(",").Length - 1).Contains("以上") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("以下") Then
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) <= Max Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("至") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(0), "[0-9]+").ToString
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) <= Max And Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    End If
                                End If
                                If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) = Val(Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString) Then
                                    Zip32 = Line.Split(",")(0)
                                    mTown = Line.Split(",")(2)
                                    Exit While
                                End If
                            ElseIf Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString Mod 2 = 0 Then
                                If Not Line.Split(",")(Line.Split(",").Length - 1).Contains("單") And Not Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") Then
                                    GoTo ReFind2
                                End If
                                If Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") Then
ReFind2:
                                    If Line.Split(",")(Line.Split(",").Length - 1).Contains("以上") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("以下") Then
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) <= Max Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("至") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(0), "[0-9]+").ToString
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) <= Max And Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    End If
                                End If
                                If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) = Val(Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString) Then
                                    Zip32 = Line.Split(",")(0)
                                    mTown = Line.Split(",")(2)
                                    Exit While
                                End If
                            End If
                        Else
                            If Regex.Match(mNo, "[0-9]+").ToString Mod 2 = 1 Then
                                If Not Line.Split(",")(Line.Split(",").Length - 1).Contains("單") And Not Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") Then
                                    GoTo ReFind3
                                End If
                                If Line.Split(",")(Line.Split(",").Length - 1).Contains("單") Then
ReFind3:
                                    If Line.Split(",")(Line.Split(",").Length - 1).Contains("以上") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo, "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("以下") Then
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo, "[0-9]+").ToString) <= Max Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("至") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(0), "[0-9]+").ToString
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo, "[0-9]+").ToString) <= Max And Val(Regex.Match(mNo, "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    End If
                                End If
                                If Val(Regex.Match(mNo, "[0-9]+").ToString) = Val(Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString) Then
                                    Zip32 = Line.Split(",")(0)
                                    mTown = Line.Split(",")(2)
                                    Exit While
                                End If
                            ElseIf Regex.Match(mNo, "[0-9]+").ToString Mod 2 = 0 Then
                                If Not Line.Split(",")(Line.Split(",").Length - 1).Contains("單") And Not Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") Then
                                    GoTo ReFind4
                                End If
                                If Line.Split(",")(Line.Split(",").Length - 1).Contains("雙") Then
ReFind4:
                                    If Line.Split(",")(Line.Split(",").Length - 1).Contains("以上") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo, "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("以下") Then
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo, "[0-9]+").ToString) <= Max Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    ElseIf Line.Split(",")(Line.Split(",").Length - 1).Contains("至") Then
                                        Dim Min As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(0), "[0-9]+").ToString
                                        Dim Max As Integer = Regex.Match(Line.Split(",")(Line.Split(",").Length - 1).Split("至")(1), "[0-9]+").ToString
                                        If Val(Regex.Match(mNo, "[0-9]+").ToString) <= Max And Val(Regex.Match(mNo, "[0-9]+").ToString) >= Min Then
                                            Zip32 = Line.Split(",")(0)
                                            mTown = Line.Split(",")(2)
                                            Exit While
                                        End If
                                    End If
                                End If
                                If Val(Regex.Match(mNo, "[0-9]+").ToString) = Val(Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString) Then
                                    Zip32 = Line.Split(",")(0)
                                    mTown = Line.Split(",")(2)
                                    Exit While
                                End If
                            End If
                        End If
                    End If
                End If
                Line = myReader.ReadLine
            End While
            Line = myReader.ReadLine
            If Line.Split(",")(Line.Split(",").Length - 1).Contains("附號") Then
                If mNo.Contains("之") Then
                    If Val(Regex.Match(mNo.Split("之")(0), "[0-9]+").ToString) = Val(Regex.Match(Line.Split(",")(Line.Split(",").Length - 1), "[0-9]+").ToString) Then
                        Zip32 = Line.Split(",")(0)
                        mTown = Line.Split(",")(2)
                    End If
                End If
            End If
            myReader.Close()
            myFile.Close()
            Dim TS As TimeSpan = Now.Subtract(dteStart)
            Debug.Print("執行時間: " & TS.TotalMilliseconds & " 毫秒")
            Return Zip32
        Catch ex As Exception
            Return ""
        End Try
    End Function
    Private Function SplitAddress(Address As String, Kind As String) As String
        Dim NewString As String = ""
        If Not Address.IndexOfAny(Kind, mLastPos) = -1 Then
            NewString = Address.Substring(mLastPos, Address.IndexOfAny(Kind) - mLastPos + 1)
            mLastPos = Address.IndexOfAny(Kind) + 1
        End If
        Return NewString
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

        If sSource = "" Then
            Return ""
        End If

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
    ''' <summary>
    ''' 省
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Province() As String
        Get
            Return mProvince
        End Get
        Set(ByVal value As String)
            mProvince = value
        End Set
    End Property
    ''' <summary>
    ''' 縣市
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property City() As String
        Get
            Return mCity
        End Get
        Set(ByVal value As String)
            mCity = value
        End Set
    End Property
    ''' <summary>
    ''' 鄉鎮市區
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Town() As String
        Get
            Return Me.mTown
        End Get
        Set(ByVal value As String)
            Me.mTown = value
        End Set
    End Property
    ''' <summary>
    ''' 村里
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Village() As String
        Get
            Return Me.mVillage
        End Get
        Set(ByVal value As String)
            Me.mVillage = value
        End Set
    End Property
    ''' <summary>
    ''' 鄰
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Lin() As String
        Get
            Return Me.mLin
        End Get
        Set(ByVal value As String)
            mLin = value
        End Set
    End Property
    ''' <summary>
    ''' 路街
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Road() As String
        Get
            Return Me.mRoad
        End Get
        Set(ByVal value As String)
            mRoad = value
        End Set
    End Property
    ''' <summary>
    ''' 段
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Sec() As String
        Get
            Return Me.mSec
        End Get
        Set(ByVal value As String)
            mSec = value
        End Set
    End Property
    ''' <summary>
    ''' 巷
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Lane() As String
        Get
            Return Me.mLane
        End Get
        Set(ByVal value As String)
            mLane = value
        End Set
    End Property
    ''' <summary>
    ''' 弄
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Alley() As String
        Get
            Return Me.mAlley
        End Get
        Set(ByVal value As String)
            mAlley = value
        End Set
    End Property
    ''' <summary>
    ''' 衖
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubAlley() As String
        Get
            Return Me.mSubAlley
        End Get
        Set(ByVal value As String)
            mSubAlley = value
        End Set
    End Property
    ''' <summary>
    ''' 號
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property No() As String
        Get
            Return Me.mNo
        End Get
        Set(ByVal value As String)
            mNo = value
        End Set
    End Property
    ' ''' <summary>
    ' ''' 之號
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property SubNo() As String
    '    Get
    '        Return Me.mSubNo
    '    End Get
    '    Set(ByVal value As String)
    '        mSubNo = value
    '    End Set
    'End Property
    ''' <summary>
    ''' 樓
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Floor() As String
        Get
            Return Me.mFloor
        End Get
        Set(ByVal value As String)
            mFloor = value
        End Set
    End Property
    ' ''' <summary>
    ' ''' 樓之
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property SubFloor() As String
    '    Get
    '        Return Me.mSubFloor
    '    End Get
    '    Set(ByVal value As String)
    '        mSubFloor = value
    '    End Set
    'End Property
    ''' <summary>
    ''' 室
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Room() As String
        Get
            Return Me.mRoom
        End Get
        Set(ByVal value As String)
            mRoom = value
        End Set
    End Property
    ''' <summary>
    ''' 郵遞區號
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Zip() As String
        Get
            mZip = GetZipCode(mFullAddress)
            Return Me.mZip
        End Get
    End Property
    ''' <summary>
    ''' 設定/取得郵遞區號資料來源 (預設 : *\Zip.txt)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ZipPath() As String
        Get
            Return Me.mZipPath
        End Get
        Set(ByVal value As String)
            mZipPath = value
        End Set
    End Property
End Class
