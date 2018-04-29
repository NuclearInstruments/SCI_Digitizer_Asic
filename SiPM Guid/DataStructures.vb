Public Class DataStructures
    Public Class AsicChannel
        Public X As Int16
        Public Y As Int16
        Public ValidLocation As Boolean
        Public Board As String
        Public Asic As Int16
        Public Channel As Int16
        Public Veto As Boolean
        Public EnergySpectrum(1024) As UInt64
        Public TimeSpectrum(1024) As UInt64
        Public Hitcounter As UInt64
    End Class


End Class
