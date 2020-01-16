Public Class SelfTrigger
    Public enable As Boolean
    Public rate As Integer
End Class
Public Class BoardSettings
    Public SN As String
    Public HV_VOLT As Double
    Public HV_MAX As Double
    Public TriggerSource As String
    Public UseChangeTrigger As Boolean
    Public EnableFrameTrigger As Boolean
    Public ExtHoldDelay As Integer
    Public EnableExternalVeto As Boolean
    Public SelfTrigger As SelfTrigger
    Public bitstreams As List(Of String)
End Class

Public Class ConfigClass
    Public ListOfDevices As List(Of String)
    Public MasterSN As String
    Public DataStorageFolder As String
    Public FileName As String
    Public BoardsSettings As List(Of BoardSettings)
End Class
