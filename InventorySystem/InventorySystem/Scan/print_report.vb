Imports MySql.Data.MySqlClient

Public Class print_report


    Private Sub print_report_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtpicker.Value = Date.Now.ToString("MMMM-dd-yyyy")
    End Sub
    Sub viewdata()
        con.Close()
        con.Open()
        Dim showreport As New MySqlCommand("SELECT
    ts.id,
    ts.status,
    ts.batch,
ts.partcode,
ts.lotnumber,
ts.remarks,
    ts.qty,
    ts.batchout,
 ts.dateout,
ts.userout,
    ts.located,
    ts.boxno,
    tsoout.Fullname,
    tm.partname AS PartName,
CASE 
            WHEN tm.section = 'MOLDING' THEN 'IM-FRM-006 (03/10/14-Rev.3)'
            WHEN tm.section = 'PAINTING' THEN 'PT-FRM-023 (11/04/13-Rev.2)'
            WHEN tm.section = 'MOTOR ASSY' THEN 'PG-FRM-005 (10/01/05-Rev.1)'
            WHEN tm.section = 'ASSEMBLY' THEN 'PG-FRM-005 (10/01/05-Rev.1)'
            WHEN tm.section = 'TUBE PUMP' THEN 'ET-FRM-008 (09/21/17-Rev.0)'
            WHEN tm.section = 'RUBBER' THEN 'RS-FRM-004 (06/01/17-Rev.0)'
            WHEN tm.section = 'SHAFT' THEN 'SH-FRM-011 (10/03/05-Rev.1)'
            ELSE tm.section = 'NO AVAILABLE CONTROL CODE'
        END AS Section,
CONCAT(UPPER(tm.section), ' SECTION') AS section_title
FROM
        inventory_fg_scan ts
    Left Join scanoperator_is tsoout ON ts.userout = tsoout.IDno
    Left Join inventory_fg_masterlist tm ON ts.partcode = tm.partcode
WHERE
        ts.dateout = '" & dtpicker.Value.ToString("yyyy-MM-dd") & "' and tsoout.Fullname = '" & cmbuser.Text & "' and ts.batchout='" & cmbbatchout.Text & "'
ORDER BY
    ts.dateout;", con)
        Dim da As New MySqlDataAdapter(showreport)
        da.Fill(dt)
        con.Close()

    End Sub

    Private Sub Guna2DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles dtpicker.ValueChanged
        Try
            con.Close()
            con.Open()
            Dim cmdselect As New MySqlCommand("Select distinct `fullname` FROM `inventory_fg_scan`
                                                INNER JOIN `scanoperator_is` ON `userout` = `IDno`
                                                WHERE `dateout`='" & dtpicker.Value.ToString("yyyy-MM-dd") & "'", con)
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
                                               
                                                WHERE `dateout`='" & dtpicker.Value.ToString("yyyy-MM-dd") & "' and `fullname`='" & cmbuser.Text & "'", con)
            dr = cmdselect.ExecuteReader
            cmbbatchout.Items.Clear()
            While (dr.Read())
                cmbbatchout.Items.Add(dr.GetString("batchout"))
            End While
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub cmbbatchout_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbbatchout.SelectedIndexChanged
        Dim myrpt As New CrystalReport1
        dt.Clear()
        viewdata()
        myrpt.Database.Tables("inventory_fg_scan").SetDataSource(dt)
        CrystalReportViewer1.ReportSource = Nothing
        CrystalReportViewer1.ReportSource = myrpt
    End Sub
End Class