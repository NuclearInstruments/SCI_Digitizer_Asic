Public Class fit_win

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged
        If MainForm.acquisition.fit_list.Count < DataGridView1.Rows.Count Then
            Dim f As New AcquisitionClass.Fitting
            f.channel_number = DataGridView1.Rows(e.RowIndex).Cells("Channel").Value.ToString.Replace("CHANNEL ", "")
            MainForm.acquisition.fit_list.Add(f)
        Else
            If DataGridView1.Columns(e.ColumnIndex).HeaderText = "Channel" Then
                MainForm.acquisition.fit_list(e.RowIndex).channel_number = CInt(DataGridView1.Rows(e.RowIndex).Cells("Channel").Value.ToString.Replace("CHANNEL ", ""))
            ElseIf DataGridView1.Columns(e.ColumnIndex).HeaderText = "Cursor 1" Then
                MainForm.acquisition.fit_list(e.RowIndex).cursor1 = CInt(DataGridView1.Rows(e.RowIndex).Cells("Channel").Value.ToString.Replace("CHANNEL ", ""))
            ElseIf DataGridView1.Columns(e.ColumnIndex).HeaderText = "Cursor 2" Then
                MainForm.acquisition.fit_list(e.RowIndex).cursor2 = CInt(DataGridView1.Rows(e.RowIndex).Cells("Channel").Value.ToString.Replace("CHANNEL ", ""))
            End If
        End If
    End Sub

    Private Sub DataGridView1_RowsRemoved(sender As Object, e As DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        MainForm.acquisition.fit_list.RemoveAt(e.RowIndex)
    End Sub

    Private Sub fit_win_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DataGridView1.Columns.Clear()

        Dim ch As New DataGridViewComboBoxColumn()
        ch.HeaderText = "Channel"
        ch.Name = "Channel"
        'ch.MaxDropDownItems = MainForm.pRT4.CheckedListBox1.CheckedIndices.Count
        'For Each i In MainForm.pRT4.CheckedListBox1.CheckedItems
        '    ch.Items.Add(i)
        'Next
        DataGridView1.Columns.Add(ch)

        DataGridView1.Columns.Add("Cursor 1", "Cursor 1")
        DataGridView1.Columns.Add("Cursor 2", "Cursor 2")
        DataGridView1.Columns.Add("mean", "Mean")
        DataGridView1.Columns.Add("fitmu", "Centroid")
        DataGridView1.Columns.Add("sigma", "STD - R")
        DataGridView1.Columns.Add("fitsigma", "FWHM - R")
        DataGridView1.Columns.Add("areaUpeak", "Area")
        DataGridView1.Columns.Add("areaFit", "Area fit")

        DataGridView1.Columns("mean").ReadOnly = True
        DataGridView1.Columns("fitmu").ReadOnly = True
        DataGridView1.Columns("sigma").ReadOnly = True
        DataGridView1.Columns("fitsigma").ReadOnly = True
        DataGridView1.Columns("areaUpeak").ReadOnly = True
        DataGridView1.Columns("areaFit").ReadOnly = True

        DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

        DataGridView1.Rows.Clear()
        For Each fit_el In MainForm.acquisition.fit_list
            DataGridView1.Rows.Add(fit_el.channel_number, fit_el.cursor1, fit_el.cursor2)
        Next
    End Sub



End Class
