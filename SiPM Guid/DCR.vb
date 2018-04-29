Imports OxyPlot

Public Class DCR
    Dim Model = New PlotModel()
    Class DataSpe
        Public Sub New(samples)
            ReDim data(samples)
        End Sub
        Public data() As DataPoint
    End Class

    Dim datas(64) As DataSpe
    Dim series(64) As OxyPlot.Series.LineSeries


    Private Sub DCR_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CheckedListBox1.Items.Clear()
        For i = 1 To 64
            CheckedListBox1.Items.Add(i)
        Next

        Model.Title = "DCR"
        plot1.Refresh()



        For i = 0 To 63
            datas(i) = New DataSpe(10)


            series(i) = New OxyPlot.Series.StairStepSeries() With {
                .Title = "CH " & i + 1,
                 .MarkerType = MarkerType.Circle,
                    .ItemsSource = datas(i).data
        }
        Next
    End Sub

    Private Sub CheckedListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CheckedListBox1.SelectedIndexChanged

        Model.Series.clear
        For Each s In CheckedListBox1.CheckedItems
            Model.Series.Add(series(s - 1))
        Next

        plot1.Model = Model

        plot1.InvalidatePlot(True)
        plot1.Refresh()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim nsample = (ep.Value - sp.Value) / ss.Value

        For i = 0 To 63
            datas(i) = New DataSpe(nsample - 1)


            series(i) = New OxyPlot.Series.StairStepSeries() With {
                .Title = "CH " & i + 1,
                 .MarkerType = MarkerType.Circle,
                    .ItemsSource = datas(i).data
        }
        Next
        Model.Series.clear
        For Each s In CheckedListBox1.CheckedItems
            Model.Series.Add(series(s - 1))
        Next

        For i = 0 To 63
            'ReDim datas(i).data(nsample - 1)
            For k = 0 To nsample - 1
                datas(i).data(k) = New DataPoint(sp.Value + ss.Value * k, 0)
            Next
        Next




        plot1.InvalidatePlot(True)

            Application.DoEvents()


    End Sub
End Class