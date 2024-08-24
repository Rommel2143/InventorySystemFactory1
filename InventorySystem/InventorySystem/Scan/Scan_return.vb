Imports MySql.Data.MySqlClient
Public Class Scan_return

    Dim batch As String
    Dim supplier As String

    'duplicate info
    Dim status As String
    Dim located As String
    Dim dateout As String
    Dim partcode As String
    Dim qrcode As String
    Dim lotnumber As String
    Dim remarks As String
    Dim qty As Integer

    'selected item
    Dim itemid As String = ""
    Dim itempartcode As String = ""
    Dim itemqty As Integer = 0

    Private Sub Scan_out_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtdate.Text = date1


    End Sub

    Private Sub txtqr_KeyDown(sender As Object, e As KeyEventArgs) Handles txtqr.KeyDown

        If e.KeyCode = Keys.Enter Then
            qrcode = txtqr.Text
            processQRcode(txtqr.Text)
        End If
    End Sub
    Private Sub processQRcode(qrcode As String)
        Try

            Dim parts() As String = qrcode.Split("|")

            'CON 1 : QR SPLITING
            If parts.Length >= 5 AndAlso parts.Length <= 8 Then
                partcode = parts(0).Remove(0, 2).Trim
                lotnumber = parts(2).Remove(0, 2).Trim
                qty = parts(3).Remove(0, 2).Trim
                remarks = parts(4).Remove(0, 2).Trim
                supplier = parts(1).Remove(0, 2).Trim

                'CON 2: DUPLICATION
                con.Close()
                con.Open()
                Dim cmdselect As New MySqlCommand("SELECT `qrcode`,`status`,`located`,`dateout` FROM `inventory_fg_scan` WHERE `qrcode`='" & qrcode & "'", con)
                dr = cmdselect.ExecuteReader
                If dr.Read = True Then
                    status = dr.GetString("status")
                    located = dr.GetString("located")
                    'dateout = dr.GetDateTime("dateout")

                    Select Case status
                        Case "OUT"
                            'update out and deduct
                            return_to_stock()
                            update_inventory_fg_scan()
                            return_ok()
                                con.Close()

                        Case "IN"
                            'duplicate
                            showduplicate()
                            txtqr.Text = ""
                                txtqr.Focus()

                        Case "WIP"
                            'need to in
                            showerror("MARKED AS WIP! Please Scan as 'IN' first.")
                            txtqr.Text = ""
                            txtqr.Focus()
                    End Select

                Else 'CON 2 else: DUPLICATION 
                    showerror("NO RECORD FOUND!")
                    con.Close()
                    txtqr.Text = ""
                    txtqr.Focus()

                End If
            Else  'CON 1 : QR SPLITING
                showerror("INVALID QR SCANNED!")
                con.Close()
                txtqr.Text = ""
                txtqr.Focus()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub return_to_stock()
        Try
            Select Case PClocation
                Case "U1-4"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `inventory_fg_masterlist` SET `stockF1`= (`stockF1`+" & qty & ") WHERE `partcode`='" & partcode & "'", con)
                    dr = cmdupdate.ExecuteReader

                Case "U5-6"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `inventory_fg_masterlist` SET `stockU6`= (`stockU6`+" & qty & ") WHERE `partcode`='" & partcode & "'", con)
                    dr = cmdupdate.ExecuteReader

            End Select

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub



    Private Sub update_inventory_fg_scan()
        Try
            con.Close()
            con.Open()
            Dim cmdupdate As New MySqlCommand("UPDATE `inventory_fg_scan` SET `status`='IN',
                                                                    `batchout`='',
                                                                    `userout`='',
                                                                    `dateout`= Null,
                                                                    `boxno`='' 
                                                              WHERE `qrcode`='" & qrcode & "'", con)
            cmdupdate.ExecuteNonQuery()



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try

    End Sub

    Private Sub showduplicate()
        Try
            labelerror.Visible = True
            texterror.Text = "DUPLICATE! Already scanned"
            soundduplicate()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub showerror(text As String)

        Try
            labelerror.Visible = True
            texterror.Text = text
            sounderror()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub return_ok()
        Try
            labelerror.Visible = False
            txtqr.Clear()
            txtqr.Focus()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    Private Sub txtqr_TextChanged(sender As Object, e As EventArgs) Handles txtqr.TextChanged

    End Sub
End Class