Public Class WeeRocAsicCommonSettings
    Public Class SingleAsicCFG
        Public SerialNumerbOfTheBoard As String
        Public AsicId As Integer
        Public Class SingleChannelCFG
            Public BiasCompEnable As Boolean
            Public BiasComp As Integer
            Public ChargeMask As Boolean
            Public TimeMask As Boolean
            Public ThCompensation As Integer
            Public TestHG As Boolean
            Public TestLG As Boolean
            Public THcompTime As Double
            Public THcompCharge As Double
            Public GainHG As Double
            Public GainLG As Double
            Public Gain As Double
            Public Offset As Double
        End Class

        Public sC() As SingleChannelCFG
    End Class

    Public Class chMap
        Public X As Int16
        Public Y As Int16
        Public ValidLocation As Boolean
        Public Board As String
        Public Asic As Int16
        Public Channel As Int16
        Public Veto As Boolean
    End Class
    Public AsicModel As String
    Public AsicCount As Integer
    Public Timestamp As DateTime
    Public AppVersion As String
    Public SignalPolarity As String
    Public ChargeThreshold As Integer
    Public TimeThreshold As Integer
    Public DelayTrigger As Integer
    Public ShaperCF As String
    Public ShaperCI As String
    Public TransferSize As String
    Public T0Mode As String
    Public psbin As Integer
    Public HvOutputOn As Boolean
    Public HVVoltage As Double
    Public T0Freq As Integer
    Public SelfTriggerEnable As Boolean
    Public SelfTRiggerFreq As Integer
    Public MonitorMux As String
    Public Channel As Integer
    Public AnalogReadout As Boolean
    Public ProcessMode As String
    Public FileFormat As String
    Public ClusterTimens As Integer
    Public EnergyModeLG As String
    Public EnergyModeHG As String
    Public ShapingTimeLG As String
    Public ShapingTimeHG As String
    Public FastShaperSource As String
    Public SCABias As String
    Public InputDacReference As String
    Public PreampBias As String
    Public TriggerSelector As String
    Public TriggerMode As String
    Public TriggerLatch As Boolean
    Public MonitorMuxDigital As String
    Public ChannelDigital As Integer
    Public HoldDelay As Integer

    Public sA() As SingleAsicCFG

    Public ExternalStartDelay As Double

    Public sMap() As chMap


    Public Enum ScanMode
        ScanTimeThreshold
        ScanHV
        ScanGain_LG
        ScanGain_HG
        ScanInputDAC
        ScanCorrThreshold
        ScanHoldDelay
        ScanExternalDelay
    End Enum

    Public Rebin As Integer = 2048
End Class
