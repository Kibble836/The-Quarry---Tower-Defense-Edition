Public Class Game

    'Private Variables



    Private Sub Game_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Initializing form
        Me.ClientSize = New Size(1000, 1000)
        Me.Name = "The Quarry"

        'Create Objects
        'Player
        Dim Player As New PictureBox
        With Player
            .Size = New Size(32, 32)
            .Name = "Player"

        End With

    End Sub
End Class
