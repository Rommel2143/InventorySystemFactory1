Imports MySql.Data.MySqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Public Class stock_monitoring

    Private Sub stock_monitoring_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
    Private Sub refreshgrid(status As String, located As String)
        Try
            con.Close()
            con.Open()
            Dim cmdrefreshgrid As New MySqlCommand("SELECT s.partcode, m.partname, sum(s.qty) as Total_Stock FROM inventory_fg_scan s
                                                    JOIN inventory_fg_masterlist m ON m.partcode= s.partcode
                                                    WHERE s.status ='" & status & "' and s.located='" & located & "'
                                                    GROUP BY s.partcode
                                                    ORDER BY m.partname ASC", con)

            Dim da As New MySqlDataAdapter(cmdrefreshgrid)
            Dim dt As New DataTable
            da.Fill(dt)
            datagrid1.DataSource = dt


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally

            con.Close()
        End Try
    End Sub


    Private Sub cmb_stock_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmb_stock.SelectedIndexChanged
        Select Case cmb_stock.Text
            Case "Unit 1-4"
                refreshgrid("IN", "U1-4")
            Case "Unit 5-6"
                refreshgrid("IN", "U5-6")
            Case "WIP"
                refreshgrid("WIP", "U1-4")
        End Select

    End Sub

    Private Sub cmbsearch_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        Try
            ' Get the selected date from dtpicker1
            Dim selectedDate As String = date1
            Dim loc As String = cmb_stock.Text
            ' Prompt the user for the save location with the date in the file name
            Using saveDialog As New SaveFileDialog()
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf"
                saveDialog.FileName = "Total Stock" & loc & selectedDate & ".pdf"

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    ' Export DataGridView to PDF using iTextSharp
                    Dim pdfDoc As New Document(PageSize.A4, 10, 10, 10, 10)
                    Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDoc, New FileStream(saveDialog.FileName, FileMode.Create))
                    pdfDoc.Open()

                    ' Adding title with Helvetica font
                    Dim titleFont As Font = FontFactory.GetFont("Helvetica", CSng(16), CType(Font.Bold, Integer))
                    pdfDoc.Add(New Paragraph("TOTAL STOCK " & loc, titleFont) With {.Alignment = Element.ALIGN_CENTER})

                    ' Add selected date from dtpicker1
                    Dim dateFont As Font = FontFactory.GetFont("Helvetica", CSng(12), CType(Font.Bold, Integer))
                    pdfDoc.Add(New Paragraph("Date: " & date1, dateFont) With {.Alignment = Element.ALIGN_CENTER})
                    pdfDoc.Add(New Paragraph(Environment.NewLine)) ' Add a space after the title and date

                    ' Create PDF table
                    Dim pdfTable As New PdfPTable(datagrid1.ColumnCount) With {
                        .WidthPercentage = 100,
                        .SpacingBefore = 10,
                        .SpacingAfter = 10
                    }

                    ' Add table headers
                    For Each column As DataGridViewColumn In datagrid1.Columns
                        Dim headerFont As Font = FontFactory.GetFont("Helvetica", CSng(12), CType(Font.Bold, Integer))
                        pdfTable.AddCell(New PdfPCell(New Phrase(column.HeaderText, headerFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER})
                    Next

                    ' Add table data
                    For Each row As DataGridViewRow In datagrid1.Rows
                        If Not row.IsNewRow Then
                            For Each cell As DataGridViewCell In row.Cells
                                Dim cellValue As String = If(cell.Value IsNot Nothing, cell.Value.ToString(), "")
                                pdfTable.AddCell(New Phrase(cellValue))
                            Next
                        End If
                    Next

                    ' Add table to document
                    pdfDoc.Add(pdfTable)
                    pdfDoc.Close()
                    writer.Close()

                    MessageBox.Show("Exported to PDF successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred while exporting: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs)

    End Sub
End Class