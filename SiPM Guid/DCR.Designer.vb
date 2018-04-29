<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DCR
    Inherits System.Windows.Forms.Form

    'Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DCR))
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ss = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ep = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.sp = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.plot1 = New OxyPlot.WindowsForms.PlotView()
        Me.CheckedListBox1 = New System.Windows.Forms.CheckedListBox()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.ss, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.sp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel2, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1362, 1025)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ProgressBar1)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.ss)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ep)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.sp)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1356, 69)
        Me.Panel1.TabIndex = 0
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Location = New System.Drawing.Point(1177, 20)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(157, 36)
        Me.ProgressBar1.TabIndex = 18
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(785, 14)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(78, 42)
        Me.Button1.TabIndex = 17
        Me.Button1.Text = "GO"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ss
        '
        Me.ss.Location = New System.Drawing.Point(587, 23)
        Me.ss.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ss.Maximum = New Decimal(New Integer() {16000, 0, 0, 0})
        Me.ss.Name = "ss"
        Me.ss.Size = New System.Drawing.Size(135, 26)
        Me.ss.TabIndex = 16
        Me.ss.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(522, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 20)
        Me.Label2.TabIndex = 15
        Me.Label2.Text = "Step"
        '
        'ep
        '
        Me.ep.Location = New System.Drawing.Point(328, 23)
        Me.ep.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.ep.Maximum = New Decimal(New Integer() {16000, 0, 0, 0})
        Me.ep.Name = "ep"
        Me.ep.Size = New System.Drawing.Size(135, 26)
        Me.ep.TabIndex = 14
        Me.ep.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(263, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 20)
        Me.Label1.TabIndex = 13
        Me.Label1.Text = "Stop"
        '
        'sp
        '
        Me.sp.Location = New System.Drawing.Point(89, 23)
        Me.sp.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.sp.Maximum = New Decimal(New Integer() {16000, 0, 0, 0})
        Me.sp.Name = "sp"
        Me.sp.Size = New System.Drawing.Size(135, 26)
        Me.sp.TabIndex = 12
        Me.sp.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(24, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 20)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "Start"
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 116.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.plot1, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.CheckedListBox1, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(3, 78)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1356, 944)
        Me.TableLayoutPanel2.TabIndex = 1
        '
        'plot1
        '
        Me.plot1.BackColor = System.Drawing.Color.White
        Me.plot1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.plot1.ForeColor = System.Drawing.Color.Black
        Me.plot1.Location = New System.Drawing.Point(120, 5)
        Me.plot1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.plot1.Name = "plot1"
        Me.plot1.PanCursor = System.Windows.Forms.Cursors.Hand
        Me.plot1.Size = New System.Drawing.Size(1232, 934)
        Me.plot1.TabIndex = 6
        Me.plot1.Text = "plot1"
        Me.plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE
        Me.plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE
        Me.plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS
        '
        'CheckedListBox1
        '
        Me.CheckedListBox1.CheckOnClick = True
        Me.CheckedListBox1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CheckedListBox1.FormattingEnabled = True
        Me.CheckedListBox1.Location = New System.Drawing.Point(3, 4)
        Me.CheckedListBox1.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.CheckedListBox1.Name = "CheckedListBox1"
        Me.CheckedListBox1.Size = New System.Drawing.Size(110, 936)
        Me.CheckedListBox1.TabIndex = 5
        '
        'DCR
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1362, 1025)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "DCR"
        Me.Text = "DCR"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.ss, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ep, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.sp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Button1 As Button
    Friend WithEvents ss As NumericUpDown
    Friend WithEvents Label2 As Label
    Friend WithEvents ep As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents sp As NumericUpDown
    Friend WithEvents Label3 As Label
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Private WithEvents plot1 As OxyPlot.WindowsForms.PlotView
    Friend WithEvents CheckedListBox1 As CheckedListBox
End Class
