Public Class CPSMonitor
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        cps.Columns.Clear()
        cps.Columns.Add("Channel", "Channel")
        cps.Columns.Add("CPS", "CPS")

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Sub CPSMonitor_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class