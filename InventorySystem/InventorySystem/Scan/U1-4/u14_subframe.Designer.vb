<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class u1_4_subframe
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(u1_4_subframe))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.MasterlistToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PARTSToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FGToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RETURNToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.userstrip = New System.Windows.Forms.ToolStripMenuItem()
        Me.LogoutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SuggestToImproveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeviceInfoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UpdateSystemToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QRCheckerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.QRCheckerToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.StockMonitoringToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btn_manage = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 41)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1459, 678)
        Me.Panel1.TabIndex = 14
        '
        'MenuStrip1
        '
        Me.MenuStrip1.AutoSize = False
        Me.MenuStrip1.Font = New System.Drawing.Font("Century Gothic", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MasterlistToolStripMenuItem, Me.userstrip, Me.QRCheckerToolStripMenuItem, Me.btn_manage})
        Me.MenuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1459, 41)
        Me.MenuStrip1.TabIndex = 13
        '
        'MasterlistToolStripMenuItem
        '
        Me.MasterlistToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PARTSToolStripMenuItem, Me.FGToolStripMenuItem, Me.RETURNToolStripMenuItem})
        Me.MasterlistToolStripMenuItem.Image = CType(resources.GetObject("MasterlistToolStripMenuItem.Image"), System.Drawing.Image)
        Me.MasterlistToolStripMenuItem.Name = "MasterlistToolStripMenuItem"
        Me.MasterlistToolStripMenuItem.Size = New System.Drawing.Size(99, 37)
        Me.MasterlistToolStripMenuItem.Text = "Scan QR"
        '
        'PARTSToolStripMenuItem
        '
        Me.PARTSToolStripMenuItem.Image = CType(resources.GetObject("PARTSToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PARTSToolStripMenuItem.Name = "PARTSToolStripMenuItem"
        Me.PARTSToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.PARTSToolStripMenuItem.Text = "IN"
        '
        'FGToolStripMenuItem
        '
        Me.FGToolStripMenuItem.Image = CType(resources.GetObject("FGToolStripMenuItem.Image"), System.Drawing.Image)
        Me.FGToolStripMenuItem.Name = "FGToolStripMenuItem"
        Me.FGToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.FGToolStripMenuItem.Text = "OUT"
        '
        'RETURNToolStripMenuItem
        '
        Me.RETURNToolStripMenuItem.Image = CType(resources.GetObject("RETURNToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RETURNToolStripMenuItem.Name = "RETURNToolStripMenuItem"
        Me.RETURNToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.RETURNToolStripMenuItem.Text = "RETURN"
        '
        'userstrip
        '
        Me.userstrip.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.userstrip.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LogoutToolStripMenuItem, Me.ExitToolStripMenuItem, Me.SuggestToImproveToolStripMenuItem, Me.DeviceInfoToolStripMenuItem, Me.UpdateSystemToolStripMenuItem})
        Me.userstrip.Image = CType(resources.GetObject("userstrip.Image"), System.Drawing.Image)
        Me.userstrip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.userstrip.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None
        Me.userstrip.Margin = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.userstrip.Name = "userstrip"
        Me.userstrip.Size = New System.Drawing.Size(83, 37)
        Me.userstrip.Text = "User"
        Me.userstrip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.userstrip.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'LogoutToolStripMenuItem
        '
        Me.LogoutToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.LogoutToolStripMenuItem.Image = CType(resources.GetObject("LogoutToolStripMenuItem.Image"), System.Drawing.Image)
        Me.LogoutToolStripMenuItem.Name = "LogoutToolStripMenuItem"
        Me.LogoutToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.LogoutToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.LogoutToolStripMenuItem.Text = "Logout"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ExitToolStripMenuItem.Image = CType(resources.GetObject("ExitToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'SuggestToImproveToolStripMenuItem
        '
        Me.SuggestToImproveToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.SuggestToImproveToolStripMenuItem.Image = CType(resources.GetObject("SuggestToImproveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.SuggestToImproveToolStripMenuItem.Name = "SuggestToImproveToolStripMenuItem"
        Me.SuggestToImproveToolStripMenuItem.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.SuggestToImproveToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.SuggestToImproveToolStripMenuItem.Text = "Suggest to improve"
        '
        'DeviceInfoToolStripMenuItem
        '
        Me.DeviceInfoToolStripMenuItem.Image = CType(resources.GetObject("DeviceInfoToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeviceInfoToolStripMenuItem.Name = "DeviceInfoToolStripMenuItem"
        Me.DeviceInfoToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.DeviceInfoToolStripMenuItem.Text = "Device info."
        '
        'UpdateSystemToolStripMenuItem
        '
        Me.UpdateSystemToolStripMenuItem.Image = CType(resources.GetObject("UpdateSystemToolStripMenuItem.Image"), System.Drawing.Image)
        Me.UpdateSystemToolStripMenuItem.Name = "UpdateSystemToolStripMenuItem"
        Me.UpdateSystemToolStripMenuItem.Size = New System.Drawing.Size(216, 22)
        Me.UpdateSystemToolStripMenuItem.Text = "Update System"
        '
        'QRCheckerToolStripMenuItem
        '
        Me.QRCheckerToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.QRCheckerToolStripMenuItem1, Me.StockMonitoringToolStripMenuItem})
        Me.QRCheckerToolStripMenuItem.Image = CType(resources.GetObject("QRCheckerToolStripMenuItem.Image"), System.Drawing.Image)
        Me.QRCheckerToolStripMenuItem.Name = "QRCheckerToolStripMenuItem"
        Me.QRCheckerToolStripMenuItem.Size = New System.Drawing.Size(115, 37)
        Me.QRCheckerToolStripMenuItem.Text = "Monitoring"
        '
        'QRCheckerToolStripMenuItem1
        '
        Me.QRCheckerToolStripMenuItem1.Image = CType(resources.GetObject("QRCheckerToolStripMenuItem1.Image"), System.Drawing.Image)
        Me.QRCheckerToolStripMenuItem1.Name = "QRCheckerToolStripMenuItem1"
        Me.QRCheckerToolStripMenuItem1.Size = New System.Drawing.Size(200, 22)
        Me.QRCheckerToolStripMenuItem1.Text = "QR Checker"
        '
        'StockMonitoringToolStripMenuItem
        '
        Me.StockMonitoringToolStripMenuItem.Image = CType(resources.GetObject("StockMonitoringToolStripMenuItem.Image"), System.Drawing.Image)
        Me.StockMonitoringToolStripMenuItem.Name = "StockMonitoringToolStripMenuItem"
        Me.StockMonitoringToolStripMenuItem.Size = New System.Drawing.Size(200, 22)
        Me.StockMonitoringToolStripMenuItem.Text = "Stock Monitoring"
        '
        'btn_manage
        '
        Me.btn_manage.Image = CType(resources.GetObject("btn_manage.Image"), System.Drawing.Image)
        Me.btn_manage.Name = "btn_manage"
        Me.btn_manage.Size = New System.Drawing.Size(99, 37)
        Me.btn_manage.Text = "Manage"
        '
        'u1_4_subframe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1459, 719)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "u1_4_subframe"
        Me.Text = "u1_4_subframe"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents MasterlistToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PARTSToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FGToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RETURNToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents userstrip As ToolStripMenuItem
    Friend WithEvents LogoutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SuggestToImproveToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DeviceInfoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents QRCheckerToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents QRCheckerToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents StockMonitoringToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btn_manage As ToolStripMenuItem
    Friend WithEvents UpdateSystemToolStripMenuItem As ToolStripMenuItem
End Class
