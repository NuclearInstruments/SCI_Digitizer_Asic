
Public Class Register

    Public Property Name As String
    Public Property Address As UInt32
    Public Property RegionSize As Integer
    Public Property Description As String
    Public Property Category As String
    Public Sub New()

    End Sub
End Class
Public Class MMCComponent
    Public Property Lists As ListModule()
    Public Property LogicAnalyzers As LogicAnalyzer()
    Public Property Oscilloscopes As Oscilloscope()
    Public Property i2c As IdueC()
    Public Property FrameTransfer As FrameTransfers()

End Class
Public Class ListModule
    Public Property Name As String
    Public Property Type As String
    Public Property Address As Integer
    Public Property Version As String
    Public Property WordSize As Integer
    Public Property Registers As Register()
End Class
Public Class LogicAnalyzer
    Public Property Name As String
    Public Property Type As String
    Public Property Address As Integer
    Public Property Version As String
    Public Property Channels As Channel()
    Public Property nsamples As Integer
    Public Property Registers As Register()
End Class
Public Class Oscilloscope
    Public Property Name As String
    Public Property Type As String
    Public Property Address As UInt32
    Public Property Version As String
    Public Property nsamples As Integer
    Public Property Channels As Integer
    Public Property Registers As Register()

End Class
Public Class iduec
    Public Property Name As String
    Public Property Type As String
    Public Property Address As UInt32
    Public Property Version As String
    Public Property Registers As Register()
End Class
Public Class FrameTransfers
    Public Property Name As String
    Public Property Type As String
    Public Property Address As UInt32
    Public Property Version As String
    Public Property Channels As Integer
    Public Property Registers As Register()

End Class
Public Class Channel
    Public Property Name As String
    Public Property bitstart As Integer
    Public Property bitwidth As Integer
End Class

Public Class SciCompilerExportClass
    Public Property Device As String
    Public Property Registers As Register()
    Public Property MMCComponents As MMCComponent
End Class
