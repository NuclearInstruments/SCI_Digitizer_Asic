
Public Class GeneralSettings
    Public Property RunTarget As String = "FREE"
    Public Property TargetValue As Double = 0
End Class

Public Class SelfTrigger
    Public Property enable As Boolean
    Public Property rate As Integer
End Class

Public Class GlobalSettings
    Public Property SHAPER_C1 As String
    Public Property SHAPER_C2 As String
    Public Property CHARGE_TRIGGER As Integer
    Public Property TIME_TRIGGER As Integer
    Public Property INTERNAL_TRIGGER_DELAY As Integer
End Class

Public Class ChannelSpecific
    Public Property ID As Integer
    Public Property BIAS As Boolean
    Public Property BIAS_OFFSET As Integer
    Public Property MASK_CHARGE As Boolean
    Public Property MASK_TIME As Boolean
    Public Property THRESHOLD_ADJ As Integer
End Class

Public Class AsicSetting
    Public Property channel_specific As List(Of ChannelSpecific)
End Class

Public Class AsicConfiguration
    Public Property global_settings As GlobalSettings
    Public Property asic_settings As List(Of AsicSetting)
End Class

Public Class BoardsSetting
    Public Property SN As String
    Public Property HV_VOLT As Double
    Public Property HV_MAX As Double
    Public Property wait_hv As Boolean
    Public Property ReadTemperatureAndHV As Boolean
    Public Property EnableHVCompensation As Boolean
    Public Property HVCompensationCoefficent As Double
    Public Property switch_off_hv_on_end As Boolean
    Public Property TriggerSource As String
    Public Property EnableFrameTrigger As Boolean
    Public Property ExtHoldDelay As Integer
    Public Property UseChangeTrigger As Boolean
    Public Property EnableExternalVeto As Boolean
    Public Property SelfTrigger As SelfTrigger
    Public Property Polarity As String
    Public Property configuration_mode As String
    Public Property asic_configuration As AsicConfiguration
    Public Property bitstreams As List(Of String)
End Class

Public Class ConfigClass
    Public Property ListOfDevices As String()
    Public Property MasterSN As String
    Public Property DataStorageFolder As String
    Public Property FileName As String
    Public Property GeneralSettings As GeneralSettings
    Public Property BoardsSettings As List(Of BoardsSetting)
End Class