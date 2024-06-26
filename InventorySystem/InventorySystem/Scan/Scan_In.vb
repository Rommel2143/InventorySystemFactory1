﻿Imports MySql.Data.MySqlClient
Public Class Scan_In

    Dim batch As String
    Dim supplier As String

    'duplicate info
    Dim status As String
    Dim located As String
    Dim datein As String
    Dim partcode As String
    Dim qrcode As String
    Dim lotnumber As String
    Dim remarks As String
    Dim qty As Integer

    'selected item
    Dim itemid As String = ""
    Dim itempartcode As String = ""
    Dim itemqty As Integer = 0

    Private Sub Scan_In_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtdate.Text = date1

       
    End Sub

    Private Sub Txtqr_KeyDown(sender As Object, e As KeyEventArgs) Handles txtqr.KeyDown

        If e.KeyCode = Keys.Enter Then
            qrcode = txtqr.Text
            ProcessQRcode(txtqr.Text)

        End If
    End Sub
    Private Sub ProcessQRcode(qrcode As String)
        Try

            Dim parts() As String = qrcode.Split("|")

            'CON 1 : QR SPLITING
            If parts.Length >= 5 AndAlso parts.Length <= 8 Then
                partcode = parts(0).Remove(0, 2).Trim
                lotnumber = parts(2).Remove(0, 2).Trim
                qty = parts(3).Remove(0, 2).Trim
                remarks = parts(4).Remove(0, 2).Trim
                supplier = parts(1).Remove(0, 2).Trim

                'CON 2 : DUPLICATE QR or GET location
                con.Close()
                con.Open()
                Dim cmdselect As New MySqlCommand("SELECT `qrcode`,`status`,`located`,`datein` FROM `tblscan` WHERE `qrcode`='" & qrcode & "'", con)
                dr = cmdselect.ExecuteReader
                If dr.Read = True Then
                    status = dr.GetString("status")
                    located = dr.GetString("located")
                    datein = dr.GetDateTime("datein")

                    If status = "WIP" Then

                        Dim cmdupdate As New MySqlCommand()
                        cmdupdate.Connection = con
                        con.Close()
                        con.Open()

                        Select Case PClocation
                            Case "U1-4"
                                cmdupdate.CommandText = "START TRANSACTION;
                                 UPDATE `tblmaster` 
                                SET `stockF1` = `stockF1` + @qty, `wipstock` = `wipstock` - @qty
                                WHERE `partcode` = @partcode;
                                
                                UPDATE `tblscan` 
                                                                SET `status`='IN',
                                                                    `batch`='" & batch & "',
                                                                    `userin`='" & idno & "',
                                                                    `datein`='" & datedb & "',
                                                                    `located`='" & PClocation & "'
                                WHERE `qrcode`='" & qrcode & "';
                                
                                COMMIT;"
                                cmdupdate.Parameters.AddWithValue("@qty", qty)
                                cmdupdate.Parameters.AddWithValue("@partcode", partcode)


                            Case "U5-6"
                                cmdupdate.CommandText = "START TRANSACTION;
                                UPDATE `tblmaster` 
                                SET `stockF1` = `stockF1` + @qty, `wipstock` = `wipstock` - @qty
                                WHERE `partcode` = @partcode;
                                
                               UPDATE `tblscan` 
                                                                SET `status`='IN',
                                                                    `batch`='" & batch & "',
                                                                    `userin`='" & idno & "',
                                                                    `datein`='" & datedb & "',
                                                                    `located`='" & PClocation & "'
                                WHERE `qrcode`='" & qrcode & "';
                                
                                COMMIT;"
                                cmdupdate.Parameters.AddWithValue("@qty", qty)
                                cmdupdate.Parameters.AddWithValue("@partcode", partcode)

                        End Select
                        cmdupdate.ExecuteNonQuery()
                        refreshgrid()
                        refreshgrid2()
                        return_ok()
                        con.Close()

                    Else
                        If located = PClocation Then
                            showduplicate(datein)
                            txtqr.Clear()
                            txtqr.Focus()
                        Else


                            Dim cmdupdate As New MySqlCommand()
                            cmdupdate.Connection = con
                            con.Close()
                            con.Open()

                            Select Case PClocation
                                Case "U1-4"
                                    cmdupdate.CommandText = "START TRANSACTION;
                                UPDATE `tblmaster` 
                                SET `stockF1` = `stockF1` + @qty, `stocku6` = `stocku6` - @qty
                                WHERE `partcode` = @partcode;
                                
                                UPDATE `tblscan` 
                                SET `located` = 'U1-4'
                                WHERE `qrcode`='" & qrcode & "';
                                
                                COMMIT;"
                                    cmdupdate.Parameters.AddWithValue("@qty", qty)
                                    cmdupdate.Parameters.AddWithValue("@partcode", partcode)


                                Case "U5-6"
                                    cmdupdate.CommandText = "START TRANSACTION;
                                UPDATE `tblmaster`
                                SET `stockF1` = `stockF1` - @qty, `stocku6` = `stocku6` + @qty
                                WHERE `partcode` = @partcode;
                                
                                UPDATE `tblscan` 
                                SET `located` = 'U5-6'
                                WHERE `qrcode`='" & qrcode & "';
                                
                                COMMIT;"
                                    cmdupdate.Parameters.AddWithValue("@qty", qty)
                                    cmdupdate.Parameters.AddWithValue("@partcode", partcode)

                            End Select
                            cmdupdate.ExecuteNonQuery()
                            refreshgrid()
                            refreshgrid2()
                            return_ok()
                            con.Close()
                        End If
                    End If
                Else 'CON 2 : NOT DUPLICATE


                    con.Close()
                    con.Open()
                    Dim cmdpartcode As New MySqlCommand("SELECT `partcode` FROM `tblmaster` WHERE `partcode`='" & partcode & "'", con)
                    dr = cmdpartcode.ExecuteReader
                    If dr.Read = True Then
                        'SAVING
                        insert_to_tblscan()
                        add_to_stock()
                        refreshgrid()
                        refreshgrid2()
                        return_ok()

                    Else  'CON 3 : PARTCODE
                        showerror("No Partcode Exists!")
                    End If

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


    Private Sub Guna2TextBox2_TextChanged(sender As Object, e As EventArgs) Handles batchcode.TextChanged
        Try
            batch = batchcode.Text
            If batchcode.Text = "" Then
                txtqr.Enabled = False
                Label4.Visible = False
                Label7.Visible = False

            Else

                viewdata("SELECT `batch`, `userin`, `datein` FROM `tblscan`
                         WHERE `datein`='" & datedb & "' and `userin`='" & idno & "' and `batch`= '" & batchcode.Text & "' and `located`='" & PClocation & "'")
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

    Private Sub insert_to_tblscan()
        Try

            con.Close()
            con.Open()
            Dim cmdinsert As New MySqlCommand("INSERT INTO `tblscan`(`status`,
                                                                    `batch`,
                                                                    `userin`,
                                                                    `datein`,
                                                                    `partcode`,
                                                                    `qrcode`,
                                                                    `lotnumber`,
                                                                    `remarks`,
                                                                    `qty`,
                                                                    `located`) 

                                                       VALUES('IN',
                                                              '" & batch & "',
                                                              '" & idno & "',
                                                              '" & datedb & "',
                                                              '" & partcode & "',
                                                              '" & qrcode & "',
                                                              '" & lotnumber & "',
                                                              '" & remarks & "',
                                                              '" & qty & "',
                                                              '" & PClocation & "')", con)
            cmdinsert.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try

    End Sub
    Private Sub add_to_stock()
        Try
            Select Case PClocation
                Case "U1-4"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `tblmaster` SET `stockF1`= (`stockF1`+" & qty & ") WHERE `partcode`='" & partcode & "'", con)
                    cmdupdate.ExecuteNonQuery()


                Case "U5-6"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `tblmaster` SET `stockU6`= (`stockU6`+" & qty & ") WHERE `partcode`='" & partcode & "'", con)
                    cmdupdate.ExecuteNonQuery()

            End Select

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub




    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub txtqr_TextChanged(sender As Object, e As EventArgs) Handles txtqr.TextChanged

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
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `id`,`batch`,`qrcode`,`partcode`,  `lotnumber`, `remarks`, `qty` FROM `tblscan`
                                                    WHERE `datein`='" & datedb & "' and `userin`='" & idno & "' and `batch`='" & batch & "' and `located`='" & PClocation & "' and `status`='IN' ", con)

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
                                                    WHERE `datein`='" & datedb & "' and `batch`='" & batch & "' and `located`='" & PClocation & "' and `userin`='" & idno & "'
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
    Private Sub datagrid1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles datagrid1.CellClick
        Try
            With datagrid1
                itemid = .SelectedCells(0).Value.ToString()
                itempartcode = .SelectedCells(2).Value.ToString()
                itemqty = .SelectedCells(5).Value()

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        Try
            If itemid = "" Then
                MessageBox.Show("No item selected!")
            Else
                If MsgBox("Are you sure to DELETE this record?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                    con.Close()
                    con.Open()
                    Dim cmddelete As New MySqlCommand("DELETE FROM `tblscan` WHERE `id`= '" & itemid & "'", con)
                    cmddelete.ExecuteNonQuery()
                    deduct_to_stock()
                    itemid = ""
                    itempartcode = ""
                    itemqty = 0
                    refreshgrid()
                    refreshgrid2()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub
    Private Sub deduct_to_stock()
        Try
            Select Case PClocation
                Case "U1-4"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `tblmaster` SET `stockF1`= (`stockF1`-" & itemqty & ") WHERE `partcode`='" & itempartcode & "'", con)
                    cmdupdate.ExecuteNonQuery()


                Case "U5-6"
                    con.Close()
                    con.Open()
                    Dim cmdupdate As New MySqlCommand("UPDATE `tblmaster` SET `stockU6`= (`stockU6`-" & itemqty & ") WHERE `partcode`='" & itempartcode & "'", con)
                    cmdupdate.ExecuteNonQuery()


            End Select

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

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
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `id`,`batch`,`qrcode`,`partcode`,  `lotnumber`, `remarks`, `qty` FROM `tblscan`
                                                     WHERE `datein`='" & datedb & "' and `located`='" & PClocation & "' and `userin`='" & idno & "' and `status`='IN' and (`qrcode` REGEXP '" & cmbsearch.Text & "' or `batch` REGEXP '" & cmbsearch.Text & "')", con)

            Dim da As New MySqlDataAdapter(cmdrefreshgrid)
            Dim dt As New DataTable
            da.Fill(dt)
            datagrid1.DataSource = dt
            datagrid1.AutoResizeColumns()

            con.Close()
            con.Open()
            Dim cmdrefreshgrid2 As New MySqlCommand("SELECT `partcode`, SUM(`qty`) FROM `tblscan`
                                                  WHERE `datein`='" & datedb & "' and `located`='" & PClocation & "' and `userin`='" & idno & "' and `status`='IN' and (`qrcode` REGEXP '" & cmbsearch.Text & "' or `batch` REGEXP '" & cmbsearch.Text & "')               
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

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        results_IN.Show()
        results_IN.BringToFront()

    End Sub
End Class