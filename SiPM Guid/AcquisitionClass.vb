Public Class AcquisitionClass


    Public General_settings As New GeneralSettings
    Public Class MAP
        Public positions(,) As String
        Public rows As Integer
        Public cols As Integer
        Public list() As String
        Public Sub New()
        End Sub
        Public Sub New(rows, cols, channels)
            ReDim positions(rows - 1, cols - 1)
            ReDim list(rows * cols - 1)
            Dim k = 0
            For i = 0 To rows - 1
                For j = 0 To cols - 1
                    If i * cols + j + 1 <= channels Then
                        positions(i, j) = i * cols + j + 1
                        list(k) = (i * cols + j + 1).ToString
                        k += 1
                    Else
                        positions(i, j) = "NC"
                    End If
                Next
            Next
        End Sub


    End Class
    Enum signalpolarity
        POSITIVE = 0
        NEGATIVE = 1
    End Enum

    '  Enum signalimpedence
    '     OHM50 = 0
    '    OHM1K = 1
    'End Enum

    Enum triggermode
        THRESHOLD = 0
        DERIVATIVE = 1
    End Enum


    Enum muxmode
        ANALOG = 0
        TRAPEZOIDAL = 1
        BASELINE = 2
        ENERGY = 3

    End Enum

    Enum triggersource
        INTERNAL = 0
        EXTERNAL = 1
        LEVEL = 2
    End Enum


    Enum samplingmethod
        INDIPENDENT = 0
        COMMON = 1
    End Enum

    Enum edge
        RISING = 0
        FALLING = 1
    End Enum



    Structure GeneralSettings
        Public polarity As signalpolarity
        Public impedence As Boolean
        Public offset As Double
        Public trigmode As triggermode
        Public trigsource As triggersource
        Public trigsource_osc As triggersource
        Public channel_osc_trigger As Int32
        Public hv_enabled As Boolean
        Public voltage As Double
        Public trigholdoff As Double
        Public derivate As Double
        Public trigdelay As Double
        Public sampling As samplingmethod
        Public pregate As Double
        Public edges As edge
        Public decimator As Double
        Public triglevel As Double
        Public mux As muxmode

    End Structure

    Public Class Channel
        Enum energyfiltermode
            INTEGRATION = 0
            TRAPEZOIDAL = 1
        End Enum
        Enum triggertype
            AUTO = 0
            MAIN = 1
        End Enum
        Public name As String
        Public id As Integer
        Public x_position As Integer
        Public y_position As Integer
        Public triglevel As Double
        Public energyfilter As energyfiltermode
        Public decayconst As Double
        Public peaking As Double
        Public flattop As Double
        Public energysample As Double
        Public gain As Double
        Public integrationtime As Double
        Public pregate As Double
        Public pileupenable As Boolean
        Public pileuptime As Double
        Public baselineinhibit As Double
        Public baselinesample As Integer
        Public stat As Statistics
        Public trigtype As triggertype
        Public spectra_checked As Boolean
        Public scope_checked As Boolean

        Public Sub New(name As String, id As Integer, x As Integer, y As Integer)
            Me.name = name
            Me.id = id
            Me.triglevel = 50
            Me.energyfilter = energyfiltermode.TRAPEZOIDAL
            Me.decayconst = 700
            Me.peaking = 1300
            Me.flattop = 100
            Me.energysample = 1600
            Me.gain = 1
            Me.integrationtime = 0.0
            Me.pregate = 20
            Me.pileupenable = True
            Me.pileuptime = 1000
            Me.baselineinhibit = 2500
            Me.baselinesample = 256
            Me.x_position = x
            Me.y_position = y
            Me.stat = New Statistics
            Me.trigtype = triggertype.AUTO
            spectra_checked = False
            scope_checked = True
        End Sub

    End Class

    Public CHList As New List(Of Channel)
    Public fit_list As New List(Of Fitting)
    Public currentMAP As New MAP
    Dim x As Integer = 0
    Dim y As Integer = 0

    Public Sub New(nch As Integer)
        General_settings.polarity = signalpolarity.POSITIVE
        General_settings.impedence = False
        General_settings.offset = 0
        General_settings.trigmode = triggermode.THRESHOLD
        General_settings.trigsource = triggersource.INTERNAL
        General_settings.channel_osc_trigger = 0
        General_settings.hv_enabled = False
        General_settings.voltage = 50
        General_settings.trigholdoff = 0
        General_settings.derivate = 0
        General_settings.trigdelay = 0
        General_settings.sampling = samplingmethod.COMMON
        General_settings.edges = edge.RISING
        General_settings.decimator = 1
        General_settings.triglevel = 1000
        General_settings.mux = muxmode.ANALOG
        General_settings.pregate = 20
        For i = 0 To nch - 1
            FindPosition(8, i + 1, x, y)
            Dim ch = New Channel("CHANNEL " + (i + 1).ToString, i + 1, x, y)
            CHList.Add(ch)
        Next
        currentMAP = New MAP(4, 8, nch)
    End Sub
    Public Sub New()

    End Sub
    Public Sub FindPosition(cols As Integer, index As Integer, ByRef x_pos As Integer, ByRef y_pos As Integer)
        Dim risultato = Math.Floor(index / cols)
        Dim resto = index Mod cols
        If resto = 0 Then
            x_pos = cols - 1
            y_pos = risultato - 1
        Else
            x_pos = resto - 1
            y_pos = risultato
        End If
    End Sub

    Public Class Fitting
        Public cursor1 As Double
        Public cursor2 As Double
        Public channel_number As Integer
    End Class


    Structure Statistics
        Public icr As Double
        Public ocr As Double
        Public deadtime As Double
        Public toteventin As Double
        Public toteventout As Double
    End Structure

End Class
