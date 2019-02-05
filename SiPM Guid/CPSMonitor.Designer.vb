<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CPSMonitor
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
        Me.cps = New System.Windows.Forms.DataGridView()
        CType(Me.cps, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cps
        '
        Me.cps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.cps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.cps.Location = New System.Drawing.Point(0, 0)
        Me.cps.Name = "cps"
        Me.cps.Size = New System.Drawing.Size(225, 893)
        Me.cps.TabIndex = 0
        '
        'CPSMonitor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(225, 893)
        Me.Controls.Add(Me.cps)
        Me.MaximizeBox = False
        Me.Name = "CPSMonitor"
        Me.Text = "Count Monitor"
        CType(Me.cps, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents cps As DataGridView
End Class
