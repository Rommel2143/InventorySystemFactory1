Imports MySql.Data.MySqlClient
Public Class u14_scan_IN


    Dim batch As String
    Dim supplier As String

    'duplicate info
    Dim status As String
    Dim located As String
    Dim datein As Date
    Dim partcode As String
    Dim qrcode As String
    Dim lotnumber As String
    Dim remarks As String
    Dim qty As Integer

    'selected item
    Dim itemid As Integer = 0


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

                'CON 2 : IF SCANNED
                con.Close()
                con.Open()
                Dim cmdselect As New MySqlCommand("SELECT `qrcode`,`status`,`located`,`datein` FROM `inventory_fg_scan` WHERE `qrcode`='" & qrcode & "' LIMIT 1", con)
                dr = cmdselect.ExecuteReader
                If dr.Read = True Then
                    status = dr.GetString("status")
                    located = dr.GetString("located")
                    datein = dr.GetDateTime("datein")

                    Select Case status

                        Case "WIP"
                            'MESSAGE ERROR
                            showerror("Update as FG first! (MOLDING SYSTEM)")

                        Case "IN"
                            If located = "U1-4" Then
                                'duplicate
                                showduplicate(datein.ToString("MMMM-dd-yyyy"))
                            ElseIf located = "U5-6" Then
                                'invalid
                                showerror("Scan OUT not performed on Unit 5-6")
                            Else
                                'no location found
                                showerror("Location Invalid")
                            End If
                        Case "OUT"
                            If located = "U1-4" Then
                                showerror("Status : OUT")
                            ElseIf located = "U5-6" Then
                                ' update
                                update_to_inventory_fg_scan(qrcode)
                                labelerror.Visible = False
                            Else
                                'no location found
                                showerror("Location Invalid")
                            End If

                    End Select

                Else 'CON 2 : IF NOT SCANNED


                    con.Close()
                    con.Open()
                    Dim cmdpartcode As New MySqlCommand("SELECT `partcode` FROM `inventory_fg_masterlist` WHERE `partcode`='" & partcode & "' LIMIT 1", con)
                    dr = cmdpartcode.ExecuteReader
                    If dr.Read = True Then
                        'SAVING
                        insert_to_inventory_fg_scan()
                        refreshgrid()
                        refreshgrid2()
                        return_ok()
                        labelerror.Visible = False
                    Else  'CON 3 : PARTCODE
                        showerror("No Partcode Exists!")
                    End If
                End If

            Else  'CON 1 : QR SPLITING
                showerror("INVALID QR FORMAT!")
                con.Close()

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            txtqr.Text = ""
            txtqr.Focus()
            refreshgrid()
            refreshgrid2()
        End Try

    End Sub


    Private Sub Guna2TextBox2_TextChanged(sender As Object, e As EventArgs) Handles batchcode.TextChanged
        Try
            batch = batchcode.Text
            If batchcode.Text = "" Then
                txtqr.Enabled = False

            Else
                txtqr.Enabled = True

                refreshgrid()
                refreshgrid2()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub



    Private Sub insert_to_inventory_fg_scan()
        Try

            con.Close()
            con.Open()
            Dim cmdinsert As New MySqlCommand("INSERT INTO `inventory_fg_scan`(`status`,
                                                                    `batch`,
                                                                    `userin`,
                                                                    `datein`,
                                                                    `partcode`,
                                                                    `qrcode`,
                                                                    `lotnumber`,
                                                                    `remarks`,
                                                                    `qty`,
                                                                    `located`,
                                                                     `pcin`) 

                                                       VALUES('IN',
                                                              '" & batch & "',
                                                              '" & idno & "',
                                                              '" & datedb & "',
                                                              '" & partcode & "',
                                                              '" & qrcode & "',
                                                              '" & lotnumber & "',
                                                              '" & remarks & "',
                                                              '" & qty & "',
                                                              'U1-4',
                                                              '" & PCname & "')", con)
            cmdinsert.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try

    End Sub





    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub
    Private Sub update_to_inventory_fg_scan(qrcode As String)
        Try

            con.Close()
            con.Open()
            Dim cmdupdate As New MySqlCommand("UPDATE `inventory_fg_scan` SET pcin= '" & PCname & "', located= 'U1-4', status='IN', batch='" & batch & "', userin='" & idno & "', datein='" & datedb & "' WHERE qrcode = '" & qrcode & "' ", con)
            cmdupdate.ExecuteNonQuery()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try

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
            Dim cmdrefreshgrid As New MySqlCommand("SELECT `id`,`batch`,`qrcode`,`partcode`,  `lotnumber`, `remarks`, `qty` FROM `inventory_fg_scan`
                                                    WHERE `datein`='" & datedb & "' and `userin`='" & idno & "' and `batch`='" & batch & "' and `located`='U1-4' and `status`='IN' ", con)

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
                                                    WHERE `datein`='" & datedb & "' and `batch`='" & batch & "' and `located`='U1-4' and `userin`='" & idno & "'
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
                itemid = .SelectedCells(0).Value

            End With
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub btndelete_Click(sender As Object, e As EventArgs) Handles btndelete.Click
        Try
            If itemid = 0 Then
                MessageBox.Show("No item selected!")
            Else
                If MsgBox("Are you sure to DELETE this record?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then

                    con.Close()
                    con.Open()
                    Dim cmddelete As New MySqlCommand("DELETE FROM `inventory_fg_scan` WHERE `id`= '" & itemid & "'", con)
                    cmddelete.ExecuteNonQuery()

                    itemid = 0

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





    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        scan_IN_results.Show()
        scan_IN_results.BringToFront()

    End Sub

    Private Sub datagrid1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles datagrid1.CellContentClick

    End Sub

    Private Sub Guna2Panel3_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel3.Paint

    End Sub
End Class