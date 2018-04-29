<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Mapping
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.DGW = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ButtonDefault = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.iROWS = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.iCOLS = New System.Windows.Forms.NumericUpDown()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonSave = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.DGW, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.iROWS, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iCOLS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.DGW, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(862, 623)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'DGW
        '
        Me.DGW.AllowUserToAddRows = False
        Me.DGW.AllowUserToDeleteRows = False
        Me.DGW.AllowUserToResizeColumns = False
        Me.DGW.AllowUserToResizeRows = False
        Me.DGW.BackgroundColor = System.Drawing.Color.White
        Me.DGW.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DGW.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DGW.Location = New System.Drawing.Point(3, 3)
        Me.DGW.Name = "DGW"
        Me.DGW.Size = New System.Drawing.Size(856, 572)
        Me.DGW.TabIndex = 1
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button2)
        Me.Panel1.Controls.Add(Me.ButtonDefault)
        Me.Panel1.Controls.Add(Me.Button3)
        Me.Panel1.Controls.Add(Me.iROWS)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.iCOLS)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ButtonSave)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 581)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(856, 39)
        Me.Panel1.TabIndex = 2
        '
        'ButtonDefault
        '
        Me.ButtonDefault.Location = New System.Drawing.Point(366, 9)
        Me.ButtonDefault.Name = "ButtonDefault"
        Me.ButtonDefault.Size = New System.Drawing.Size(75, 23)
        Me.ButtonDefault.TabIndex = 6
        Me.ButtonDefault.Text = "Default"
        Me.ButtonDefault.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(274, 9)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "Resize"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'iROWS
        '
        Me.iROWS.Location = New System.Drawing.Point(194, 11)
        Me.iROWS.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iROWS.Name = "iROWS"
        Me.iROWS.Size = New System.Drawing.Size(64, 20)
        Me.iROWS.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(154, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Rows"
        '
        'iCOLS
        '
        Me.iCOLS.Location = New System.Drawing.Point(77, 11)
        Me.iCOLS.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.iCOLS.Name = "iCOLS"
        Me.iCOLS.Size = New System.Drawing.Size(64, 20)
        Me.iCOLS.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Columns"
        '
        'ButtonSave
        '
        Me.ButtonSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonSave.Location = New System.Drawing.Point(773, 9)
        Me.ButtonSave.Name = "ButtonSave"
        Me.ButtonSave.Size = New System.Drawing.Size(75, 23)
        Me.ButtonSave.TabIndex = 0
        Me.ButtonSave.Text = "APPLY"
        Me.ButtonSave.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(458, 9)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(114, 23)
        Me.Button2.TabIndex = 7
        Me.Button2.Text = "BOARD LAYOUT"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Mapping
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "Mapping"
        Me.Size = New System.Drawing.Size(862, 623)
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.DGW, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.iROWS, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iCOLS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents DGW As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ButtonSave As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents iROWS As NumericUpDown
    Friend WithEvents Label2 As Label
    Friend WithEvents iCOLS As NumericUpDown
    Friend WithEvents Label1 As Label
    Friend WithEvents ButtonDefault As Button
    Friend WithEvents Button2 As Button
End Class
