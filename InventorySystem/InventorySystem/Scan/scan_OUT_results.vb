

Imports MySql.Data.MySqlClient
Public Class scan_OUT_results
    Dim itempartcode As String

    Private Sub scan_results_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        dtpicker.Value = Date.Now
    End Sub


    Private Sub datagrid1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles datagrid1.CellClick
        Try
            With datagrid1
                itempartcode = .SelectedCells(0).Value.ToString()
            End With

            con.Close()
            con.Open()
            Dim cmdrefreshgrid As New MySqlCommand("SELECT ts.`lotnumber`, ts.`remarks`, ts.`qty`
                                                    FROM `inventory_fg_scan` ts
                                                    LEFT JOIN scanoperator_is so ON ts.userout = so.IDno
                                                    WHERE       `dateout`='" & dtpicker.Value.ToString("yyyy-MM-dd") & "' 
                                                    and `partcode`='" & itempartcode & "'
                                                            and `located`='" & PClocation & "' 
                                                            and `Fullname`='" & cmbuser.Text & "'  
                                                            and `batchout`='" & cmbbatchin.Text & "' ", con)

            Dim da As New MySqlDataAdapter(cmdrefreshgrid)
            Dim dt As New DataTable
            da.Fill(dt)
            datagrid2.DataSource = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub cmbbatchout_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles cmbbatchin.SelectedIndexChanged
        Try


            con.Close()
            con.Open()
            Dim cmdrefreshgrid As New MySqlCommand("SELECT ts.`lotnumber`, ts.`remarks`, ts.`qty`
                                                    FROM `inventory_fg_scan` ts
                                                    LEFT JOIN scanoperator_is so ON ts.userout = so.IDno
                                                    WHERE       `dateout`='" & dtpicker.Value.ToString("yyyy-MM-dd") & "' 
                                                            and `located`='" & PClocation & "' 
                                                            and `Fullname`='" & cmbuser.Text & "'  
                                                            and `batchout`='" & cmbbatchin.Text & "' 
                                                                    ", con)

            Dim da As New MySqlDataAdapter(cmdrefreshgrid)
            Dim dt As New DataTable
            da.Fill(dt)
            datagrid2.DataSource = dt


            con.Close()
            con.Open()
            Dim cmdrefreshgrid2 As New MySqlCommand("SELECT ts.`partcode` AS Partcode,fm.partname, SUM(`qty`) AS TOTAL ,COUNT(ts.id) AS COUNT
                                                  FROM `inventory_fg_scan` ts
                                                    LEFT JOIN scanoperator_is so ON ts.userout = so.IDno
                                                    LEFT JOIN inventory_fg_masterlist fm ON fm.partcode= ts.partcode
                                                    WHERE       `dateout`='" & dtpicker.Value.ToString("yyyy-MM-dd") & "' 
                                                            and `located`='" & PClocation & "' 
                                                            and `Fullname`='" & cmbuser.Text & "'  
                                                            and `batchout`='" & cmbbatchin.Text & "'          
                                                  GROUP BY partcode", con)

            Dim da2 As New MySqlDataAdapter(cmdrefreshgrid2)
            Dim dt2 As New DataTable
            da2.Fill(dt2)
            datagrid1.DataSource = dt2

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            con.Close()
        End Try
    End Sub

    Private Sub dtpicker_ValueChanged_1(sender As Object, e As EventArgs) Handles dtpicker.ValueChanged
        Try
            con.Close()
            con.Open()
            Dim cmdselect As New MySqlCommand("Select distinct `fullname` FROM `inventory_fg_scan`
                                                INNER JOIN `scanoperator_is` ON `userout` = `IDno`
                                                WHERE located='" & PClocation & "' and `dateout`='" & dtpicker.Value.ToString("yyyy-MM-dd") & "'", con)
            dr = cmdselect.ExecuteReader
            cmbuser.Items.Clear()
            While (dr.Read())
                cmbuser.Items.Add(dr.GetString("fullname"))
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub cmbuser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbuser.SelectedIndexChanged
        Try
            con.Close()
            con.Open()
            Dim cmdselect As New MySqlCommand("Select distinct ts.`batchout` FROM `inventory_fg_scan` ts
                                              Left Join scanoperator_is tsoout ON ts.userout = tsoout.IDno
                                               
                                                WHERE `located`='" & PClocation & "' and `dateout`='" & dtpicker.Value.ToString("yyyy-MM-dd") & "' and `fullname`='" & cmbuser.Text & "'", con)
            dr = cmdselect.ExecuteReader
            cmbbatchin.Items.Clear()
            While (dr.Read())
                cmbbatchin.Items.Add(dr.GetString("batchout"))
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub datagrid1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles datagrid1.CellContentClick

    End Sub
End Class