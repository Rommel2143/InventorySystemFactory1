Imports MySql.Data.MySqlClient
Public Class Scan_out

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
                Dim cmdselect As New MySqlCommand("SELECT `qrcode`,`status`,`located`,`dateout` FROM `tblscan` WHERE `qrcode`='" & qrcode & "' LIMIT 1", con)
                dr = cmdselect.ExecuteReader
                If dr.Read = True Then
                    status = dr.GetString("status")
                    located = dr.GetString("located")
                    'dateout = dr.GetDateTime("dateout")

                    Select Case status
                        Case "IN"
                            'update out and deduct
                            If located = PClocation Then
                                deduct_to_stock()
                                update_tblscan()
                                refreshgrid()
                                refreshgrid2()
                                return_ok()
                                con.Close()
                            Else
                                showerror("INVALID LOCATION! Please Scan as 'IN' first.")
                                con.Close()
                                txtqr.Text = ""
                                txtqr.Focus()
                            End If

                        Case "OUT"
                            con.Close()
                            con.Open()
                            Dim cmdselectdate As New MySqlCommand("SELECT `dateout` FROM `tblscan` WHERE `qrcode`='" & qrcode & "' LIMIT 1", con)
                            dr = cmdselectdate.ExecuteReader
                            If dr.Read = True Then

                                dateout = dr.GetDateTime("dateout")
                                'duplicate
                                showduplicate(dateout)
                                txtqr.Text = ""
                                txtqr.Focus()
                            End If
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

    Private Sub deduct_to_stock()
        Try
            Select Case PClocation
                Case "U1-4"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `tblmaster` SET `stockF1`= (`stockF1`-" & qty & ") WHERE `partcode`='" & partcode & "'", con)
                    cmdupdate.ExecuteNonQuery()

                Case "U5-6"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `tblmaster` SET `stockU6`= (`stockU6`-" & qty & ") WHERE `partcode`='" & partcode & "'", con)
                    cmdupdate.ExecuteNonQuery()


            End Select

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub
    Private Sub Guna2TextBox2_TextChanged(sender As Object, e As EventArgs) Handles batchcode.TextChanged
        Try
            batch = batchcode.Text
            If batchcode.Text = "" Then
                txtqr.Enabled = False
                Label4.Visible = False
                Label7.Visible = False

            Else

                viewdata("Select `batch`, `userin`, `datein` FROM `tblscan`
                         WHERE `datein`='" & datedb & "' and `located`='" & PClocation & "' and `userout`='" & idno & "' and `batchout`= '" & batchcode.Text & "' LIMIT 1")
                If dr.Read = True Then
                    Label4.Visible = True
                    Label7.Visible = True
                    txtqr.Enabled = False
                Else
                    txtqr.Enabled = True
                    Label4.Visible = False
                    Label7.Visible = False
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs)
        results_IN.Show()
        results_IN.BringToFront()
    End Sub

    Private Sub update_tblscan()
        Try
            con.Close()
            con.Open()
            Dim cmdupdate As New MySqlCommand("UPDATE `tblscan` SET `status`='OUT',`batchout`='" & batch & "',`dateout`='" & datedb & "',`userout`='" & idno & "',`boxno`='" & txtboxno.Text & "' WHERE `qrcode`='" & qrcode & "'", con)
            cmdupdate.ExecuteNonQuery()



        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try

    End Sub

    Private Sub showduplicate(ByVal datetext As String)
        Try
            labelerror.Visible = True
            texterror.Text = "DUPLICATE! Already scanned on '" & datetext & "'"
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

    Private Sub refreshgrid()
        Try
            con.Close()
            con.Open()
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `id`,`batchout`,`qrcode`,`partcode`,  `lotnumber`, `remarks`, `qty`,`boxno` FROM `tblscan`
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
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `partcode`, SUM(`qty`) FROM `tblscan`
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

    Private Sub datagrid1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub datagrid1_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs) Handles datagrid1.CellContentClick

    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) 

        txtqr.Enabled = True
        Label4.Visible = False
        Label7.Visible = False

        refreshgrid()
        refreshgrid2()
    End Sub

    Private Sub cmbsearch_TextChanged(sender As Object, e As EventArgs) Handles cmbsearch.TextChanged
        Try
            con.Close()
            con.Open()
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `id`,`batchout`,`qrcode`,`partcode`,  `lotnumber`, `remarks`, `qty`,`boxno` FROM `tblscan`
                                                     WHERE `dateout`='" & datedb & "' and `located`='" & PClocation & "' and `userout`='" & idno & "' and `status`='OUT' and (`qrcode` REGEXP '" & cmbsearch.Text & "'  or `batchout` REGEXP '" & cmbsearch.Text & "')", con)

            Dim da As New MySqlDataAdapter(cmdrefreshgrid)
            Dim dt As New DataTable
            da.Fill(dt)
            datagrid1.DataSource = dt
            datagrid1.AutoResizeColumns()

            con.Close()
            con.Open()
            Dim cmdrefreshgrid2 As New MySqlCommand("SELECT `partcode`, SUM(`qty`) FROM `tblscan`
                                                  WHERE `dateout`='" & datedb & "' and `located`='" & PClocation & "' and `userout`='" & idno & "' and `status`='OUT' and (`qrcode` REGEXP '" & cmbsearch.Text & "'  or `batchout` REGEXP '" & cmbsearch.Text & "')
                                                    GROUP BY partcode", con)

            Dim da2 As New MySqlDataAdapter(cmdrefreshgrid2)
            Dim dt2 As New DataTable
            da2.Fill(dt2)
            datagrid2.DataSource = dt2
            datagrid2.AutoResizeColumns()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Private Sub txtqr_TextChanged(sender As Object, e As EventArgs) Handles txtqr.TextChanged

    End Sub

    Private Sub txtboxno_TextChanged(sender As Object, e As EventArgs) Handles txtboxno.TextChanged

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
        results_OUT.Show()
        results_OUT.BringToFront()
    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Label7_Click_1(sender As Object, e As EventArgs) Handles Label7.Click

        txtqr.Enabled = True
        Label4.Visible = False
        Label7.Visible = False

        refreshgrid()
        refreshgrid2()
    End Sub
End Class