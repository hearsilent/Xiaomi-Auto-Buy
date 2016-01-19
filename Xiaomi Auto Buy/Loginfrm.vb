Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.IO
Imports System.IO.Compression
Imports System.Text.RegularExpressions
Imports Xiaomi_Auto_Buy.SilentWebModule
Imports System.Security.Cryptography
Imports System.Security
Imports System.Xml
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Threading
Public Class Loginfrm
    Dim LoginThread As Thread
    Dim DianYuanThread As Thread
    Public XmlPath As String = "Configs.xml"
    Public cookies As New CookieContainer()
    Public Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
                            (ByVal hwnd As IntPtr, _
                             ByVal wMsg As Integer, _
                             ByVal wParam As IntPtr, _
                             ByVal lParam As Byte()) _
                             As Integer
    Public Const EM_SETCUEBANNER As Integer = &H1501

    Dim _buyUrl As String = "http://buy.mi.com/tw"
    Dim _dmSite As String = "http://sgp01.tp.hd.mi.com"
    Dim _home As String = "http://www.mi.com/tw/"
    Dim _loginUrl As String = "http://buy.mi.com/tw/site/login"
    Dim _timestampUrl As String = "http://hd.global.mi.com/gettimestamp"
    Dim _hdgetUrl As String = _dmSite + "/hdget/tw?source=bigtap&product="
    Dim _hdinfoUrl As String = _dmSite + "/hdinfo/tw"
    Dim _modeUrl As String = _dmSite + "/getmode/tw/?product="
    Dim _shopCartUrl As String = _buyUrl + "/cart/add/"
    Public Shared Function Encrypt(ByVal pToEncrypt As String, ByVal sKey As String) As String
        Dim des As New DESCryptoServiceProvider()
        Dim inputByteArray() As Byte
        inputByteArray = Encoding.Default.GetBytes(pToEncrypt)
        '建立加密對象的密鑰和偏移量
        '原文使用ASCIIEncoding.ASCII方法的GetBytes方法
        '使得輸入密碼必須輸入英文文本
        des.Key = ASCIIEncoding.ASCII.GetBytes(sKey)
        des.IV = ASCIIEncoding.ASCII.GetBytes(sKey)
        '寫二進制數組到加密流
        '(把內存流中的內容全部寫入)
        Dim ms As New System.IO.MemoryStream()
        Dim cs As New CryptoStream(ms, des.CreateEncryptor, CryptoStreamMode.Write)
        '寫二進制數組到加密流
        '(把內存流中的內容全部寫入)
        cs.Write(inputByteArray, 0, inputByteArray.Length)
        cs.FlushFinalBlock()

        '建立輸出字符串     
        Dim ret As New StringBuilder()
        Dim b As Byte
        For Each b In ms.ToArray()
            ret.AppendFormat("{0:X2}", b)
        Next

        Return ret.ToString()
    End Function

    '解密方法
    Public Shared Function Decrypt(ByVal pToDecrypt As String, ByVal sKey As String) As String
        Dim des As New DESCryptoServiceProvider()
        '把字符串放入byte數組
        Dim len As Integer
        len = pToDecrypt.Length / 2 - 1
        Dim inputByteArray(len) As Byte
        Dim x, i As Integer
        For x = 0 To len
            i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16)
            inputByteArray(x) = CType(i, Byte)
        Next
        '建立加密對象的密鑰和偏移量，此值重要，不能修改
        des.Key = ASCIIEncoding.ASCII.GetBytes(sKey)
        des.IV = ASCIIEncoding.ASCII.GetBytes(sKey)
        Dim ms As New System.IO.MemoryStream()
        Dim cs As New CryptoStream(ms, des.CreateDecryptor, CryptoStreamMode.Write)
        cs.Write(inputByteArray, 0, inputByteArray.Length)
        cs.FlushFinalBlock()
        Return Encoding.Default.GetString(ms.ToArray)

    End Function
    Dim Course As Boolean
    Dim CourseUsername, CourseAccount, CoursePassword As String
    Dim FinishLogin As Boolean = False
    Dim _userId As String
    Private Sub StatusBtn_Click(sender As Object, e As EventArgs) Handles LoginBtn.Click
        'LoginBackground()
        LoginThread = New Thread(AddressOf Me.LoginBackground)
        LoginThread.Start()
        Timer.Enabled = True
    End Sub
    Sub LoginBackground()
        Try
            Dim dteStart As DateTime = Now

            User.Enabled = False
            Pwd.Enabled = False
            LoginBtn.Enabled = False

            Dim userName As String = User.Text
            Dim password As String = Pwd.Text
            'userName = Replace(userName, "+", "%2B")
            CourseAccount = userName
            CoursePassword = password

            'Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
            'parameters.Add("user", userName)
            'parameters.Add("_json", "true")
            ''parameters.Add("hidden", "")
            'parameters.Add("pwd", password)
            'parameters.Add("sid", "passport")
            'parameters.Add("_sign", "KKkRvCpZoDC+gLdeyOsdMhwV0Xg=")
            'parameters.Add("callback", "https://account.xiaomi.com")
            'parameters.Add("qs", "%3Fsid%3Dpassport")
            'parameters.Add("auto", "true")
            ''parameters.Add("pwd", password)
            ''parameters.Add("sid", "mi_xiaomitw")
            ''parameters.Add("_sign", "wX4Za84ZYMcOFfoUewmcD1Z21MY=")
            ''parameters.Add("callback", "https://account.xiaomi.com")
            ''parameters.Add("qs", "%3Fsid%3Dpassport")
            ''parameters.Add("auto", "true")

            'Dim response As HttpWebResponse = HttpWebResponseUtility.CreatePostHttpResponse("https://account.xiaomi.com/pass/serviceLoginAuth", parameters, Nothing, Nothing, Encoding.UTF8, cookies)
            'Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            'Dim respHTML As String = reader.ReadToEnd()
            ''Debug.Print(respHTML)

            'If respHTML Like "*登录验证失败*" Then
            '    MsgBox("帳號或密碼輸入錯誤 !", MsgBoxStyle.Critical)
            '    User.Enabled = True
            '    Pwd.Enabled = True
            '    LoginBtn.Enabled = True
            '    Exit Sub
            'End If

            Dim parameters As IDictionary(Of String, String) = New Dictionary(Of String, String)()
            parameters.Add("user", System.Uri.EscapeDataString(User.Text))
            parameters.Add("_json", "true")
            parameters.Add("pwd", System.Uri.EscapeDataString(Pwd.Text))
            parameters.Add("callback", System.Uri.EscapeDataString("http://buy.mi.com/tw/login/callback?followup=http%3A%2F%2Fwww.mi.com%2Ftw%2Findex.html&sign=NzExNzllMGQ1ZjdkZDU3ZjE5YzAxZDAyYzBkMGNkMjVlNjQ2ZTBmYg,,"))
            parameters.Add("sid", System.Uri.EscapeDataString("mi_xiaomitw"))
            parameters.Add("qs", System.Uri.EscapeDataString("%3Fcallback%3Dhttp%253A%252F%252Fbuy.mi.com%252Ftw%252Flogin%252Fcallback%253Ffollowup%253Dhttp%25253A%25252F%25252Fwww.mi.com%25252Ftw%25252Findex.html%2526sign%253DNzExNzllMGQ1ZjdkZDU3ZjE5YzAxZDAyYzBkMGNkMjVlNjQ2ZTBmYg%252C%252C%26sid%3Dmi_xiaomitw%26_locale%3Dzh_TW"))
            parameters.Add("hidden", "")
            parameters.Add("_sign", System.Uri.EscapeDataString("fnp8MuchrpbgkFiK5CZgYQjflTA="))
            parameters.Add("serviceParam", System.Uri.EscapeDataString("{""checkSafePhone"":false}"))
            parameters.Add("captCode", "")
            parameters.Add("auto", "true")

            Dim response As HttpWebResponse = HttpWebResponseUtility.CreatePostHttpResponse("https://account.xiaomi.com/pass/serviceLoginAuth2", parameters, Nothing, Nothing, Encoding.UTF8, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML As String = reader.ReadToEnd()
            Dim t As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML.Replace("&&&START&&&", ""))
            If Not t.Item("desc").ToString = "成功" Then
                MsgBox("帳號或密碼輸入錯誤 !", MsgBoxStyle.Critical)
                User.Enabled = True
                Pwd.Enabled = True
                LoginBtn.Enabled = True
                Exit Sub
            End If

            'Dim startPos As Integer, endPos As Integer
            'startPos = respHTML.IndexOf("<a href=""/pass/setNickname?userId=", StringComparison.CurrentCultureIgnoreCase) + 34
            'endPos = respHTML.IndexOf("&nickname=", StringComparison.CurrentCultureIgnoreCase)
            'Dim _userId As String = respHTML.Substring(startPos, endPos - startPos)
            'Me.Text = "Xiaomi Auto Buy (By Silent) @ " & _userId
            _userId = t.Item("userId").ToString '= respHTML.Substring(startPos, endPos - startPos)
            'Me.Text = "Xiaomi Auto Buy (By Silent) @ " & _userId

            'response = HttpWebResponseUtility.CreateGetHttpResponse("http://buy.mi.com/tw/site/login", Nothing, Nothing, cookies)
            'reader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            'respHTML = reader.ReadToEnd()

            Dim TS As TimeSpan = Now.Subtract(dteStart)
            Debug.Print("耗時 : " & TS.TotalMilliseconds)
            FinishLogin = True

            ''行動電源
            'DianYuanThread = New Thread(AddressOf Me.DianYuanBackground)
            'DianYuanThread.Start()
        Catch ex As Exception
            MsgBox("系統抓取異常 , 請稍後再試 :)", MsgBoxStyle.Critical, "Opps ! Something Error :(")
            User.Enabled = True
            Pwd.Enabled = True
            LoginBtn.Enabled = True
        End Try
    End Sub
    Public Sub LoadSetting()
        If File.Exists(Application.StartupPath & "/" & XmlPath) Then
            Dim configs As BindingList(Of Config) = XmlSerialize.DeserializeFromXml(Of BindingList(Of Config))("Configs.xml")
            If Not configs.Item(0).Account = "" Then
                User.DataBindings.Add("Text", configs, "Account")
            End If
            If Not configs.Item(0).Pwd = "" Then
                Try
                    Pwd.DataBindings.Add("Text", configs, "Pwd")
                    Pwd.Text = Decrypt(Pwd.Text, "SilentKC")
                Catch ex As Exception

                End Try
            End If
        End If
    End Sub
    Public Sub SaveSetting()
        Dim configs As BindingList(Of Config) = Nothing
        configs = New BindingList(Of Config)()
        configs.Add(New Config() With {.Account = User.Text, .Pwd = Loginfrm.Encrypt(Pwd.Text, "SilentKC"), .Manager = Guid.NewGuid})
        XmlSerialize.SerializeToXml("Configs.xml", configs)
    End Sub
    Private Sub Mainfrm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        'SaveSetting()
    End Sub

    Private Sub Mainfrm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Form.CheckForIllegalCrossThreadCalls = False
        LoadSetting()
        SendMessage(User.Handle, _
                             EM_SETCUEBANNER, _
                             IntPtr.Zero, _
                             System.Text.Encoding.Unicode.GetBytes("E-mail/手機號碼/小米ID"))
        SendMessage(Pwd.Handle, _
                             EM_SETCUEBANNER, _
                             IntPtr.Zero, _
                             System.Text.Encoding.Unicode.GetBytes("密碼"))
    End Sub

    Private Sub Timer_Tick(sender As Object, e As EventArgs) Handles Timer.Tick
        If FinishLogin = True Then
            Dim Loginfrm As New XiaomiFrm(CourseAccount, CoursePassword, _userId, cookies)
            Loginfrm.Show()
            Timer.Enabled = False
        End If
    End Sub
