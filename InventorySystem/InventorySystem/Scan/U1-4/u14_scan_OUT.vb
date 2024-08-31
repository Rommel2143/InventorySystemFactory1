
Imports MySql.Data.MySqlClient
Public Class u14_scan_OUT

    Dim batch As String
    Dim supplier As String

    'duplicate info
    Dim status As String
    Dim located As String
    Dim dateout As Date
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
            txtboxno.Text = ""
            txtboxno.Focus()

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
                Dim cmdselect As New MySqlCommand("SELECT `qrcode`,`status`,`located` FROM `inventory_fg_scan` WHERE `qrcode`='" & qrcode & "' LIMIT 1", con)
                dr = cmdselect.ExecuteReader
                If dr.Read = True Then
                    status = dr.GetString("status")
                    located = dr.GetString("located")


                    Select Case status
                        Case "IN" '//////////////
                            If located = "U1-4" Then
                                'update out and deduct
                                update_inventory_fg_scan()
                                labelerror.Visible = False

                                con.Close()
                            ElseIf located = "U5-6" Then
                                ' showerror("Scanned OUT not performed on UNIT 5-6")
                                update_inventory_fg_scan()
                                labelerror.Visible = False
                            Else
                                showerror("Invalid Location")
                            End If

                        Case "OUT" '//////////////////
                            con.Close()
                            con.Open()
                            Dim cmdselectdate As New MySqlCommand("SELECT `dateout` FROM `inventory_fg_scan` WHERE `qrcode`='" & qrcode & "' LIMIT 1", con)
                            dr = cmdselectdate.ExecuteReader
                            If dr.Read = True Then

                                dateout = dr.GetDateTime("dateout")
                                'duplicate
                                showduplicate(dateout.ToString("MMMM-dd-yyyy"))

                            End If

                        Case "WIP"  '//////////////////////
                            'need to in
                            showerror("Status: WIP (MOLDING SYSTEM)")

                    End Select

                Else 'CON 2 else: DUPLICATION 
                    showerror("NO RECORD FOUND!")
                    con.Close()

                End If
            Else  'CON 1 : QR SPLITING
                showerror("INVALID QR SCANNED!")
                con.Close()

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            refreshgrid()
            refreshgrid2()
            txtqr.Text = ""
            txtqr.Focus()


        End Try

    End Sub


    Private Sub Guna2TextBox2_TextChanged(sender As Object, e As EventArgs) Handles batchcode.TextChanged
        Try
            batch = batchcode.Text
            If batchcode.Text = "" Then
                txtqr.Enabled = False


            Else
                refreshgrid()
                refreshgrid2()

                txtqr.Enabled = True

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub



    Private Sub update_inventory_fg_scan()
        Try
            con.Close()
            con.Open()
            Dim cmdupdate As New MySqlCommand("UPDATE `inventory_fg_scan` SET pcout='" & PCname & "', located='U1-4',`status`='OUT',`batchout`='" & batch & "',`dateout`='" & datedb & "',`userout`='" & idno & "',`boxno`='" & txtboxno.Text & "' WHERE `qrcode`='" & txtqr.Text & "'", con)
            cmdupdate.ExecuteNonQuery()



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try

    End Sub



    Private Sub refreshgrid()
        Try
            con.Close()
            con.Open()
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `id`,`batchout`,`qrcode`,`partcode`,  `lotnumber`, `remarks`, `qty`,`boxno` FROM `inventory_fg_scan`
                                                     WHERE `dateout`='" & datedb & "' and `batchout`='" & batch & "' and `userout`='" & idno & "' and `located`='" & PClocation & "' and `status`='OUT' ", con)

            Dim da As New MySqlDataAdapter(cmdrefreshgrid)
            Dim dt As New DataTable
            da.Fill(dt)
            datagrid1.DataSource = dt
            datagrid1.AutoResizeColumns()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally

            con.Close()
        End Try
    End Sub

    Private Sub refreshgrid2()
        Try
            con.Close()
            con.Open()
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `partcode`, SUM(`qty`) FROM `inventory_fg_scan`
                                                    WHERE `dateout`='" & datedb & "' and `batchout`='" & batch & "' and `located`='" & PClocation & "' and `userout`='" & idno & "'
                                                    GROUP BY partcode", con)

            Dim da As New MySqlDataAdapter(cmdrefreshgrid)
            Dim dt As New DataTable
            da.Fill(dt)
            datagrid2.DataSource = dt
            datagrid2.AutoResizeColumns()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally

            con.Close()
        End Try
    End Sub



    Private Sub txtboxno_KeyDown(sender As Object, e As KeyEventArgs) Handles txtboxno.KeyDown
        If e.KeyCode = Keys.Enter Then

            processQRcode(txtqr.Text)

        End If
    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        print_report.Show()
        print_report.BringToFront()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        scan_OUT_results.Show()
        scan_OUT_results.BringToFront()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub showduplicate(ByVal datetext As Date)
        Try
            labelerror.Visible = True
            texterror.Text = "DUPLICATE! Already scanned on '" & datetext.ToString("MMMM-dd-yyyy") & "'"
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

    Private Sub txtqr_TextChanged(sender As Object, e As EventArgs) Handles txtqr.TextChanged

    End Sub

    Private Sub txtboxno_TextChanged(sender As Object, e As EventArgs) Handles txtboxno.TextChanged

    End Sub
End Class
