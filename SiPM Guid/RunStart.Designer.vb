<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RunStart
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RunStart))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.pRun = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.pMRun = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pBoard = New System.Windows.Forms.TextBox()
        Me.pAcq = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.pBias = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pDT = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.pNote = New System.Windows.Forms.TextBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.cTargetValue = New System.Windows.Forms.TextBox()
        Me.cTargetMode = New System.Windows.Forms.ComboBox()
        Me.unit = New System.Windows.Forms.Label()
        Me.UnitMult = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(31, 79)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 24)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Run Number"
        '
        'pRun
        '
        Me.pRun.BackColor = System.Drawing.Color.Yellow
        Me.pRun.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.pRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pRun.Location = New System.Drawing.Point(265, 65)
        Me.pRun.Margin = New System.Windows.Forms.Padding(2)
        Me.pRun.Name = "pRun"
        Me.pRun.Size = New System.Drawing.Size(217, 37)
        Me.pRun.TabIndex = 1
        Me.pRun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(31, 118)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(197, 24)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Machine Run Number"
        '
        'pMRun
        '
        Me.pMRun.BackColor = System.Drawing.Color.Fuchsia
        Me.pMRun.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.pMRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pMRun.Location = New System.Drawing.Point(265, 106)
        Me.pMRun.Margin = New System.Windows.Forms.Padding(2)
        Me.pMRun.Name = "pMRun"
        Me.pMRun.Size = New System.Drawing.Size(217, 37)
        Me.pMRun.TabIndex = 3
        Me.pMRun.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(34, 151)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(93, 24)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Run Type"
        '
        'pBoard
        '
        Me.pBoard.BackColor = System.Drawing.Color.Black
        Me.pBoard.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.pBoard.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pBoard.ForeColor = System.Drawing.Color.White
        Me.pBoard.Location = New System.Drawing.Point(265, 153)
        Me.pBoard.Margin = New System.Windows.Forms.Padding(2)
        Me.pBoard.Name = "pBoard"
        Me.pBoard.Size = New System.Drawing.Size(217, 22)
        Me.pBoard.TabIndex = 5
        Me.pBoard.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'pAcq
        '
        Me.pAcq.BackColor = System.Drawing.Color.Black
        Me.pAcq.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.pAcq.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pAcq.ForeColor = System.Drawing.Color.White
        Me.pAcq.Location = New System.Drawing.Point(265, 264)
        Me.pAcq.Margin = New System.Windows.Forms.Padding(2)
        Me.pAcq.Name = "pAcq"
        Me.pAcq.Size = New System.Drawing.Size(217, 22)
        Me.pAcq.TabIndex = 7
        Me.pAcq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(34, 262)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(123, 24)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "User Param 2"
        '
        'pBias
        '
        Me.pBias.BackColor = System.Drawing.Color.Black
        Me.pBias.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.pBias.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pBias.ForeColor = System.Drawing.Color.White
        Me.pBias.Location = New System.Drawing.Point(265, 344)
        Me.pBias.Margin = New System.Windows.Forms.Padding(2)
        Me.pBias.Name = "pBias"
        Me.pBias.Size = New System.Drawing.Size(217, 22)
        Me.pBias.TabIndex = 11
        Me.pBias.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(34, 342)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(51, 24)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "BIAS"
        '
        'pDT
        '
        Me.pDT.BackColor = System.Drawing.Color.Lime
        Me.pDT.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.pDT.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pDT.ForeColor = System.Drawing.Color.Black
        Me.pDT.Location = New System.Drawing.Point(265, 405)
        Me.pDT.Margin = New System.Windows.Forms.Padding(2)
        Me.pDT.Name = "pDT"
        Me.pDT.Size = New System.Drawing.Size(217, 22)
        Me.pDT.TabIndex = 13
        Me.pDT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(31, 405)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(96, 24)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Date Time"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(34, 493)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(50, 24)
        Me.Label8.TabIndex = 14
        Me.Label8.Text = "Note"
        '
        'pNote
        '
        Me.pNote.Location = New System.Drawing.Point(35, 522)
        Me.pNote.Margin = New System.Windows.Forms.Padding(2)
        Me.pNote.Multiline = True
        Me.pNote.Name = "pNote"
        Me.pNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.pNote.Size = New System.Drawing.Size(450, 70)
        Me.pNote.TabIndex = 15
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Lime
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(365, 607)
        Me.Button1.Margin = New System.Windows.Forms.Padding(2)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(119, 36)
        Me.Button1.TabIndex = 16
        Me.Button1.Text = "START"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.Red
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(35, 607)
        Me.Button2.Margin = New System.Windows.Forms.Padding(2)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(119, 36)
        Me.Button2.TabIndex = 17
        Me.Button2.Text = "ABORT"
        Me.Button2.UseVisualStyleBackColor = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(30, 22)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(65, 24)
        Me.Label9.TabIndex = 18
        Me.Label9.Text = "Folder"
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(117, 25)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(311, 20)
        Me.TextBox1.TabIndex = 19
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(433, 25)
        Me.Button3.Margin = New System.Windows.Forms.Padding(2)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(50, 18)
        Me.Button3.TabIndex = 20
        Me.Button3.Text = "..."
        Me.Button3.UseVisualStyleBackColor = True
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.Color.Black
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.ForeColor = System.Drawing.Color.White
        Me.TextBox3.Location = New System.Drawing.Point(265, 189)
        Me.TextBox3.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(217, 22)
        Me.TextBox3.TabIndex = 24
        Me.TextBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(34, 187)
        Me.Label11.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(123, 24)
        Me.Label11.TabIndex = 23
        Me.Label11.Text = "User Param 0"
        '
        'TextBox4
        '
        Me.TextBox4.BackColor = System.Drawing.Color.Black
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.ForeColor = System.Drawing.Color.White
        Me.TextBox4.Location = New System.Drawing.Point(265, 226)
        Me.TextBox4.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.Size = New System.Drawing.Size(217, 22)
        Me.TextBox4.TabIndex = 26
        Me.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(34, 224)
        Me.Label12.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(123, 24)
        Me.Label12.TabIndex = 25
        Me.Label12.Text = "User Param 1"
        '
        'TextBox5
        '
        Me.TextBox5.BackColor = System.Drawing.Color.Black
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.ForeColor = System.Drawing.Color.White
        Me.TextBox5.Location = New System.Drawing.Point(266, 300)
        Me.TextBox5.Margin = New System.Windows.Forms.Padding(2)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.Size = New System.Drawing.Size(217, 22)
        Me.TextBox5.TabIndex = 28
        Me.TextBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(34, 298)
        Me.Label13.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(123, 24)
        Me.Label13.TabIndex = 27
        Me.Label13.Text = "User Param 3"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(34, 443)
        Me.Label14.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(64, 24)
        Me.Label14.TabIndex = 29
        Me.Label14.Text = "Target"
        '
        'cTargetValue
        '
        Me.cTargetValue.Location = New System.Drawing.Point(265, 474)
        Me.cTargetValue.Name = "cTargetValue"
        Me.cTargetValue.Size = New System.Drawing.Size(163, 20)
        Me.cTargetValue.TabIndex = 30
        Me.cTargetValue.Text = "1000"
        '
        'cTargetMode
        '
        Me.cTargetMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cTargetMode.FormattingEnabled = True
        Me.cTargetMode.Location = New System.Drawing.Point(265, 443)
        Me.cTargetMode.Name = "cTargetMode"
        Me.cTargetMode.Size = New System.Drawing.Size(217, 21)
        Me.cTargetMode.TabIndex = 31
        '
        'unit
        '
        Me.unit.AutoSize = True
        Me.unit.Location = New System.Drawing.Point(437, 478)
        Me.unit.Name = "unit"
        Me.unit.Size = New System.Drawing.Size(0, 13)
        Me.unit.TabIndex = 32
        '
        'UnitMult
        '
        Me.UnitMult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.UnitMult.FormattingEnabled = True
        Me.UnitMult.Location = New System.Drawing.Point(440, 473)
        Me.UnitMult.Name = "UnitMult"
        Me.UnitMult.Size = New System.Drawing.Size(41, 21)
        Me.UnitMult.TabIndex = 33
        '
        'RunStart
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(511, 657)
        Me.Controls.Add(Me.UnitMult)
        Me.Controls.Add(Me.unit)
        Me.Controls.Add(Me.cTargetMode)
        Me.Controls.Add(Me.cTargetValue)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.pNote)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.pDT)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.pBias)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.pAcq)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.pBoard)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.pMRun)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.pRun)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.MaximizeBox = False
        Me.Name = "RunStart"
        Me.Text = "Start Run"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents pRun As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents pMRun As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents pBoard As TextBox
    Friend WithEvents pAcq As TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents pBias As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents pDT As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents pNote As TextBox
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents cTargetValue As TextBox
    Friend WithEvents cTargetMode As ComboBox
    Friend WithEvents unit As Label
    Friend WithEvents UnitMult As ComboBox
End Class
