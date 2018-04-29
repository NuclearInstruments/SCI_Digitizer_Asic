<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class pImmediate
    Inherits System.Windows.Forms.UserControl

    'UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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

    'Richiesto da Progettazione Windows Form
    Private components As System.ComponentModel.IContainer

    'NOTA: la procedura che segue è richiesta da Progettazione Windows Form
    'Può essere modificata in Progettazione Windows Form.  
    'Non modificarla mediante l'editor del codice.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.plot1 = New OxyPlot.WindowsForms.PlotView()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'plot1
        '
        Me.plot1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plot1.Location = New System.Drawing.Point(0, 0)
        Me.plot1.Name = "plot1"
        Me.plot1.PanCursor = System.Windows.Forms.Cursors.Hand
        Me.plot1.Size = New System.Drawing.Size(563, 383)
        Me.plot1.TabIndex = 2
        Me.plot1.Text = "plot1"
        Me.plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'pImmediate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.plot1)
        Me.Name = "pImmediate"
        Me.Size = New System.Drawing.Size(563, 383)
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents plot1 As OxyPlot.WindowsForms.PlotView
    Friend WithEvents Timer1 As Timer
End Class
