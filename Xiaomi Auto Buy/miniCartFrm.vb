Imports Xiaomi_Auto_Buy.SilentWebModule
Imports System.Net
Imports System.IO
Public Class miniCartFrm
    Public cookies As New CookieContainer()
    Dim _dmSite As String = "http://sgp01.tp.hd.mi.com"
    Dim _timestampUrl As String = _dmSite + "/gettimestamp"
    Dim _miniNew As String = "http://buy.mi.com/tw/cart/miniNew"
    Dim ItemList As New ListBox
    Public Sub New(LoginCookies As CookieContainer)
        ' 此為設計工具所需的呼叫。
        InitializeComponent()
        cookies = LoginCookies
    End Sub
    Sub refreshMiniNew()
        Try
            ItemList.Items.Clear()
            Dim _miniCart As String = CheckShopCart()
            If _miniCart = Nothing Then
                Dim NoItemLabel As Label = New Label
                Me.Controls.Add(NoItemLabel)
                NoItemLabel.Text = "購物車中沒有商品！"
                NoItemLabel.Font = New Font("微軟正黑體", 9.75, FontStyle.Bold)
                NoItemLabel.AutoSize = True
                Dim g As Graphics = Me.CreateGraphics()
                Dim strSize As SizeF = g.MeasureString(NoItemLabel.Text, NoItemLabel.Font)
                NoItemLabel.Location = New Point((Me.Size.Width - strSize.Width) / 2, (Me.Size.Height - strSize.Height) / 2)
                Exit Sub
            Else
                Debug.Print(_miniCart)
                Dim t As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(_miniCart)
                Me.Controls.Clear()
                Dim itemCount As Integer = Val(t.Item("totalItem").ToString)
                Dim labelArray As Label() = New Label(itemCount) {}
                Dim picArray As PictureBox() = New PictureBox(itemCount) {}
                Dim picbuttonArray As PictureBox() = New PictureBox(itemCount) {}
                Dim ClearCartButton As Button = New Button
                Me.Size = New Point(325, 36 + 60 * itemCount + 35)

                If itemCount = 0 Then
                    Dim NoItemLabel As Label = New Label
                    Me.Controls.Add(NoItemLabel)
                    NoItemLabel.Text = "購物車中沒有商品！"
                    NoItemLabel.Font = New Font("微軟正黑體", 9.75, FontStyle.Bold)
                    NoItemLabel.AutoSize = True
                    Dim g As Graphics = Me.CreateGraphics()
                    Dim strSize As SizeF = g.MeasureString(NoItemLabel.Text, NoItemLabel.Font)
                    NoItemLabel.Location = New Point((Me.Size.Width - strSize.Width) / 2, (Me.Size.Height - strSize.Height) / 2)
                    Exit Sub
                End If

                For i = 0 To itemCount - 1
                    ItemList.Items.Add(t.Item("items")(i)("itemId").ToString)

                    picArray(i) = New PictureBox
                    labelArray(i) = New Label
                    picbuttonArray(i) = New PictureBox
                    Me.Controls.Add(picArray(i))
                    Me.Controls.Add(labelArray(i))
                    Me.Controls.Add(picbuttonArray(i))

                    picArray(i).Size = New Point(60, 60)
                    picArray(i).InitialImage = Nothing

                    picArray(i).ImageLocation = t.Item("items")(i)("image_url").ToString + "?width=60&height=60"
                    picArray(i).Top = 12 + picArray(i).Height * i
                    picArray(i).Left = 13

                    labelArray(i).Text = StrConv(t.Item("items")(i)("product_name").ToString, VbStrConv.TraditionalChinese, 2052) + " NT$" + t.Item("items")(i)("salePrice").ToString + " x" + t.Item("items")(i)("num").ToString
                    Dim g As Graphics = Me.CreateGraphics()
                    Dim strSize As SizeF = g.MeasureString(labelArray(i).Text, labelArray(i).Font)
                    If strSize.Width / 180 > 1 Then
                        labelArray(i).Location = New Point(73, picArray(i).Location.Y + 13)
                    Else
                        labelArray(i).Location = New Point(73, picArray(i).Location.Y + 21)
                    End If
                    labelArray(i).Size = New Point(200, 40)
                    picbuttonArray(i).Name = i
                    picbuttonArray(i).Location = New Point(labelArray(i).Location.X + 210, picArray(i).Location.Y + 20)
                    picbuttonArray(i).Size = New Point(18, 18)
                    picbuttonArray(i).Image = My.Resources.cancel1

                    AddHandler picbuttonArray(i).MouseMove, AddressOf picbutton_MouseMove
                    AddHandler picbuttonArray(i).MouseLeave, AddressOf picbutton_MouseLeave
                    AddHandler picbuttonArray(i).MouseClick, AddressOf picbutton_Click
                Next

                Me.Controls.Add(ClearCartButton)
                ClearCartButton.Size = New Point(100, 35)
                ClearCartButton.Font = New Font("微軟正黑體", 9.75, FontStyle.Bold)
                ClearCartButton.FlatStyle = FlatStyle.Flat
                ClearCartButton.FlatAppearance.BorderSize = 0
                ClearCartButton.ForeColor = Color.White
                ClearCartButton.Text = "清空購物車"
                ClearCartButton.BackColor = Color.FromArgb(255, 111, 61)
                ClearCartButton.Location = New Point((Me.Size.Width - ClearCartButton.Size.Width) / 2, Me.Size.Height - 12 - ClearCartButton.Size.Height)
                AddHandler ClearCartButton.Click, AddressOf ClearCartButton_Click
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub miniCartFrm_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
        Me.Close()
    End Sub
    Private Sub miniCartFrm_Load(sender As Object, e As EventArgs) Handles Me.Load
        refreshMiniNew()
    End Sub
    Function GetServerTime() As String
        Try
            'Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_timestampUrl, Nothing, Nothing, cookies)
            'Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            'Dim respHTML As String = reader.ReadToEnd()
            'response.Close()
            'Return respHTML.Split("=")(1).Trim
            Return CLng(Now.Subtract(New System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds)
        Catch ex As Exception
            Return CLng(Now.Subtract(New System.DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds)
        End Try
    End Function
    Function CheckShopCart()
        Try
            Dim _ServerTime As String = GetServerTime()

            Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse(_miniNew, Nothing, Nothing, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML = reader.ReadToEnd()
            response.Close()
            'Dim tx As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.JsonConvert.DeserializeObject(respHTML)
            Return respHTML
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Private Sub picbutton_MouseMove(sender As Object, e As MouseEventArgs)
        DirectCast((sender), PictureBox).Image = My.Resources.cancel2
    End Sub

    Private Sub picbutton_MouseLeave(sender As Object, e As EventArgs)
        DirectCast((sender), PictureBox).Image = My.Resources.cancel1
    End Sub
    Private Sub ClearCartButton_Click(sender As Object, e As EventArgs)
        For i = 0 To ItemList.Items.Count - 1
            DeleteItem(i, True)
        Next
        refreshMiniNew()
    End Sub
    Private Sub picbutton_Click(sender As Object, e As EventArgs)
        'Debug.Print(DirectCast((sender), PictureBox).Name)
        DeleteItem(DirectCast((sender), PictureBox).Name, False)
    End Sub
    Function DeleteItem(_itemIndex As String, DeleteAll As Boolean)
        Try
            Dim response As HttpWebResponse = HttpWebResponseUtility.CreateGetHttpResponse("http://buy.mi.com/tw/cart/delete/" + ItemList.Items(Val(_itemIndex)) + "?ajax=cart-grid", Nothing, Nothing, cookies)
            Dim reader As StreamReader = New StreamReader(response.GetResponseStream, System.Text.Encoding.GetEncoding("UTF-8"))
            Dim respHTML = reader.ReadToEnd()
            Debug.Print(respHTML)
            response.Close()
            If DeleteAll = False Then
                refreshMiniNew()
            End If
            Return respHTML
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub miniCartFrm_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Me.Visible = False Then
            Me.Close()
        End If
    End Sub
End Class