End Class
Namespace SilentWebModule
    ''' <summary>
    ''' 有關HTTP請求的模組
    ''' </summary>
    Public Class HttpWebResponseUtility
        'Private Shared ReadOnly DefaultUserAgent As String = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0; MAMIJS)"
        Private Shared ReadOnly DefaultUserAgent As String = "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.146 Safari/537.36"
        ''' <summary>
        ''' 創建GET方式的HTTP請求
        ''' </summary>
        ''' <param name="url">請求的URL</param>
        ''' <param name="timeout">請求的超時時間</param>
        ''' <param name="userAgent">請求的客戶端瀏覽器信息，可以為空</param>
        ''' <param name="cookies">隨同HTTP請求發送的Cookie信息，如果不需要身分驗證可以為空</param>
        ''' <returns></returns>
        Public Shared Function CreateGetHttpResponse(url As String, timeout As System.Nullable(Of Integer), userAgent As String, cookies As CookieContainer) As HttpWebResponse
            If String.IsNullOrEmpty(url) Then
                Throw New ArgumentNullException("url")
            End If
            Dim request As HttpWebRequest = TryCast(WebRequest.Create(url), HttpWebRequest)
            request.Method = "GET"
            request.KeepAlive = True
            request.UserAgent = DefaultUserAgent
            If Not String.IsNullOrEmpty(userAgent) Then
                request.UserAgent = userAgent
            End If
            If timeout.HasValue Then
                request.Timeout = timeout.Value
            End If
            'If url.Contains("http://buy.mi.com/tw/cart/delete/") Then
            '    request.Accept = "*/*"
            '    request.Headers.Add("Accept-Encoding", "gzip, deflate")
            '    request.Headers.Add("Accept-Language", "zh-tw,en-us;q=0.7,en;q=0.3")
            'End If
            If cookies IsNot Nothing Then
                request.CookieContainer = cookies
                'request.CookieContainer = New CookieContainer()
                'request.CookieContainer.Add(cookies)
            End If
            Return TryCast(request.GetResponse(), HttpWebResponse)
        End Function
        ''' <summary>
        ''' 創建POST方式的HTTP請求
        ''' </summary>
        ''' <param name="url">請求的URL</param>
        ''' <param name="parameters">隨同請求POST的參數名稱及參數值字典</param>
        ''' <param name="timeout">請求的超時時間</param>
        ''' <param name="userAgent">請求的客戶端瀏覽器信息，可以為空</param>
        ''' <param name="requestEncoding">發送HTTP請求時所用的編碼</param>
        ''' <param name="cookies">隨同HTTP請求發送的Cookie信息，如果不需要身分驗證可以為空</param>
        ''' <returns></returns>
        Public Shared Function CreatePostHttpResponse(url As String, parameters As IDictionary(Of String, String), timeout As System.Nullable(Of Integer), userAgent As String, requestEncoding As Encoding, cookies As CookieContainer) As HttpWebResponse
            If String.IsNullOrEmpty(url) Then
                Throw New ArgumentNullException("url")
            End If
            If requestEncoding Is Nothing Then
                Throw New ArgumentNullException("requestEncoding")
            End If
            Dim request As HttpWebRequest = Nothing
            '如果是發送HTTPS請求
            If url.StartsWith("https", StringComparison.OrdinalIgnoreCase) Then
                ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf CheckValidationResult)
                request = TryCast(WebRequest.Create(url), HttpWebRequest)
                request.ProtocolVersion = HttpVersion.Version11
            Else
                request = TryCast(WebRequest.Create(url), HttpWebRequest)
            End If
            request.Method = "POST"
            request.KeepAlive = True
            request.ContentType = "application/x-www-form-urlencoded"

            If url.Contains("http://buy.mi.com/tw/address/save") Then
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8"
                request.Accept = "*/*"
                request.Headers.Add("Accept-Encoding", "gzip, deflate")
                request.Headers.Add("Accept-Language", "zh-tw,en-us;q=0.7,en;q=0.3")
            End If

            If Not String.IsNullOrEmpty(userAgent) Then
                request.UserAgent = userAgent
            Else
                request.UserAgent = DefaultUserAgent
            End If

            If timeout.HasValue Then
                request.Timeout = timeout.Value
            End If
            If cookies IsNot Nothing Then
                request.CookieContainer = cookies
                'request.CookieContainer = New CookieContainer()
                'request.CookieContainer.Add(cookies)
            End If
            '如果需要POST數據
            If Not (parameters Is Nothing OrElse parameters.Count = 0) Then
                Dim buffer As New StringBuilder()
                Dim i As Integer = 0
                For Each key As String In parameters.Keys
                    If i > 0 Then
                        buffer.AppendFormat("&{0}={1}", key, parameters(key))
                    Else
                        buffer.AppendFormat("{0}={1}", key, parameters(key))
                    End If
                    i += 1
                Next
                Dim data As Byte() = requestEncoding.GetBytes(buffer.ToString())
                Using stream As Stream = request.GetRequestStream()
                    stream.Write(data, 0, data.Length)
                End Using
            End If
            Return TryCast(request.GetResponse(), HttpWebResponse)

        End Function

        Private Shared Function CheckValidationResult(sender As Object, certificate As X509Certificate, chain As X509Chain, errors As SslPolicyErrors) As Boolean
            Return True
            '總是接受
        End Function
    End Class
End Namespace

Namespace SystemAPI.Function.EncryptLibrary
    Public Class EncryptSHA
        ''' <summary>
        ''' 使用SHA加密訊息
        ''' </summary>
        ''' <param name="sourceMessage">原始資訊</param>
        ''' <param name="SHAType">SHA加密方式</param>
        ''' <returns>string</returns>
        Public Function Encrypt(sourceMessage As String, SHAType As EnumSHAType) As String
            If String.IsNullOrEmpty(sourceMessage) Then
                Return String.Empty
            End If

            '字串先轉成byte[]
            Dim Message As Byte() = Encoding.Unicode.GetBytes(sourceMessage)
            Dim HashImplement As HashAlgorithm = Nothing

            '選擇要使用的SHA加密方式
            Select Case SHAType
                Case EnumSHAType.SHA1
                    HashImplement = New SHA1Managed()
                    Exit Select
                Case EnumSHAType.SHA256
                    HashImplement = New SHA256Managed()
                    Exit Select
                Case EnumSHAType.SHA384
                    HashImplement = New SHA384Managed()
                    Exit Select
                Case EnumSHAType.SHA512
                    HashImplement = New SHA512Managed()
                    Exit Select
            End Select

            '取Hash值
            Dim HashValue As Byte() = HashImplement.ComputeHash(Message)

            '把byte[]轉成string後，再回傳
            Return BitConverter.ToString(HashValue).Replace("-", "").ToLower()

        End Function

        Public Enum EnumSHAType
            SHA1
            SHA256
            SHA384
            SHA512
        End Enum

    End Class
End Namespace
Public Class XmlSerialize
    Public Shared Sub SerializeToXml(FileName As String, [Object] As Object)
        Dim xml As XmlSerializer = Nothing
        Dim stream As Stream = Nothing
        Dim writer As StreamWriter = Nothing
        Try
            xml = New XmlSerializer([Object].[GetType]())
            stream = New FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read)
            writer = New StreamWriter(stream, Encoding.UTF8)
            xml.Serialize(writer, [Object])
        Catch ex As Exception
            Throw ex
        Finally
            writer.Close()
            stream.Close()
        End Try
    End Sub
    Public Shared Function DeserializeFromXml(Of T)(FileName As String) As T
        Dim xml As XmlSerializer = Nothing
        Dim stream As Stream = Nothing
        Dim reader As StreamReader = Nothing
        Try
            xml = New XmlSerializer(GetType(T))
            stream = New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None)
            reader = New StreamReader(stream, Encoding.UTF8)
            Dim obj As Object = xml.Deserialize(reader)
            If obj Is Nothing Then
                Return Nothing
            Else
                Return DirectCast(obj, T)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            stream.Close()
            reader.Close()
        End Try
    End Function
End Class