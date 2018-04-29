Public Class Mapping

    Dim CURRENTMAP(,) As String
    Dim ROWS As Integer = 4
    Dim COLS As Integer = 8
    Dim TOTALCHANNEL = 32
    Dim BLOCKSIE = 55
    Dim ptb As New List(Of Label)
    Dim button1 As New Button
    Dim lab1 As New Label
    Dim lab2 As New Label
    Dim numeric_col As New NumericUpDown
    Dim numeric_row As New NumericUpDown
    Dim inhibit = True

    Public Sub EnableDisableUpdate(disable As Boolean)
        If disable Then
            ButtonSave.Enabled = False
            ButtonDefault.Enabled = False
        Else
            ButtonSave.Enabled = True
            ButtonDefault.Enabled = True
        End If
    End Sub
    Public Sub DefaultLayout()
        Dim EX As Int16, EY As Int16
        EX = 0
        EY = 0

        For Each eGC In MainForm.GC
            EX = IIf(eGC.X > EX, eGC.X, EX)
            EY = IIf(eGC.Y > EY, eGC.Y, EY)
        Next

        COLS = EX + 1
        ROWS = EY + 1

        iROWS.Value = ROWS
        iCOLS.Value = COLS
        ResizeMaps(ROWS, COLS)
    End Sub
    Private Sub Mapping_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DefaultLayout()

    End Sub

    Private Sub Mapping_Resize(sender As Object, e As EventArgs) Handles Me.Resize

    End Sub
    Dim ABCD() = {"A", "B", "C", "D"}
    Public Sub ResizeMaps(ROWS As Integer, COLS As Integer)
        DGW.Rows.Clear()
        DGW.Columns.Clear()
        DGW.EditMode = DataGridViewEditMode.EditOnEnter
        DGW.RowTemplate.Height = 30

        For i = 0 To COLS - 1
            Dim cmb As New DataGridViewComboBoxColumn()
            cmb.FlatStyle = FlatStyle.Flat
            cmb.HeaderText = i + 1
            cmb.Name = "COL" & i
            cmb.Items.Add("NC")

            For Each z In MainForm.GC
                cmb.Items.Add(z.Channel & "(" & ABCD(z.Asic) & ") - " & z.Board.Substring(4))
            Next

            DGW.Columns.Add(cmb)
        Next
        For j = 0 To ROWS - 1
            DGW.Rows.Add()
            ' DGW.Rows(j).HeaderCell.Value = j
        Next
        For j = 0 To ROWS - 1

            For i = 0 To COLS - 1
                DGW.Rows(j).Cells(i).Value = "NC"
                For Each z In MainForm.GC
                    If z.X = i And z.Y = j Then
                        If z.ValidLocation = True Then
                            DGW.Rows(j).Cells(i).Value = z.Channel & "(" & ABCD(z.Asic) & ") - " & z.Board.Substring(4)
                        End If
                    End If
                Next
            Next
        Next


    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub DGW_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs)

    End Sub

    Private Sub DGW_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        For Each z In MainForm.GC
            z.ValidLocation = False
        Next
        For j = 0 To DGW.RowCount - 1
            For i = 0 To DGW.ColumnCount - 1
                For Each z In MainForm.GC
                    If DGW.Rows(j).Cells(i).Value = z.Channel & "(" & ABCD(z.Asic) & ") - " & z.Board.Substring(4) Then
                        z.X = i
                        z.Y = j
                        z.ValidLocation = True
                    End If
                Next
            Next
        Next
        ResizeMaps(ROWS, COLS)
        MainForm.UpdateChannels()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ROWS = iROWS.Value
        COLS = iCOLS.Value


        ResizeMaps(ROWS, COLS)
    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles ButtonDefault.Click
        MainForm.AssignDefaultChannelMapping()
        DefaultLayout()
    End Sub

    Private Sub DGW_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DGW.CellContentClick

    End Sub

    Private Sub DGW_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles DGW.CellFormatting
        DGW.Rows(e.RowIndex).HeaderCell.Value = (e.RowIndex + 1).ToString()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        MainForm.AssignDefaultChannelMapping()
        MainForm.AssignDefaultBoardChannelMapping()
        DefaultLayout()

    End Sub
End Class
